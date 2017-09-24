using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _002_AsyncAwait_Decompiled
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Main ThreaID {0}", Thread.CurrentThread.ManagedThreadId);
			MyClass myClass = new MyClass();
			myClass.OperationAsync();

			Console.ReadKey();
		}
		
	}

	class MyClass
	{
		public void Operation()
		{
			Console.WriteLine("Opearation ThreaID {0}", Thread.CurrentThread.ManagedThreadId);
			Console.WriteLine("Begin");
			Thread.Sleep(2000);
			Console.WriteLine("End");
		}

		public void OperationAsync()
		{
			AsyncStateMachine stateMachine;
			stateMachine.outer = this;
			stateMachine.builder = AsyncVoidMethodBuilder.Create();
			stateMachine.builder.Start(ref stateMachine);
		}
	}

	struct AsyncStateMachine : IAsyncStateMachine
	{
		public MyClass outer;
		public AsyncVoidMethodBuilder builder;

		public void MoveNext()
		{
			Task task = new Task(outer.Operation);
			task.Start();
		}

		public void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			throw new NotImplementedException();
		}
	}
}
