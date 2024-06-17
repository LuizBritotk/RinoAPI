using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rino.Domain.Entities;
using Rino.Domain.Handlers;

namespace Rino.API.Controllers

{
    [ApiController]
    [Route("v1/[controller]")]
    public class DocumentController : ControllerBase
    {
    }
}
