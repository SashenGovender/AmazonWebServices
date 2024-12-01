using Amazon.SQS;
using AWSSimpleQueueConsumer.Models;
using AWSSimpleQueueConsumer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace SqsConsumer
{
    public static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Stopped program because of exception - {exception}");
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
              .ConfigureServices((hostContext, services) =>
              {
                  services.AddHostedService<SimpleQueueConsumerHostedService>();

                  //https://stackoverflow.com/questions/47917125/how-to-set-aws-credentials-with-net-core
                  //https://stackoverflow.com/questions/43053495/how-to-set-credentials-on-aws-sdk-on-net-core
                  var awsSqsConfiguration = new AWSSimpleQueueConfiguration();
                  hostContext.Configuration.GetSection("AWSSQSConfiguration").Bind(awsSqsConfiguration);
                  services.AddSingleton(awsSqsConfiguration);

                  var basicCredentials = new Amazon.Runtime.BasicAWSCredentials(
                      awsSqsConfiguration.AWSAccessKey, awsSqsConfiguration.AWSSecretKey);
                  var region = Amazon.RegionEndpoint.GetBySystemName(awsSqsConfiguration.AWSRegion);

                  services.AddSingleton<IAmazonSQS>(sp => new AmazonSQSClient(basicCredentials, region));

                  services.AddSingleton<ISimpleQueueConsumerService, SimpleQueueConsumerService>();
              });
        }
    }
}
