using System.Collections.Generic;
using UnityEngine;
namespace Tools
{
    public sealed class SimpleEvent
    {
        private readonly Dictionary<string, mAction> mEvents = new Dictionary<string, mAction>();
        public SimpleEvent()
        {
            mEvents.Clear();
        }
        public void Append(MonoBehaviour _mono, mAction _callBack)
        {
            if (_callBack == null)
                return;
            if (mEvents.ContainsKey(_mono.name) && mEvents[_mono.name] == _callBack)
            {
                Debug.LogError($"{_mono.name}�ظ������¼�");
            }
            else
            {
                mEvents.Add(_mono.name, _callBack);
            }
        }
        public void Trigger()
        {
            foreach (var _item in mEvents)
            {
                _item.Value?.Invoke();
            }
        }
        public void Remove(MonoBehaviour _mono)
        {
            if (mEvents.ContainsKey(_mono.name))
            {
                mEvents.Remove(_mono.name);
            }
        }
        public void Clear()
        {
            mEvents.Clear();
        }
    }
}

