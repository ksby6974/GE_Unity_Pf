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
    // ����������������������������������������������������
    // ĳ���Ϳ� ���� ���� ǥ��â
    // ����������������������������������������������������
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

        // ��������������������������������������������������
        // �ʱ�ȭ
        // ��������������������������������������������������
        public void InitCanvas()
        {
            highlightRoot.gameObject.SetActive(false);

            for (int i = 0; i < Enum.GetNames(typeof(StatusType)).Length; i++)
            {
                // �����̻� ��ųʸ� �ʱ�ȭ
                StatusDict.Add((StatusType)i, null);
            }

            TargetCanvas = GetComponent<Canvas>();

            if (TargetCanvas)
            {
                // ī�޶� �̵�
                TargetCanvas.worldCamera = Camera.main;
            }
        }

        // ��������������������������������������������������
        // ���� �̻� ����
        // ��������������������������������������������������
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

        // ��������������������������������������������������
        // ���� �̻� ����
        // ��������������������������������������������������
        public void ClearStatus(StatusType targetStatus)
        {
            if (StatusDict[targetStatus])
            {
                Destroy(StatusDict[targetStatus].gameObject);
            }

            StatusDict[targetStatus] = null;
        }

        // ��������������������������������������������������
        // ���� �̻� ����
        // ��������������������������������������������������
        public void UpdateStatusText(StatusType targetStatus, int value)
        {
            if (StatusDict[targetStatus] == null) return;

            StatusDict[targetStatus].StatusValueText.text = $"{value}";
        }

        // ��������������������������������������������������
        // ȸ�� ǥ��
        // ��������������������������������������������������
        public void UpdateHealthText(int currentHealth, int maxHealth) => currentHealthText.text = $"{currentHealth}/{maxHealth}";

        // ��������������������������������������������������
        // ����
        // ��������������������������������������������������
        public void SetHighlight(bool open) => highlightRoot.gameObject.SetActive(open);

        // ��������������������������������������������������
        // ���콺 �ø��� ���� �̻� ���� ǥ��
        // ��������������������������������������������������
        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowTooltipInfo();
        }

        // ��������������������������������������������������
        // ���콺 ������ ���� �̻� ���� ��ǥ��
        // ��������������������������������������������������
        public void OnPointerExit(PointerEventData eventData)
        {
            HideTooltipInfo(TipManager.Instance);
        }

        // ��������������������������������������������������
        // ���� �̻� ���� ǥ��
        // ��������������������������������������������������
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

        // ��������������������������������������������������
        // ���� �̻� ���� ǥ��
        // ��������������������������������������������������
        public void ShowTooltipInfo(TipManager tipManager, string content, string header = "", Transform tooltipStaticTransform = null, CursorType targetCursor = CursorType.normal, Camera cam = null, float delayShow = 0)
        {
            tipManager.ShowTooltip(content, header, tooltipStaticTransform, targetCursor, cam, delayShow);
        }

        // ��������������������������������������������������
        // ���� �̻� ���� ��ǥ��
        // ��������������������������������������������������
        public void HideTooltipInfo(TipManager tipManager)
        {
            tipManager.HideTooltip();
        }
    }

}