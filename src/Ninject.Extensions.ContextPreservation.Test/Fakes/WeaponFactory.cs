//-------------------------------------------------------------------------------
// <copyright file="WeaponFactory.cs" company="bbv Software Services AG">
//   Copyright (c) 2010-2011 bbv Software Services AG
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

namespace Ninject.Extensions.ContextPreservation.Fakes
{
    using Ninject.Parameters;
    using Ninject.Syntax;

    /// <summary>
    /// WeaponFactory used in the tests.
    /// </summary>
    public class WeaponFactory : IWeaponFactory
    {
        /// <summary>
        /// The resolution root.
        /// </summary>
        private readonly IResolutionRoot resolutionRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeaponFactory"/> class.
        /// </summary>
        /// <param name="resolutionRoot">The resolution root.</param>
        public WeaponFactory(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }

        /// <summary>
        /// Creates a new weapon.
        /// </summary>
        /// <returns>The newly created weapon.</returns>
        public IWeapon CreateWeapon()
        {
            return this.resolutionRoot.Get<IWeapon>();
        }

        /// <summary>
        /// Creates a named weapon
        /// </summary>
        /// <param name="name">The name argument.</param>
        /// <returns>The newly created weapon.</returns>
        public INamedWeapon CreateNamedWeapon(string name)
        {
            return this.resolutionRoot.Get<INamedWeapon>(new ConstructorArgument("name", name));
        }
    }
}