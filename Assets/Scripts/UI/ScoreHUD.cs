using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreHUD : MonoBehaviour
{
    public Color colorOnAdd;
    public Color colorOnSubstract;
    public Ease easeType;

    private TextMeshProUGUI ui;
    private float originalFontSize;
    private float lastScore;
    private Vector3 originalLocalPosition;

    private void Start()
    {
        ui = GetComponent<TextMeshProUGUI>();
        lastScore = 0;
        originalFontSize = ui.fontSize;
        originalLocalPosition = ui.rectTransform.localPosition;
    }

    public void Play(int score)
    {
        Color toUse = score > lastScore ? colorOnAdd : colorOnSubstract;
        lastScore = score;
        ui.SetText(score + "");

        ui.transform.DOPunchPosition(new Vector3(-15, -15), 0.34f, 5, 0.7f).OnComplete(() => ui.transform.localPosition = originalLocalPosition);
        DOTween.To(x => ui.fontSize = x, ui.fontSize, ui.fontSize + 12, 0.3f).SetEase(easeType).OnComplete(() => ui.fontSize = originalFontSize);
        DOTween.To(() => ui.color, x => ui.color = x, toUse, 0.3f).SetEase(easeType).OnComplete(() => ui.color = Color.white);
    }
}