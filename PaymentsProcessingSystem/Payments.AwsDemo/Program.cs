using Amazon.DynamoDBv2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Payments.AwsDemo
{
    public static class Program
    {
        private static void Main(string[] args)
        {

            var builder = CreateApplicationBuilder(args);

            using var host = builder.Build();

            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("Payments.AwsDemo");

            // Temporary smoke test: verify dependency registration.
            var dynamoDbClient = host.Services.GetRequiredService<IAmazonDynamoDB>();

            logger.LogInformation(
                "AWS DynamoDB client registered successfully. Client type: {ClientType}",
                dynamoDbClient.GetType().Name);
        }

        private static HostApplicationBuilder CreateApplicationBuilder(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            ConfigureAws(builder);

            return builder;
        }

        private static void ConfigureAws(IHostApplicationBuilder builder)
        {
            var awsOptions = builder.Configuration.GetAWSOptions("AWS");

            builder.Services.AddDefaultAWSOptions(awsOptions);
            builder.Services.AddAWSService<IAmazonDynamoDB>();
        }
    }
}
