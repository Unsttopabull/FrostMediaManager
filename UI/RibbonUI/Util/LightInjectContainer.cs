﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using LightInject;

namespace Frost.RibbonUI.Util {

    public static class LightInjectContainer {
        private static readonly ServiceContainer Container = new ServiceContainer();

        public static void Dispose() {
            if (Container != null) {
                Container.Dispose();
            }
        }

        #region Method Wrappers

        /// <summary>
        /// Registers the <paramref name="serviceType"/> with the <paramref name="implementingType"/>.
        /// </summary>
        /// <param name="serviceType">The service type to register.</param><param name="implementingType">The implementing type.</param>
        public static void Register(Type serviceType, Type implementingType) {
            Container.Register(serviceType, implementingType);
        }

        /// <summary>
        /// Registers the <paramref name="serviceType"/> with the <paramref name="implementingType"/>.
        /// </summary>
        /// <param name="serviceType">The service type to register.</param><param name="implementingType">The implementing type.</param><param name="lifetime">The <see cref="T:LightInject.ILifetime"/> instance that controls the lifetime of the registered service.</param>
        public static void Register(Type serviceType, Type implementingType, ILifetime lifetime) {
            Container.Register(serviceType, implementingType, lifetime);
        }

        /// <summary>
        /// Registers the <paramref name="serviceType"/> with the <paramref name="implementingType"/>.
        /// </summary>
        /// <param name="serviceType">The service type to register.</param><param name="implementingType">The implementing type.</param><param name="serviceName">The name of the service.</param>
        public static void Register(Type serviceType, Type implementingType, string serviceName) {
            Container.Register(serviceType, implementingType, serviceName);
        }

