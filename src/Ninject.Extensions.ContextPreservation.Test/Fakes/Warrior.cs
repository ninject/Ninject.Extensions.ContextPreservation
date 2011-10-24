//-------------------------------------------------------------------------------
// <copyright file="Warrior.cs" company="bbv Software Services AG">
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
    public class Warrior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Warrior"/> class.
        /// </summary>
        /// <param name="weapon">The weapon.</param>
        public Warrior(IWeapon weapon)
        {
            this.Weapon = weapon;
        }

        /// <summary>
        /// Gets the weapon.
        /// </summary>
        /// <value>The weapon.</value>
        public IWeapon Weapon { get; private set; }
    }
}