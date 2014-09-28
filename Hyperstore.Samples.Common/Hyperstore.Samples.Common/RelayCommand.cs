using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Hyperstore.Samples.Common
{
    public class RelayCommand : ICommand
    {
        private readonly Action _executeHandler;
        private readonly Func<bool> _canExecuteHandler;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action executeHandler, Func<bool> canExecuteHandler)
        {
            _executeHandler = executeHandler;
            _canExecuteHandler = canExecuteHandler;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteHandler != null ? _canExecuteHandler() : true;
        }

        public void Execute(object parameter)
        {
            _executeHandler();
        }
    }
}
