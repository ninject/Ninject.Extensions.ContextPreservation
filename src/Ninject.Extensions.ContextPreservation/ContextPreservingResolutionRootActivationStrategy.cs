// -------------------------------------------------------------------------------------------------
// <copyright file="ContextPreservingResolutionRootActivationStrategy.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
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