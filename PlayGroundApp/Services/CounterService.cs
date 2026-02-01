using PlayGroundApp.Models;

namespace PlayGroundApp.Services
{
    /// <summary>
    /// Service implementation for managing counter operations.
    /// Implements the ICounterService interface and manages the CounterModel.
    /// Responsible for handling counter business logic including incrementing
    /// and retrieving the current count value.
    /// </summary>
    public class CounterService : ICounterService
    {
        /// <summary>
        /// Internal reference to the CounterModel instance holding the counter state.
        /// </summary>
        private readonly CounterModel counter;

        /// <summary>
        /// Initializes a new instance of the CounterService class.
        /// Creates a new CounterModel to manage the counter state.
        /// </summary>
        public CounterService()
        {
            this.counter = new CounterModel();
        }

        /// <summary>
        /// Retrieves the current count value from the model.
        /// </summary>
        /// <returns>The current integer count value stored in the CounterModel.</returns>
        public int GetCount()
        {
            return this.counter.Count;
        }

        /// <summary>
        /// Increments the counter value by one.
        /// Updates the internal CounterModel's Count property.
        /// </summary>
        public void IncrementCount()
        {
            this.counter.Count++;
        }
    }
}