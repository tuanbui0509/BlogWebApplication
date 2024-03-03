using Microsoft.AspNetCore.Mvc;

namespace BlogWebApplication.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        
    }
}