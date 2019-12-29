using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Potestas.Configuration;
using Potestas.Exceptions;
using Potestas.Logging;

namespace Potestas
{
    /* TASK. Implement method Load to load factories interfaces from assembly provided.
     * 1. Consider some classes could be private.
     * 2. Consider using special attribute to exclude some factories from creation.
     * 3. Consider refactoring of factory interfaces.
     * 4. Consider making an extension for Assembly class.
     */
    public class FactoriesLoader<T> where T : IEnergyObservation
    {
        public (ISourceFactory<T>[] SourceFactories, IProcessingFactory<T>[] ProcessingFactories) Load(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var assemblyTypes = assembly.GetTypes();
            var sourceFactories = this.GetFactories<ISourceFactory<T>>(assemblyTypes);
            var processingFactories = this.GetFactories<IProcessingFactory<T>>(assemblyTypes);

            return (sourceFactories, processingFactories);
        }

        private TIFactory[] GetFactories<TIFactory>(Type[] assemblyTypes)
        {
            var factories = assemblyTypes.Where(type => this.CheckType(type, typeof(TIFactory).GetGenericTypeDefinition()))
                .Select(type => this.CreateFactoryInstance<TIFactory>(type, assemblyTypes)).ToArray();

            return factories;
        }

        private TIFactory CreateFactoryInstance<TIFactory>(Type typeToBeCreated, Type[] assemblyTypes)
        {
            var typeConstructors = typeToBeCreated.GetConstructors();

            if (typeConstructors.Any(c => c.GetParameters().Length == 0))
            {
                return this.CreateInstanceOfTypeWithoutParameters<TIFactory>(typeToBeCreated);
            }

            var (typeConstructorParams, isConfigurationParamOnly) = this.GetConstructorTypeParams(typeConstructors);

           var resolvedConstructorTypes = this.GetResolvedConstructorParams(typeToBeCreated, isConfigurationParamOnly, typeConstructorParams, assemblyTypes);

            return typeToBeCreated.ContainsGenericParameters
                ? (TIFactory)Activator.CreateInstance(typeToBeCreated.MakeGenericType(typeof(T)), resolvedConstructorTypes)
                : (TIFactory)Activator.CreateInstance(typeToBeCreated, resolvedConstructorTypes);
        }

        private TIFactory CreateInstanceOfTypeWithoutParameters<TIFactory>(Type typeToBeCreated)
        {
            return typeToBeCreated.ContainsGenericParameters
                ? (TIFactory)Activator.CreateInstance(typeToBeCreated.MakeGenericType(typeof(T)))
                : (TIFactory)Activator.CreateInstance(typeToBeCreated);
        }

        private (ParameterInfo[], bool) GetConstructorTypeParams(ConstructorInfo[] typeConstructors)
        {
            var isConfigurationParamOnly = false;
            var typeConstructorParams = Array.Empty<ParameterInfo>();
            foreach (var typeConstructor in typeConstructors)
            {
                typeConstructorParams = typeConstructor.GetParameters();
                if (typeConstructorParams.Length == 1 && typeConstructorParams[0].ParameterType == typeof(IConfiguration))
                {
                    isConfigurationParamOnly = true;
                    break;
                }
            }

            return (typeConstructorParams, isConfigurationParamOnly);
        }

        private object[] GetResolvedConstructorParams(Type typeToBeCreated, bool isConfigurationParamOnly, ParameterInfo[] typeConstructorParams, Type[] assemblyTypes)
        {
            var resolvedConstructorTypes = new List<object>();
            if (isConfigurationParamOnly)
            {
                if (assemblyTypes.Any(t => t.IsClass && t.GetInterfaces().Any(i => i == typeof(IConfiguration))))
                {
	                resolvedConstructorTypes.Add(Activator.CreateInstance(assemblyTypes.First(t =>
		                t.IsClass && t.GetInterfaces().Any(i => i == typeof(IConfiguration)))));
                }
                else
                {
                    resolvedConstructorTypes.Add(Activator.CreateInstance(typeof(Configuration.Configuration)));
                }
            }
            else
            {
	            resolvedConstructorTypes.AddRange(typeConstructorParams.Select(param =>
		            Activator.CreateInstance(
			            assemblyTypes.FirstOrDefault(t =>
				            t.IsClass && (param.ParameterType == t ||
				                          t.GetInterfaces().Any(i => param.ParameterType == i))) ??
			            throw new NotRecognizedFactoryConstructorParameterException(typeToBeCreated, param.ParameterType))));
            }

            return resolvedConstructorTypes.ToArray();
        }

        private bool CheckType(Type type, Type implementedInterface)
        {
            return type.IsClass && !type.IsAbstract && type.IsPublic &&
                   type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == implementedInterface) &&
                   !type.IsDefined(typeof(ExcludeFactoryCreationAttribute));
        }
    }
}
