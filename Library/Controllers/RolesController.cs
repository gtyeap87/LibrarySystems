using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/roles")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class RolesController() : ControllerBase
    {
        //todo : implement role crud operations
    }
}