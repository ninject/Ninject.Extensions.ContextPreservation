// -------------------------------------------------------------------------------------------------
// <copyright file="ContextPreservingResolutionRoot.cs" company="Ninject Project Contributors">
//   Copyright (c) 2010-2017 Ninject Project Contributors
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.ContextPreservation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ninject.Activation;
    using Ninject.Components;
    using Ninject.Parameters;
    using Ninject.Planning.Bindings;
    using Ninject.Planning.Targets;
    using Ninject.Syntax;

    /// <summary>
    /// A resolution root that preserves the context of the factory when resolves a new request.
    /// </summary>
    public class ContextPreservingResolutionRoot : NinjectComponent, IResolutionRoot
    {
        /// <summary>
        /// The parent context.
        /// </summary>
        private IContext context;

        /// <summary>
        /// The parent target.
        /// </summary>
        private ITarget target;

        /// <summary>
        /// A list of all inherited parameters that are passed to the new request.
        /// </summary>
        private IList<IParameter> inheritedParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextPreservingResolutionRoot"/> class.
        /// </summary>
        public ContextPreservingResolutionRoot()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextPreservingResolutionRoot"/> class.
        /// </summary>
        /// <param name="context">The parent context.</param>
        public ContextPreservingResolutionRoot(IContext context)
        {
            this.DefineParentContext(context, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextPreservingResolutionRoot"/> class.
        /// </summary>
        /// <param name="context">The parent context.</param>
        /// <param name="target">The parent target.</param>
        public ContextPreservingResolutionRoot(IContext context, ITarget target)
        {
            this.DefineParentContext(context, target);
        }

        /// <summary>
        /// Determines whether this instance can resolve the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///     <c>true</c> if this instance can resolve the specified request; otherwise, <c>false</c>.
        /// </returns>
        public bool CanResolve(IRequest request)
        {
            return this.context.Kernel.CanResolve(request);
        }

        /// <summary>
        /// Determines whether the specified request can be resolved.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="ignoreImplicitBindings">if set to <c>true</c> implicit bindings are ignored.</param>
        /// <returns>
        ///     <c>True</c> if the request can be resolved; otherwise, <c>false</c>.
        /// </returns>
        public bool CanResolve(IRequest request, bool ignoreImplicitBindings)
        {
            return this.context.Kernel.CanResolve(request, ignoreImplicitBindings);
        }

        /// <summary>
        /// Resolves the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The object that was retrieved by resolving the request.</returns>
        public IEnumerable<object> Resolve(IRequest request)
        {
            return this.context.Kernel.Resolve(request);
        }

        /// <summary>
        /// Creates a request.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="constraint">The constraint.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="isOptional">if set to <c>true</c> the parameter is optional.</param>
        /// <param name="isUnique">if set to <c>true</c> the request must return one unique binding.</param>
        /// <returns>The created request.</returns>
        public IRequest CreateRequest(Type service, Func<IBindingMetadata, bool> constraint, IEnumerable<IParameter> parameters, bool isOptional, bool isUnique)
        {
            IRequest request = this.context.Kernel.CreateRequest(service, constraint, parameters.Union(this.inheritedParameters), isOptional, isUnique);
            return new ContextPreservingRequest(request, this.context, this.target);
        }

        /// <summary>
        /// Injects the specified existing instance, without managing its lifecycle.
        /// </summary>
        /// <param name="instance">The instance to inject.</param>
        /// <param name="parameters">The parameters to pass to the request.</param>
        public void Inject(object instance, params IParameter[] parameters)
        {
            this.context.Kernel.Inject(instance, parameters);
        }

        /// <summary>
        /// Deactivates and releases the specified instance if it is currently managed by Ninject.
        /// </summary>
        /// <param name="instance">The instance to release.</param>
        /// <returns><see langword="True"/> if the instance was found and released; otherwise <see langword="false"/>.</returns>
        public bool Release(object instance)
        {
            return this.context.Kernel.Release(instance);
        }

        /// <summary>
        /// Defines the parent context.
        /// </summary>
        /// <param name="context">The parent context.</param>
        /// <param name="target">The parent target.</param>
        internal void DefineParentContext(IContext context, ITarget target)
        {
            this.context = context;
            this.target = target;
            if (context != null)
            {
                this.inheritedParameters = context.Parameters.Where(p => p.ShouldInherit).ToList();
            }
        }

        /// <summary>
        /// <see cref="IRequest"/> decorator that returns the configured parent context and target.
        /// </summary>
        private class ContextPreservingRequest : IRequest
        {
            /// <summary>
            /// The parent context.
            /// </summary>
            private readonly IContext parentContext;

            /// <summary>
            /// The decorated request.
            /// </summary>
            private readonly IRequest originalRequest;

            /// <summary>
            /// The original target.
            /// </summary>
            private readonly ITarget originalTarget;

            /// <summary>
            /// Initializes a new instance of the <see cref="ContextPreservingRequest"/> class.
            /// </summary>
            /// <param name="originalRequest">The original request.</param>
            /// <param name="parentContext">The parent context.</param>
            /// <param name="originalTarget">The original target.</param>
            public ContextPreservingRequest(IRequest originalRequest, IContext parentContext, ITarget originalTarget)
            {
                this.parentContext = parentContext;
                this.originalRequest = originalRequest;
                this.originalTarget = originalTarget;
            }

            /// <summary>
            /// Gets the service.
            /// </summary>
            /// <value>The service.</value>
            public Type Service
            {
                get
                {
                    return this.originalRequest.Service;
                }
            }

            /// <summary>
            /// Gets the parent request.
            /// </summary>
            /// <value>The parent request.</value>
            public IRequest ParentRequest
            {
                get
                {
                    return this.parentContext.Request;
                }
            }

            /// <summary>
            /// Gets the parent context.
            /// </summary>
            /// <value>The parent context.</value>
            public IContext ParentContext
            {
                get
                {
                    return this.parentContext;
                }
            }

            /// <summary>
            /// Gets the target.
            /// </summary>
            /// <value>The target.</value>
            public ITarget Target
            {
                get
                {
                    return this.originalTarget;
                }
            }

            /// <summary>
            /// Gets the constraint.
            /// </summary>
            /// <value>The constraint.</value>
            public Func<IBindingMetadata, bool> Constraint
            {
                get
                {
                    return this.originalRequest.Constraint;
                }
            }

            /// <summary>
            /// Gets the parameters.
            /// </summary>
            /// <value>The parameters.</value>
            public ICollection<IParameter> Parameters
            {
                get
                {
                    return this.originalRequest.Parameters;
                }
            }

            /// <summary>
            /// Gets the active bindings.
            /// </summary>
            /// <value>The active bindings.</value>
            public Stack<IBinding> ActiveBindings
            {
                get
                {
                    return this.originalRequest.ActiveBindings;
                }
            }

            /// <summary>
            /// Gets the depth.
            /// </summary>
            /// <value>The depth.</value>
            public int Depth
            {
                get
                {
                    return this.originalRequest.Depth + this.parentContext.Request.Depth + 1;
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is optional.
            /// </summary>
            /// <value>
            ///     <c>true</c> if this instance is optional; otherwise, <c>false</c>.
            /// </value>
            public bool IsOptional
            {
                get { return this.originalRequest.IsOptional; }
                set { this.originalRequest.IsOptional = value; }
            }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is unique.
            /// </summary>
            /// <value><c>true</c> if this instance is unique; otherwise, <c>false</c>.</value>
            public bool IsUnique
            {
                get { return this.originalRequest.IsUnique; }
                set { this.originalRequest.IsUnique = value; }
            }

            /// <summary>
            /// Gets or sets a value indicating whether the request should force to return a unique value even if the request is optional.
            /// If this value is set true the request will throw an ActivationException if there are multiple satisfying bingings rather
            /// than returning null for the request is optional. For none optional requests this parameter does not change anything.
            /// </summary>
            public bool ForceUnique
            {
                get { return this.originalRequest.ForceUnique; }
                set { this.originalRequest.ForceUnique = value; }
            }

            /// <summary>
            /// Tests if the binding matched the given binding.
            /// </summary>
            /// <param name="binding">The binding.</param>
            /// <returns>True if the bindings match.</returns>
            public bool Matches(IBinding binding)
            {
                return this.originalRequest.Matches(binding);
            }

            /// <summary>
            /// Gets the scope.
            /// </summary>
            /// <returns>Returns the scope of the original request.</returns>
            public object GetScope()
            {
                return this.originalRequest.GetScope();
            }

            /// <summary>
            /// Creates a child.
            /// </summary>
            /// <param name="service">The service.</param>
            /// <param name="parentContext">The parent context.</param>
            /// <param name="target">The target.</param>
            /// <returns>The created child request.</returns>
            public IRequest CreateChild(Type service, IContext parentContext, ITarget target)
            {
                return this.originalRequest.CreateChild(service, parentContext, target);
            }
        }
    }
}