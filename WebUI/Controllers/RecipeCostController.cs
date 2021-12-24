using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult Get()
        {
            return Ok(_recipeCostService.GetRecipesCost());
        }
    }
}
