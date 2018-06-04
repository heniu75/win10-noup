namespace AkkaDiTest
{
    public class AkkaHoconConfig
    {
        public static string Hocon = @"
akka {
            # here we are configuring log levels
            log-config-on-start = off
            stdout-loglevel = INFO
            loglevel = DEBUG
            # this config section will be referenced as akka.actor
}";
    }
}
