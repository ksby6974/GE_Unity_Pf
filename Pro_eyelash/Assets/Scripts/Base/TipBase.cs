using chataan.Scripts.Enums;
using chataan.Scripts.Interface;
using chataan.Scripts.Managers;
using UnityEngine;

namespace chataan.Scripts.Tips
{
    public class TipBase : MonoBehaviour, ITip
    {
        [SerializeField] protected string headerText = "";
        [TextArea][SerializeField] protected string contentText;
        [SerializeField] private Transform tooltipStaticTargetTransform;
        [SerializeField] private CursorType cursorType = CursorType.normal;
        [SerializeField] private float delayShowDuration = 0;

        protected virtual void ShowTooltipInfo()
        {
            ShowTooltipInfo(TipManager.Instance, contentText, headerText, tooltipStaticTargetTransform, cursorType, delayShow: delayShowDuration);
        }

        public void ShowTooltipInfo(TipManager tooltipManager, string content, string header = "", Transform tooltipStaticTransform = null, CursorType targetCursor = CursorType.normal, Camera cam = null, float delayShow = 0)
        {
            tooltipManager.ShowTooltip(content, header, tooltipStaticTransform, targetCursor, cam, delayShow);
        }

        public virtual void HideTooltipInfo(TipManager tooltipManager)
        {
            TipManager.Instance.HideTooltip();
        }
    }

}
