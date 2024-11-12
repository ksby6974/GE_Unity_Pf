using chataan.Scripts.Card;
using chataan.Scripts.Data.Card;
using chataan.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace chataan.Scripts.UI
{

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 보상 선택 클래스 : 카드
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class SetChoiceCard : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        [SerializeField] private float showScaleRate = 1.15f;
        private CardBase _cardBase;
        private Vector3 _initalScale;
        public Action OnCardChose;
        public CoreManager CoreManager => CoreManager.Instance;
        public UIManager UIManager => UIManager.Instance;

        // ─────────────────────────
        // 보상 생성
        // ─────────────────────────
        public void BuildReward(CardData cardData)
        {
            _cardBase = GetComponent<CardBase>();
            _initalScale = transform.localScale;
            _cardBase.SetCard(cardData);
            _cardBase.UpdateCardText();
        }

        // ─────────────────────────
        // 보상 성택
        // ─────────────────────────
        private void OnChoice()
        {
            // 추가
            if (CoreManager != null)
            {
                CoreManager.SavePlayData.CurrentCardsList.Add(_cardBase.CardData);
            }

            // 비활성화
            if (UIManager != null)
            {
                UIManager.RewardCanvas.ChoicePanel.DisablePanel();
            }
            OnCardChose?.Invoke();
        }

        // ─────────────────────────
        // 
        // ─────────────────────────
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = _initalScale * showScaleRate;
        }

        // ─────────────────────────
        // 
        // ─────────────────────────
        public void OnPointerDown(PointerEventData eventData)
        {

        }
        // ─────────────────────────
        // 
        // ─────────────────────────
        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = _initalScale;
        }

        // ─────────────────────────
        // 
        // ─────────────────────────
        public void OnPointerUp(PointerEventData eventData)
        {
            OnChoice();
        }
    }
}
