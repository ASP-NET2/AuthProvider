using AuthApi.Models;
using Azure.Core;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace AuthApi.Handlers
{
    public class ServiceBusHandler : IHostedService
    {
        private readonly IConfiguration _configuration;
     //   private IServiceProvider _serviceProvider;

        private readonly ServiceBusSender _sender;
        private readonly ServiceBusClient _client;

        public ServiceBusHandler(IConfiguration configuration)
        {
            _configuration = configuration;
          //  _serviceProvider = serviceProvider;

            _client = new ServiceBusClient(_configuration.GetConnectionString("ServiceBus"));
            _sender = _client.CreateSender(_configuration.GetValue<string>("ServiceBus:SenderQueue"));
        }

        private async Task HandleMessageAsync(ProcessMessageEventArgs args)
        {
            try
            {
                var body = args.Message.Body.ToString();

                if(!string.IsNullOrEmpty(body))
                {
                  var request = JsonConvert.DeserializeObject<VerificationRequest>(body);

                    if(request != null && !string.IsNullOrEmpty(request.Email))
                    {
                        var email = request.Email;

                        //var verificationRequest = new VerificationRequest
                        //{
                        //    Email = request.Email,
                        //    VerificationType = request.VerificationType,
                        //};

                        var jsonMessage = JsonConvert.SerializeObject(email);

                        var verificationMessage = new ServiceBusMessage(jsonMessage)
                        {
                            ContentType = "application/json"
                        };

                        await _sender.SendMessageAsync(verificationMessage);

                    }
                }

            }catch (Exception ex)
            {
                Console.WriteLine($"Error handling message: {ex.Message}");
            }
        }
       

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
