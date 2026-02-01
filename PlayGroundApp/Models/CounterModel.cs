namespace PlayGroundApp.Models
{
    /// <summary>
    /// Represents the counter model for the counter example functionality.
    /// This is a simple model that stores the counter value.
    /// Used in conjunction with CounterService for managing counter state.
    /// </summary>
    public class CounterModel
    {
        /// <summary>
        /// The current count value.
        /// </summary>
        public int Count { get; set; }
    }
}