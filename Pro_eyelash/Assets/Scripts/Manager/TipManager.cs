using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using chataan.Scripts.Utils;
using chataan.Scripts.Enums;
using chataan.Scripts.Data.Keyword;
using chataan.Scripts.Tips;

namespace chataan.Scripts.Managers
{
    public class TipManager : Singleton<TipManager>
    {
        [Header("References")]
        [SerializeField] private SetTip setTip;
        [SerializeField] private SetTipCursor setTipCursor;
        [SerializeField] private TooltipText tooltipTextPrefab;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private KeywordData keywordData;

        [Header("Settings")]
        [SerializeField] private AnimationCurve fadeCurve;
        [SerializeField] private float showDelayTime = 0.5f;
        [SerializeField] private bool canChangeCursor;

        public KeywordData KeywordData => keywordData;
        private List<TooltipText> _tooltipTextList = new List<TooltipText>();
        private SetTip SetTip => setTip;
        private SetTipCursor SetTipCursor => setTipCursor;

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
        public void ShowTooltip(string contentText = "", string headerText = "", Transform tooltipTargetTransform = null, CursorType cursorType = CursorType.normal, Camera cam = null, float delayShow = 0)
        {
            StartCoroutine(Show(delayShow));
            _currentShownTooltipCount++;
            if (_tooltipTextList.Count < _currentShownTooltipCount)
            {
                var newTooltip = Instantiate(tooltipTextPrefab, SetTip.transform);
                _tooltipTextList.Add(newTooltip);
            }

            _tooltipTextList[_currentShownTooltipCount - 1].gameObject.SetActive(true);
            _tooltipTextList[_currentShownTooltipCount - 1].SetText(contentText, headerText);

            SetTip.SetFollowPos(tooltipTargetTransform, cam);

            if (canChangeCursor)
            {
                SetTipCursor.SetActiveCursor(cursorType);
            }
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
                SetTipCursor.SetActiveCursor(CursorType.normal);
            }
        }
    }
}