using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity;

namespace TelleR.Unity
{
    public class UnityContainerFactory
    {
        public static UnityContainer Container
        {
            get
            {
                if (_instamce == null)
                {
                    _instamce = new UnityContainerFactory();
                }
                return _instamce.GetContainer();
            }
        }

        private UnityContainerFactory() { }

        private UnityContainer GetContainer()
        {
            if (_container == null)
            {
                _container = CreateContainer();
            }
            return _container;
        }

        private UnityContainer CreateContainer()
        {
            var container = new UnityContainer();
            SetupContainer(container);
            return container;
        }

        private void SetupContainer(UnityContainer container)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var foundTypes = assemblies.SelectMany(GetSetupClassesFromAssembly);

            PerformContainerSetup(container, foundTypes);

            AppDomain.CurrentDomain.AssemblyLoad +=
                (o, args) =>
                {
                    PerformContainerSetup(container, GetSetupClassesFromAssembly(args.LoadedAssembly));
                };
        }

        private static void PerformContainerSetup(UnityContainer container, IEnumerable<Type> setupTypes)
        {
            try
            {
                foreach (var setupClass in setupTypes)
                {
                    var instance = Activator.CreateInstance(setupClass) as IUnityContainerSetup;
                    if (instance != null)
                        instance.SetupContainer(container);
                }
            }
            catch (ReflectionTypeLoadException e)
            {
                if (e.LoaderExceptions != null && e.LoaderExceptions.Any())
                {
                    throw e.LoaderExceptions.First();
                }
            }
        }

        private IEnumerable<Type> GetSetupClassesFromAssembly(Assembly assembly)
        {
            if (!assembly.FullName.StartsWith("Microsoft") && !assembly.FullName.StartsWith("System"))
            {
                return assembly.GetTypes().Where(
                    type =>
                    typeof(IUnityContainerSetup).IsAssignableFrom(type) && type != typeof(IUnityContainerSetup));
            }

            return new Type[0];
        }

        private static volatile UnityContainerFactory _instamce;
        private UnityContainer _container;
    }
}
