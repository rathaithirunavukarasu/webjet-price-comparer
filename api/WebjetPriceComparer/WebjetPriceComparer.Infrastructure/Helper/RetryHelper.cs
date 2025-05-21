namespace WebjetPriceComparer.Infrastructure.Helper
{
    /// <summary>
    /// Provides helper methods for executing actions with retry logic.
    /// </summary>
    public static class RetryHelper
    {
        /// <summary>
        /// Executes the specified asynchronous action with retry logic.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="action">The asynchronous action to execute.</param>
        /// <param name="maxRetries">The maximum number of retry attempts. Default is 3.</param>
        /// <param name="delayMilliseconds">The delay in milliseconds between retries. Default is 1000.</param>
        /// <returns>The result of the action if successful; otherwise, <c>default</c> for type <typeparamref name="T"/>.</returns>
        public static async Task<T?> ExecuteWithRetryAsync<T>(
            Func<Task<T?>> action,
            int maxRetries = 3,
            int delayMilliseconds = 1000)
        {
            while (maxRetries-- > 0)
            {
                try
                {
                    return await action();
                }
                catch
                {
                    if (maxRetries == 0)
                        break;

                    await Task.Delay(delayMilliseconds);
                }
            }

            return default;
        }
    }
}
