namespace Win10NoUp.Library.Hosts
{
    public interface IMyCoreSingletonService
    {
        int MyId { get; set; }
    }

    public class MyCoreSingletonService : IMyCoreSingletonService
    {
        public static int idx = 0;
        public object lockObject = new object();

        public MyCoreSingletonService()
        {
            lock (lockObject)
            {
                MyId = idx++;
            }
        }

        public int MyId { get; set; }
    }

    public interface IMyCoreTransientService
    {
        int MyId { get; set; }
    }

    public class MyCoreTransientService : IMyCoreTransientService
    {
        public static int idx = 0;
        public object lockObject = new object();

        public MyCoreTransientService()
        {
            lock (lockObject)
            {
                MyId = idx++;
            }
        }

        public int MyId { get; set; }
    }
}
