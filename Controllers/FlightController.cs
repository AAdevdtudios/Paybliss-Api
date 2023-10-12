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
            return Ok();
        }
    }
}
