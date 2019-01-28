using System.Collections.Generic;
using System.Threading.Tasks;

namespace AZSync.API.Messaging
{
    public interface IPublisher
    {
        Task PublishBlob<T>(List<T> items);
    }
}