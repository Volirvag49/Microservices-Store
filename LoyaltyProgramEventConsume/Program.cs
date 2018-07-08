using System.ServiceProcess;
//using Newtonsoft.Json;
using static System.Console;

namespace LoyaltyProgramEventConsume
{
    public class Program : ServiceBase
    {
        private EventSubscriber subscriber;

        public static void Main(string[] args) => new Program().Main();

        public void Main()
        {
          this.subscriber = new EventSubscriber("localhost:50000");
          //Run(this);
          OnStart(null);
          ReadLine();
        }

    protected override void OnStart(string[] args)
    {
      this.subscriber.Start();
    }

    protected override void OnStop()
    {
      this.subscriber.Stop();
    }
    }
}
