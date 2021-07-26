using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Berger.Common.Extensions
{
    public class TaskExt
    {

        public static async Task<IEnumerable<T>> WhenAll<T>(params Task<T>[] tasks)
        {
            var allTasks = Task.WhenAll(tasks);
            try
            {
                return await allTasks;
            }
            catch (Exception)
            {
                // ignored
            }

            throw allTasks.Exception ?? throw new Exception("Aggregate task error.");
        }


    }
}
