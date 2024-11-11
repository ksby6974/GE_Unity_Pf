using chataan.Scripts.Chara;
using chataan.Scripts.Data.Card;
using chataan.Scripts.Enums;
using chataan.Scripts.Gets;
using chataan.Scripts.Interface;
using chataan.Scripts.Managers;
using chataan.Scripts.Tips;
using chataan.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace chataan.Scripts.Card
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 카드 기본 클래스
    // 
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class CardBase : MonoBehaviour, ITip, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Base References")]
        [SerializeField] protected Transform descriptionRoot;
        [SerializeField] protected Image cardImage;
        [SerializeField] protected Image passiveImage;
        [SerializeField] protected TextMeshProUGUI nameTextField;
        [SerializeField] protected TextMeshProUGUI descTextField;
        [SerializeField] protected TextMeshProUGUI manaTextField;
        [SerializeField] protected List<RarityBase> rarityBaseList;

        public CardData CardData { get; private set; }
        public bool IsInactive { get; protected set; }
        protected Transform CachedTransform { get; set; }
        protected WaitForEndOfFrame CachedWaitFrame { get; set; }
        public bool IsPlayable { get; protected set; } = true;

        public List<RarityBase> RarityBaseList => rarityBaseList;
        protected FxManager FxManager => FxManager.Instance;
        protected SoundManager SoundManager => SoundManager.Instance;
        protected CoreManager CoreManager => CoreManager.Instance;
        protected BattleManager BattleManager => BattleManager.Instance;
        protected PlayerManager PlayerManager => PlayerManager.Instance;

        public bool IsExhausted { get; private set; }

        // ─────────────────────────
        // Awake
        // ─────────────────────────
        protected virtual void Awake()
        {
            CachedTransform = transform;
            CachedWaitFrame = new WaitForEndOfFrame();
        }

        // ─────────────────────────
        // 카드 설정
        // ─────────────────────────
        public virtual void SetCard(CardData targetProfile, bool isPlayable = true)
        {
            CardData = targetProfile;
            IsPlayable = isPlayable;
            nameTextField.text = CardData.CardName;
            descTextField.text = CardData.MyDescription;
            manaTextField.text = CardData.Cost.ToString();
            cardImage.sprite = CardData.CardSprite;
            foreach (var rarityRoot in RarityBaseList)
            {
                rarityRoot.gameObject.SetActive(rarityRoot.Rarity == CardData.Rarity);
            }
        }

        // ─────────────────────────
        // 카드 사용
        // ─────────────────────────
        public virtual void Use(CharaBase self, CharaBase targetCharacter, List<EnemyBase> allEnemies, List<MyBase> allAllies)
        {
            // 사용 불가 카드
            if (!IsPlayable)
            {
                return;
            }

            // 카드 사용 코루틴
            StartCoroutine(CardUseRoutine(self, targetCharacter, allEnemies, allAllies));
        }

        // ─────────────────────────
        // 카드 사용 코루틴
        // ─────────────────────────
        private IEnumerator CardUseRoutine(CharaBase self, CharaBase targetCharacter, List<EnemyBase> allEnemies, List<MyBase> allAllies)
        {
            // 행동력 소모 : 행동력 요구량 마큼
            SpendAP(CardData.Cost);

            foreach (var playerAction in CardData.CardActionDataList)
            {
                // 이미 선언한 카드가 있다면 그 카드의 사용 애니메이션이 사라질 때까지 대기 후
                yield return new WaitForSeconds(playerAction.ActionDelay);

                // 카드가 효과를 발휘하는 대상 지정
                var targetList = DetermineTargets(targetCharacter, allEnemies, allAllies, playerAction);

                foreach (var target in targetList)
                {
                    CardActionProcessor.GetAction(playerAction.CardEffectType).DoAction(new CardActionParameters(playerAction.ActionValue,target, self, CardData, this));
                }
            }

            PlayerManager.OnCardPlayed(this);
        }

        // ─────────────────────────
        // 카드가 효과를 발휘하는 대상 지정
        // ─────────────────────────
        private static List<CharaBase> DetermineTargets(CharaBase targetCharacter, List<EnemyBase> allEnemies, List<MyBase> allAllies, CardActionData playerAction)
        {
            List<CharaBase> targetList = new List<CharaBase>();
            switch (playerAction.TargetType)
            {
                // 적
                case TargetType.Enemy:
                    targetList.Add(targetCharacter);
                    break;

                // 플레이어 자신
                case TargetType.Ally:
                    targetList.Add(targetCharacter);
                    break;

                // 적 전부(광역)
                case TargetType.AllEnemies:
                    foreach (var enemyBase in allEnemies)
                    {
                        targetList.Add(enemyBase);
                    }
                    break;

                // 아군 전부
                case TargetType.AllAllies:
                    foreach (var allyBase in allAllies)
                    {
                        targetList.Add(allyBase);
                    }
                    break;

                // 무작위 적
                case TargetType.RandomEnemy:
                    if (allEnemies.Count > 0)
                    {
                        targetList.Add(allEnemies.GetRandomItem());
                    }
                    break;

                // 무작위 아군
                case TargetType.RandomAlly:
                    if (allAllies.Count > 0)
                    {
                        targetList.Add(allAllies.GetRandomItem());
                    }
                    break;

                // 그 외
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return targetList;
        }

        // ─────────────────────────
        // 카드 버리기
        // ─────────────────────────
        public virtual void Discard()
        {
            // 카드 고갈
            if (IsExhausted)
            {
                return;
            }

            // 카드 사용 불가
            if (!IsPlayable)
            {
                return;
            }
            PlayerManager.OnCardDiscarded(this);

            // 버리기
            StartCoroutine(DiscardRoutine());
        }

        // 카드 휘발성
        public virtual void Exhaust(bool destroy = true)
        {
            if (IsExhausted)
            {
                return;
            }

            if (!IsPlayable)
            {
                return;
            }

            // 휘발
            IsExhausted = true;
            PlayerManager.OnCardExhausted(this);

            // 파괴
            StartCoroutine(ExhaustRoutine(destroy));
        }

        // 행동력 소모
        protected virtual void SpendAP(int value)
        {
            if (!IsPlayable)
            {
                return;
            }
            CoreManager.SavePlayData.CurrentMana -= value;
        }

        // 
        public virtual void SetInactiveMaterialState(bool isInactive)
        {
            if (!IsPlayable) return;
            if (isInactive == this.IsInactive) return;

            IsInactive = isInactive;
            passiveImage.gameObject.SetActive(isInactive);
        }

        // 효과 발동 시 추가 설명문이 필요한 카드용
        public virtual void UpdateCardText()
        {
            CardData.UpdateDescription();
            nameTextField.text = CardData.CardName;
            descTextField.text = CardData.MyDescription;
            manaTextField.text = CardData.Cost.ToString();
        }

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━
        // 코루틴
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━
        #region Routines
        protected virtual IEnumerator DiscardRoutine(bool destroy = true)
        {
            var timer = 0f;
            transform.SetParent(PlayerManager.HandManager.discardTransform);

            var startPos = CachedTransform.localPosition;
            var endPos = Vector3.zero;

            var startScale = CachedTransform.localScale;
            var endScale = Vector3.zero;

            var startRot = CachedTransform.localRotation;
            var endRot = Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360);

            while (true)
            {
                timer += Time.deltaTime * 5;

                CachedTransform.localPosition = Vector3.Lerp(startPos, endPos, timer);
                CachedTransform.localRotation = Quaternion.Lerp(startRot, endRot, timer);
                CachedTransform.localScale = Vector3.Lerp(startScale, endScale, timer);

                if (timer >= 1f) break;

                yield return CachedWaitFrame;
            }

            if (destroy)
                Destroy(gameObject);

        }

        protected virtual IEnumerator ExhaustRoutine(bool destroy = true)
        {
            var timer = 0f;
            transform.SetParent(PlayerManager.HandManager.exhaustTransform);

            var startPos = CachedTransform.localPosition;
            var endPos = Vector3.zero;

            var startScale = CachedTransform.localScale;
            var endScale = Vector3.zero;

            var startRot = CachedTransform.localRotation;
            var endRot = Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360);

            while (true)
            {
                timer += Time.deltaTime * 5;

                CachedTransform.localPosition = Vector3.Lerp(startPos, endPos, timer);
                CachedTransform.localRotation = Quaternion.Lerp(startRot, endRot, timer);
                CachedTransform.localScale = Vector3.Lerp(startScale, endScale, timer);

                if (timer >= 1f) break;

                yield return CachedWaitFrame;
            }

            if (destroy)
                Destroy(gameObject);

        }

        #endregion

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━
        // 카드 위로 마우스 올렸을 때
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━
        #region Pointer Events
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            ShowTooltipInfo();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            HideTooltipInfo(TipManager.Instance);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            HideTooltipInfo(TipManager.Instance);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            ShowTooltipInfo();
        }
        #endregion

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━
        // 팁용
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━
        #region Tooltip
        protected virtual void ShowTooltipInfo()
        {
            if (!descriptionRoot) return;
            if (CardData.KeywordsList.Count <= 0) return;

            var tooltipManager = TipManager.Instance;
            foreach (var cardDataSpecialKeyword in CardData.KeywordsList)
            {
                var specialKeyword = tooltipManager.KeywordData.KeywordBaseList.Find(x => x.Keyword == cardDataSpecialKeyword);
                if (specialKeyword != null)
                    ShowTooltipInfo(tooltipManager, specialKeyword.GetContent(), specialKeyword.GetHeader(), descriptionRoot, CursorType.normal, PlayerManager ? PlayerManager.HandManager.cam : Camera.main);
            }
        }
        public virtual void ShowTooltipInfo(TipManager tooltipManager, string content, string header = "", Transform tooltipStaticTransform = null, CursorType targetCursor = CursorType.normal, Camera cam = null, float delayShow = 0)
        {
            tooltipManager.ShowTooltip(content, header, tooltipStaticTransform, targetCursor, cam, delayShow);
        }

        public virtual void HideTooltipInfo(TipManager tooltipManager)
        {
            tooltipManager.HideTooltip();
        }
        #endregion
    }
}
