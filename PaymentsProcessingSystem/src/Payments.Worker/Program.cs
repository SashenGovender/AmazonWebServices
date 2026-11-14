using Microsoft.Extensions.Hosting;

namespace Payments.Worker;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        var host = builder.Build();

        host.Run();
    }
}
