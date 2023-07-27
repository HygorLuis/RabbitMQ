using Rabbit.Models.Entities;

namespace Rabbit.Repositories.Interfaces
{
    public interface IRabbitMessageRepository
    {
        void SendMessage(Message message);
    }
}
