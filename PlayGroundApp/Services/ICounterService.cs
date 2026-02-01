namespace PlayGroundApp.Services
{
    /// <summary>
    /// Interface that defines the contract for counter service operations.
    /// Provides abstraction for counter-related business logic,
    /// enabling dependency injection and easy testing.
    /// </summary>
    public interface ICounterService
    {
        /// <summary>
        /// Retrieves the current count value.
        /// </summary>
        /// <returns>The current integer count value.</returns>
        int GetCount();

        /// <summary>
        /// Increments the counter by one.
        /// </summary>
        void IncrementCount();
    }
}