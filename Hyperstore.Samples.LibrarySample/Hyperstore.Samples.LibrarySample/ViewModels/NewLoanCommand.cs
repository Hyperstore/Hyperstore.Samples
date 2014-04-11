using Hyperstore.Modeling;
using Hyperstore.Modeling.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperstore.Samples.LibrarySample.ViewModels
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///  Command for a new loan with its command handler
    /// </summary>
    /// <seealso cref="T:Hyperstore.Modeling.Commands.AbstractDomainCommand"/>
    /// <seealso cref="T:Hyperstore.Modeling.Commands.ICommandHandler{Hyperstore.Samples.LibrarySample.ViewModels.NewLoanCommand}"/>
    ///-------------------------------------------------------------------------------------------------
    class NewLoanCommand : AbstractDomainCommand, ICommandHandler<NewLoanCommand>
    {
        public Member Member { get; private set; }
        public Book Book { get; set; }

        public NewLoanCommand(Book book, Member member) : base( ((IModelElement)book).DomainModel)
        {
            Contract.Requires(book != null, "book");
            Contract.Requires(Member != null, "member");

            this.Member = member;
            this.Book = book;
        }

        Modeling.Events.IEvent ICommandHandler<NewLoanCommand>.Handle(ExecutionCommandContext<NewLoanCommand> context)
        {
            var loan = new Loan();
            loan.Member = Member;
            loan.Book = Book;
            Book.Copies -= 1;
            Member.Library.Loans.Add(loan);
            return null;
        }
    }
}
