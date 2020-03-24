using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreServer.Common.Core
{
    public class EventObservable<T> : IObservable<T>
    {
        private readonly List<IObserver<T>> _observers;

        public EventObservable()
        {
            _observers = new List<IObserver<T>>();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new Unsubscriber(_observers, observer);
        }

        public void Run(Func<T> func)
        {
            foreach (var observer in _observers.ToList())
            {
                try
                {
                    observer.OnNext(func());
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }
                finally
                {
                    observer.OnCompleted();
                }
            }
        }

        public void Clear()
        {
            _observers.Clear();
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<T>> _observers;
            private readonly IObserver<T> _observer;

            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers != null && _observers.Contains(_observer))
                {
                    _observers.Remove(_observer);
                }
            }
        }
    }
}