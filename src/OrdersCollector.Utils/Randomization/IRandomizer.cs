using System.Collections.Generic;

namespace OrdersCollector.Utils.Randomization
{
    public interface IRandomizer
    {
        T GetRandomItem<T>(IList<T> collection);
    }
}
