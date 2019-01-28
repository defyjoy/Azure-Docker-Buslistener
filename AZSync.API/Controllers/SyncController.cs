using AZSync.API.Messaging;
using AZSync.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AZSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private HubsynctestContext context;
        private readonly IPublisher publisher;
        public SyncController(IPublisher publisher)
        {
            this.context = new HubsynctestContext();
            this.publisher = publisher;
        }

        public async Task<IActionResult> Get()
        {
            var members = await context.Member.Where(x => x.Contact.Count != 0)
                .Include(x => x.Contact).ToListAsync();

            await publisher.PublishBlob(members);
            return Ok(members);
        }
    }
}