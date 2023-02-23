using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEvent : MonoBehaviour
{
    [System.Serializable]
    public class DEC
    {
        public float delay;
        public UnityEngine.Events.UnityEvent<string> onInvoke;
        public bool invoked = false;
    }
    public string eventName;
    public DEC[] events;
    bool _invoke = false;
    public float _time = 0f;
    float _maxTime;

    public void Run()
    {
        _time = 0f;
        foreach (DEC dec in events)
        {
            dec.invoked = false;
            if (_maxTime < dec.delay)
                _maxTime = dec.delay;
        }
        _invoke = true;
    }

    private void Update()
    {
        if (!_invoke) return;
        _time += Time.deltaTime;
        foreach(DEC dec in events)
        {
            if (dec.invoked) continue;
            if(dec.delay <= _time)
            {
                dec.onInvoke.Invoke(eventName);
                dec.invoked = true;
            }
        }
        if (_time >= _maxTime)
            _invoke = false;
    }
}
