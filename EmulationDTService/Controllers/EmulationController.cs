using Emulation.Code;
using EmulationDTService.Workers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace EmulationDTService.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly ILogger<ExampleController> _logger;
        
        public ExampleController(ILogger<ExampleController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/getData")]
        public string GetData()
        {
            _logger.LogInformation("GET test succeed");
            return "Test string for GET";
        }

        
        [HttpPost]
        [Route("/sendData")]
        public string TestPost([FromBody] TestStructure testStr)
        {
            _logger.LogInformation("Post test succeed " + testStr.TestString);
            
            //Worker._signalInc.Add(new SignalInc(25001, 20));

            
            
            return "Post test succeed";
        }
        
        
    }

    public class TestStructure
    {
        public string TestString { get; set; }
    }
}