using chataan.Scripts.Data.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace chataan.Scripts.UI
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 상태이상 아이콘 기본 클래스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class StatusIconBase : MonoBehaviour
    {
        [SerializeField] private Image statusImage;
        [SerializeField] private TextMeshProUGUI statusValueText;

        public StatusIconData MyStatusIconData { get; private set; } = null;

        public Image StatusImage => statusImage;

        public TextMeshProUGUI StatusValueText => statusValueText;

        // ─────────────────────────
        // 상태이상 설정 설정
        // ─────────────────────────
        public void SetStatus(StatusIconData statusIconData)
        {
            MyStatusIconData = statusIconData;
            StatusImage.sprite = statusIconData.IconSprite;
        }

        // ─────────────────────────
        // 값 설정
        // ─────────────────────────
        public void SetStatusValue(int statusValue)
        {
            StatusValueText.text = statusValue.ToString();
        }
    }
}
