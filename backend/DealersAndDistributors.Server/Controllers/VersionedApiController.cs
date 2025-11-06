using Microsoft.AspNetCore.Mvc;

namespace DealersAndDistributors.Server.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class VersionedApiController : ControllerBase    
{
}

