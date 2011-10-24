//-------------------------------------------------------------------------------
// <copyright file="ParentWithOpenGeneric.cs" company="bbv Software Services AG">
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
    /// Test parent class with an open generic.
    /// </summary>
    public class ParentWithOpenGeneric
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Warrior"/> class.
        /// </summary>
        /// <param name="openGeneric">The open generic.</param>
        public ParentWithOpenGeneric(IOpenGeneric<int> openGeneric)
        {
            this.OpenGeneric = openGeneric;
        }

        /// <summary>
        /// Gets the open generic injected to this instance.
        /// </summary>
        /// <returns>The open generic injected to this instance.</returns>
        public IOpenGeneric<int> OpenGeneric { get; private set; }
    }
}