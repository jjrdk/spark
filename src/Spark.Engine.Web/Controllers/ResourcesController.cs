namespace Spark.Engine.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ResourcesController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}
