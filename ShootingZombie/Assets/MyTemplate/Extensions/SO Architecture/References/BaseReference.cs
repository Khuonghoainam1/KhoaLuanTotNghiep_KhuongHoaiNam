using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    public class BaseReference<TBase, TVariable> : BaseReference where TVariable : BaseVariable<TBase>
    {
        public BaseReference() { }
        public BaseReference(TBase baseValue)
        {
            _useConstant = true;
            _constantValue = baseValue;
        }

        protected readonly List<IGameEventListener> _listeners = new List<IGameEventListener>();
        protected readonly List<System.Action> _actions = new List<System.Action>();

        [SerializeField]
        protected bool _useConstant = false;
        [SerializeField]
        protected TBase _constantValue = default(TBase);
        [SerializeField]
        protected TVariable _variable = default(TVariable);

        public TBase Value
        {
            get
            {
                return (_useConstant || _variable == null) ? _constantValue : _variable.Value;
            }
            set
            {
                if (!_useConstant && _variable != null)
                {
                    _variable.Value = value;
                }
                else
                {
                    _useConstant = true;
                    _constantValue = value;
                    Raise();
                }
            }
        }
        public bool IsValueDefined
        {
            get
            {
                return _useConstant || _variable != null;
            }
        }

        public BaseReference CreateCopy()
        {
            BaseReference<TBase, TVariable> copy = (BaseReference<TBase, TVariable>)System.Activator.CreateInstance(GetType());
            copy._useConstant = _useConstant;
            copy._constantValue = _constantValue;
            if (_useConstant)
            {
                copy._variable = _variable;
            }
            else
            {
                copy._variable = ScriptableObject.Instantiate(_variable);
            }            

            return copy;
        }
        public void AddListener(IGameEventListener listener)
        {
            if (_variable != null)
            {
                _variable.AddListener(listener);
            }
            else
            {
                if (!_listeners.Contains(listener))
                    _listeners.Add(listener);
            }
        }
        public void RemoveListener(IGameEventListener listener)
        {
            if (_variable != null)
            {
                _variable.RemoveListener(listener);
            }
            else
            {
                if (_listeners.Contains(listener))
                    _listeners.Remove(listener);
            }
        }
        public void AddListener(System.Action action)
        {
            if (_variable != null)
            {
                _variable.AddListener(action);
            }
            else
            {
                if (!_actions.Contains(action))
                    _actions.Add(action);
            }
        }
        public void RemoveListener(System.Action action)
        {
            if (_variable != null)
            {
                _variable.RemoveListener(action);
            }
            else
            {
                if (_actions.Contains(action))
                    _actions.Remove(action);
            }
        }
        public override string ToString()
        {
            return Value.ToString();
        }

        private void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised();

            for (int i = _actions.Count - 1; i >= 0; i--)
                _actions[i]();
        }

        public void Reset()
        {
            _listeners.Clear();
            _actions.Clear();
        }
    }

    //Can't get property drawer to work with generic arguments
    public abstract class BaseReference { } 
}