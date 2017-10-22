// -------------------------------------------------------------------------------------------------
// <copyright file="ContextPreservingResolutionRootActivationStrategy.cs" company="Ninject Project Contributors">
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

    /// <summary>
    /// Strategy for defining the parent context for <see cref="ContextPreservingResolutionRoot"/>.
    /// </summary>
    public class ContextPreservingResolutionRootActivationStrategy : ActivationStrategy
    {
        /// <summary>
        /// Activates the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference.</param>
        public override void Activate(IContext context, InstanceReference reference)
        {
            reference.IfInstanceIs<ContextPreservingResolutionRoot>(namedScopeResolutionRoot => DefineParentContext(context, namedScopeResolutionRoot));
        }

        /// <summary>
        /// Defines the parent context for the specified resolution root.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="namedScopeResolutionRoot">The named scope resolution root.</param>
        private static void DefineParentContext(IContext context, ContextPreservingResolutionRoot namedScopeResolutionRoot)
        {
            if (context.Request.Target.Member.DeclaringType.FullName == "Ninject.Extensions.Factory.FactoryInterceptor")
            {
                namedScopeResolutionRoot.DefineParentContext(context.Request.ParentRequest.ParentContext, context.Request.ParentRequest.ParentRequest.Target);
            }
            else
            {
                namedScopeResolutionRoot.DefineParentContext(context.Request.ParentContext, context.Request.Target);
            }
        }
    }
}