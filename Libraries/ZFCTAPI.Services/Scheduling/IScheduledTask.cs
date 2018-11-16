using System.Threading;
using System.Threading.Tasks;


namespace ZFCTAPI.Services.Scheduling
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
