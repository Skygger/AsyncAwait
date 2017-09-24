using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _004_AsyncAwait_Decompiled
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
			Console.WriteLine("Operation ThreaID {0}", Thread.CurrentThread.ManagedThreadId);
			Console.WriteLine("Begin");
			Thread.Sleep(2000);
			Console.WriteLine("End");
		}

		public void OperationAsync()
		{
			AsyncStateMachine stateMachine;
			stateMachine.outer = this;
			stateMachine.builder = AsyncVoidMethodBuilder.Create();
			stateMachine.state = -1;
			stateMachine.builder.Start(ref stateMachine);
		}
	}

	struct AsyncStateMachine : IAsyncStateMachine
	{
		public MyClass outer;
		public AsyncVoidMethodBuilder builder;
		public int state;

		void IAsyncStateMachine.MoveNext()
		{
			if (state == -1)
			{
				Console.WriteLine("OperationAsync Part I: ThreaID {0}\n", Thread.CurrentThread.ManagedThreadId);
				Task task = new Task(outer.Operation);
				task.Start();

				state = 0;

				TaskAwaiter awaiter = task.GetAwaiter();
				builder.AwaitOnCompleted(ref awaiter, ref this);

				return;
			}
			
			Console.WriteLine("\nOperationAsync Part II: ThreaID {0}", Thread.CurrentThread.ManagedThreadId);
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			builder.SetStateMachine(stateMachine);
		}
	}
}
