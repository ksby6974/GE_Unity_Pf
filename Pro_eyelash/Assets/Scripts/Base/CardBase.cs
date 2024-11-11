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
    // ����������������������������������������������������
    // ī�� �⺻ Ŭ����
    // 
    // ����������������������������������������������������
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

        // ��������������������������������������������������
        // Awake
        // ��������������������������������������������������
        protected virtual void Awake()
        {
            CachedTransform = transform;
            CachedWaitFrame = new WaitForEndOfFrame();
        }

        // ��������������������������������������������������
        // ī�� ����
        // ��������������������������������������������������
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

        // ��������������������������������������������������
        // ī�� ���
        // ��������������������������������������������������
        public virtual void Use(CharaBase self, CharaBase targetCharacter, List<EnemyBase> allEnemies, List<MyBase> allAllies)
        {
            // ��� �Ұ� ī��
            if (!IsPlayable)
            {
                return;
            }

            // ī�� ��� �ڷ�ƾ
            StartCoroutine(CardUseRoutine(self, targetCharacter, allEnemies, allAllies));
        }

        // ��������������������������������������������������
        // ī�� ��� �ڷ�ƾ
        // ��������������������������������������������������
        private IEnumerator CardUseRoutine(CharaBase self, CharaBase targetCharacter, List<EnemyBase> allEnemies, List<MyBase> allAllies)
        {
            // �ൿ�� �Ҹ� : �ൿ�� �䱸�� ��ŭ
            SpendAP(CardData.Cost);

            foreach (var playerAction in CardData.CardActionDataList)
            {
                // �̹� ������ ī�尡 �ִٸ� �� ī���� ��� �ִϸ��̼��� ����� ������ ��� ��
                yield return new WaitForSeconds(playerAction.ActionDelay);

                // ī�尡 ȿ���� �����ϴ� ��� ����
                var targetList = DetermineTargets(targetCharacter, allEnemies, allAllies, playerAction);

                foreach (var target in targetList)
                {
                    CardActionProcessor.GetAction(playerAction.CardEffectType).DoAction(new CardActionParameters(playerAction.ActionValue,target, self, CardData, this));
                }
            }

            PlayerManager.OnCardPlayed(this);
        }

        // ��������������������������������������������������
        // ī�尡 ȿ���� �����ϴ� ��� ����
        // ��������������������������������������������������
        private static List<CharaBase> DetermineTargets(CharaBase targetCharacter, List<EnemyBase> allEnemies, List<MyBase> allAllies, CardActionData playerAction)
        {
            List<CharaBase> targetList = new List<CharaBase>();
            switch (playerAction.TargetType)
            {
                // ��
                case TargetType.Enemy:
                    targetList.Add(targetCharacter);
                    break;

                // �÷��̾� �ڽ�
                case TargetType.Ally:
                    targetList.Add(targetCharacter);
                    break;

                // �� ����(����)
                case TargetType.AllEnemies:
                    foreach (var enemyBase in allEnemies)
                    {
                        targetList.Add(enemyBase);
                    }
                    break;

                // �Ʊ� ����
                case TargetType.AllAllies:
                    foreach (var allyBase in allAllies)
                    {
                        targetList.Add(allyBase);
                    }
                    break;

                // ������ ��
                case TargetType.RandomEnemy:
                    if (allEnemies.Count > 0)
                    {
                        targetList.Add(allEnemies.GetRandomItem());
                    }
                    break;

                // ������ �Ʊ�
                case TargetType.RandomAlly:
                    if (allAllies.Count > 0)
                    {
                        targetList.Add(allAllies.GetRandomItem());
                    }
                    break;

                // �� ��
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return targetList;
        }

        // ��������������������������������������������������
        // ī�� ������
        // ��������������������������������������������������
        public virtual void Discard()
        {
            // ī�� ��
            if (IsExhausted)
            {
                return;
            }

            // ī�� ��� �Ұ�
            if (!IsPlayable)
            {
                return;
            }
            PlayerManager.OnCardDiscarded(this);

            // ������
            StartCoroutine(DiscardRoutine());
        }

        // ī�� �ֹ߼�
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

            // �ֹ�
            IsExhausted = true;
            PlayerManager.OnCardExhausted(this);

            // �ı�
            StartCoroutine(ExhaustRoutine(destroy));
        }

        // �ൿ�� �Ҹ�
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

        // ȿ�� �ߵ� �� �߰� ������ �ʿ��� ī���
        public virtual void UpdateCardText()
        {
            CardData.UpdateDescription();
            nameTextField.text = CardData.CardName;
            descTextField.text = CardData.MyDescription;
            manaTextField.text = CardData.Cost.ToString();
        }

        // ����������������������������������������������������
        // �ڷ�ƾ
        // ����������������������������������������������������
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

        // ����������������������������������������������������
        // ī�� ���� ���콺 �÷��� ��
        // ����������������������������������������������������
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

        // ����������������������������������������������������
        // ����
        // ����������������������������������������������������
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
