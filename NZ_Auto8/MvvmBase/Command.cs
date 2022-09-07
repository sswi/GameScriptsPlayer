using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace XE.Commands
{
    public class Command : ICommand
    {
        private readonly Func<object, bool> _canExecute;

        private readonly Action<object> _execute;

        private readonly WeakEventManager _weakEventManager = new WeakEventManager();

        //
        // 摘要:
        //     Occurs when the target of the Command should reevaluate whether or not the Command
        //     can be executed.
        //
        // 言论：
        //     To be added.

        public event EventHandler CanExecuteChanged

        {
            add
            {
                _weakEventManager.AddEventHandler(value, "CanExecuteChanged");
            }
            remove
            {
                _weakEventManager.RemoveEventHandler(value, "CanExecuteChanged");
            }
        }


        public Command(Action<object> execute)

        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public Command(Action execute)
            : this((Action<object>)delegate
            {
                execute();
            })
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        public Command(Action<object> execute, Func<object, bool> canExecute)
            : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public Command(Action execute, Func<bool> canExecute)
            : this(delegate
            {
                execute();
            }, (object o) => canExecute())
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }
        }

        //
        // 摘要:
        //     Returns a System.Boolean indicating if the Command can be exectued with the given
        //     parameter.
        //
        // 参数:
        //   parameter:
        //     An System.Object used as parameter to determine if the Command can be executed.
        //
        // 返回结果:
        //     true if the Command can be executed, false otherwise.
        //
        // 言论：
        //     If no canExecute parameter was passed to the Command constructor, this method
        //     always returns true.
        //     If the Command was created with non-generic execute parameter, the parameter
        //     of this method is ignored.

        public bool CanExecute(object parameter)

        {
            if (_canExecute != null)
            {
                return _canExecute(parameter);
            }

            return true;
        }

        //
        // 摘要:
        //     Invokes the execute Action
        //
        // 参数:
        //   parameter:
        //     An System.Object used as parameter for the execute Action.
        //
        // 言论：
        //     If the Command was created with non-generic execute parameter, the parameter
        //     of this method is ignored.

        public void Execute(object parameter) => _execute(parameter);


        //
        // 摘要:
        //     Send a System.Windows.Input.ICommand.CanExecuteChanged
        //
        // 言论：
        //     To be added.
        public void ChangeCanExecute()
        {
            _weakEventManager.HandleEvent(this, EventArgs.Empty, "CanExecuteChanged");
        }
    }


    public class WeakEventManager
    {
        private struct Subscription
        {
            public readonly WeakReference Subscriber;

            public readonly MethodInfo Handler;

            public Subscription(WeakReference subscriber, MethodInfo handler)
            {
                Subscriber = subscriber;
                Handler = (handler ?? throw new ArgumentNullException(nameof(handler)));
            }
        }

        private readonly Dictionary<string, List<Subscription>> _eventHandlers = new Dictionary<string, List<Subscription>> ();

        //
        // 摘要:
        //     To be added.
        //
        // 参数:
        //   handler:
        //     To be added.
        //
        //   eventName:
        //     To be added.
        //
        // 类型参数:
        //   TEventArgs:
        //     To be added.
        //
        // 言论：
        //     To be added.
        public void AddEventHandler<TEventArgs>(EventHandler<TEventArgs> handler, [CallerMemberName] string? eventName = null) where TEventArgs : EventArgs
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

#pragma warning disable CS8604 // 引用类型参数可能为 null。
            AddEventHandler(eventName, handler.Target, handler.GetMethodInfo());
#pragma warning restore CS8604 // 引用类型参数可能为 null。
        }

        //
        // 摘要:
        //     To be added.
        //
        // 参数:
        //   handler:
        //     To be added.
        //
        //   eventName:
        //     To be added.
        //
        // 言论：
        //     To be added.
        public void AddEventHandler(EventHandler handler, [CallerMemberName] string? eventName = null)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

#pragma warning disable CS8604 // 引用类型参数可能为 null。
            AddEventHandler(eventName, handler.Target, handler.GetMethodInfo());
#pragma warning restore CS8604 // 引用类型参数可能为 null。
        }

        //
        // 摘要:
        //     To be added.
        //
        // 参数:
        //   sender:
        //     To be added.
        //
        //   args:
        //     To be added.
        //
        //   eventName:
        //     To be added.
        //
        // 言论：
        //     To be added.
        public void HandleEvent(object sender, object args, string eventName)
        {
            List<(object, MethodInfo)> list = new List<(object, MethodInfo)> ();
            List<Subscription> list2 = new List<Subscription> ();

            if (_eventHandlers.TryGetValue(eventName, out List<Subscription> value))

            {
                for (int i = 0; i < value.Count; i++)
                {
                    Subscription item = value[i];
                    if (item.Subscriber == null)
                    {

                        list.Add((null, item.Handler));

                        continue;
                    }


                    object target = item.Subscriber.Target;

                    if (target == null)
                    {
                        list2.Add(item);
                    }
                    else
                    {
                        list.Add((target, item.Handler));
                    }
                }

                for (int j = 0; j < list2.Count; j++)
                {
                    Subscription item2 = list2[j];
                    value.Remove(item2);
                }
            }

            for (int k = 0; k < list.Count; k++)
            {
                (object, MethodInfo) tuple = list[k];
                object item3 = tuple.Item1;
                tuple.Item2.Invoke(item3, new object[2]
                {
                    sender,
                    args
                });
            }
        }

        //
        // 摘要:
        //     To be added.
        //
        // 参数:
        //   handler:
        //     To be added.
        //
        //   eventName:
        //     To be added.
        //
        // 类型参数:
        //   TEventArgs:
        //     To be added.
        //
        // 言论：
        //     To be added.
        public void RemoveEventHandler<TEventArgs>(EventHandler<TEventArgs> handler, [CallerMemberName] string? eventName = null) where TEventArgs : EventArgs
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            RemoveEventHandler(eventName, handler.Target, handler.GetMethodInfo());

        }

        //
        // 摘要:
        //     To be added.
        //
        // 参数:
        //   handler:
        //     To be added.
        //
        //   eventName:
        //     To be added.
        //
        // 言论：
        //     To be added.
        public void RemoveEventHandler(EventHandler handler, [CallerMemberName] string? eventName = null)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }


            RemoveEventHandler(eventName, handler.Target, handler.GetMethodInfo());

        }

        private void AddEventHandler(string eventName, object handlerTarget, MethodInfo methodInfo)
        {

            if (!_eventHandlers.TryGetValue(eventName, out List<Subscription> value))

            {
                value = new List<Subscription>();
                _eventHandlers.Add(eventName, value);
            }

            if (handlerTarget == null)
            {

                value.Add(new Subscription(null, methodInfo));

            }
            else
            {
                value.Add(new Subscription(new WeakReference(handlerTarget), methodInfo));
            }
        }

        private void RemoveEventHandler(string eventName, object handlerTarget, MemberInfo methodInfo)
        {
            if (!_eventHandlers.TryGetValue(eventName, out var value))
            {
                return;
            }

            for (int num = value.Count; num > 0; num--)
            {
                Subscription item = value[num - 1];
                if (item.Subscriber?.Target == handlerTarget && !(item.Handler.Name != methodInfo.Name))
                {
                    value.Remove(item);
                    break;
                }
            }
        }
    }
}
