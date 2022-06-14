using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;


public static class DotweenExtension
{
    public static TweenerCore<int, int, NoOptions> DOTextValueChange(this TMP_Text target, int value, float duration)
    {
        return int.TryParse(target.text, out var start)
            ? DOTween.To(() => start, x =>
            {
                target.text = x.ToString();
                start = x;
            }, value, duration)
            : null;
    }
}