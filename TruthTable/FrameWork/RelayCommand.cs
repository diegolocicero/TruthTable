using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SaleWPF.FrameWork
{
    public class RelayCommand<T> : ICommand
    {
        public RelayCommand(Action<T> execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public RelayCommand(Action<T> execute) : this(execute, null) { }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Func<bool> canExecute;
        private Action<T> execute;

        public bool CanExecute(object parameter)
        {
            return (canExecute == null) || canExecute();
        }
        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }

    public class RelayCommand : ICommand
    {
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public RelayCommand(Action execute) : this(execute, null) { }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Func<bool> canExecute;
        private Action execute;

        public bool CanExecute(object parameter)
        {
            return (canExecute == null) || canExecute();
        }
        public void Execute(object parameter)
        {
            execute();
        }
    }
}
