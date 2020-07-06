using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cookbook.Client.Utils
{
    /// <summary>Custom implementation of asynchronous command class</summary>
    public class AsyncCommand : ICommand
    {
        private bool _isExecuting;
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private async Task ExecuteAsync()
        {
            if (CanExecute(null))
            {
                try
                {
                    _isExecuting = true;
                    await _execute();
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) =>
            !_isExecuting && (_canExecute?.Invoke() ?? true);

        public void Execute(object parameter) => ExecuteAsync();
    }
}
