using chataan.Scripts.Enums;
using chataan.Scripts.Managers;
using chataan.Scripts.Interface;
using chataan.Scripts.Data.Containers;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using chataan.Scripts.Tips;
using chataan.Scripts.Data.Keyword;
using chataan.Scripts.UI;
using System.Linq;

namespace chataan.Scripts.Chara
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 캐릭터에 관한 정보 표시창
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [RequireComponent(typeof(Canvas))]
    public class CharaCanvas : MonoBehaviour, ICanvas
    {
        [Header("References")]
        [SerializeField] protected Transform statusIconRoot;
        [SerializeField] protected Transform highlightRoot;
        [SerializeField] protected Transform descriptionRoot;
        [SerializeField] protected StatusIconsData statusIconsData;
        [SerializeField] protected TextMeshProUGUI currentHealthText;

        protected Dictionary<StatusType, StatusIconBase> StatusDict = new Dictionary<StatusType, StatusIconBase>();
        protected Canvas TargetCanvas;

        protected FxManager FxManager => FxManager.Instance;
        protected SoundManager SoundManager => SoundManager.Instance;
        protected CoreManager CoreManager => CoreManager.Instance;
        protected BattleManager BattleManager => BattleManager.Instance;
        protected PlayerManager PlayerManager => PlayerManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        // ─────────────────────────
        // 초기화
        // ─────────────────────────
        public void InitCanvas()
        {
            highlightRoot.gameObject.SetActive(false);

            for (int i = 0; i < Enum.GetNames(typeof(StatusType)).Length; i++)
            {
                // 상태이상 딕셔너리 초기화
                StatusDict.Add((StatusType)i, null);
            }

            TargetCanvas = GetComponent<Canvas>();

            if (TargetCanvas)
            {
                // 카메라 이동
                TargetCanvas.worldCamera = Camera.main;
            }
        }

        // ─────────────────────────
        // 상태 이상 적용
        // ─────────────────────────
        public void ApplyStatus(StatusType targetStatus, int value)
        {
            if (StatusDict[targetStatus] == null)
            {
                var targetData = statusIconsData.StatusIconList.FirstOrDefault(x => x.IconStatus == targetStatus);

                if (targetData == null) return;

                var clone = Instantiate(statusIconsData.StatusIconBasePrefab, statusIconRoot);
                clone.SetStatus(targetData);
                StatusDict[targetStatus] = clone;
            }

            StatusDict[targetStatus].SetStatusValue(value);
        }

        // ─────────────────────────
        // 상태 이상 제거
        // ─────────────────────────
        public void ClearStatus(StatusType targetStatus)
        {
            if (StatusDict[targetStatus])
            {
                Destroy(StatusDict[targetStatus].gameObject);
            }

            StatusDict[targetStatus] = null;
        }

        // ─────────────────────────
        // 상태 이상 설명문
        // ─────────────────────────
        public void UpdateStatusText(StatusType targetStatus, int value)
        {
            if (StatusDict[targetStatus] == null) return;

            StatusDict[targetStatus].StatusValueText.text = $"{value}";
        }

        // ─────────────────────────
        // 회복 표시
        // ─────────────────────────
        public void UpdateHealthText(int currentHealth, int maxHealth) => currentHealthText.text = $"{currentHealth}/{maxHealth}";

        // ─────────────────────────
        // 강조
        // ─────────────────────────
        public void SetHighlight(bool open) => highlightRoot.gameObject.SetActive(open);

        // ─────────────────────────
        // 마우스 올리면 상태 이상 설명 표시
        // ─────────────────────────
        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowTooltipInfo();
        }

        // ─────────────────────────
        // 마우스 내리면 상태 이상 설명 비표시
        // ─────────────────────────
        public void OnPointerExit(PointerEventData eventData)
        {
            HideTooltipInfo(TipManager.Instance);
        }

        // ─────────────────────────
        // 상태 이상 설명 표시
        // ─────────────────────────
        public void ShowTooltipInfo()
        {
            var tipManager = TipManager.Instance;
            var specialKeywords = new List<KeywordType>();

            foreach (var statusIcon in StatusDict)
            {
                if (statusIcon.Value == null) continue;

                var statusData = statusIcon.Value.MyStatusIconData;
                foreach (var statusDataSpecialKeyword in statusData.KeywordType)
                {
                    if (specialKeywords.Contains(statusDataSpecialKeyword)) continue;
                    specialKeywords.Add(statusDataSpecialKeyword);
                }
            }

            foreach (var specialKeyword in specialKeywords)
            {
                var specialKeywordData = tipManager.KeywordData.KeywordBaseList.Find(x => x.Keyword == specialKeyword);
                if (specialKeywordData != null)
                {
                    ShowTooltipInfo(tipManager, specialKeywordData.GetContent(), specialKeywordData.GetHeader(), descriptionRoot);
                }
            }

        }

        // ─────────────────────────
        // 상태 이상 설명 표시
        // ─────────────────────────
        public void ShowTooltipInfo(TipManager tipManager, string content, string header = "", Transform tooltipStaticTransform = null, CursorType targetCursor = CursorType.normal, Camera cam = null, float delayShow = 0)
        {
            tipManager.ShowTooltip(content, header, tooltipStaticTransform, targetCursor, cam, delayShow);
        }

        // ─────────────────────────
        // 상태 이상 설명 비표시
        // ─────────────────────────
        public void HideTooltipInfo(TipManager tipManager)
        {
            tipManager.HideTooltip();
        }
    }

}