        /// <summary>
        /// Registers the <paramref name="serviceType"/> with the <paramref name="implementingType"/>.
        /// </summary>
        /// <param name="serviceType">The service type to register.</param><param name="implementingType">The implementing type.</param><param name="serviceName">The name of the service.</param><param name="lifetime">The <see cref="T:LightInject.ILifetime"/> instance that controls the lifetime of the registered service.</param>
        public static void Register(Type serviceType, Type implementingType, string serviceName, ILifetime lifetime) {
            Container.Register(serviceType, implementingType, serviceName, lifetime);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam><typeparam name="TImplementation">The implementing type.</typeparam>
        public static void Register<TService, TImplementation>() where TImplementation : TService {
            Container.Register<TService, TImplementation>();
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam><typeparam name="TImplementation">The implementing type.</typeparam><param name="lifetime">The <see cref="T:LightInject.ILifetime"/> instance that controls the lifetime of the registered service.</param>
        public static void Register<TService, TImplementation>(ILifetime lifetime) where TImplementation : TService {
            Container.Register<TService, TImplementation>(lifetime);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam><typeparam name="TImplementation">The implementing type.</typeparam><param name="serviceName">The name of the service.</param>
        public static void Register<TService, TImplementation>(string serviceName) where TImplementation : TService {
            Container.Register<TService, TImplementation>(serviceName);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam><typeparam name="TImplementation">The implementing type.</typeparam><param name="serviceName">The name of the service.</param><param name="lifetime">The <see cref="T:LightInject.ILifetime"/> instance that controls the lifetime of the registered service.</param>
        public static void Register<TService, TImplementation>(string serviceName, ILifetime lifetime) where TImplementation : TService {
            Container.Register<TService, TImplementation>(serviceName, lifetime);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the given <paramref name="instance"/>. 
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam><param name="instance">The instance returned when this service is requested.</param>
        public static void RegisterInstance<TService>(TService instance) {
            Container.RegisterInstance<TService>(instance);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the given <paramref name="instance"/>. 
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam><param name="instance">The instance returned when this service is requested.</param><param name="serviceName">The name of the service.</param>
        public static void RegisterInstance<TService>(TService instance, string serviceName) {
            Container.RegisterInstance<TService>(instance, serviceName);
        }

        /// <summary>
        /// Registers the <paramref name="serviceType"/> with the given <paramref name="instance"/>. 
        /// </summary>
        /// <param name="serviceType">The service type to register.</param><param name="instance">The instance returned when this service is requested.</param>
        public static void RegisterInstance(Type serviceType, object instance) {
            Container.RegisterInstance(serviceType, instance);
        }

        /// <summary>
        /// Registers the <paramref name="serviceType"/> with the given <paramref name="instance"/>. 
        /// </summary>
        /// <param name="serviceType">The service type to register.</param><param name="instance">The instance returned when this service is requested.</param><param name="serviceName">The name of the service.</param>
        public static void RegisterInstance(Type serviceType, object instance, string serviceName) {
            Container.RegisterInstance(serviceType, instance, serviceName);
        }

        /// <summary>
        /// Registers a concrete type as a service.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam>
        public static void Register<TService>() {
            Container.Register<TService>();
        }

        /// <summary>
        /// Registers a concrete type as a service.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam><param name="lifetime">The <see cref="T:LightInject.ILifetime"/> instance that controls the lifetime of the registered service.</param>
        public static void Register<TService>(ILifetime lifetime) {
            Container.Register<TService>(lifetime);
        }

        /// <summary>
        /// Registers a concrete type as a service.
        /// </summary>
        /// <param name="serviceType">The concrete type to register.</param>
        public static void Register(Type serviceType) {
            Container.Register(serviceType);
        }

        /// <summary>
        /// Registers a concrete type as a service.
        /// </summary>
        /// <param name="serviceType">The concrete type to register.</param><param name="lifetime">The <see cref="T:LightInject.ILifetime"/> instance that controls the lifetime of the registered service.</param>
        public static void Register(Type serviceType, ILifetime lifetime) {
            Container.Register(serviceType, lifetime);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <paramref name="factory"/> that 
        ///             describes the dependencies of the service. 
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam><param name="factory">A factory delegate used to create the <typeparamref name="TService"/> instance.</param>
        public static void Register<TService>(Expression<Func<IServiceFactory, TService>> factory) {
            Container.Register<TService>(factory);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <paramref name="factory"/> that 
        ///             describes the dependencies of the service. 
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam><typeparam name="TService">The service type to register.</typeparam><param name="factory">A factory delegate used to create the <typeparamref name="TService"/> instance.</param>
        public static void Register<T, TService>(Expression<Func<IServiceFactory, T, TService>> factory) {
            Container.Register<T, TService>(factory);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <paramref name="factory"/> that 
        ///             describes the dependencies of the service. 
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam><typeparam name="TService">The service type to register.</typeparam><param name="factory">A factory delegate used to create the <typeparamref name="TService"/> instance.</param><param name="serviceName">The name of the service.</param>
        public static void Register<T, TService>(Expression<Func<IServiceFactory, T, TService>> factory, string serviceName) {
            Container.Register<T, TService>(factory, serviceName);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <paramref name="factory"/> that 
        ///             describes the dependencies of the service. 
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam><typeparam name="T2">The type of the second parameter.</typeparam><typeparam name="TService">The service type to register.</typeparam><param name="factory">A factory delegate used to create the <typeparamref name="TService"/> instance.</param>
        public static void Register<T1, T2, TService>(Expression<Func<IServiceFactory, T1, T2, TService>> factory) {
            Container.Register<T1, T2, TService>(factory);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <paramref name="factory"/> that 
        ///             describes the dependencies of the service. 
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam><typeparam name="T2">The type of the second parameter.</typeparam><typeparam name="TService">The service type to register.</typeparam><param name="factory">A factory delegate used to create the <typeparamref name="TService"/> instance.</param><param name="serviceName">The name of the service.</param>
        public static void Register<T1, T2, TService>(Expression<Func<IServiceFactory, T1, T2, TService>> factory, string serviceName) {
            Container.Register<T1, T2, TService>(factory, serviceName);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <paramref name="factory"/> that 
        ///             describes the dependencies of the service. 
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam><typeparam name="T2">The type of the second parameter.</typeparam><typeparam name="T3">The type of the third parameter.</typeparam><typeparam name="TService">The service type to register.</typeparam><param name="factory">A factory delegate used to create the <typeparamref name="TService"/> instance.</param>
        public static void Register<T1, T2, T3, TService>(Expression<Func<IServiceFactory, T1, T2, T3, TService>> factory) {
            Container.Register<T1, T2, T3, TService>(factory);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <paramref name="factory"/> that 
        ///             describes the dependencies of the service. 
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam><typeparam name="T2">The type of the second parameter.</typeparam><typeparam name="T3">The type of the third parameter.</typeparam><typeparam name="TService">The service type to register.</typeparam><param name="factory">A factory delegate used to create the <typeparamref name="TService"/> instance.</param><param name="serviceName">The name of the service.</param>
        public static void Register<T1, T2, T3, TService>(Expression<Func<IServiceFactory, T1, T2, T3, TService>> factory, string serviceName) {
            Container.Register<T1, T2, T3, TService>(factory, serviceName);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <paramref name="factory"/> that 
        ///             describes the dependencies of the service. 
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam><typeparam name="T2">The type of the second parameter.</typeparam><typeparam name="T3">The type of the third parameter.</typeparam><typeparam name="T4">The type of the fourth parameter.</typeparam><typeparam name="TService">The service type to register.</typeparam><param name="factory">A factory delegate used to create the <typeparamref name="TService"/> instance.</param>
        public static void Register<T1, T2, T3, T4, TService>(Expression<Func<IServiceFactory, T1, T2, T3, T4, TService>> factory) {
            Container.Register<T1, T2, T3, T4, TService>(factory);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <paramref name="factory"/> that 
        ///             describes the dependencies of the service. 
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam><typeparam name="T2">The type of the second parameter.</typeparam><typeparam name="T3">The type of the third parameter.</typeparam><typeparam name="T4">The type of the fourth parameter.</typeparam><typeparam name="TService">The service type to register.</typeparam><param name="factory">A factory delegate used to create the <typeparamref name="TService"/> instance.</param><param name="serviceName">The name of the service.</param>
        public static void Register<T1, T2, T3, T4, TService>(Expression<Func<IServiceFactory, T1, T2, T3, T4, TService>> factory, string serviceName) {
            Container.Register<T1, T2, T3, T4, TService>(factory, serviceName);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <paramref name="expression"/> that 
        ///             describes the dependencies of the service. 
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam><param name="expression">The lambdaExpression that describes the dependencies of the service.</param><param name="lifetime">The <see cref="T:LightInject.ILifetime"/> instance that controls the lifetime of the registered service.</param>
        public static void Register<TService>(Expression<Func<IServiceFactory, TService>> expression, ILifetime lifetime) {
            Container.Register<TService>(expression, lifetime);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <paramref name="expression"/> that 
        ///             describes the dependencies of the service. 
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam><param name="expression">The lambdaExpression that describes the dependencies of the service.</param><param name="serviceName">The name of the service.</param>
        public static void Register<TService>(Expression<Func<IServiceFactory, TService>> expression, string serviceName) {
            Container.Register<TService>(expression, serviceName);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the <paramref name="expression"/> that 
        ///             describes the dependencies of the service. 
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam><param name="expression">The lambdaExpression that describes the dependencies of the service.</param><param name="serviceName">The name of the service.</param><param name="lifetime">The <see cref="T:LightInject.ILifetime"/> instance that controls the lifetime of the registered service.</param>
        public static void Register<TService>(Expression<Func<IServiceFactory, TService>> expression, string serviceName, ILifetime lifetime) {
            Container.Register<TService>(expression, serviceName, lifetime);
        }

        /// <summary>
        /// Registers a custom factory delegate used to create services that is otherwise unknown to the service container.
        /// </summary>
        /// <param name="predicate">Determines if the service can be created by the <paramref name="factory"/> delegate.</param><param name="factory">Creates a service instance according to the <paramref name="predicate"/> predicate.</param>
        public static void RegisterFallback(Func<Type, string, bool> predicate, Func<ServiceRequest, object> factory) {
            Container.RegisterFallback(predicate, factory);
        }

        /// <summary>
        /// Registers a custom factory delegate used to create services that is otherwise unknown to the service container.
        /// </summary>
        /// <param name="predicate">Determines if the service can be created by the <paramref name="factory"/> delegate.</param><param name="factory">Creates a service instance according to the <paramref name="predicate"/> predicate.</param><param name="lifetime">The <see cref="T:LightInject.ILifetime"/> instance that controls the lifetime of the registered service.</param>
        public static void RegisterFallback(Func<Type, string, bool> predicate, Func<ServiceRequest, object> factory, ILifetime lifetime) {
            Container.RegisterFallback(predicate, factory, lifetime);
        }

        /// <summary>
        /// Registers a service based on a <see cref="T:LightInject.ServiceRegistration"/> instance.
        /// </summary>
        /// <param name="serviceRegistration">The <see cref="T:LightInject.ServiceRegistration"/> instance that contains service metadata.</param>
        public static void Register(ServiceRegistration serviceRegistration) {
            Container.Register(serviceRegistration);
        }

        /// <summary>
        /// Registers services from the given <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to be scanned for services.</param>
        /// <remarks>
        /// If the target <paramref name="assembly"/> contains an implementation of the <see cref="T:LightInject.ICompositionRoot"/> interface, this 
        ///             will be used to configure the container.
        /// </remarks>
        public static void RegisterAssembly(Assembly assembly) {
            Container.RegisterAssembly(assembly);
        }

        /// <summary>
        /// Registers services from the given <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to be scanned for services.</param><param name="shouldRegister">A function delegate that determines if a service implementation should be registered.</param>
        /// <remarks>
        /// If the target <paramref name="assembly"/> contains an implementation of the <see cref="T:LightInject.ICompositionRoot"/> interface, this 
        ///             will be used to configure the container.
        /// </remarks>
        public static void RegisterAssembly(Assembly assembly, Func<Type, Type, bool> shouldRegister) {
            Container.RegisterAssembly(assembly, shouldRegister);
        }

        /// <summary>
        /// Registers services from the given <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to be scanned for services.</param><param name="lifetime">The <see cref="T:LightInject.ILifetime"/> instance that controls the lifetime of the registered service.</param>
        /// <remarks>
        /// If the target <paramref name="assembly"/> contains an implementation of the <see cref="T:LightInject.ICompositionRoot"/> interface, this 
        ///             will be used to configure the container.
        /// </remarks>
        public static void RegisterAssembly(Assembly assembly, Func<ILifetime> lifetime) {
            Container.RegisterAssembly(assembly, lifetime);
        }

        /// <summary>
        /// Registers services from the given <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to be scanned for services.</param><param name="lifetimeFactory">The <see cref="T:LightInject.ILifetime"/> factory that controls the lifetime of the registered service.</param><param name="shouldRegister">A function delegate that determines if a service implementation should be registered.</param>
        /// <remarks>
        /// If the target <paramref name="assembly"/> contains an implementation of the <see cref="T:LightInject.ICompositionRoot"/> interface, this 
        ///             will be used to configure the container.
        /// </remarks>
        public static void RegisterAssembly(Assembly assembly, Func<ILifetime> lifetimeFactory, Func<Type, Type, bool> shouldRegister) {
            Container.RegisterAssembly(assembly, lifetimeFactory, shouldRegister);
        }

        /// <summary>
        /// Registers services from the given <typeparamref name="TCompositionRoot"/> type.
        /// </summary>
        /// <typeparam name="TCompositionRoot">The type of <see cref="T:LightInject.ICompositionRoot"/> to register from.</typeparam>
        public static void RegisterFrom<TCompositionRoot>() where TCompositionRoot : ICompositionRoot, new() {
            Container.RegisterFrom<TCompositionRoot>();
        }

        /// <summary>
        /// Registers services from assemblies in the base directory that matches the <paramref name="searchPattern"/>.
        /// </summary>
        /// <param name="searchPattern">The search pattern used to filter the assembly files.</param>
        public static void RegisterAssembly(string searchPattern) {
            Container.RegisterAssembly(searchPattern);
        }

        /// <summary>
        /// Decorates the <paramref name="serviceType"/> with the given <paramref name="decoratorType"/>.
        /// </summary>
        /// <param name="serviceType">The target service type.</param><param name="decoratorType">The decorator type used to decorate the <paramref name="serviceType"/>.</param><param name="predicate">A function delegate that determines if the <paramref name="decoratorType"/>
        ///             should be applied to the target <paramref name="serviceType"/>.</param>
        public static void Decorate(Type serviceType, Type decoratorType, Func<ServiceRegistration, bool> predicate) {
            Container.Decorate(serviceType, decoratorType, predicate);
        }

        /// <summary>
        /// Decorates the <paramref name="serviceType"/> with the given <paramref name="decoratorType"/>.
        /// </summary>
        /// <param name="serviceType">The target service type.</param><param name="decoratorType">The decorator type used to decorate the <paramref name="serviceType"/>.</param>
        public static void Decorate(Type serviceType, Type decoratorType) {
            Container.Decorate(serviceType, decoratorType);
        }

        /// <summary>
        /// Decorates the <typeparamref name="TService"/> with the given <typeparamref name="TDecorator"/>.
        /// </summary>
        /// <typeparam name="TService">The target service type.</typeparam><typeparam name="TDecorator">The decorator type used to decorate the <typeparamref name="TService"/>.</typeparam>
        public static void Decorate<TService, TDecorator>() where TDecorator : TService {
            Container.Decorate<TService, TDecorator>();
        }

        /// <summary>
        /// Decorates the <typeparamref name="TService"/> using the given decorator <paramref name="factory"/>.
        /// </summary>
        /// <typeparam name="TService">The target service type.</typeparam><param name="factory">A factory delegate used to create a decorator instance.</param>
        public static void Decorate<TService>(Expression<Func<IServiceFactory, TService, TService>> factory) {
            Container.Decorate<TService>(factory);
        }

        /// <summary>
        /// Registers a decorator based on a <see cref="T:LightInject.DecoratorRegistration"/> instance.
        /// </summary>
        /// <param name="decoratorRegistration">The <see cref="T:LightInject.DecoratorRegistration"/> instance that contains the decorator metadata.</param>
        public static void Decorate(DecoratorRegistration decoratorRegistration) {
            Container.Decorate(decoratorRegistration);
        }

        /// <summary>
        /// Gets a list of <see cref="T:LightInject.ServiceRegistration"/> instances that represents the 
        ///             registered services.          
        /// </summary>
        public static IEnumerable<ServiceRegistration> AvailableServices {
            get { return Container.AvailableServices; }
        }

        /// <summary>
        /// Gets an instance of the given <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type of the requested service.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static object GetInstance(Type serviceType) {
            return Container.GetInstance(serviceType);
        }

        /// <summary>
        /// Gets an instance of the given <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type of the requested service.</param><param name="arguments">The arguments to be passed to the target instance.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static object GetInstance(Type serviceType, object[] arguments) {
            return Container.GetInstance(serviceType, arguments);
        }

        /// <summary>
        /// Gets an instance of the given <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type of the requested service.</param><param name="serviceName">The name of the requested service.</param><param name="arguments">The arguments to be passed to the target instance.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static object GetInstance(Type serviceType, string serviceName, object[] arguments) {
            return Container.GetInstance(serviceType, serviceName, arguments);
        }

        /// <summary>
        /// Gets a named instance of the given <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type of the requested service.</param><param name="serviceName">The name of the requested service.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static object GetInstance(Type serviceType, string serviceName) {
            return Container.GetInstance(serviceType, serviceName);
        }

        /// <summary>
        /// Gets an instance of the given <typeparamref name="TService"/> type.
        /// </summary>
        /// <typeparam name="TService">The type of the requested service.</typeparam>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static TService GetInstance<TService>() {
            return Container.GetInstance<TService>();
        }

        /// <summary>
        /// Gets a named instance of the given <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the requested service.</typeparam><param name="serviceName">The name of the requested service.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static TService GetInstance<TService>(string serviceName) {
            return Container.GetInstance<TService>(serviceName);
        }

        /// <summary>
        /// Gets an instance of the given <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam><typeparam name="TService">The type of the requested service.</typeparam><param name="value">The argument value.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static TService GetInstance<T, TService>(T value) {
            return Container.GetInstance<T, TService>(value);
        }

        /// <summary>
        /// Gets an instance of the given <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam><typeparam name="TService">The type of the requested service.</typeparam><param name="value">The argument value.</param><param name="serviceName">The name of the requested service.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static TService GetInstance<T, TService>(T value, string serviceName) {
            return Container.GetInstance<T, TService>(value, serviceName);
        }

        /// <summary>
        /// Gets an instance of the given <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam><typeparam name="T2">The type of the second parameter.</typeparam><typeparam name="TService">The type of the requested service.</typeparam><param name="arg1">The first argument value.</param><param name="arg2">The second argument value.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static TService GetInstance<T1, T2, TService>(T1 arg1, T2 arg2) {
            return Container.GetInstance<T1, T2, TService>(arg1, arg2);
        }

        /// <summary>
        /// Gets an instance of the given <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam><typeparam name="T2">The type of the second parameter.</typeparam><typeparam name="TService">The type of the requested service.</typeparam><param name="arg1">The first argument value.</param><param name="arg2">The second argument value.</param><param name="serviceName">The name of the requested service.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static TService GetInstance<T1, T2, TService>(T1 arg1, T2 arg2, string serviceName) {
            return Container.GetInstance<T1, T2, TService>(arg1, arg2, serviceName);
        }

        /// <summary>
        /// Gets an instance of the given <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam><typeparam name="T2">The type of the second parameter.</typeparam><typeparam name="T3">The type of the third parameter.</typeparam><typeparam name="TService">The type of the requested service.</typeparam><param name="arg1">The first argument value.</param><param name="arg2">The second argument value.</param><param name="arg3">The third argument value.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static TService GetInstance<T1, T2, T3, TService>(T1 arg1, T2 arg2, T3 arg3) {
            return Container.GetInstance<T1, T2, T3, TService>(arg1, arg2, arg3);
        }

        /// <summary>
        /// Gets an instance of the given <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam><typeparam name="T2">The type of the second parameter.</typeparam><typeparam name="T3">The type of the third parameter.</typeparam><typeparam name="TService">The type of the requested service.</typeparam><param name="arg1">The first argument value.</param><param name="arg2">The second argument value.</param><param name="arg3">The third argument value.</param><param name="serviceName">The name of the requested service.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static TService GetInstance<T1, T2, T3, TService>(T1 arg1, T2 arg2, T3 arg3, string serviceName) {
            return Container.GetInstance<T1, T2, T3, TService>(arg1, arg2, arg3, serviceName);
        }

        /// <summary>
        /// Gets an instance of the given <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam><typeparam name="T2">The type of the second parameter.</typeparam><typeparam name="T3">The type of the third parameter.</typeparam><typeparam name="T4">The type of the fourth parameter.</typeparam><typeparam name="TService">The type of the requested service.</typeparam><param name="arg1">The first argument value.</param><param name="arg2">The second argument value.</param><param name="arg3">The third argument value.</param><param name="arg4">The fourth argument value.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static TService GetInstance<T1, T2, T3, T4, TService>(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            return Container.GetInstance<T1, T2, T3, T4, TService>(arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Gets an instance of the given <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam><typeparam name="T2">The type of the second parameter.</typeparam><typeparam name="T3">The type of the third parameter.</typeparam><typeparam name="T4">The type of the fourth parameter.</typeparam><typeparam name="TService">The type of the requested service.</typeparam><param name="arg1">The first argument value.</param><param name="arg2">The second argument value.</param><param name="arg3">The third argument value.</param><param name="arg4">The fourth argument value.</param><param name="serviceName">The name of the requested service.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        public static TService GetInstance<T1, T2, T3, T4, TService>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, string serviceName) {
            return Container.GetInstance<T1, T2, T3, T4, TService>(arg1, arg2, arg3, arg4, serviceName);
        }

        /// <summary>
        /// Gets an instance of the given <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type of the requested service.</param>
        /// <returns>
        /// The requested service instance if available, otherwise null.
        /// </returns>
        public static object TryGetInstance(Type serviceType) {
            return Container.TryGetInstance(serviceType);
        }

        /// <summary>
        /// Gets a named instance of the given <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type of the requested service.</param><param name="serviceName">The name of the requested service.</param>
        /// <returns>
        /// The requested service instance if available, otherwise null.
        /// </returns>
        public static object TryGetInstance(Type serviceType, string serviceName) {
            return Container.TryGetInstance(serviceType, serviceName);
        }

        /// <summary>
        /// Tries to get an instance of the given <typeparamref name="TService"/> type.
        /// </summary>
        /// <typeparam name="TService">The type of the requested service.</typeparam>
        /// <returns>
        /// The requested service instance if available, otherwise default(T).
        /// </returns>
        public static TService TryGetInstance<TService>() {
            return Container.TryGetInstance<TService>();
        }

        /// <summary>
        /// Tries to get an instance of the given <typeparamref name="TService"/> type.
        /// </summary>
        /// <typeparam name="TService">The type of the requested service.</typeparam><param name="serviceName">The name of the requested service.</param>
        /// <returns>
        /// The requested service instance if available, otherwise default(T).
        /// </returns>
        public static TService TryGetInstance<TService>(string serviceName) {
            return Container.TryGetInstance<TService>(serviceName);
        }

        /// <summary>
        /// Gets all instances of the given <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type of services to resolve.</param>
        /// <returns>
        /// A list that contains all implementations of the <paramref name="serviceType"/>.
        /// </returns>
        public static IEnumerable<object> GetAllInstances(Type serviceType) {
            return Container.GetAllInstances(serviceType);
        }

        /// <summary>
        /// Gets all instances of type <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">The type of services to resolve.</typeparam>
        /// <returns>
        /// A list that contains all implementations of the <typeparamref name="TService"/> type.
        /// </returns>
        public static IEnumerable<TService> GetAllInstances<TService>() {
            return Container.GetAllInstances<TService>();
        }

        /// <summary>
        /// Returns <b>true</b> if the container can create the requested service, otherwise <b>false</b>.
        /// </summary>
        /// <param name="serviceType">The <see cref="T:System.Type"/> of the service.</param><param name="serviceName">The name of the service.</param>
        /// <returns>
        /// <b>true</b> if the container can create the requested service, otherwise <b>false</b>.
        /// </returns>
        public static bool CanGetInstance(Type serviceType, string serviceName) {
            return Container.CanGetInstance(serviceType, serviceName);
        }

        /// <summary>
        /// Returns <b>true</b> if the container can create the requested service, otherwise <b>false</b>.
        /// </summary>
        /// <param name="serviceName">The name of the service.</param>
        /// <returns>
        /// <b>true</b> if the container can create the requested service, otherwise <b>false</b>.
        /// </returns>
        public static bool CanGetInstance<TService>(string serviceName) {
            return Container.CanGetInstance(typeof(TService), serviceName);
        }

        /// <summary>
        /// Returns <b>true</b> if the container can create the requested service, otherwise <b>false</b>.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the container can create the requested service, otherwise <b>false</b>.
        /// </returns>
        public static bool CanGetInstance<TService>() {
            return Container.CanGetInstance(typeof(TService), "");
        }

        /// <summary>
        /// Starts a new <see cref="T:LightInject.Scope"/>.
        /// </summary>
        /// <returns>
        /// <see cref="T:LightInject.Scope"/>
        /// </returns>
        public static Scope BeginScope() {
            return Container.BeginScope();
        }

        /// <summary>
        /// Injects the property dependencies for a given <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The target instance for which to inject its property dependencies.</param>
        /// <returns>
        /// The <paramref name="instance"/> with its property dependencies injected.
        /// </returns>
        public static object InjectProperties(object instance) {
            return Container.InjectProperties(instance);
        }

        #endregion

    }

}