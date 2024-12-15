using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CompieTest_NoticeBoard.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoticeBoardController : ControllerBase
    {
        private readonly ILogger<NoticeBoardController> _logger;
        private readonly string _filePath = Path.Combine("Data", "noticeBoardItems.json");

        public NoticeBoardController(ILogger<NoticeBoardController> logger)
        {
            _logger = logger;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            NoticeBoardItem[]? noticeBoardItemsArray = null;
            NoticeBoardItems? noticeBoardItems = null;
            NoticeBoardItem? existingItem = null;
            string updatedJsonContent = string.Empty;

            try
            {
                noticeBoardItems = await LoadNoticeBoardItemsAsync();

                existingItem = noticeBoardItems.Items.FirstOrDefault(i => i.Id == id);

                if (existingItem == null)
                {
                    return NotFound("Item not found.");
                }

                noticeBoardItems.Items.Remove(existingItem);

                updatedJsonContent = JsonConvert.SerializeObject(noticeBoardItems, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(_filePath, updatedJsonContent);

                noticeBoardItems = await LoadNoticeBoardItemsAsync();

                if (noticeBoardItems != null && noticeBoardItems.Items != null)
                {
                    noticeBoardItemsArray = noticeBoardItems.Items.OrderByDescending(item => item.CreateDate).ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok(noticeBoardItemsArray ?? new NoticeBoardItem[0]);
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
                else if (updatedItem.UpdateDate != null)
                {
                    existingItem.UpdateDate = updatedItem.UpdateDate;
                }

                updatedJsonContent = JsonConvert.SerializeObject(noticeBoardItems, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(_filePath, updatedJsonContent);

                noticeBoardItems = await LoadNoticeBoardItemsAsync();

                if (noticeBoardItems != null && noticeBoardItems.Items != null)
                {
                    noticeBoardItemsArray = noticeBoardItems.Items.OrderByDescending(item => item.CreateDate).ToArray();
                }
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

            return Ok(noticeBoardItemsArray ?? new NoticeBoardItem[0]);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NoticeBoardItem updatedItem)
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
                else if (updatedItem.UpdateDate != null)
                {
                    existingItem.UpdateDate = updatedItem.UpdateDate;
                }

                updatedJsonContent = JsonConvert.SerializeObject(noticeBoardItems, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(_filePath, updatedJsonContent);

                noticeBoardItems = await LoadNoticeBoardItemsAsync();

                if (noticeBoardItems != null && noticeBoardItems.Items != null)
                {
                    noticeBoardItemsArray = noticeBoardItems.Items.OrderByDescending(item => item.CreateDate).ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok(noticeBoardItemsArray ?? new NoticeBoardItem[0]);         
        }

        [HttpPost(Name = "AddNoticeBoardItem")]
        public async Task<IActionResult> Add([FromBody] NoticeBoardItem newItem)
        {
            NoticeBoardItem[]? noticeBoardItemsArray = null;
            NoticeBoardItems? noticeBoardItems = null;
            string updatedJsonContent = string.Empty;

            if (newItem == null)
            {
                return BadRequest("Invalid item.");
            }

            try
            {
                noticeBoardItems = await LoadNoticeBoardItemsAsync();

                if (noticeBoardItems.Items.Any())
                {
                    newItem.Id = noticeBoardItems.Items.Max(i => i.Id) + 1;
                }
                else
                {
                    newItem.Id = 1;
                }

                newItem.CreateDate = DateTime.UtcNow;
                newItem.UpdateDate = DateTime.UtcNow;

                noticeBoardItems.Items.Add(newItem);

                updatedJsonContent = JsonConvert.SerializeObject(noticeBoardItems, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(_filePath, updatedJsonContent);

                noticeBoardItems = await LoadNoticeBoardItemsAsync();

                if (noticeBoardItems != null && noticeBoardItems.Items != null)
                {
                    noticeBoardItemsArray = noticeBoardItems.Items.OrderByDescending(item => item.CreateDate).ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok(noticeBoardItemsArray ?? new NoticeBoardItem[0]);
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
                    noticeBoardItemsArray = noticeBoardItems.Items.OrderByDescending(item => item.CreateDate).ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok(noticeBoardItemsArray ?? new NoticeBoardItem[0]);
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
    }
}