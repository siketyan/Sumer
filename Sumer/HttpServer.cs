using System;
using System.IO;
using System.Net;

namespace Sumer
{
    public class HttpServer : IDisposable
    {
        private readonly HttpListener _listener;

        public HttpServer(string prefix)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix);
        }

        public void Start()
        {
            _listener.Start();
            ListenAsync();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private async void ListenAsync()
        {
            while (true)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    using (var response = context.Response)
                    using (var stream = response.OutputStream)
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine("[ OK ] Sumer Bot is now running!");
                    }
                }
                catch
                {
                    // Ignore
                }
            }
        }

        public void Dispose()
        {
            _listener.Close();
        }
    }
}
