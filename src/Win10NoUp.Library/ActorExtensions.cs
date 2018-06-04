using Akka.Actor;

namespace Win10NoUp.Library
{
    public static class ActorExtensions
    {
        public static void GracefulShutdown(this IActorRef actor)
        {
            actor.Tell(PoisonPill.Instance);
        }
    }
}
