using Enfonica.Voice.V1Beta1;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDialler.Controllers
{
    [ApiController]
    public class CallController : ControllerBase
    {
        private readonly ILogger<CallController> _logger;
        private static readonly JsonParser _jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

        public CallController(ILogger<CallController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets called when Enfonica needs instructions about how to control a call.
        /// </summary>
        [HttpPost]
        [Route("/handle-call")]
        public async Task<IActionResult> HandleCall()
        {
            // parse the request
            string json;
            using (var streamReader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                json = await streamReader.ReadToEndAsync();
            }
            var callRequest = _jsonParser.Parse<CallRequest>(json);

            // control the call
            _logger.LogInformation($"Received request to control {callRequest.Call.Name}, generating speech");
            return Content(@"<Response><Say>Hi! This is a test of the API dialler! Good day.</Say></Response>",
                "application/xml");
        }

        /// <summary>
        /// Gets called when the state of an Enfonica call changes.
        /// </summary>
        [HttpPost]
        [Route("/state-update")]
        public async Task<IActionResult> StateUpdate()
        {
            // parse the request
            string json;
            using (var streamReader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                json = await streamReader.ReadToEndAsync();
            }
            var call = _jsonParser.Parse<Call>(json);

            // log the new state
            _logger.LogInformation($"{call.Name} has changed state to {call.State}");
            return NoContent();
        }
    }
}
