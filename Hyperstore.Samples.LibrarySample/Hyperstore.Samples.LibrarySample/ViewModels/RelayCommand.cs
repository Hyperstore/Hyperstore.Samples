using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Hyperstore.Samples.LibrarySample.ViewModels
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///  A relay command for the mvvm pattern
    /// </summary>
    /// <seealso cref="T:System.Windows.Input.ICommand"/>
    ///-------------------------------------------------------------------------------------------------
    public class RelayCommand : ICommand
    {
        #region Fields

        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        public bool CanExecute(object parameters)
        {
            return canExecute == null ? true : canExecute(parameters);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameters)
        {
            execute(parameters);
        }

        #endregion // ICommand Members
    }
}
