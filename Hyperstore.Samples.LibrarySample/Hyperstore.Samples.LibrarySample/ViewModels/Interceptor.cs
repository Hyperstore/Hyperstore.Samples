using Hyperstore.Modeling.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperstore.Samples.LibrarySample.ViewModels
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///  Interceptor for the NewLoanCommand. 
    ///  This interceptor is registered by composition when the store is initialized (with store.DependencyResolver.ComposeAsync(this.GetType().Assembly);)
    /// </summary>
    /// <seealso cref="T:Hyperstore.Modeling.Commands.AbstractCommandInterceptor{Hyperstore.Samples.LibrarySample.ViewModels.NewLoanCommand}"/>
    ///-------------------------------------------------------------------------------------------------
    [CommandInterceptor]
    class Interceptor : AbstractCommandInterceptor<NewLoanCommand>
    {
        public override BeforeContinuationStatus OnBeforeExecution(ExecutionCommandContext<NewLoanCommand> context)
        {
            if (context.Command.Member.Name.Contains("9"))
            {
                context.Log(new Modeling.DiagnosticMessage(Modeling.MessageType.Error, "Member 9 is not authorized to loan a book.", "Error"));
                return BeforeContinuationStatus.Abort; // Command aborted
            }

            return base.OnBeforeExecution(context);
        }
    }
}
