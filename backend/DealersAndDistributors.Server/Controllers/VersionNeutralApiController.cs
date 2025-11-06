using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace DealersAndDistributors.Server.Controllers;

[Route("api/[controller]")]
[ApiVersionNeutral]
[ApiController]
public class VersionNeutralApiController : ControllerBase
{
}

