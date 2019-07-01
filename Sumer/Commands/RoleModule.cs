using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sumer.Commands
{
    [Group("role")]
    public class RoleModule : ModuleBase
    {
        [Command]
        public async Task ShowByUserAsync(SocketGuildUser user)
        {
            var lines = new List<string>
            {
                Context.User.Mention,
                $"Roles which {user.Username} has:"
            };

            foreach (var role in user.Roles)
            {
                if (role.Name.StartsWith('@'))
                {
                    continue;
                }

                lines.Add($"- {role.Name}");
            }

            await ReplyAsync(string.Join('\n', lines));
        }

        [Command("list")]
        public async Task ListAsync()
        {
            var lines = new List<string>
            {
                Context.User.Mention,
                "All roles in this guild:"
            };

            foreach (var role in Context.Guild.Roles)
            {
                if (role.Name.StartsWith('@') || !role.IsMentionable)
                {
                    continue;
                }

                lines.Add($"- {role.Name}");
            }

            await ReplyAsync(string.Join('\n', lines));
        }

        [Command("gimme")]
        public async Task GiveToCurrentUserAsync(params SocketRole[] roles)
        {
            var user = Context.User as SocketGuildUser;
            foreach (var role in roles)
            {
                if (!role.IsMentionable)
                {
                    await ReplyAsync("The role is not available to grant you.");
                    return;
                }

                await user.AddRoleAsync(role);
            }

            await ReplyAsync($"{Context.User.Mention} Done!");
        }

        [RequireOwner]
        [Command("give")]
        public async Task GiveToUserAsync(SocketGuildUser user, params SocketRole[] roles)
        {
            foreach (var role in roles)
            {
                await user.AddRoleAsync(role);
            }

            await ReplyAsync($"{Context.User.Mention} Done!");
        }

        [Command("bye")]
        public async Task TakeFromCurrentUserAsync(params SocketRole[] roles)
        {
            if (!(Context.User is SocketGuildUser user))
            {
                await ReplyAsync($"{Context.User.Mention} Failed to get who are you.");
                return;
            }

            await user.RemoveRolesAsync(roles);
            await ReplyAsync($"{Context.User.Mention} Done!");
        }

        [RequireOwner]
        [Command("take")]
        public async Task TakeFromUserAsync(SocketGuildUser user, params SocketRole[] roles)
        {
            await user.RemoveRolesAsync(roles);
            await ReplyAsync($"{Context.User.Mention} Done!");
        }
    }
}
