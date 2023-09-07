using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Artemis.Contracts.Repositories
{
    public abstract class Repository<T> where T : class
    {
        protected async Task<List<T>> HandleNullCancelTask(Task<List<T>> task)
        {
            try
            {
                return await task;
            }
            catch (Exception e) when (e is ArgumentNullException ||
                                      e is OperationCanceledException)
            {
                try
                {
                    Console.WriteLine(e);
                }
                catch (IOException)
                {
                }
                return new List<T>();
            }
        }

        protected async Task<T?> HandleNullCancelTask(Task<T?> task)
        {
            try
            {
                return await task;
            }
            catch (Exception e) when (e is ArgumentNullException ||
                                      e is OperationCanceledException)
            {
                try
                {
                    Console.WriteLine(e);
                }
                catch (IOException)
                {
                }
                return null;
            }
        }

        protected async Task HandleCancelTask(ValueTask<EntityEntry<T>> task)
        {
            try
            {
                await task;
            }
            catch (OperationCanceledException e)
            {
                try
                {
                    Console.WriteLine(e);
                }
                catch (IOException)
                {
                }
            }
        }

        protected async Task HandleCancelTask(Task task)
        {
            try
            {
                await task;
            }
            catch (OperationCanceledException e)
            {
                try
                {
                    Console.WriteLine(e);
                }
                catch (IOException)
                {
                }
            }
        }
    }
}
