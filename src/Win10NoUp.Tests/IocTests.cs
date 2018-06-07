using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Win10NoUp.Library.Hosts;
using Win10NoUp.Library.Reflection;
using Xunit;

namespace Win10NoUp.Tests
{
    public class IocTests
    {
        public class MyClassDependentOnAFactory
        {
            public bool Created = false;
            public MyClassDependentOnAFactory(Func<IMyAutofacTransientService> factory)
            {
                var instance = factory();
                Assert.True(instance is MyAutofacTransientService);
                Created = true;
            }
        }

        [Fact]
        public void Autofac_supports_default_factories_for_types_already_registered()
        {
            var container = new ContainerBuilder();
            container.RegisterType<MyAutofacTransientService>().As<IMyAutofacTransientService>();
            container.RegisterType<MyClassDependentOnAFactory>();
            using (var provider = container.Build())
            {
                var myClassDependentOnAFactory = provider.Resolve<MyClassDependentOnAFactory>();
                Assert.True(myClassDependentOnAFactory.Created);
            }
        }

        public interface testInterfaceA { }
        public class ClassA : testInterfaceA { }
        public class ClassB : testInterfaceA { }

        [Fact]
        public void AllTypes_class_correctly_identifies_all_instances_implementing_that_type()
        {
            var container = new ContainerBuilder();
            // see https://stackoverflow.com/questions/15226536/register-generic-type-with-autofac
            container.RegisterGeneric(typeof(AllTypes<>)).AsSelf();
            using (var provider = container.Build())
            {
                var allTypesOfInterfaceA = provider.Resolve<AllTypes<testInterfaceA>>();
                Assert.NotNull(allTypesOfInterfaceA);
                Assert.Equal(2, allTypesOfInterfaceA.Types.Count);
                Assert.Contains(allTypesOfInterfaceA.Types, x => x.Name == "ClassA");
                Assert.Contains(allTypesOfInterfaceA.Types, x => x.Name == "ClassB");
            }
        }

        [Fact]
        public void Autofac_can_instantiate_registered_classes_from_AllTypes_at_run_time()
        {
            var container = new ContainerBuilder();
            container.RegisterGeneric(typeof(AllTypes<>)).AsSelf();
            container.RegisterType<ClassA>();
            container.RegisterType<ClassB>();
            using (var provider = container.Build())
            {
                var allTypesOfInterfaceA = provider.Resolve<AllTypes<testInterfaceA>>();
                var instances = new List<object>();
                foreach (var type in allTypesOfInterfaceA.Types)
                {
                    var instance = provider.Resolve(type);
                    instances.Add(instance);
                }
                Assert.Equal(2, instances.Count());
                Assert.Contains(instances, x => x.GetType().Name == "ClassA");
                Assert.Contains(instances, x => x.GetType().Name == "ClassB");
            }
        }

        [Fact]
        public void Autofac_can_enable_allInstances_type_to_resolve_registered_classes_at_run_time()
        {
            var container = new ContainerBuilder();
            container.RegisterGeneric(typeof(AllTypes<>)).AsSelf();
            container.RegisterGeneric(typeof(AllInstances<>)).AsSelf();
            container.RegisterType<ClassA>();
            container.RegisterType<ClassB>();
            container.Register<Func<Type, object>>((c, p) =>
            {
                // see https://stackoverflow.com/questions/20583339/autofac-and-func-factories
                var context = c.Resolve<IComponentContext>();
                return (Type type) =>
                {
                    return context.Resolve(type);
                };
            });

            using (var provider = container.Build())
            {
                var allInstances1 = provider.Resolve<AllInstances<testInterfaceA>>();
                var instances = allInstances1.Instances;
                Assert.Equal(2, instances.Count());
                Assert.Contains(instances, x => x.GetType().Name == "ClassA");
                Assert.Contains(instances, x => x.GetType().Name == "ClassB");
            }
        }

        public class MyTestInterfaceACollection
        {
            public MyTestInterfaceACollection(AllTypeInstances<testInterfaceA> typeInstances)
            {
                Types = typeInstances.Types;
                Instances = typeInstances.Instances;
            }
            public IList<testInterfaceA> Instances { get; private set; }
            public IList<Type> Types { get; private set; }
        }

        [Fact]
        public void Confirm()
        {
            var container = new ContainerBuilder();
            container.RegisterGeneric(typeof(AllTypes<>)).AsSelf();
            container.RegisterGeneric(typeof(AllInstances<>)).AsSelf();
            container.RegisterGeneric(typeof(AllTypeInstances<>)).AsSelf();
            container.RegisterType<ClassA>();
            container.RegisterType<ClassB>();
            container.Register<Func<Type, object>>((c, p) =>
            {
                // see https://stackoverflow.com/questions/20583339/autofac-and-func-factories
                var context = c.Resolve<IComponentContext>();
                return (Type type) =>
                {
                    return context.Resolve(type);
                };
            });
            container.RegisterType<MyTestInterfaceACollection>();

            using (var provider = container.Build())
            {
                var collection = provider.Resolve<MyTestInterfaceACollection>();
                var types = collection.Types;
                Assert.Equal(2, types.Count());
                Assert.Contains(types, x => x.Name == "ClassA");
                Assert.Contains(types, x => x.Name == "ClassB");
                var instances = collection.Instances;
                Assert.Equal(2, instances.Count());
                Assert.Contains(instances, x => x.GetType().Name == "ClassA");
                Assert.Contains(instances, x => x.GetType().Name == "ClassB");
            }
        }
    }
}
