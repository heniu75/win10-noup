using System;
using System.Collections.Generic;
using System.Text;

namespace Win10NoUp.Library
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeProvider()
        {
            
        }

        public DateTime Now => DateTime.Now;
    }
}
