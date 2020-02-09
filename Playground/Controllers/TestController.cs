using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Playground.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {

        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello World from TestController";
        }
    }
}
