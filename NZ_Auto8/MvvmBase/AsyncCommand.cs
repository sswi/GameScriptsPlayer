using System;
using System.Threading.Tasks;
using System.Windows.Input;
using XE.Commands.Abstraction;

namespace XE.Commands
{
    public class AsyncCommand : IAsyncCommand
    {
        public event EventHandler? CanExecuteChanged;
        private bool _isExecuting;
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private readonly IErrorHandler _errorHandler;


        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null!, IErrorHandler errorHandler = null!)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }

        public bool CanExecute()
        {
            return !_isExecuting && (_canExecute?.Invoke() ?? true);
        }

        public async Task ExecuteAsync()
        {
            if (CanExecute())
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

            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Explicit implementations
#pragma warning disable CS8769 // 参数类型中引用类型的为 Null 性与实现的成员不匹配(可能是由于为 Null 性特性)。
        bool ICommand.CanExecute(object parameter)
#pragma warning restore CS8769 // 参数类型中引用类型的为 Null 性与实现的成员不匹配(可能是由于为 Null 性特性)。
        {
            return CanExecute();
        }

#pragma warning disable CS8769 // 参数类型中引用类型的为 Null 性与实现的成员不匹配(可能是由于为 Null 性特性)。
        void ICommand.Execute(object parameter)
#pragma warning restore CS8769 // 参数类型中引用类型的为 Null 性与实现的成员不匹配(可能是由于为 Null 性特性)。
        {
            ExecuteAsync().FireAndForgetSafeAsync(_errorHandler);
        }
        #endregion
    }

    public class AsyncCommand<T> : IAsyncCommand<T>
    {
        public event EventHandler? CanExecuteChanged;

        private bool _isExecuting;
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;
        private readonly IErrorHandler _errorHandler;

        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null!, IErrorHandler errorHandler = null!)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }
#pragma warning disable IDE0051 // 删除未使用的私有成员
        private AsyncCommand(Func<T, Task> execute, Func<bool> canExecute, IErrorHandler errorHandler = null!)
#pragma warning restore IDE0051 // 删除未使用的私有成员
        {
            _execute = execute;
            _canExecute = _ => canExecute();
            _errorHandler = errorHandler;
        }

        public bool CanExecute(T parameter)
        {
            return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
        }

        public async Task ExecuteAsync(T parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    await _execute(parameter);
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Explicit implementations
#pragma warning disable CS8769 // 参数类型中引用类型的为 Null 性与实现的成员不匹配(可能是由于为 Null 性特性)。
        bool ICommand.CanExecute(object parameter)
#pragma warning restore CS8769 // 参数类型中引用类型的为 Null 性与实现的成员不匹配(可能是由于为 Null 性特性)。
        {
            return CanExecute((T)parameter);
        }

#pragma warning disable CS8769 // 参数类型中引用类型的为 Null 性与实现的成员不匹配(可能是由于为 Null 性特性)。
        void ICommand.Execute(object parameter)
#pragma warning restore CS8769 // 参数类型中引用类型的为 Null 性与实现的成员不匹配(可能是由于为 Null 性特性)。
        {
            ExecuteAsync((T)parameter).FireAndForgetSafeAsync(_errorHandler);
        }
        #endregion
    }
}
