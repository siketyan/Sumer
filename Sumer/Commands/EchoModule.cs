using Discord.Commands;
using System.Threading.Tasks;

namespace Sumer.Commands
{
    public class EchoModule : ModuleBase
    {
        [Command("echo")]
        public async Task EchoAsync(string str)
        {
            await ReplyAsync(str);
        }
    }
}
