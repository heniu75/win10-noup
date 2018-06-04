using System;
using System.Collections.Generic;

namespace Win10NoUp.Library.Reflection
{
    public class AllTypes<T> where T : class
    {
        protected List<Type> _Types;
        public AllTypes()
        {
        }

        public List<Type> Types
        {
            get { Init(); return _Types; }
        }

        private void Init()
        {
            if (_Types == null)
                _Types = ReflectionUtil.GetImplementingTypes<T>();
        }
    }
}
