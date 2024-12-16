using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CompieTest_NoticeBoard.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoticeBoardController : ControllerBase
    {
        private readonly ILogger<NoticeBoardController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly string _filePath = Path.Combine("Data", "noticeBoardItems.json"); 

        public NoticeBoardController(ILogger<NoticeBoardController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var noticeBoardItems = await LoadNoticeBoardItemsAsync();
                var existingItem = noticeBoardItems.Items.FirstOrDefault(i => i.Id == id);

                if (existingItem == null)
                {
                    return NotFound("Item not found.");
                }

                DeleteImage(existingItem.ImageUrl);

                noticeBoardItems.Items.Remove(existingItem);

                await SaveNoticeBoardItemsAsync(noticeBoardItems);

                var noticeBoardItemsArray = GetSortedNoticeBoardItemsArray(noticeBoardItems);
                return Ok(noticeBoardItemsArray);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] NoticeBoardItem updatedItem)
        {
            NoticeBoardItem[]? noticeBoardItemsArray = null;
            NoticeBoardItems? noticeBoardItems = null;
            NoticeBoardItem? existingItem = null;
            string updatedJsonContent = string.Empty;

            if (updatedItem == null)
            {
                return BadRequest("Invalid patch document.");
            }

            try
            {
                noticeBoardItems = await LoadNoticeBoardItemsAsync();

                existingItem = noticeBoardItems.Items.FirstOrDefault(i => i.Id == id);

                if (existingItem == null)
                {
                    return NotFound("Item not found.");
                }
                if (updatedItem.Title != null && updatedItem.Content != null && updatedItem.UpdateDate != null)
                {
                    existingItem.Title = updatedItem.Title;
                    existingItem.Content = updatedItem.Content;
                    existingItem.UpdateDate = updatedItem.UpdateDate;
                }
                else if (updatedItem.Title != null)
                {
                    existingItem.Title = updatedItem.Title;
                }
                else if (updatedItem.Content != null)
                {
                    existingItem.Content = updatedItem.Content;
                }
                
                existingItem.UpdateDate = updatedItem.UpdateDate;

                await SaveNoticeBoardItemsAsync(noticeBoardItems);

                noticeBoardItems = await LoadNoticeBoardItemsAsync();

                noticeBoardItemsArray = GetSortedNoticeBoardItemsArray(noticeBoardItems);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON deserialization error: {ex.Message}");
                return BadRequest("Error deserializing JSON");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok(noticeBoardItemsArray ?? Array.Empty<NoticeBoardItem>());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NoticeBoardItem updatedItem)
        {
            NoticeBoardItem[]? noticeBoardItemsArray = null;
            NoticeBoardItem? existingItem = null;
            string updatedJsonContent = string.Empty;

            if (updatedItem == null)
            {
                return BadRequest("Invalid item.");
            }

            try
            {
                NoticeBoardItems noticeBoardItems = await LoadNoticeBoardItemsAsync();

                existingItem = noticeBoardItems.Items.FirstOrDefault(i => i.Id == id);

                if (existingItem == null)
                {
                    return NotFound("Item not found.");
                }

                if (updatedItem.Title != null && updatedItem.Content != null && updatedItem.UpdateDate != null)
                {
                    existingItem.Title = updatedItem.Title;
                    existingItem.Content = updatedItem.Content;
                    existingItem.UpdateDate = updatedItem.UpdateDate;
                }
                else if (updatedItem.Title != null)
                {
                    existingItem.Title = updatedItem.Title;
                }
                else if (updatedItem.Content != null)
                {
                    existingItem.Content = updatedItem.Content;
                }
                
                existingItem.UpdateDate = updatedItem.UpdateDate;

                await SaveNoticeBoardItemsAsync(noticeBoardItems);

                noticeBoardItems = await LoadNoticeBoardItemsAsync();

                noticeBoardItemsArray = GetSortedNoticeBoardItemsArray(noticeBoardItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok(noticeBoardItemsArray ?? Array.Empty<NoticeBoardItem>());         
        }

        [HttpPost(Name = "AddNoticeBoardItem")]
        public async Task<IActionResult> Post([FromForm] string title, [FromForm] string content, [FromForm] IFormFile image)
        {
            NoticeBoardItem[]? noticeBoardItemsArray = null;
            NoticeBoardItems? noticeBoardItems = null;
            string updatedJsonContent = string.Empty;

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                return BadRequest("Invalid item.");
            }

            try
            {
                string imageUrl = string.Empty;

                if (image != null)
                {
                    var uploadsPath = Path.Combine(_environment.WebRootPath, "images");
                    var filePath = Path.Combine(uploadsPath, image.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    imageUrl = image.FileName;
                }

                noticeBoardItems = await LoadNoticeBoardItemsAsync();

                var newItem = new NoticeBoardItem
                {
                    Title = title,
                    Content = content,
                    ImageUrl = imageUrl,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                if (noticeBoardItems.Items.Any())
                {
                    newItem.Id = noticeBoardItems.Items.Max(i => i.Id) + 1;
                }
                else
                {
                    newItem.Id = 1;
                }

                noticeBoardItems.Items.Add(newItem);

                await SaveNoticeBoardItemsAsync(noticeBoardItems);

                noticeBoardItems = await LoadNoticeBoardItemsAsync();

                noticeBoardItemsArray = GetSortedNoticeBoardItemsArray(noticeBoardItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok(noticeBoardItemsArray ?? Array.Empty<NoticeBoardItem>());
        }

        [HttpGet(Name = "GetNoticeBoard")]
        public async Task<IActionResult> Get()
        {
            NoticeBoardItem[]? noticeBoardItemsArray = null;
            NoticeBoardItems? noticeBoardItems = null;

            try
            {
                noticeBoardItems = await LoadNoticeBoardItemsAsync();
                
                if (noticeBoardItems != null && noticeBoardItems.Items != null)
                {
                    noticeBoardItemsArray = GetSortedNoticeBoardItemsArray(noticeBoardItems);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok(noticeBoardItemsArray ?? new NoticeBoardItem[0]);
        }

        //Private Methods =========================================================

        private NoticeBoardItem[] GetSortedNoticeBoardItemsArray(NoticeBoardItems? noticeBoardItems)
        {
            NoticeBoardItem[] result = [];

            if (noticeBoardItems != null && noticeBoardItems.Items != null)
            {
                result = noticeBoardItems.Items.OrderByDescending(item => item.CreateDate).ToArray();
            }

            return result;
        }

        private async Task<NoticeBoardItems> LoadNoticeBoardItemsAsync()
        {
            string jsonContent = string.Empty;

            try
            {
                if (!System.IO.File.Exists(_filePath))
                {
                    _logger.LogError($"JSON file not found: {_filePath}");
                    return new NoticeBoardItems { Items = new List<NoticeBoardItem>() };
                }

                jsonContent = await System.IO.File.ReadAllTextAsync(_filePath);
                return JsonConvert.DeserializeObject<NoticeBoardItems>(jsonContent);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON deserialization error: {ex.Message}");
                throw new Exception("Error deserializing JSON", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                throw new Exception("Internal server error", ex);
            }
        }

        private void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }
                
            var uploadsPath = Path.Combine(_environment.WebRootPath, "images");
            var filePath = Path.Combine(uploadsPath, imageUrl);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private async Task SaveNoticeBoardItemsAsync(NoticeBoardItems noticeBoardItems)
        {
            var updatedJsonContent = JsonConvert.SerializeObject(noticeBoardItems, Formatting.Indented);
            await System.IO.File.WriteAllTextAsync(_filePath, updatedJsonContent);
        }
    }
}