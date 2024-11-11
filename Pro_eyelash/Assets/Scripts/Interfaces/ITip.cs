using chataan.Scripts.Enums;
using chataan.Scripts.Tips;
using UnityEngine;

namespace chataan.Scripts.Interface
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 도움말 표시창
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public interface ITip
    {
        void ShowTooltipInfo(TipManager tipManager, string content, string header = "", Transform tooltipStaticTransform = null, CursorType targetCursor = CursorType.normal, Camera cam = null, float delayShow = 0);

        void HideTooltipInfo(TipManager tipManager);
    }
}
