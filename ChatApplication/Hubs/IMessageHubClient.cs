using System.Threading.Tasks;

namespace ChatApplication
{
    public interface IMessageHubClient
    {
        Task NewMessage();
    }
}
