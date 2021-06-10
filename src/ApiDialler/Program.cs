using Enfonica.Voice.V1Beta1;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDialler
{
    public class Program
    {
        /// <summary>
        /// The phone number to place the call to, in +E164 format.
        /// For example: +61400999888
        /// </summary>
        const string CALL_TO = "";
        /// <summary>
        /// The caller ID to use for the outgoing call, in +E164 format.
        /// For example: +61721000000
        /// </summary>
        const string CALL_FROM = "";
        /// <summary>
        /// The Enfonica project name to use to create the call.
        /// For example: projects/my-project
        /// </summary>
        const string PROJECT = "";
        /// <summary>
        /// The public URL where this service is accessible.
        /// For example: https://sad89ujd9as9a7sd.ngrok.io
        /// </summary>
        const string PUBLIC_BASE_URL = "";

        public static async Task Main(string[] args)
        {
            // make sure consts have been configured
            if (CALL_TO == "" || CALL_FROM == "" || PROJECT == "" || PUBLIC_BASE_URL == "")
            {
                Console.WriteLine("Set up the constants in Program.cs before continuing");
                await Task.Delay(5000);
                return;
            }

            // start web server
            var host = CreateHostBuilder(args).Build();
            var logger = (ILogger<Program>)host.Services.GetService(typeof(ILogger<Program>));
            var hostRunTask = host.RunAsync();

            // create Enfonica client
            var client = new CallsClientBuilder().Build();

            // start outgoing call
            logger.LogInformation("Creating outgoing API call");
            var request = new CreateCallRequest()
            {
                Parent = PROJECT,
                Call = new Call()
                {
                    To = CALL_TO,
                    From = CALL_FROM,
                    Options = new Call.Types.ApiCallOptions()
                    {
                        StateUpdateUri = new Uri(new Uri(PUBLIC_BASE_URL), "/state-update").ToString()
                    }
                }
            };
            request.Call.Options.HandlerUris.Add(new Uri(new Uri(PUBLIC_BASE_URL), "/handle-call").ToString());
            Call call;
            try
            {
                call = await client.CreateCallAsync(request);
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Failed creating call");
                throw;
            }

            // log the call we've just created
            logger.LogInformation($"Created call {call.Name}");
            logger.LogInformation(
                "If your environment is configured correctly, you should now receive " +
                "status updates for the outgoing call");

            // wait for web server to shut down
            await hostRunTask;
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
