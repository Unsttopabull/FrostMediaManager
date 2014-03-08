using System;
using System.Collections.Generic;
using Frost.Common.Models;

namespace Frost.Common {
    public static class ModelCreator {
        private static readonly Dictionary<string, Dictionary<Type, Type>> Systems;

        static ModelCreator() {
            Systems = new Dictionary<string, Dictionary<Type, Type>>();
        }

        public static string CurrentSystem { get; private set; }

        public static IEnumerable<string> KnownSystems { get { return Systems.Keys; }}

        public static bool ChangeSystem(string system) {
            if (!Systems.ContainsKey(system)) {
                return false;
            }
            CurrentSystem = system;
            return true;
        }

        public static bool RemoveSystem(string system) {
            if (!Systems.ContainsKey(system)) {
                return false;
            }

            Systems.Remove(system);
            return true;
        }

        public static bool RegisterSystem(string system) {
            if (Systems.ContainsKey(system)) {
                return false;
            }
            Systems.Add(system, new Dictionary<Type, Type>());
            return true;
        }

        public static void RegisterSystem(IModelRegistrator registrator) {
            SystemModels sm = new SystemModels();
            registrator.Register(sm);

            if (string.IsNullOrEmpty(sm.SystemName)) {
                throw new InvalidOperationException("System has no name");
            }

            if (Systems.ContainsKey(sm.SystemName)) {
                Systems[sm.SystemName] = sm.TypeMappings;
            }
            else {
                Systems.Add(sm.SystemName, sm.TypeMappings);
            }
        }

        public static void Register<TInterface, TImplementation>() where TInterface : IMovieEntity where TImplementation : TInterface, new() {
            if (string.IsNullOrEmpty(CurrentSystem)) {
                throw new InvalidOperationException("No system currently registered");
            }

            Type iType = typeof(TInterface);

            if (Systems[CurrentSystem].ContainsKey(iType)) {
                Systems[CurrentSystem][iType] = typeof(TImplementation);
            }
            else {
                Systems[CurrentSystem].Add(typeof(TInterface), typeof(TImplementation));
            }
        }

        public static TInterface Create<TInterface>() {
            return (TInterface) Activator.CreateInstance(Systems[CurrentSystem][typeof(TInterface)]);
        }
    }

    public class SystemModels {
        internal readonly Dictionary<Type, Type> TypeMappings;

        public SystemModels() {
            TypeMappings = new Dictionary<Type, Type>();
        }

        public string SystemName { get; set; }

        public void Register<TInterface, TImplementation>() where TInterface : IMovieEntity where TImplementation : TInterface, new() {
            Type iType = typeof(TInterface);
            if (TypeMappings.ContainsKey(iType)) {
                TypeMappings[iType] = typeof(TImplementation);
            }
            else {
                TypeMappings.Add(iType, typeof(TImplementation));
            }
        }
    }

}
