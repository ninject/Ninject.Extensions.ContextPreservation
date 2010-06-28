Imagine the following scenario:
- A class A gets injected a factory F
- Factory F get injected the IResolutionRoot
- Factory F has a method CreateFoo
- When class a calls CreateFoo the request does not know where it actually lives.

This extension preserves the context over such factories. That means the parent of the new request is the factory request


When using this modules some things have to be done differently:
1. Bindings for classes that have multiple interfaces are defines like this:
   kernel.Bind<MyClass>().ToSelf()
   kernel.BindInterfaceToBinding<IA, MyClass>();
   kernel.BindInterfaceToBinding<IB, MyClass>();
2. If you bind to a method that resolves an instance using the kernel then do it like this:
   
   bindingRoot.Bind<TInterface>().ToMethod(context => context.ContextPreservingGet<MyClass>());
 
   or this

   bindingRoot.Bind<TInterface>().ToMethod(context => return new ContextPreservingResolutionRoot(context, context.Request.Target).Get<MyClass>());

   But NEVER use the kernel form the context directly.

