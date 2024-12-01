using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using AWSSimpleQueueConsumer.Models;

namespace AWSSimpleQueueConsumer.Services
{
    public class SimpleQueueConsumerHostedService : BackgroundService
    {
        private readonly ILogger<SimpleQueueConsumerHostedService> _logger;
        private readonly ISimpleQueueConsumerService _simpleQueueConsumerService;
        private readonly AWSSimpleQueueConfiguration _sqsConfig;

        public SimpleQueueConsumerHostedService(
            ILogger<SimpleQueueConsumerHostedService> logger,
            ISimpleQueueConsumerService simpleQueueConsumerService,
            AWSSimpleQueueConfiguration sqsConfig)
        {
            _logger = logger;
            _simpleQueueConsumerService = simpleQueueConsumerService;
            _sqsConfig = sqsConfig;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SQS Consumer Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _simpleQueueConsumerService.ReceiveMessageAsync(_sqsConfig.AWSQueueUrl, stoppingToken);

                if (response?.Messages.Count > 0)
                {
                    foreach (var message in response.Messages)
                    {
                        _logger.LogInformation($"Received message: {message.Body}");
                        _logger.LogInformation($"Received message attribute: {message.MessageAttributes["EventType"].StringValue}");

                        await _simpleQueueConsumerService.DeleteMessageAsync(
                            _sqsConfig.AWSQueueUrl, message.ReceiptHandle, stoppingToken);
                    }
                }
                else
                {
                    _logger.LogInformation("No messages received.");
                }
            }

            _logger.LogInformation("SQS Consumer Service is stopping.");
        }
    }
}