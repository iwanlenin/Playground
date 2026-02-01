using PlayGroundApp.Models;

namespace PlayGroundApp.Services
{
    public class CounterService : ICounterService
    {
        private readonly CounterModel counter;

        public CounterService()
        {
            this.counter = new CounterModel();
        }

        public int GetCount()
        {
            return this.counter.Count;
        }

        public void IncrementCount()
        {
            this.counter.Count++;
        }
    }
}