// -------------------------------------------------------------------------------------------------
// <copyright file="ContextPreservationModule.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
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