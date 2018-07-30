using Microsoft.AspNetCore.Mvc;

namespace OrdersCollector.Controllers
{
    public class HealthController : Controller
    {
        [Route("/hc")]
        public IActionResult GetHealthStatus()
        {
            // to be replaced with a health check library
            // when need to implement actual health checks for dependencies
            return Json(new
            {
                status = "Healthy"
            });
        }
    }
}
