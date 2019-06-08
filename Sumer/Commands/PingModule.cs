using Discord.Commands;
using System.Threading.Tasks;

namespace Sumer.Commands
{
    public class PingModule : ModuleBase
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("pong");
        }
    }
}
