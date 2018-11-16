using ZFCTAPI.Data.Message;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Messages
{
    public interface IQueueWeChatMsgService : IRepository<QueueWeChatMsg>
    {
    }

    public class QueueWeChatMsgService : Repository<QueueWeChatMsg>, IQueueWeChatMsgService
    {
    }
}