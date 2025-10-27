using System.Collections;
using System.Collections.Generic;
using Type = System.Type;

namespace ScriptableObjectArchitecture
{
    public abstract class BaseCollection : SOArchitectureBaseObject, IEnumerable, IGameEvent
    {
        public object this[int index]
        {
            get
            {
                return List[index];
            }
            set
            {
                List[index] = value;
            }
        }

        public int Count { get { return List.Count; } }

        public abstract IList List { get; }
        public abstract Type Type { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }
        public bool Contains(object obj)
        {
            return List.Contains(obj);
        }

        #region IGameEvent
        protected readonly List<IGameEventListener> _listeners = new List<IGameEventListener>();
        protected readonly List<System.Action> _actions = new List<System.Action>();

        public void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised();

            for (int i = _actions.Count - 1; i >= 0; i--)
                _actions[i]();
        }

        public void AddListener(IGameEventListener listener)
        {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }

        public void RemoveListener(IGameEventListener listener)
        {
            if (_listeners.Contains(listener))
                _listeners.Remove(listener);
        }

        public void AddListener(System.Action action)
        {
            if (!_actions.Contains(action))
                _actions.Add(action);
        }
        public void RemoveListener(System.Action action)
        {
            if (_actions.Contains(action))
                _actions.Remove(action);
        }

        public void Reset()
        {
            _listeners.Clear();
            _actions.Clear();
        }
        #endregion
    }
}