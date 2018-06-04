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
using Win10NoUp.Library.Reflection;

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

    public class RepeatActionWorker : ReceiveActor
    {
        public RepeatActionWorker(IRepeatAction repeatAction)
        {
            Receive<RepeatMessage>((m) =>
            {
                repeatAction.Execute();
            });
        }
    }

    public class RepeatActionManager : ReceiveActor
    {
        public RepeatActionManager(ILogger<RepeatActionManager> logger,
            AllTypeInstances<IRepeatAction> repeatActions,
            Func<Type, ActorBase> activator)
        {
            logger.LogDebug($"In ctor()");

            var children = new List<IActorRef>();
            int idx = 0;
            foreach (var action in repeatActions.Instances)
            {
                var workerActorProps = Props.Create(() => new RepeatActionWorker(action));
                var workerActor = Context.ActorOf(workerActorProps, $"repeatActionWorker-{idx++}");
                children.Add(workerActor);
            }

            foreach (var action in repeatActions.Instances)
            {
                Context.System.Scheduler.ScheduleTellRepeatedly(
                    TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30),
                    Self, new RepeatMessage(), Self);
            }
        }
    }
}
