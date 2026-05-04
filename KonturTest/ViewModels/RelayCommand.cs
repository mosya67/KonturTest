using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KonturTest.ViewModels
{
    // Простая реализация команд (RelayCommand)
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<Task> _executeAsync; // Для асинхронных методов

        public RelayCommand(Action execute) => _execute = execute;
        public RelayCommand(Func<Task> executeAsync) => _executeAsync = executeAsync;

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
        {
            if (_executeAsync != null) await _executeAsync();
            else _execute?.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}
