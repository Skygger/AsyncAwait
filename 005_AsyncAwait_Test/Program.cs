using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _005_AsyncAwait_Test
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
			stateMachine.counterCallMoveNext = 0;
			stateMachine.builder.Start(ref stateMachine);
		}
		private struct AsyncStateMachine : IAsyncStateMachine
		{
			public MyClass outer;
			public AsyncVoidMethodBuilder builder;
			public int state;

			public int counterCallMoveNext;
			

			//builder.Start первый раз вызывает метод MoveNext() синхронно
			//а второй раз builder.AwaitOnCompleted вызывает его асинхронно
			void IAsyncStateMachine.MoveNext()
			{
				Console.WriteLine("\nМетод MoveNext вызван {0}-й раз в потоке {1}", ++counterCallMoveNext, Thread.CurrentThread.ManagedThreadId);
				if (state == -1)
				{
					Console.WriteLine("OperationAsync Part I: ThreaID {0}\n", Thread.CurrentThread.ManagedThreadId);
					Task task = new Task(outer.Operation);
					task.Start();

					state = 0;

					TaskAwaiter awaiter = task.GetAwaiter();
					builder.AwaitOnCompleted(ref awaiter, ref this); //закомментировать

					return;
				}

				Console.WriteLine("\nOperationAsync Part II: ThreaID {0}", Thread.CurrentThread.ManagedThreadId);
			}

			//builder.AwaitOnCompleted вызывает этот метод синхронно, во время выполнения задачи
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				Console.WriteLine("stateMachine.GetHashCode(): {0}", stateMachine.GetHashCode());
				Console.WriteLine("this.GetHashCode():         {0}", this.GetHashCode());
				builder.SetStateMachine(stateMachine);
			}
		}
	}

	
}
