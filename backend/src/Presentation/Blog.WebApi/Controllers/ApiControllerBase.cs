using Microsoft.AspNetCore.Mvc;

namespace Blog.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        
    }
}