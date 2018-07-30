using System;
using System.Collections.Generic;

namespace OrdersCollector.Utils.Randomization
{
    public class Randomizer : IRandomizer
    {
        private ThreadSafeRandom random = new ThreadSafeRandom();

        public T GetRandomItem<T>(IList<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection.Count == 0)
            {
                return default(T);
            }

            return collection[random.Next(collection.Count)];
        }

        private class ThreadSafeRandom
        {
            private object syncRoot = new object();
            private Random random = new Random();

            public int Next(int maxValue)
            {
                lock (syncRoot)
                {
                    return random.Next(maxValue);
                }
            }
        }
    }
}
