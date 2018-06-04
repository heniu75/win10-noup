using System;
using System.Collections.Generic;

namespace Win10NoUp.Library.Reflection
{
    public class AllTypeInstances<T> where T : class
    {
        private readonly AllTypes<T> _allTypes;
        private readonly AllTypeInstances<T> _allTypeInstances;

        public AllTypeInstances(AllTypes<T> allTypes, AllTypeInstances<T> allTypeInstances)
        {
            _allTypes = allTypes;
            _allTypeInstances = allTypeInstances;
        }

        public List<Type> Types => _allTypes.Types;
        public List<T> Instances => _allTypeInstances.Instances;
    }
}
