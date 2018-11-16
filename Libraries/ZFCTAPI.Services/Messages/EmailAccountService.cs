using ZFCTAPI.Data.Message;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Messages
{
    public interface IEmailAccountService : IRepository<EmailAccount>
    {
    }

    public class EmailAccountService : Repository<EmailAccount>, IEmailAccountService
    {
    }
}