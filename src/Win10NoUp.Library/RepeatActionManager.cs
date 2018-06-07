using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.DI.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win10NoUp.Library.Actions;
using Win10NoUp.Library.Attributes;
using Win10NoUp.Library.Messages;
using Win10NoUp.Library.Reflection;

namespace Win10NoUp.Library
{
    public class RepeatMessage : BaseMessage
    {
        public IRepeatingAction RepeatingAction { get; private set; }
        public static int Idx { get; private set; }
        public RepeatMessage(IRepeatingAction repeatAction)
        {
            RepeatingAction = repeatAction;
            CorrelationId = Idx++.ToString();
        }
    }

    public class RepeatActionWorker : ReceiveActor
    {
        public RepeatActionWorker()
        {
            RepeatMessage repeatMessage = null;
            Receive<RepeatMessage>((msg) =>
            {
                msg.RepeatingAction.Execute();
                if (repeatMessage == null)
                    repeatMessage = new RepeatMessage(msg.RepeatingAction);
                Context.System.Scheduler.ScheduleTellOnce(
                    TimeSpan.FromSeconds(repeatMessage.RepeatingAction.CycleInSeconds),
                    Self, repeatMessage, Self);
            });
        }
    }

    [AutoConfigurationDto]
    public class RepeatActionManagerConfig
    {
        public RepeatActionManagerConfig()
        {
            StartupDelayInSeconds = 10;
        }

        public int StartupDelayInSeconds { get; set; }
    }

    public class RepeatActionManager : ReceiveActor
    {
        public RepeatActionManager(ILogger<RepeatActionManager> logger,
            IOptions<RepeatActionManagerConfig> config, AllTypeInstances<IRepeatAction> repeatActions)
        {
            logger.LogDebug($"In ctor()");

            var children = new List<IActorRef>();
            int idx = 0;
            foreach (var instance in repeatActions.Instances)
            {
                foreach (var action in instance.RepeatingActions)
                {
                    var workerActor = Context.ActorOf(Context.DI().Props<RepeatActionWorker>(),
                        $"{nameof(RepeatActionWorker)}-{action.ActionName}{idx++}");
                    Context.System.Scheduler.ScheduleTellOnce(
                        TimeSpan.FromSeconds(config.Value.StartupDelayInSeconds),
                        workerActor, new RepeatMessage(action), Self);
                    children.Add(workerActor);
                }
            }
        }
    }
}
