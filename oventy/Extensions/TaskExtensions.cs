using System;
using System.Threading.Tasks;

namespace oventy
{
    public static class TaskExtensions
    {
        /// <summary>
        ///     Add timeout to a Task 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="timeoutInMilliseconds"></param>
        /// <returns></returns>
        public static async Task<T> WithTimeout<T>(
                this Task<T> task,
                int timeoutInMilliseconds)
        {
            var retTask = await Task.WhenAny(task, Task.Delay(timeoutInMilliseconds))
                                    .ConfigureAwait(false);

            if(retTask is Task<T>)
                return task.Result;
            return default(T);
        }
    }
}
