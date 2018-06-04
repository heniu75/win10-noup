using System;
using System.Collections.Generic;
using System.Linq;

namespace Win10NoUp.Library.Reflection
{
    public class AllInstances<T> where T : class
    {
        private readonly AllTypes<T> _allTypes;
        private readonly Func<Type, object> _activator;
        private readonly List<T> _instances = new List<T>();

        public AllInstances(AllTypes<T> allTypes, Func<Type, object> activator)
        {
            _allTypes = allTypes;
            _activator = activator;
        }

        public List<T> Instances
        {
            get
            {
                if (_instances.Any()) return _instances;
                foreach (var type in _allTypes.Types)
                {
                    var instance = _activator(type);
                    var typedInstance = instance as T;
                    _instances.Add(typedInstance);
                }
                return _instances;
            }
        }
    }
}