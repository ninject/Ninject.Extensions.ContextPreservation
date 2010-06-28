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
    using Ninject.Activation;
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
        /// Resolves a binding using a <see cref="ContextPreservingResolutionRoot"/>.
        /// </summary>
        /// <typeparam name="T">The binding to resolve.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns>The resolved instance</returns>
        public static T ContextPreservingGet<T>(this IContext context)
        {
            return new ContextPreservingResolutionRoot(context, context.Request.Target).Get<T>();
        }
    }
}