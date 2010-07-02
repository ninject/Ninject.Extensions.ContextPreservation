//-------------------------------------------------------------------------------
// <copyright file="ContextPreservationModule.cs" company="bbv Software Services AG">
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
            this.Bind<IResolutionRoot>().To<ContextPreservingResolutionRoot>();
        }
    }
}