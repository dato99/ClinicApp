using ClinicApp.Models;
using ClinicApp.Packages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : MainController
    {
        IPKG_DSH_CATEGORY package;
        public CategoryController(IPKG_DSH_CATEGORY package)
        {
            this.package = package;
        }

        [HttpGet]
        public IActionResult GetCategory()
        {
            List<Category> categories = new List<Category>();
            try
            {
                categories = package.get_category();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "System error, Try Again.");
            }
            return Ok(categories);
        }
    }
}
