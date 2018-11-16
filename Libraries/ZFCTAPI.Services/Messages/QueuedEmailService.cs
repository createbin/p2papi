using ZFCTAPI.Data.Message;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Messages
{
    public interface IQueuedEmailService : IRepository<QueuedEmail>
    {
    }

    public class QueuedEmailService : Repository<QueuedEmail>, IQueuedEmailService
    {
    }
}