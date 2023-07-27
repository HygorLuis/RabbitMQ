using Microsoft.AspNetCore.Mvc;
using Rabbit.Models.Entities;
using Rabbit.Services.Interfaces;

namespace Rabbit.Publisher.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMessageController : ControllerBase
    {
        private readonly IRabbitMessageService _service;

        public RabbitMessageController(IRabbitMessageService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Post(Message message)
        {
            _service.SendMessage(message);
            return Ok("Menssagem enviada com sucesso!");
        }
    }
}
