using System;
using System.Threading;
using System.Threading.Tasks;


namespace dotnetAccountant
{
	public static class TaskExtensions
	{
		public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
		{

			using (var tokenSource = new CancellationTokenSource())
			{

				var completedTask = await Task.WhenAny(task, Task.Delay(timeout, tokenSource.Token));
				if (completedTask == task)
				{
					tokenSource.Cancel();
					return await task;  // Very important in order to propagate exceptions
				}
				else
				{
					throw new TimeoutException("The operation has timed out.");
				}
			}
		}
	}
}