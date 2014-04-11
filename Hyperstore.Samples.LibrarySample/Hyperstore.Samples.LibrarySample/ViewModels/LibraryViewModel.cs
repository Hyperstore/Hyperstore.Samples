using Hyperstore.Modeling;
using Hyperstore.Modeling.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hyperstore.Samples.LibrarySample.ViewModels
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///  A ViewModel for the library.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    class LibraryViewModel
    {
        // Technical fields
        private IHyperstore store; 
        private IUndoManager undoManager;

        // MVVM properties
        public Library Library { get; private set; }
        public Book SelectedBook { get; set; }
        public Member SelectedMember { get; set; }

        // MVVM Commands
        public ICommand UndoCommand { get; set; }
        public ICommand RedoCommand { get; set; }
        public ICommand CreateLoanCommand { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Constructor.
        /// </summary>
        /// <param name="wnd">
        ///  The associated window.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public LibraryViewModel(FrameworkElement wnd)
        {
            Initialize(wnd);
        }

        public async Task Initialize(FrameworkElement wnd)
        {
            var domain = await InitializeStore();
            PopulateLibraryDomain(domain);

            UndoCommand = new RelayCommand(p => { undoManager.Undo(); }, p => undoManager.CanUndo);
            RedoCommand = new RelayCommand(p => { undoManager.Redo(); }, p => undoManager.CanRedo);

            CreateLoanCommand = new RelayCommand(p => CreateLoan(), p => SelectedBook != null && SelectedMember != null && SelectedBook.Copies > 0);

            undoManager = new UndoManager(store);
            // Now, all changes in the domain will be memorized
            undoManager.RegisterDomain(domain);

            // Collaborative mode, show how the model is synchronized between muliples instances.
            // To try it, uncomment the following line and run several instances (outside Visual Studio) of the application (locally). Every change will be repercuted on other instances. 
            //await InitializeCollaborativeMode(domain);
            
            wnd.DataContext = this;
        }

        private async Task InitializeCollaborativeMode(IDomainModel domain)
        {
            // Configure a bidirectionnel communication for this domain
            store.EventBus.RegisterDomainPolicies(domain, new Modeling.Messaging.ChannelPolicy(EventPropagationStrategy.All), new Modeling.Messaging.ChannelPolicy(EventPropagationStrategy.All));

            // Open a peer to peer channel
            await store.EventBus.OpenAsync(new Hyperstore.Modeling.Messaging.PeerToPeerPChannel(new Uri("net.p2p://localhost/Library")));
        }

        private void CreateLoan()
        {
            try
            {
                // Run a command
                using (var session = store.BeginSession())
                {
                    session.Execute(new NewLoanCommand(SelectedBook, SelectedMember));
                    session.AcceptChanges();
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            };
        }

        private void PopulateLibraryDomain(IDomainModel domain)
        {
            // All changes must be include in a session
            using (var session = store.BeginSession())
            {
                // Forcing the id is optional by necessary for the synchronization sample
                Library = new Library(id:domain.CreateId("0")); // Force an unique id
                Library.Name = "The shop around the corner";
                session.AcceptChanges();
            }

            // Add some books
            Parallel.For(0, 10, i =>
            {
                using (var session = store.BeginSession())
                {
                    var book = new Book(id: domain.CreateId("B" + i.ToString()));
                    book.Copies = i + 1;
                    book.Author = "Author " + i.ToString();
                    book.Title = "Book " + i.ToString();
                    Library.Books.Add(book);
                    session.AcceptChanges();
                }
            });

            // And some members
            Parallel.For(0, 10, i =>
            {
                using (var session = store.BeginSession())
                {
                    var member = new Member(id: domain.CreateId("M" + i.ToString()));
                    member.Name = "Member " + i.ToString();
                    Library.Members.Add(member);
                    session.AcceptChanges();
                }
            });
        }

        private async Task<IDomainModel> InitializeStore()
        {
            // Create a store
            store = new Store();
            // This optional step is used to include the command interceptor
            await store.DependencyResolver.ComposeAsync(this.GetType().Assembly);
            // Load the library schema
            await store.LoadSchemaAsync(new MyLibraryDefinition());
            // And instanciate a domain to manage all the elements of the domain
            var domain = await store.CreateDomainModelAsync("MyLib");
            // Set as default domain
            store.DefaultSessionConfiguration.DefaultDomainModel = domain;
            return domain;
        }
    }
}
