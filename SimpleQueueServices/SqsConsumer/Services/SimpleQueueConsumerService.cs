using Amazon.SQS.Model;
using Amazon.SQS;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AWSSimpleQueueConsumer.Services
{
    public class SimpleQueueConsumerService : ISimpleQueueConsumerService
    {
        private readonly ILogger<SimpleQueueConsumerService> _logger;
        private readonly IAmazonSQS _sqsClient;

        public SimpleQueueConsumerService(ILogger<SimpleQueueConsumerService> logger, IAmazonSQS sqsClient)
        {
            _logger = logger;
            _sqsClient = sqsClient;
        }

        public async Task<ReceiveMessageResponse?> ReceiveMessageAsync(string queueUrl, CancellationToken token)
        {
            try
            {
                //https://stackoverflow.com/questions/23564689/cannot-access-amazon-sqs-message-attributes-in-c-sharp
                var receiveRequest = new ReceiveMessageRequest
                {
                    QueueUrl = queueUrl,
                    MaxNumberOfMessages = 5,
                    WaitTimeSeconds = 10, // Long polling
                    MessageAttributeNames = new List<string> { "EventType" }
                };

                var response = await _sqsClient.ReceiveMessageAsync(receiveRequest, token);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in receiving message: {ex.Message}");
            }

            return null;
        }

        public async Task<DeleteMessageResponse?> DeleteMessageAsync(string queueUrl, string receiptHandle, CancellationToken token)
        {
            try
            {
                var deleteRequest = new DeleteMessageRequest
                {
                    QueueUrl = queueUrl,
                    ReceiptHandle = receiptHandle
                };
                var response = await _sqsClient.DeleteMessageAsync(deleteRequest, token);

                _logger.LogInformation("Message deleted.");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred in deleting message: {Exception}", ex.Message);
            }

            return null;
        }
    }
}