using Hyperstore.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperstore.Samples.Overview
{
    class Model
    {
        public Customer Customer { get; private set; }

        public async Task Initialize()
        {
            var domain = await StoreBuilder.CreateDomain<MyModelDefinition>("test");
            using (var session = domain.Store.BeginSession())
            {
                Customer = new Customer(domain);
                session.AcceptChanges();
            }
        }
    }
}
