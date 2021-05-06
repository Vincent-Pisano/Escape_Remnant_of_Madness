using UnityEngine;
using UnityEngine.UI;

public class UISanityBar : Singleton<UISanityBar>
{
    public Image mask;
    private float _originalSize;

    void Start()
    {
        _originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _originalSize * value);
    }
}
