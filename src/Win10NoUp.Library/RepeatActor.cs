using System;
using System.Collections.Generic;
using System.ServiceProcess;
using Akka.Actor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win10NoUp.Library.Actions;
using Win10NoUp.Library.Config;
using Win10NoUp.Library.FileCopy;
using Win10NoUp.Library.Messages;

namespace Win10NoUp.Library
{
    public class RepeatMessage : BaseMessage
    {
        public static int Idx { get; private set; }
        public RepeatMessage()
        {
            CorrelationId = Idx++.ToString();
        }
    }

    public interface IRepeatAction
    {
        int OffsetInSeconds { get; }
        int CycleInSeconds { get; }
        void Execute(RepeatMessage message, ILogger logger);
    }

    public class RepeatActorWorker : ReceiveActor
    {
        //public RepeatActorWorker()
    }

    public class RepeatActor : ReceiveActor
    {
        private List<IActorRef> _children = new List<IActorRef>();

        public RepeatActor(ILogger<RepeatActor> logger,
            IRepeatActionCollection repeatActions)
        {
            //var logger = loggerFactory.CreateLogger(this.GetType().Name);
            logger.LogDebug($"In ctor()");

//            var workerProps = Props.Create(() => new FileCopyActor(fileSystem))
               // .WithRouter(new Round(NumberOfWorkers));
          //  _workerRouter = Context.ActorOf(workerProps, "workers");
  //          ActorSystem 
            // get the ball rolling - note the exact same message is sent every single time here
    //        Context.System.Scheduler.ScheduleTellRepeatedly(
      //          TimeSpan.FromSeconds(options.Value.CycleInSeconds), TimeSpan.FromSeconds(options.Value.CycleInSeconds),
        //        Self, new RepeatMessage(), Self);

            Receive<RepeatMessage>((m) =>
            {
                logger.LogDebug($"Received StopServiceMessage {m.CorrelationId}");
          //      repeatAction.Execute(m, logger);
                logger.LogDebug($"Processed {m.CorrelationId}");
            });
        }
    }
}
