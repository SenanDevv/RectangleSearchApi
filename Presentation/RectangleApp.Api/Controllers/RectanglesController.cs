using Microsoft.AspNetCore.Mvc;
using Project.Service.Dtos;
using Project.Service.Services.Abstraction;

namespace RectangleApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RectanglesController : ControllerBase
    {
        private readonly IRectangleService _rectangleService;

        public RectanglesController(IRectangleService rectangleService)
        {
            _rectangleService = rectangleService;
        }

        [HttpPost]
        [ModelStateControl]
        public IActionResult Search([FromBody] SearchRectangleBySegmentDto segment)
        {
            var result = _rectangleService.GetAllRectangles(segment);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ModelStateControl]
        public async Task<IActionResult> GetRectangle(int id)
        {
            var result = await _rectangleService.GetRectangleAsync(id);
            return Ok(result);
        }
    }
}
