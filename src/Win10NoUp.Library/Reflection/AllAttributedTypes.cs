using System;
using System.Collections.Generic;

namespace Win10NoUp.Library.Reflection
{
    public class AllAttributedTypes<T> where T : class
    {
        private List<Type> _types;

        public List<Type> Types
        {
            get { Init(); return _types; }
        }

        private void Init()
        {
            if (_types == null)
                _types = ReflectionUtil.GetTypesWithAttribute<T>();
        }
    }
}
