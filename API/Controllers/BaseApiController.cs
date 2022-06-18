using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class BaseApiController : ControllerBase
   {
      private IMediator _IMediator;

      protected IMediator Mediator => _IMediator ??= HttpContext.RequestServices.GetService<IMediator>();
   }
}