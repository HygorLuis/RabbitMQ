using Rabbit.Models.Entities;
using Rabbit.Repositories.Interfaces;
using Rabbit.Services.Interfaces;

namespace Rabbit.Services.Services
{
    public class RabbitMessageService : IRabbitMessageService
    {
        private readonly IRabbitMessageRepository _repository;

        public RabbitMessageService(IRabbitMessageRepository repository)
        {
            _repository = repository;
        }

        public void SendMessage(Message message)
        {
            _repository.SendMessage(message);
        }
    }
}
