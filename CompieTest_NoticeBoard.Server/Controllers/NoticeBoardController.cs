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

        [HttpPost(Name = "AddNoticeBoardItem")]
        public async Task<IActionResult> Add([FromBody] NoticeBoardItem newItem)
        {
            if (newItem == null)
            {
                return BadRequest("Invalid item.");
            }

            NoticeBoardItems noticeBoardItems = null;

            try
            {
                if (System.IO.File.Exists(_filePath))
                {
                    string jsonContent = await System.IO.File.ReadAllTextAsync(_filePath);
                    noticeBoardItems = JsonConvert.DeserializeObject<NoticeBoardItems>(jsonContent);
                }
                else
                {
                    noticeBoardItems = new NoticeBoardItems { Items = new List<NoticeBoardItem>() };
                }

                noticeBoardItems.Items.Add(newItem);

                string updatedJsonContent = JsonConvert.SerializeObject(noticeBoardItems, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(_filePath, updatedJsonContent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok(newItem);
        }

        [HttpGet(Name = "GetNoticeBoard")]
        public async Task<IActionResult> Get()
        {
            NoticeBoardItems noticeBoardItems = null;
            NoticeBoardItem[] noticeBoardItemsArray = null;

            try
            {
                if (!System.IO.File.Exists(_filePath))
                {
                    _logger.LogError($"JSON file not found: {_filePath}");
                    return NotFound("JSON file not found");
                }

                string jsonContent = await System.IO.File.ReadAllTextAsync(_filePath);
                noticeBoardItems = JsonConvert.DeserializeObject<NoticeBoardItems>(jsonContent);
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

            if (noticeBoardItems != null && noticeBoardItems.Items != null)
            {
                noticeBoardItemsArray = noticeBoardItems.Items.ToArray();
            }

            return Ok(noticeBoardItemsArray ?? new NoticeBoardItem[0]);
        }
    }
}