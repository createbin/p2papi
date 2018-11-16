using ZFCTAPI.Data.Message;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Messages
{
    public interface IQueuedSMSService : IRepository<QueuedSMS>
    {
    }

    public class QueuedSMSService : Repository<QueuedSMS>, IQueuedSMSService
    {
    }
}