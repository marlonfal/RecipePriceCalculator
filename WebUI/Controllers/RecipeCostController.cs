using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.RecipeCost;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeCostController : ControllerBase
    {
        private readonly IRecipeCostService _recipeCostService;

        public RecipeCostController(IRecipeCostService recipeCostService)
        {
            _recipeCostService = recipeCostService;
        }

        public async Task<IActionResult> Get()
        {
            var result = await _recipeCostService.Get();
            if (result != null) return Ok(result);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _recipeCostService.Get(id);
            if (result != null) return Ok(result);
            return NotFound();
        }
    }
}
