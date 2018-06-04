namespace Win10NoUp.Library.Hosts
{
    public interface IMyAutofacSingletonService
    {
        int MyId { get; set; }
    }

    public class MyAutofacSingletonService : IMyAutofacSingletonService
    {
        public static int idx = 0;
        public object lockObject = new object();

        public MyAutofacSingletonService()
        {
            lock (lockObject)
            {
                MyId = idx++;
            }
        }

        public int MyId { get; set; }
    }

    public interface IMyAutofacTransientService
    {
        int MyId { get; set; }
    }

    public class MyAutofacTransientService : IMyAutofacTransientService
    {
        public static int idx = 0;
        public object lockObject = new object();

        public MyAutofacTransientService()
        {
            lock (lockObject)
            {
                MyId = idx++;
            }
        }

        public int MyId { get; set; }
    }
}
