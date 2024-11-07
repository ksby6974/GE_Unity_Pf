using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using chataan.Scripts.Utils;

public class TipManager : Singleton<TipManager>
{
    [Header("References")]
    //[SerializeField] private TooltipController tooltipController;
    //[SerializeField] private CursorController cursorController;
    [SerializeField] private TooltipText tooltipTextPrefab;
    [SerializeField] private CanvasGroup canvasGroup;
    //[SerializeField] private SpecialKeywordData specialKeywordData;

    [Header("Settings")]
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private float showDelayTime = 0.5f;
    [SerializeField] private bool canChangeCursor;

    //public SpecialKeywordData SpecialKeywordData => specialKeywordData;
    private List<TooltipText> _tooltipTextList = new List<TooltipText>();
   // private TooltipController TooltipController => tooltipController;
    //private CursorController CursorController => cursorController;

    private int _currentShownTooltipCount;

    // 팁 표시
    private IEnumerator Show(float delay = 0)
    {
        var waitFrame = new WaitForEndOfFrame();
        float timer = 0;

        canvasGroup.alpha = 0;

        yield return new WaitForSeconds(delay);

        while (true)
        {
            timer += Time.deltaTime;

            var invValue = Mathf.InverseLerp(0, showDelayTime, timer);
            canvasGroup.alpha = fadeCurve.Evaluate(invValue);

            if (timer >= showDelayTime)
            {
                canvasGroup.alpha = 1;
                break;
            }
            yield return waitFrame;
        }
    }

    // 표시
    public void ShowTooltip(string contentText = "", string headerText = "", Transform tooltipTargetTransform = null, CursorType cursorType = CursorType.Default, Camera cam = null, float delayShow = 0)
    {
        StartCoroutine(Show(delayShow));
        _currentShownTooltipCount++;
        if (_tooltipTextList.Count < _currentShownTooltipCount)
        {
            var newTooltip = Instantiate(tooltipTextPrefab, TooltipController.transform);
            _tooltipTextList.Add(newTooltip);
        }

        _tooltipTextList[_currentShownTooltipCount - 1].gameObject.SetActive(true);
        _tooltipTextList[_currentShownTooltipCount - 1].SetText(contentText, headerText);

        TooltipController.SetFollowPos(tooltipTargetTransform, cam);

        if (canChangeCursor)
            CursorController.SetActiveCursor(cursorType);

    }

    // 팁 비표시
    public void HideTooltip()
    {
        StopAllCoroutines();
        _currentShownTooltipCount = 0;
        canvasGroup.alpha = 0;

        // 텍스트 비활성화
        foreach (var tooltipText in _tooltipTextList)
        {
            tooltipText.gameObject.SetActive(false);
        }

        // 
        if (canChangeCursor)
        {
            CursorController.SetActiveCursor(CursorType.Default);
        }
    }
}
