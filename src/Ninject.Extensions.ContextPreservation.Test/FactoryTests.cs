//-------------------------------------------------------------------------------
// <copyright file="FactoryTests.cs" company="bbv Software Services AG">
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

namespace Ninject.Extensions.ContextPreservation
{
    using System;
    using FluentAssertions;
    using Ninject.Extensions.ContextPreservation.Fakes;
    using Ninject.Extensions.Factory;
    using Xunit;

    /// <summary>
    /// Test context preservation for the factory extension.
    /// </summary>
    public class FactoryTests : IDisposable
    {
        /// <summary>
        /// The kernel used in the tests.
        /// </summary>
        private readonly StandardKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryTests"/> class.
        /// </summary>
        public FactoryTests()
        {
            this.kernel = new StandardKernel();
#if NO_ASSEMBLY_SCANNING
            this.kernel.Load(new FuncModule(), new ContextPreservationModule());
#endif
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.kernel.Dispose();
        }

#if !CFNET && !SILVERLIGHT_20 && !WINDOWS_PHONE && !MONO
        /// <summary>
        /// Verifies that the context is preserved for ToFactory
        /// </summary>
        [Fact]
        public void FactoryContextIsPreserved()
        {
            this.kernel.Bind<IWeapon>().To<Dagger>().WhenInjectedInto<Village>();
            this.kernel.Bind<IWeaponFactory>().ToFactory();
            
            var village = this.kernel.Get<Village>();
            var weapon = village.GetWeapon();

            weapon.Should().BeOfType<Dagger>();
        }

        /// <summary>
        /// Whens the using factory directly_ must not throw.
        /// </summary>
        [Fact]
        public void FactoryCanBeResolvedDirectly()
        {
            this.kernel.Bind<IWeapon>().To<Dagger>();
            this.kernel.Bind<IWeaponFactory>().ToFactory();

            var factory = this.kernel.Get<IWeaponFactory>();

            factory.CreateWeapon().Should().BeOfType<Dagger>();
        }
#endif

        /// <summary>
        /// Verifies that the context is preserved for Func
        /// </summary>
        [Fact]
        public void FuncContextIsPreserved()
        {
            this.kernel.Bind<IWeapon>().To<Dagger>().WhenInjectedInto<FuncFactory>();

            var factory = this.kernel.Get<FuncFactory>();
            var weapon = factory.CreateWeapon();

            weapon.Should().BeOfType<Dagger>();
        }

#if !NET_35 && !NETCF && !SILVERLIGHT_20 && !SILVERLIGHT_30 && !WINDOWS_PHONE
        /// <summary>
        /// Verifies that the context is preserved for Lazy
        /// </summary>
        [Fact]
        public void LazyContextIsPreserved()
        {
            this.kernel.Bind<IWeapon>().To<Dagger>().WhenInjectedInto<FuncFactory>();

            var factory = this.kernel.Get<FuncFactory>();
            var weapon = factory.GetLazyWeapon();

            weapon.Should().BeOfType<Dagger>();
        }
#endif
    }
}