using System;

namespace CoreServer.Common.Core
{
    public class EventObserver<T> : IObserver<T>
    {
        private IDisposable _unsubscriber;

        private readonly Action<T> _action;
        private readonly Action<Exception> _errorAction;
        private readonly Action _completedAction;

        public EventObserver(Action<T> action, Action<Exception> errorAction = null, Action completedAction = null)
        {
            _action = action;
            _errorAction = errorAction;
            _completedAction = completedAction;
        }

        public void Subscribe(IObservable<T> provider)
        {
            if (provider != null)
            {
                _unsubscriber = provider.Subscribe(this);
            }
        }

        public void Unsubscribe()
        {
            _unsubscriber?.Dispose();
        }

        public void OnCompleted()
        {
            _completedAction?.Invoke();
        }

        public void OnError(Exception error)
        {
            _errorAction?.Invoke(error);
        }

        public void OnNext(T value)
        {
            _action?.Invoke(value);
        }
    }
}