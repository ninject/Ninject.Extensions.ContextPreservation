// -------------------------------------------------------------------------------------------------
// <copyright file="ContextPreservationModule.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010-2017 Ninject Project Contributors. All rights reserved.
//
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   You may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.ContextPreservation
{
    using System;

    using Ninject.Activation;
    using Ninject.Activation.Strategies;
    using Ninject.Modules;
    using Ninject.Syntax;

    /// <summary>
    /// This module provides a <see cref="IResolutionRoot"/> binding that preserves the context.
    /// New Get requests can access the context that was used to create the resolution root.
    /// </summary>
    public class ContextPreservationModule : NinjectModule
    {
        /// <summary>
        /// Loads this instance.
        /// </summary>
        public override void Load()
        {
            this.Kernel.Components.Add<IActivationStrategy, ContextPreservingResolutionRootActivationStrategy>();
            this.Rebind<IResolutionRoot>().To<ContextPreservingResolutionRoot>();
            this.Rebind<Func<IContext, IResolutionRoot>>().ToMethod(ctx => GetContextPreservingResolutionRoot);
        }

        /// <summary>
        /// Gets the context preserving resolution root for the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The context preserving resolution root for the given context.</returns>
        private static IResolutionRoot GetContextPreservingResolutionRoot(IContext context)
        {
#if !NET_35 && !NETCF && !SILVERLIGHT_20 && !SILVERLIGHT_30 && !WINDOWS_PHONE
            if (context.Request.ParentRequest != null &&
                context.Request.ParentRequest.Service.IsGenericType &&
                context.Request.ParentRequest.Service.GetGenericTypeDefinition() == typeof(Lazy<>))
            {
                context = context.Request.ParentContext;
            }
#endif

            return context.GetContextPreservingResolutionRoot();
        }
    }
}