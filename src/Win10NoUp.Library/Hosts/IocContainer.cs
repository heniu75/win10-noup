//using System;
//using Autofac;
//using Win10NoUp.Library.Actors;
//using Win10NoUp.Library.FileCopy;
//using Win10NoUp.Library.FileCopy.@new;

//namespace Win10NoUp.Library.Hosts
//{
//    public interface IocContainerWrapper : IDisposable
//    {
//        T Resolve<T>();
//    }

//    public class IocContainer : IocContainerWrapper
//    {
//        private IContainer _container;

//        public IocContainer(IConfigureServices configureServices I)
//        {
//            var builder = new ContainerBuilder();
//            RegisterTypes(builder);
//            _container = builder.Build();
//        }

//        private static string key(object a)
//        {
//            return a.ToString() + "-key";
//        }

//        private static string key(object a, object b)
//        {
//            return a.ToString() + "-" + b.ToString();
//        }

//        private static string key(object a, object b, object c)
//        {
//            return a.ToString() + "-" + b.ToString() + "-" + c.ToString();
//        }

//        public IContainer Container { get { return _container; } }

//        public void Dispose()
//        {
//            if (_container != null)
//            {
//                _container.Dispose();
//                _container = null;
//            }
//        }
//    }
//}
