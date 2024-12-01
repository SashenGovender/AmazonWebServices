namespace AWSSimpleQueueConsumer.Models
{
    public class AWSSimpleQueueConfiguration
    {
        public string AWSAccessKey { get; init; } = default!;
        public string AWSSecretKey { get; init; } = default!;
        public string AWSRegion { get; init; } = default!;
        public string AWSQueueUrl { get; init; } = default!;
    }
}