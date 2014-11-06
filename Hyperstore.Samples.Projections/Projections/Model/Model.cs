using Hyperstore.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperstore.Samples.Projections
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
        public Library Library { get; private set; }

        public async Task Initialize()
        {
            // Quickly create a new domain named 'test' with its schema MyModelDefinition
            var domain = await StoreBuilder.CreateDomain<LibraryDefinition>("lib");

            // Allow to omit the domain argument when you create a domain element.            
            domain.Store.DefaultSessionConfiguration.DefaultDomainModel = domain;

            // Instanciate a new Library within the domain (always in a session)
            var rnd = new Random();
            using (var session = domain.Store.BeginSession())
            { 
                Library = new Library();
                Library.Name = "My library";

                for (int i = 1; i < 20; i++)
                {
                    var b = new Book();
                    b.Title = "Book " + i.ToString();
                    b.Copies = 1 + rnd.Next(10);
                    Library.Books.Add(b);

                    var m = new Member();
                    m.Name = "Member " + i.ToString();
                    Library.Members.Add(m);
                }

                session.AcceptChanges();
            }
        }
    }
}
