//-------------------------------------------------------------------------------
// <copyright file="ContextPreservationExtensionMethods.cs" company="bbv Software Services AG">
//   Copyright (c) 2010 bbv Software Services AG
//   Author: Remo Gloor remo.gloor@bbv.ch
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

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
        /// Binds an interface to binding.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TBinding">The type of the binding the interface is bound to.</typeparam>
        /// <param name="bindingRoot">The binding root.</param>
        /// <returns>The binding syntax.</returns>
        public static IBindingWhenInNamedWithOrOnSyntax<TInterface> BindInterfaceToBinding<TInterface, TBinding>(this IBindingRoot bindingRoot)
            where TBinding : TInterface
        {
            return bindingRoot.Bind<TInterface>().ToMethod(context => context.ContextPreservingGet<TBinding>());
        }

        /// <summary>
        /// Binds an interface to binding.
        /// </summary>
        /// <param name="bindingRoot">The binding root.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="bindingType">Type of the binding.</param>
        /// <returns>The binding syntax.</returns>
        public static IBindingWhenInNamedWithOrOnSyntax<object> BindInterfaceToBinding(this IBindingRoot bindingRoot, Type interfaceType, Type bindingType)
        {
            return bindingRoot.Bind(interfaceType).ToMethod(context => context.ContextPreservingGet(bindingType));
        }
        
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
            if (service.IsGenericType)
            {
                service = service.MakeGenericType(context.GenericArguments);
            }

            return service;
        }
    }
}