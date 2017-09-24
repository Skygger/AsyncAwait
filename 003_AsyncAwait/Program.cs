using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _003_AsyncAwait
{
	class MyClass
	{
		public void Operation()
		{
			Console.WriteLine("Operation ThreaID {0}", Thread.CurrentThread.ManagedThreadId);
			Console.WriteLine("Begin");
			Thread.Sleep(2000);
			Console.WriteLine("End");
		}

		public async void OperationAsync()
		{
			//метода начинается выполняться в контексте первичного потока
			Console.WriteLine("OperationAsync Part I: ThreaID {0}\n", Thread.CurrentThread.ManagedThreadId);

			Task task = new Task(Operation);
			task.Start();
			await task;

			//метод завершает работу в контексте вторичного потока
			Console.WriteLine("\nOperationAsync Part II: ThreaID {0}", Thread.CurrentThread.ManagedThreadId);

		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Main ThreaID {0}\n", Thread.CurrentThread.ManagedThreadId);
			MyClass myClass = new MyClass();
			myClass.OperationAsync();

			Console.ReadKey();
		}
	}
}
