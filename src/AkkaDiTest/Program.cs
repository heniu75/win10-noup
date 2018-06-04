using System;

namespace AkkaDiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //using (var servicesFactory = new CoreServiceProviderFactory(new ConfigureCoreServices()))
            //using (var appHost = new ActorContainerHost(servicesFactory))
            using (var actorHost = new ActorSystemHost(new [] { new ConfigureAutofacActorServices() } ))
            {
                actorHost.Tell("Hello there mr actor host");
                actorHost.Start();
                Console.WriteLine("Press enter to stop");
                Console.ReadLine();
            }
        }
    }
}
