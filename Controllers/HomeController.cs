using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceLayerWebApi.Models;
using ServiceLayerWebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceLayerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        //UserCrud users;//We can make it global if we want to use in multiple methods
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration config) {
            _configuration = config;
        }



        [Route("InsertUser")]
        [HttpPost]
        public IActionResult InsertUser(UserRequestModel requestModel)
        {
            try
            {
                UserCrud user = new UserCrud(_configuration);
                var response = user.InsertUser(requestModel);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
