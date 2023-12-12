using Microsoft.AspNetCore.Mvc;

namespace Paybliss.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        //Initialize using parameters
        
        public FlightController()
        {
                
        }

        [HttpGet("flight/search")]
        public IActionResult getFlight()
        {
            Random random = new Random();
            return Ok(random.NextInt64(10000000000,99999999999));
        }
    }
}
