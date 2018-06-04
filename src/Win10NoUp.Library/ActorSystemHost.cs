//using Akka.Actor;
//using Akka.Configuration;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Win10NoUp.Library.Config;

//namespace Win10NoUp.Library
//{
//    public interface IActorSystemHost : IStartStop, ISendAnyMessage
//    {
//    }

//    public interface ISendAnyMessage
//    {
//        void Tell(object anyMessage);
//    }

//    public class ActorSystemHost : IActorSystemHost
//    {
//        private readonly IFileSystem _fileSystem;
//        private readonly StopServiceActorConfiguration _stopServiceActorConfiguration;
//        private readonly ILoggerFactory _loggerFactory;
//        private readonly ApplicationConfig _applicationConfig;
//        private ActorSystem _actorSystem;
//        private IActorRef _applicationManager;
//        public static int idx = 0;
//        private object lockObject = new object();
//        private bool _initialised = false;
//        public ActorSystemHost(IFileSystem fileSystem, IOptions<ApplicationConfig> config, IOptions<StopServiceActorConfiguration> stopServiceActorConfiguration, ILoggerFactory loggerFactory)
//        {
//            _fileSystem = fileSystem;
//            _stopServiceActorConfiguration = stopServiceActorConfiguration.Value;
//            _loggerFactory = loggerFactory;
//            _applicationConfig = config.Value;
//            lock (lockObject)
//            {
//                idx++;
//            }
//        }

//        public void Start()
//        {
//            if (!_initialised)
//            {
//                var hoconConfig = ConfigurationFactory.ParseString(AkkaHoconConfig.Hocon);
//                _actorSystem = ActorSystem.Create("Win10NoUpActorSystem", hoconConfig);
//                var applicationManagerProps = Props.Create(() => new ApplicationManagerActor(_fileSystem, _applicationConfig, _stopServiceActorConfiguration, _loggerFactory));
//                _applicationManager = _actorSystem.ActorOf(applicationManagerProps, "applicationManager");
//                _initialised = true;
//            }
//        }

//        public void Stop()
//        {
//            if (_initialised)
//            {
//                _applicationManager.GracefulShutdown();
//                _actorSystem.Terminate();
//                _actorSystem.Dispose();
//                _actorSystem = null;
//                _initialised = false;
//            }
//        }

//        public void Tell(object anyMessage)
//        {
//            if (_initialised)
//            {
//                _applicationManager.Tell(anyMessage);
//            }
//        }
//    }
//}
