using Hyperstore.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperstore.Samples.Overview
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///  The data model 
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    class Model 
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Current customer
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public Customer Customer { get; private set; }

        public async Task Initialize()
        {
            // Quickly create a new domain named 'test' with its schema MyModelDefinition
            var domain = await StoreBuilder.CreateDomain<MyModelDefinition>("test");

            // Instanciate a new Customer within the domain (always in a session)
            using (var session = domain.Store.BeginSession())
            {
                Customer = new Customer(domain);
                session.AcceptChanges();
            }
        }
    }
}
