using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Sumer
{
    public class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private ServiceProvider _services;

        private string _prefix;

        public static void Main(string[] args) => new Program().MainAsync(args).GetAwaiter().GetResult();
        public async Task MainAsync(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Usage: Sumer.dll [token] [address] [port] [prefix]");
                Environment.ExitCode = 1;
                return;
            }

            var token = args[0];
            var url = $"http://{args[1]}:{args[2]}/";
            _prefix = args[3];

            Console.WriteLine("Token: (Hidden)");
            Console.WriteLine($"URL: {url}");
            Console.WriteLine($"Prefix: {_prefix}");

            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().BuildServiceProvider();

            _client.MessageReceived += OnMessageReceivedAsync;
            _client.Log += OnLog;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            using (var server = new HttpServer(url))
            {
                server.Start();
                await Task.Delay(-1);
            }
        }

        private async Task OnMessageReceivedAsync(SocketMessage message)
        {
            var userMessage = message as SocketUserMessage;
            if (message is null || message.Author.IsBot)
            {
                return;
            }

            var argumentPosition = 0;
            if (!(userMessage.HasStringPrefix(_prefix, ref argumentPosition) ||
                  userMessage.HasMentionPrefix(_client.CurrentUser, ref argumentPosition)))
            {
                return;
            }

            await userMessage.AddReactionAsync(new Emoji("\uD83D\uDC40"));

            var context = new CommandContext(_client, userMessage);
            var result = await _commands.ExecuteAsync(context, argumentPosition, _services);
            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        private Task OnLog(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}
