using System;
using System.Collections.Generic;
using System.Text;
using Win10NoUp.Library.Reflection;

namespace Win10NoUp.Library.Actions
{
    public interface IRepeatActionCollection
    {
        IList<IRepeatAction> RepeatActions { get; }
    }

    public class RepeatActionCollection : IRepeatActionCollection
    {
        public RepeatActionCollection(AllTypes<IRepeatAction> repeatActionTypes)
        {

        }

        public IList<IRepeatAction> RepeatActions { get; private set; }
    }
}
