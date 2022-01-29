using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TokenBasedAuth_NetCore.Entities.Trivia;
using TokenBasedAuth_NetCore.Models;
using TokenBasedAuth_NetCore.Repository;
using TokenBasedAuth_NetCore.Services.Trivia;
using TokenBasedAuth_NetCore.UnitofWork;

namespace TokenBasedAuth_NetCore.Controllers.Trivia
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TriviaController : Controller
    {
        private readonly ITriviaService _triviaService;

        public TriviaController(
          ITriviaService triviaService)
        {
            _triviaService = triviaService;

        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var categories = _triviaService.GetAll();

            if (categories == null || categories.Count() == 0)
                return BadRequest("Categoris Not Found");

            ResponseModel<IEnumerable<Category>> responseModel = new ResponseModel<IEnumerable<Category>>()
            {
                Status = HttpStatusCode.OK,
                Message = "Succesfully retrieved",
                Result = categories
            };
            return Ok(responseModel);

        }
    }
}
