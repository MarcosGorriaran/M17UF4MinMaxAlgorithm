using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIcon : SmoothTransition
{
    [SerializeField, Range(0,1)]
    float _minTransparency;
    [SerializeField, Range(0,1)]
    float _maxTransparency;
    [SerializeField]
    Image _loadingImage;
    public override void LoadingHasStoped()
    {
        if(SmoothCoroutine != null)
        {
            StopCoroutine(SmoothCoroutine);
        }
        SmoothCoroutine = StartCoroutine(SmoothAlpha(_maxTransparency, _minTransparency));
    }
    public override void LoadingHasStarted()
    {
        if (SmoothCoroutine != null)
        {
            StopCoroutine(SmoothCoroutine);
        }
        SmoothCoroutine = StartCoroutine(SmoothAlpha(_minTransparency, _maxTransparency));
    }
    protected override IEnumerator SmoothAlpha(float from, float to)
    {
        float actualTime = 0;
        while (actualTime <= Duration)
        {
            actualTime += Time.deltaTime;
            float alphaValue = Mathf.Lerp(from, to, actualTime / Duration);
            _loadingImage.color = new Color(_loadingImage.color.r, _loadingImage.color.g, _loadingImage.color.b, alphaValue);
            yield return null;
        }
    }
}
