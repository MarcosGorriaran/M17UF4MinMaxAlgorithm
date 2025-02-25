using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmoothTransition : MonoBehaviour
{
    [SerializeField]
    float _duration;
    protected Coroutine SmoothCoroutine;
    
    protected float Duration
    {
        get
        {
            return _duration;
        }
    }

    public abstract void LoadingHasStoped();
    public abstract void LoadingHasStarted();
    protected abstract IEnumerator SmoothAlpha(float from, float to);
}
