//-------------------------------------------------------------------------------
// <copyright file="Town.cs" company="bbv Software Services AG">
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
    /// A test class used in the unit tests
    /// </summary>
    public class Town
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Town"/> class.
        /// </summary>
        /// <param name="weaponFactory">The weapon factory.</param>
        public Town(IWeaponFactory weaponFactory)
        {
            this.WeaponFactory = weaponFactory;
        }

        /// <summary>
        /// Gets or sets the weapon factory.
        /// </summary>
        /// <value>The weapon factory.</value>
        public IWeaponFactory WeaponFactory { get; set; }
    }
}