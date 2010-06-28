//-------------------------------------------------------------------------------
// <copyright file="ContextPreservationTests.cs" company="bbv Software Services AG">
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
    using Ninject;
    using Ninject.Parameters;
    using Ninject.Syntax;
    using NUnit.Framework;

    /// <summary>
    /// Tests the implementation of <see cref="ContextPreservation"/>.
    /// </summary>
    [TestFixture]
    public class ContextPreservationTests
    {
        /// <summary>
        /// The kernel used in the tests.
        /// </summary>
        private IKernel kernel;

        /// <summary>
        /// Child interface used in the tests.
        /// </summary>
        private interface IChild
        {
            /// <summary>
            /// Gets the grand child.
            /// </summary>
            /// <value>The grand child.</value>
            GrandChild GrandChild { get; }
        }
        
        /// <summary>
        /// Sets up the kernel.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false });
            this.kernel.Load(new ContextPreservationModule());
        }

        /// <summary>
        /// Disposes the kernel.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.kernel.Dispose();
        }

        /// <summary>
        /// The context is preserved when a factory created new objects.
        /// </summary>
        [Test]
        public void ContextPreserved()
        {
            this.kernel.Bind<Factory>().ToSelf();
            this.kernel.Bind<Child>().ToSelf().WhenInjectedInto<Factory>();
            this.kernel.Bind<GrandChild>().ToSelf();

            var factory = this.kernel.Get<Factory>();
            var child = factory.CreateChild();

            Assert.IsNotNull(child);
        }

        /// <summary>
        /// Parameters are passed through factories.
        /// </summary>
        [Test]
        public void ParametersArePassed()
        {
            const string Name = "TheName";
            this.kernel.Bind<Factory>().ToSelf();
            this.kernel.Bind<ChildWithArgument>().ToSelf();

            var factory = this.kernel.Get<Factory>();
            var child = factory.CreateChildWithArgument(Name);

            Assert.IsNotNull(child);
            Assert.AreEqual(Name, child.Name);
        }

        /// <summary>
        /// Targets the is resolution root owner.
        /// </summary>
        [Test]
        public void TargetIsResolutionRootOwner()
        {
            this.kernel.Bind<Factory>().ToSelf().Named("Parent");
            this.kernel.Bind<Child>().ToSelf().WhenParentNamed("Parent");
            this.kernel.Bind<GrandChild>().ToSelf();

            var factory = this.kernel.Get<Factory>();
            var child = factory.CreateChild();

            Assert.IsNotNull(child);
        }

        /// <summary>
        /// BindInterfaceToBinding returns an instance of the type bound to the given binding when the interface is requested.
        /// </summary>
        [Test]
        public void BindInterfaceToBinding()
        {
            this.kernel.Bind<Parent>().ToSelf();
            this.kernel.Bind<Child>().ToSelf().WhenInjectedInto<Parent>();
            this.kernel.BindInterfaceToBinding<IChild, Child>();
            this.kernel.Bind<GrandChild>().ToSelf();

            var parent = this.kernel.Get<Parent>();

            Assert.IsNotNull(parent);
            Assert.IsNotNull(parent.Child);
            Assert.IsNotNull(parent.Child.GrandChild);
        }

        /// <summary>
        /// <see cref="BindInterfaceToBinding"/> returns an instance of the type bound to the given binding when the interface is requested.
        /// The interface can be requested directly.
        /// </summary>
        [Test]
        public void BindInterfaceToBindingWhenDirectlyResolved()
        {
            this.kernel.Bind<Child>().ToSelf();
            this.kernel.BindInterfaceToBinding<IChild, Child>();

            var child = this.kernel.Get<IChild>();

            Assert.IsNotNull(child);
            Assert.IsNotNull(child.GrandChild);
        }

        /// <summary>
        /// Test parent class.
        /// </summary>
        private class Parent
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Parent"/> class.
            /// </summary>
            /// <param name="child">The child.</param>
            public Parent(IChild child)
            {
                this.Child = child;
            }

            /// <summary>
            /// Gets the child.
            /// </summary>
            /// <value>The child.</value>
            public IChild Child { get; private set; }
        }

        /// <summary>
        /// Factory used in the tests.
        /// </summary>
        private class Factory
        {
            /// <summary>
            /// The resolution root.
            /// </summary>
            private readonly IResolutionRoot resolutionRoot;

            /// <summary>
            /// Initializes a new instance of the <see cref="Factory"/> class.
            /// </summary>
            /// <param name="resolutionRoot">The resolution root.</param>
            public Factory(IResolutionRoot resolutionRoot)
            {
                this.resolutionRoot = resolutionRoot;
            }

            /// <summary>
            /// Creates a new child.
            /// </summary>
            /// <returns>The newly created child.</returns>
            public Child CreateChild()
            {
                return this.resolutionRoot.Get<Child>();
            }

            /// <summary>
            /// Creates a child with an argument.
            /// </summary>
            /// <param name="name">The name argument.</param>
            /// <returns>The newly created child.</returns>
            public ChildWithArgument CreateChildWithArgument(string name)
            {
                return this.resolutionRoot.Get<ChildWithArgument>(new ConstructorArgument("name", name));
            }
        }

        /// <summary>
        /// A child class used in the tests.
        /// </summary>
        private class Child : IChild
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Child"/> class.
            /// </summary>
            /// <param name="grandChild">The grand child.</param>
            public Child(GrandChild grandChild)
            {
                this.GrandChild = grandChild;
            }

            /// <summary>
            /// Gets the grand child.
            /// </summary>
            /// <value>The grand child.</value>
            public GrandChild GrandChild { get; private set; }
        }

        /// <summary>
        /// A child that has an argument in the constructor.
        /// </summary>
        private class ChildWithArgument : Child
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ChildWithArgument"/> class.
            /// </summary>
            /// <param name="name">The name of the child.</param>
            /// <param name="grandChild">The grand child.</param>
            public ChildWithArgument(string name, GrandChild grandChild)
                : base(grandChild)
            {
                this.Name = name;
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>The name of the child.</value>
            public string Name { get; private set; }
        }

        /// <summary>
        /// A grand child class.
        /// </summary>
        private class GrandChild
        {
        }
    }
}