using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QompanyVKApp.Data;
using VkNet;
using VkNet.Model.RequestParams;

namespace QompanyVKApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var vk = new VkApi();
            vk.Authorize(new ApiAuthParams
            {
                AccessToken = "40099039dc84bbba636fff7051de15181a2da99a439ae66557e5646a64f36d75faebb023eb4cc7e2d86f6"
            });
            var send = vk.Messages.Send(new MessagesSendParams
            {
                Message = "TestFrom .NET",
                UserId = 12520313,
            });

            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
