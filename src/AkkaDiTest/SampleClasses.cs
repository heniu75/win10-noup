using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace AkkaDiTest
{
    public interface IMySingletonClass
    {
        int MyId { get; set; }
    }

    public class MySingletonClass : IMySingletonClass
    {
        public static int idx = 0;
        public object lockObject = new object();

        public MySingletonClass()
        {
            lock (lockObject)
            {
                MyId = idx++;
            }
        }

        public int MyId { get; set; }
    }

    public interface IMyTransientClass
    {
        int MyId { get; set; }
    }

    public class MyTransientClass : IMyTransientClass
    {
        public static int idx = 0;
        public object lockObject = new object();

        public MyTransientClass()
        {
            lock (lockObject)
            {
                MyId = idx++;
            }
        }

        public int MyId { get; set; }
    }


}
