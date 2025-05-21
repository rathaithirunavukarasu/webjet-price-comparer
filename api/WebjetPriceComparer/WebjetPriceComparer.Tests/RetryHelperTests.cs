using WebjetPriceComparer.Infrastructure.Helper;

namespace WebjetPriceComparer.Tests.Helpers
{
    public class RetryHelperTests
    {
        [Fact]
        public async Task ExecuteWithRetryAsync_ReturnsImmediately_OnFirstSuccess()
        {
            // Arrange
            var attemptCount = 0;

            Task<string?> TestAction()
            {
                attemptCount++;
                return Task.FromResult("Success" as string);
            }

            // Act
            var result = await RetryHelper.ExecuteWithRetryAsync(TestAction);

            // Assert
            Assert.Equal("Success", result);
            Assert.Equal(1, attemptCount);
        }

        [Fact]
        public async Task ExecuteWithRetryAsync_RetriesAndSucceeds()
        {
            // Arrange
            var attemptCount = 0;

            Task<string?> TestAction()
            {
                attemptCount++;
                if (attemptCount < 3)
                    throw new InvalidOperationException("Fail");

                return Task.FromResult("Recovered" as string);
            }

            // Act
            var result = await RetryHelper.ExecuteWithRetryAsync(TestAction, maxRetries: 5, delayMilliseconds: 1);

            // Assert
            Assert.Equal("Recovered", result);
            Assert.Equal(3, attemptCount);
        }

        [Fact]
        public async Task ExecuteWithRetryAsync_ReturnsDefault_WhenAllRetriesFail()
        {
            // Arrange
            var attemptCount = 0;

            Task<string?> TestAction()
            {
                attemptCount++;
                throw new Exception("Persistent failure");
            }

            // Act
            var result = await RetryHelper.ExecuteWithRetryAsync(TestAction, maxRetries: 3, delayMilliseconds: 1);

            // Assert
            Assert.Null(result);
            Assert.Equal(3, attemptCount);
        }

        [Fact]
        public async Task ExecuteWithRetryAsync_ThrowsOnSingleTry()
        {
            // Arrange
            var attemptCount = 0;

            Task<string?> TestAction()
            {
                attemptCount++;
                throw new Exception("Immediate failure");
            }

            // Act
            var result = await RetryHelper.ExecuteWithRetryAsync(TestAction, maxRetries: 1, delayMilliseconds: 1);

            // Assert
            Assert.Null(result);
            Assert.Equal(1, attemptCount);
        }
    }
}
