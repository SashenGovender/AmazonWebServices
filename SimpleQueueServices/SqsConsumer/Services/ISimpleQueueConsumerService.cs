using Amazon.SQS.Model;
using System.Threading;
using System.Threading.Tasks;

namespace AWSSimpleQueueConsumer.Services
{
    public interface ISimpleQueueConsumerService
    {
        Task<ReceiveMessageResponse?> ReceiveMessageAsync(string queueUrl, CancellationToken token);
        Task<DeleteMessageResponse?> DeleteMessageAsync(string queueUrl, string receiptHandle, CancellationToken token);
    }
}