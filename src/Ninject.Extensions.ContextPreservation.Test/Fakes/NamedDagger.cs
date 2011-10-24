//-------------------------------------------------------------------------------
// <copyright file="NamedDagger.cs" company="bbv Software Services AG">
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
    /// A test implementation of <see cref="INamedWeapon"/> used in the unit tests
    /// </summary>
    public class NamedDagger : Dagger, INamedWeapon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedDagger"/> class. 
        /// </summary>
        /// <param name="name">
        /// The name of the weapon.
        /// </param>
        public NamedDagger(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the weapon.
        /// </summary>
        /// <value>The name of the weapon.</value>
        public string Name { get; private set; }
    }
}