using chataan.Scripts.Interface;
using chataan.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace chataan.Scripts.Tips
{
    public class SetTipView : TipBase, ITip2D
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowTooltipInfo();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideTooltipInfo(TipManager.Instance);
        }
    }
}