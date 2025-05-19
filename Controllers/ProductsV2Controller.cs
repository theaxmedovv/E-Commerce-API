using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductsV2Controller : ControllerBase
    {
        /// <summary>
        /// Gets products for API version 2.0.
        /// </summary>
        [HttpGet]
        public ActionResult<string> GetProducts()
        {
            return "This is version 2.0 of the Products API.";
        }
    }
}
