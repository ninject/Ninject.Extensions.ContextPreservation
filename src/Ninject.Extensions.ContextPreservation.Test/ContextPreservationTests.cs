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
    using System;
    using Ninject;
    using Ninject.Parameters;
    using Ninject.Syntax;
#if SILVERLIGHT
#if SILVERLIGHT_MSTEST
        using MsTest.Should;
        using Microsoft.VisualStudio.TestTools.UnitTesting;
        using Fact = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#else
    using UnitDriven;
    using UnitDriven.Should;
    using Fact = UnitDriven.TestMethodAttribute;
#endif
#else
    using Ninject.Tests.MSTestAttributes;
    using Xunit;
    using Xunit.Should;
#endif

    /// <summary>
    /// Tests the implementation of <see cref="ContextPreservation"/>.
    /// </summary>
    [TestClass]
    public class ContextPreservationTests : IDisposable
    {
        /// <summary>
        /// The kernel used in the tests.
        /// </summary>
        private IKernel kernel;

        /// <summary>
        /// Child interface used in the tests.
        /// </summary>
        public interface IChild
        {
            /// <summary>
            /// Gets the grand child.
            /// </summary>
            /// <value>The grand child.</value>
            GrandChild GrandChild { get; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextPreservationTests"/> class.
        /// </summary>
        public ContextPreservationTests()
        {
            this.SetUp();
        }

        [TestInitialize]
        public void SetUp()
        {
            this.kernel = new StandardKernel(new NinjectSettings
            {
#if !SILVERLIGHT
                LoadExtensions = false
#endif
            });
            this.kernel.Load(new ContextPreservationModule());
        }

        /// <summary>
        /// Disposes the kernel.
        /// </summary>
        public void Dispose()
        {
            this.kernel.Dispose();
        }

        /// <summary>
        /// The context is preserved when a factory created new objects.
        /// </summary>
        [Fact]
        public void ContextPreserved()
        {
            this.kernel.Bind<Factory>().ToSelf();
            this.kernel.Bind<Child>().ToSelf().WhenInjectedInto<Factory>();
            this.kernel.Bind<GrandChild>().ToSelf();

            var factory = this.kernel.Get<Factory>();
            var child = factory.CreateChild();

            child.ShouldNotBeNull();
        }

        /// <summary>
        /// Parameters are passed through factories.
        /// </summary>
        [Fact]
        public void ParametersArePassed()
        {
            const string Name = "TheName";
            this.kernel.Bind<Factory>().ToSelf();
            this.kernel.Bind<ChildWithArgument>().ToSelf();

            var factory = this.kernel.Get<Factory>();
            var child = factory.CreateChildWithArgument(Name);

            child.ShouldNotBeNull();
            child.Name.ShouldBe(Name);
        }

        /// <summary>
        /// Targets the is resolution root owner.
        /// </summary>
        [Fact]
        public void TargetIsResolutionRootOwner()
        {
            this.kernel.Bind<Factory>().ToSelf().Named("Parent");
            this.kernel.Bind<Child>().ToSelf().WhenParentNamed("Parent");
            this.kernel.Bind<GrandChild>().ToSelf();

            var factory = this.kernel.Get<Factory>();
            var child = factory.CreateChild();

            child.ShouldNotBeNull();
        }

        /// <summary>
        /// BindInterfaceToBinding returns an instance of the type bound to the given binding when the interface is requested.
        /// </summary>
        [Fact]
        public void BindInterfaceToBinding()
        {
            this.kernel.Bind<Parent>().ToSelf();
            this.kernel.Bind<Child>().ToSelf().WhenInjectedInto<Parent>();
            this.kernel.BindInterfaceToBinding<IChild, Child>();
            this.kernel.Bind<GrandChild>().ToSelf();

            var parent = this.kernel.Get<Parent>();

            parent.ShouldNotBeNull();
            parent.Child.ShouldNotBeNull();
            parent.Child.GrandChild.ShouldNotBeNull();
        }

        /// <summary>
        /// <see cref="BindInterfaceToBinding"/> returns an instance of the type bound to the given binding when the interface is requested.
        /// The interface can be requested directly.
        /// </summary>
        [Fact]
        public void BindInterfaceToBindingWhenDirectlyResolved()
        {
            this.kernel.Bind<Child>().ToSelf();
            this.kernel.BindInterfaceToBinding<IChild, Child>();

            var child = this.kernel.Get<IChild>();

            child.ShouldNotBeNull();
            child.GrandChild.ShouldNotBeNull();
        }

        /// <summary>
        /// The ContextPreservingGet extension method uses a 
        /// <see cref="ContextPreservingResolutionRoot"/> to get the requested instance.
        /// </summary>
        [Fact]
        public void ContextPreservingGetExtensionMethodWithoutArguments()
        {
            this.kernel.Bind<Parent>().ToSelf();
            this.kernel.Bind<IChild>().ToMethod(ctx => ctx.ContextPreservingGet<Child>());
            this.kernel.Bind<Child>().ToSelf().WhenInjectedInto<Parent>();

            var parent = this.kernel.Get<Parent>();

            parent.Child.ShouldNotBeNull();
        }

        /// <summary>
        /// The ContextPreservingGet extension method uses a 
        /// <see cref="ContextPreservingResolutionRoot"/> to get the requested instance.
        /// Works also with name.
        /// </summary>
        [Fact]
        public void ContextPreservingGetExtensionMethodWithName()
        {
            this.kernel.Bind<Parent>().ToSelf();
            this.kernel.Bind<IChild>().ToMethod(ctx => ctx.ContextPreservingGet<ChildWithArgument>("1"));
            this.kernel.Bind<ChildWithArgument>().ToSelf()
                       .WhenInjectedInto<Parent>().Named("1").WithConstructorArgument("name", "1");
            this.kernel.Bind<ChildWithArgument>().ToSelf()
                       .WhenInjectedInto<Parent>().Named("2").WithConstructorArgument("name", "2");

            var parent = this.kernel.Get<Parent>();

            parent.Child.ShouldNotBeNull();
            ((ChildWithArgument)parent.Child).Name.ShouldBe("1");
        }

        /// <summary>
        /// The ContextPreservingGet extension method uses a 
        /// <see cref="ContextPreservingResolutionRoot"/> to get the requested instance.
        /// Works also with constraint.
        /// </summary>
        [Fact]
        public void ContextPreservingGetExtensionMethodWithConstraint()
        {
            this.kernel.Bind<Parent>().ToSelf();
            this.kernel.Bind<IChild>().ToMethod(ctx => ctx.ContextPreservingGet<ChildWithArgument>(m => m.Has("2")));
            this.kernel.Bind<ChildWithArgument>().ToSelf()
                       .WhenInjectedInto<Parent>().WithMetadata("1", null).WithConstructorArgument("name", "1");
            this.kernel.Bind<ChildWithArgument>().ToSelf()
                       .WhenInjectedInto<Parent>().WithMetadata("2", null).WithConstructorArgument("name", "2");

            var parent = this.kernel.Get<Parent>();

            parent.Child.ShouldNotBeNull();
            ((ChildWithArgument)parent.Child).Name.ShouldBe("2");
        }


        /// <summary>
        /// The ContextPreservingGet extension method uses a 
        /// <see cref="ContextPreservingResolutionRoot"/> to get the requested instance.
        /// Works also with parameters.
        /// </summary>
        [Fact]
        public void ContextPreservingGetExtensionMethodWithParameters()
        {
            this.kernel.Bind<Parent>().ToSelf();
            this.kernel.Bind<IChild>().ToMethod(
                ctx => ctx.ContextPreservingGet<ChildWithArgument>(
                            new ConstructorArgument("name", "3"), 
                            new PropertyValue("SomeProperty", "4")));
            this.kernel.Bind<ChildWithArgument>().ToSelf().WhenInjectedInto<Parent>();
            var parent = this.kernel.Get<Parent>();

            parent.Child.ShouldNotBeNull();
            ((ChildWithArgument)parent.Child).Name.ShouldBe("3");
            ((ChildWithArgument)parent.Child).SomeProperty.ShouldBe("4");
        }

        /// <summary>
        /// The GetContextPreservingResolutionRoot extension method creates a
        /// <see cref="ContextPreservingResolutionRoot"/> for the current context instance. 
        /// </summary>
        [Fact]
        public void GetContextPreservingResolutionRootExtensionMethod()
        {
            this.kernel.Bind<Parent>().ToSelf();
            this.kernel.Bind<IChild>().ToMethod(ctx => ctx.GetContextPreservingResolutionRoot().Get<Child>());
            this.kernel.Bind<Child>().ToSelf().WhenInjectedInto<Parent>();
            var parent = this.kernel.Get<Parent>();

            parent.Child.ShouldNotBeNull();
        }

        /// <summary>
        /// Test parent class.
        /// </summary>
        public class Parent
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
        public class Factory
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
        public class Child : IChild
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
        public class ChildWithArgument : Child
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

            /// <summary>
            /// Gets or sets the test property value.
            /// </summary>
            /// <value>The test property value.</value>
            public string SomeProperty { get; set; }
        }

        /// <summary>
        /// A grand child class.
        /// </summary>
        public class GrandChild
        {
        }
    }
}