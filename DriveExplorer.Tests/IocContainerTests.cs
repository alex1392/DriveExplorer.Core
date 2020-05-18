using System;
using System.Linq;
using Xunit;

namespace DriveExplorer.IoC {
	public class IocContainerTests {
		[Fact]
		public void GetDefaultIocTwice_IdenticalInstances() {
			//Given
			var expected = IocContainer.Default;
			//When
			var actual = IocContainer.Default;
			//Then
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void Reset_IsRegister_False() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<MyClass>();
			//When
			ioc.Reset();
			//Then
			bool condition = ioc.IsRegistered<MyClass>();
			Assert.False(condition);
		}

		[Fact]
		public void DoRegisterClass_IsRegister_True() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<MyClass>();
			//When
			var conditionA = ioc.IsRegistered<MyClass>();
			//Then
			Assert.True(conditionA);
		}

		[Fact]
		public void NoRegisterClass_IsRegister_False() {
			//Given
			var ioc = new IocContainer();
			//When
			var conditionB = ioc.IsRegistered<MyClass>();
			//Then
			Assert.False(conditionB);
		}

		[Fact]
		public void NoRegisterClass_GetInstance_Throws() {
			//Given
			var ioc = new IocContainer();
			//When
			Func<MyClass> func = ioc.GetInstance<MyClass>;
			//Then
			Assert.Throws<InvalidOperationException>(func);
		}

		[Fact]
		public void DoGetInstanceTwice_IdenticalInstances() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<MyClass>();
			var instanceA = ioc.GetInstance<MyClass>();
			//When
			var instanceB = ioc.GetInstance<MyClass>();
			//Then
			Assert.Equal(instanceA, instanceB);
		}

		[Fact]
		public void RegisterDependedClass_GetDependentClassInstance_ResultNotNull() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<DependedClass>();
			ioc.Register<DependentClass>();
			//When
			var instance = ioc.GetInstance<DependentClass>();
			//Then
			Assert.NotNull(instance);
		}

		[Fact]
		public void NoRegisterDependedClass_GetDependentClassInstance_Throws() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<DependentClass>();
			//When
			Func<DependentClass> func = ioc.GetInstance<DependentClass>;
			//Then
			Assert.Throws<InvalidOperationException>(func);
		}

		[Fact]
		public void DoGetInstanceClass_IsCreated_True() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<MyClass>();
			var instanceA = ioc.GetInstance<MyClass>();
			//When
			var conditionA = ioc.IsCreated<MyClass>();
			//Then
			Assert.True(conditionA);
		}

		[Fact]
		public void NoGetInstanceClass_IsCreated_False() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<MyClass>();

			//When
			var conditionB = ioc.IsCreated<MyClass>();

			//Then
			Assert.False(conditionB);
		}

		[Fact]
		public void DoRegisterInterface_IsRegistered_True() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<IClass, MyClass>();
			//When
			var condition = ioc.IsRegistered<IClass>();
			//Then
			Assert.True(condition);
		}

		[Fact]
		public void NoRegisterInterface_IsRegistered_False() {
			//Given
			var ioc = new IocContainer();
			//When
			var condition = ioc.IsRegistered<IClass>();
			//Then
			Assert.False(condition);
		}

		[Fact]
		public void DoGetInstanceInterface_IsCreated_True() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<IClass, MyClass>();
			var instance = ioc.GetInstance<IClass>();
			//When
			var condition = ioc.IsCreated<IClass>();
			//Then
			Assert.True(condition);
		}

		[Fact]
		public void NoGetInstanceInterface_IsCreated_False() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<IClass, MyClass>();
			//When
			var condition = ioc.IsCreated<IClass>();
			//Then
			Assert.False(condition);
		}

		[Fact]
		public void DoRegisterInterfaceWithClass_GetInstance_InstanceIsClass() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<IClass, MyClass>();
			//When
			var instance = ioc.GetInstance<IClass>();
			//Then
			Assert.True(instance is MyClass);
		}

		[Fact]
		public void RegisterClassTwice_ThrowError() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<MyClass>();
			//When
			Action action = ioc.Register<MyClass>;
			//Then
			Assert.Throws<InvalidOperationException>(action);
		}

		[Fact]
		public void RegisterInterfaceTwice_ThorwsError() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<IClass, MyClass>();
			//When
			Action action = ioc.Register<IClass, MyClass>;
			//Then
			Assert.Throws<InvalidOperationException>(action);
		}

		[Fact]
		public void RegisterInterfaceWithRegisterClass_Throws() {
			//Given
			var ioc = new IocContainer();
			//When
			Action action = ioc.Register<IClass>;
			//Then
			Assert.Throws<ArgumentException>(action);
		}

		[Fact]
		public void RegisterClassWithRegisterInterface_Throws() {
			//Given
			var ioc = new IocContainer();
			//When
			Action action = ioc.Register<MyClass, MyClass>;
			//Then
			Assert.Throws<ArgumentException>(action);
		}

		[Fact]
		public void UnregisterClass_IsRegistered_False() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<MyClass>();
			ioc.Unregister<MyClass>();
			//When
			var condition = ioc.IsRegistered<MyClass>();
			//Then
			Assert.False(condition);
		}

		[Fact]
		public void UnregisterInterface_IsRegistered_False() {
			//Given
			var ioc = new IocContainer();
			ioc.Register<IClass, MyClass>();
			ioc.Unregister<IClass>();
			//When
			var condition = ioc.IsRegistered<IClass>();
			//Then
			Assert.False(condition);
		}

		[Fact]
		public void RegisterClassWithoutPublicCtor_Throws() {
			//Given
			var ioc = new IocContainer();
			//When
			Action action = ioc.Register<ClassWithoutPublicCtor>;
			//Then
			Assert.Throws<ArgumentException>(action);
		}
	}

	public class DependedClass {
		public DependedClass() { }
	}

	public class DependentClass {
		public DependentClass() { }

		[PreferredConstructor]
		public DependentClass(DependedClass c) { }
	}
	public interface IClass {

	}

	public class MyClass : IClass {
		public MyClass() { }
	}

	public class ClassWithoutPublicCtor : IClass {
		private ClassWithoutPublicCtor() { }
	}
}