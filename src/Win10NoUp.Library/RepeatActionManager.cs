using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.DI.Core;
using Microsoft.Extensions.Logging;
using Win10NoUp.Library.Actions;
using Win10NoUp.Library.Messages;
using Win10NoUp.Library.Reflection;

namespace Win10NoUp.Library
{
    public class RepeatMessage : BaseMessage
    {
        public IRepeatAction RepeatAction { get; private set; }
        public static int Idx { get; private set; }
        public RepeatMessage(IRepeatAction repeatAction)
        {
            RepeatAction = repeatAction;
            CorrelationId = Idx++.ToString();
        }
    }

    public class RepeatActionWorker : ReceiveActor
    {
        public RepeatActionWorker()
        {
            Receive<RepeatMessage>((msg) =>
            {
                msg.RepeatAction.Execute();
            });
        }
    }

    public class RepeatActionManager : ReceiveActor
    {
        public RepeatActionManager(ILogger<RepeatActionManager> logger,
            AllTypeInstances<IRepeatAction> repeatActions)
        {
            logger.LogDebug($"In ctor()");

            var children = new List<IActorRef>();
            int idx = 0;
            foreach (var action in repeatActions.Instances)
            {
                var workerActor = Context.ActorOf(Context.DI().Props<RepeatActionWorker>(), $"{nameof(RepeatActionWorker)}-{idx++}");
                Context.System.Scheduler.ScheduleTellRepeatedly(
                    TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30),
                    workerActor, new RepeatMessage(action), Self);
                children.Add(workerActor);
            }
        }
    }
}
