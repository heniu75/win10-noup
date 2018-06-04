using System;
using System.Collections.Generic;

namespace Win10NoUp.Library.Reflection
{
    public class AllTypeInstances<T> where T : class
    {
        private readonly AllTypes<T> _allTypes;
        private readonly AllInstances<T> _allInstances;

        public AllTypeInstances(AllTypes<T> allTypes, AllInstances<T> allInstances)
        {
            _allTypes = allTypes;
            _allInstances = allInstances;
        }

        public List<Type> Types => _allTypes.Types;
        public List<T> Instances => _allInstances.Instances;
    }
}
