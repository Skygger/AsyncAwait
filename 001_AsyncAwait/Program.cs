using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _001_AsyncAwait
{
	class MyClass
	{
		public void Operation()
		{
			Console.WriteLine("Opearation ThreaID {0}", Thread.CurrentThread.ManagedThreadId);
			Console.WriteLine("Begin");
			Thread.Sleep(2000);
			Console.WriteLine("End");
		}

		public async void OperationAsync()
		{
			Task task = new Task(Operation);
			task.Start();
			await task;
		}
	}

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
}
