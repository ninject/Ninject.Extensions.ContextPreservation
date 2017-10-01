// -------------------------------------------------------------------------------------------------
// <copyright file="ContextPreservationExtensionMethods.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.ContextPreservation
{
    using System;

    using Ninject.Activation;
    using Ninject.Parameters;
    using Ninject.Planning.Bindings;
    using Ninject.Syntax;

    /// <summary>
    /// Extension methods used for context preservation.
    /// </summary>
    public static class ContextPreservationExtensionMethods
    {
        /// <summary>
        /// Resolves a binding using a <see cref="ContextPreservingResolutionRoot"/>.
        /// </summary>
        /// <typeparam name="T">The binding to resolve.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>The resolved instance</returns>
        public static T ContextPreservingGet<T>(this IContext context, params IParameter[] parameters)
        {
            return context.GetContextPreservingResolutionRoot().Get<T>(parameters);
        }

        /// <summary>
        /// Resolves a binding using a <see cref="ContextPreservingResolutionRoot"/>.
        /// </summary>
        /// <typeparam name="T">The binding to resolve.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>The resolved instance</returns>
        public static T ContextPreservingGet<T>(this IContext context, string name, params IParameter[] parameters)
        {
            return context.GetContextPreservingResolutionRoot().Get<T>(name, parameters);
        }

        /// <summary>
        /// Resolves a binding using a <see cref="ContextPreservingResolutionRoot"/>.
        /// </summary>
        /// <typeparam name="T">The binding to resolve.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="constraint">The constraint to apply to the binding.</param>
        /// <param name="parameters">The parameters to pass to the request</param>
        /// <returns>The resolved instance</returns>
        public static T ContextPreservingGet<T>(this IContext context, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return context.GetContextPreservingResolutionRoot().Get<T>(constraint, parameters);
        }

        /// <summary>
        /// Resolves a binding using a <see cref="ContextPreservingResolutionRoot"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="service">The service to resolve.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>The resolved instance</returns>
        public static object ContextPreservingGet(this IContext context, Type service, params IParameter[] parameters)
        {
            service = GetServiceType(service, context);
            return context.GetContextPreservingResolutionRoot().Get(service, parameters);
        }

        /// <summary>
        /// Resolves a binding using a <see cref="ContextPreservingResolutionRoot"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="service">The service to resolve.</param>
        /// <param name="name">The name of the binding.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        /// <returns>The resolved instance</returns>
        public static object ContextPreservingGet(this IContext context, Type service, string name, params IParameter[] parameters)
        {
            service = GetServiceType(service, context);
            return context.GetContextPreservingResolutionRoot().Get(service, name, parameters);
        }

        /// <summary>
        /// Resolves a binding using a <see cref="ContextPreservingResolutionRoot"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="service">The service to resolve.</param>
        /// <param name="constraint">The constraint to apply to the binding.</param>
        /// <param name="parameters">The parameters to pass to the request</param>
        /// <returns>The resolved instance</returns>
        public static object ContextPreservingGet(this IContext context, Type service, Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            service = GetServiceType(service, context);
            return context.GetContextPreservingResolutionRoot().Get(service, constraint, parameters);
        }

        /// <summary>
        /// Gets a <see cref="ContextPreservingResolutionRoot"/> for the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The newly created <see cref="ContextPreservingResolutionRoot"/></returns>
        public static IResolutionRoot GetContextPreservingResolutionRoot(this IContext context)
        {
            return new ContextPreservingResolutionRoot(context, context.Request.Target);
        }

        /// <summary>
        /// Gets the type of the service. Converting open generics to the matching type.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="context">The context.</param>
        /// <returns>The type of the service.</returns>
        private static Type GetServiceType(Type service, IContext context)
        {
            if (service.IsGenericTypeDefinition)
            {
                service = service.MakeGenericType(context.GenericArguments);
            }

            return service;
        }
    }
}