//-------------------------------------------------------------------------------
// <copyright file="FuncFactory.cs" company="bbv Software Services AG">
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
    using System;

    /// <summary>
    /// Factory using <see cref="Func{T}"/> and Lazy{T}
    /// </summary>
    public class FuncFactory
    {
        /// <summary>
        /// The func factory
        /// </summary>
        private readonly Func<IWeapon> weaponFactory;

#if !NET_35 && !NETCF && !SILVERLIGHT_20 && !SILVERLIGHT_30 && !WINDOWS_PHONE
        /// <summary>
        /// The Lazy value
        /// </summary>
        private readonly Lazy<IWeapon> lazyWeapon;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncFactory"/> class.
        /// </summary>
        /// <param name="weaponFactory">The weapon factory.</param>
        /// <param name="lazyWeapon">The lazy value.</param>
        public FuncFactory(Func<IWeapon> weaponFactory, Lazy<IWeapon> lazyWeapon)
        {
            this.weaponFactory = weaponFactory;
            this.lazyWeapon = lazyWeapon;
        }

        /// <summary>
        /// Gets the lazy weapon.
        /// </summary>
        /// <returns>The lazy value for the weapon.</returns>
        public IWeapon GetLazyWeapon()
        {
            return this.lazyWeapon.Value;
        }
#else

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncFactory"/> class.
        /// </summary>
        /// <param name="weaponFactory">The weapon factory.</param>
        public FuncFactory(Func<IWeapon> weaponFactory)
        {
            this.weaponFactory = weaponFactory;
        }

#endif

        /// <summary>
        /// Creates the weapon.
        /// </summary>
        /// <returns>The newly created weapon.</returns>
        public IWeapon CreateWeapon()
        {
            return this.weaponFactory();
        }
    }
}