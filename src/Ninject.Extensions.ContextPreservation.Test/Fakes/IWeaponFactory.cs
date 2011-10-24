//-------------------------------------------------------------------------------
// <copyright file="IWeaponFactory.cs" company="bbv Software Services AG">
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
    /// <summary>
    /// A test interface used in the unit tests.
    /// </summary>
    public interface IWeaponFactory
    {
        /// <summary>
        /// Creates a new weapon.
        /// </summary>
        /// <returns>The newly created weapon.</returns>
        IWeapon CreateWeapon();

        /// <summary>
        /// Creates a named weapon
        /// </summary>
        /// <param name="name">The name argument.</param>
        /// <returns>The newly created weapon.</returns>
        INamedWeapon CreateNamedWeapon(string name);
    }
}