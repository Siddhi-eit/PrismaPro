using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.Abstract
{
    public interface IConnectionMapping<T>
    {
        public void Add(T key, string connectionId);

        public IEnumerable<string> GetConnections(T key);

        public void Remove(T key, string connectionId);

        public void Remove(T key);

    }
}
