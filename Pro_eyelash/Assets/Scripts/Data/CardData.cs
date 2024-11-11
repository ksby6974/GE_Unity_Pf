using chataan.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using chataan.Scripts.Gets.ColorEx;
using chataan.Scripts.Managers;

namespace chataan.Scripts.Data.Card
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 카드 기본 데이터
    // Card 클래스에 포함되는 해당 카드의 고유 데이터
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [CreateAssetMenu(fileName = "CardData", menuName = "Chataan/Collection/CardData", order = 0)]
    public class CardData : ScriptableObject
    {
        [Header("Card Profile")]
        [SerializeField] private string id;
        [SerializeField] private string cardName;
        [SerializeField] private int cost;
        [SerializeField] private Sprite cardSprite;
        [SerializeField] private RarityType rarity;

        [Header("Action Settings")]
        [SerializeField] private bool usableWithoutTarget;
        [SerializeField] private bool exhaustAfterPlay;
        [SerializeField] private List<CardActionData> cardActionDataList;

        [Header("Description")]
        [SerializeField] private List<CardDescriptionData> cardDescriptionDataList;
        [SerializeField] private List<KeywordType> specialKeywordsList;

        [Header("Fx")]
        [SerializeField] private AudioActionType audioType;

        public string Id => id;
        public bool UsableWithoutTarget => usableWithoutTarget;
        public int Cost => cost;
        public string CardName => cardName;
        public Sprite CardSprite => cardSprite;
        public List<CardActionData> CardActionDataList => cardActionDataList;
        public List<CardDescriptionData> CardDescriptionDataList => cardDescriptionDataList;
        public List<KeywordType> KeywordsList => specialKeywordsList;
        public AudioActionType AudioType => audioType;
        public string MyDescription { get; set; }
        public RarityType Rarity => rarity;

        public bool ExhaustAfterPlay => exhaustAfterPlay;

        // ─────────────────────────
        // 카드 설명 갱신
        // 피해 변화와 같은 여러 이유로 설명이 갱신될 수 있음 
        // ─────────────────────────
        public void UpdateDescription()
        {

            var str = new StringBuilder();

            foreach (var descriptionData in cardDescriptionDataList)
            {
                str.Append(descriptionData.UseModifier
                    ? descriptionData.GetModifiedValue(this)
                    : descriptionData.GetDescription());
            }

            MyDescription = str.ToString();
        }

        // ─────────────────────────
        // 유니티 에디터용
        // ─────────────────────────
        #region Editor Methods
#if UNITY_EDITOR
        public void EditCardName(string newName) => cardName = newName;
        public void EditId(string newId) => id = newId;
        public void EditManaCost(int newCost) => cost = newCost;
        public void EditRarity(RarityType targetRarity) => rarity = targetRarity;
        public void EditCardSprite(Sprite newSprite) => cardSprite = newSprite;
        public void EditUsableWithoutTarget(bool newStatus) => usableWithoutTarget = newStatus;
        public void EditExhaustAfterPlay(bool newStatus) => exhaustAfterPlay = newStatus;
        public void EditCardActionDataList(List<CardActionData> newCardActionDataList) =>
            cardActionDataList = newCardActionDataList;
        public void EditCardDescriptionDataList(List<CardDescriptionData> newCardDescriptionDataList) =>
            cardDescriptionDataList = newCardDescriptionDataList;
        public void EditSpecialKeywordsList(List<KeywordType> newSpecialKeywordsList) =>
            specialKeywordsList = newSpecialKeywordsList;
        public void EditAudioType(AudioActionType newAudioActionType) => audioType = newAudioActionType;
#endif
        #endregion
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 카드 효과 데이터
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [Serializable]
    public class CardActionData
    {
        [SerializeField] private CardEffectType cardEffectType;
        [SerializeField] private TargetType targetType;
        [SerializeField] private float actionValue;
        [SerializeField] private float actionDelay;

        public TargetType TargetType => targetType;
        public CardEffectType CardEffectType => cardEffectType;
        public float ActionValue => actionValue;
        public float ActionDelay => actionDelay;

        #region Editor

        // ─────────────────────────
        // 유니티 에디터 테스트용
        // ─────────────────────────
#if UNITY_EDITOR
        public void EditActionType(CardEffectType newType) => cardEffectType = newType;
        public void EditActionTarget(TargetType newTargetType) => targetType = newTargetType;
        public void EditActionValue(float newValue) => actionValue = newValue;
        public void EditActionDelay(float newValue) => actionDelay = newValue;

#endif


        #endregion
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 카드 설명 데이터
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [Serializable]
    public class CardDescriptionData
    {
        [Header("Text")]
        [SerializeField] private string descriptionText;
        [SerializeField] private bool enableOverrideColor;
        [SerializeField] private Color overrideColor = Color.black;

        [Header("Modifer")]
        [SerializeField] private bool useModifier;
        [SerializeField] private int modifiedActionValueIndex;
        [SerializeField] private StatusType modiferStats;
        [SerializeField] private bool usePrefixOnModifiedValue;
        [SerializeField] private string modifiedValuePrefix = "*";
        [SerializeField] private bool overrideColorOnValueScaled;

        public string DescriptionText => descriptionText;
        public bool EnableOverrideColor => enableOverrideColor;
        public Color OverrideColor => overrideColor;
        public bool UseModifier => useModifier;
        public int ModifiedActionValueIndex => modifiedActionValueIndex;
        public StatusType ModiferStats => modiferStats;
        public bool UsePrefixOnModifiedValue => usePrefixOnModifiedValue;
        public string ModifiedValuePrefix => modifiedValuePrefix;
        public bool OverrideColorOnValueScaled => overrideColorOnValueScaled;

        private BattleManager BattleManager => BattleManager.Instance;

        // ─────────────────────────
        // 설명 가져오기
        // ─────────────────────────
        public string GetDescription()
        {
            // 성능 저하를 줄이기 위해 스트링빌더 사용
            var str = new StringBuilder();

            str.Append(DescriptionText);

            if (EnableOverrideColor && !string.IsNullOrEmpty(str.ToString()))
                str.Replace(str.ToString(), GetColorEX.ColorString(str.ToString(), OverrideColor));

            return str.ToString();
        }

        // ─────────────────────────
        // 수정값 
        // ─────────────────────────
        public string GetModifiedValue(CardData cardData)
        {
            // 더 이상 카드 효과 적용할 것이 없음
            if (cardData.CardActionDataList.Count <= 0)
            {
                return "";
            }

            if (ModifiedActionValueIndex >= cardData.CardActionDataList.Count)
            {
                modifiedActionValueIndex = cardData.CardActionDataList.Count - 1;
            }    

            if (ModifiedActionValueIndex < 0)
            {
                modifiedActionValueIndex = 0;
            }

            var str = new StringBuilder();
            var value = cardData.CardActionDataList[ModifiedActionValueIndex].ActionValue;
            var modifer = 0;


            // 전투 페이즈 시
            if (BattleManager)
            {
                var player = BattleManager.CurrentMainAlly;

                if (player)
                {
                    modifer = player.CharacterStats.StatusDict[ModiferStats].StatusValue;
                    value += modifer;

                    if (modifer != 0)
                    {
                        if (usePrefixOnModifiedValue)
                            str.Append(modifiedValuePrefix);
                    }
                }
            }

            str.Append(value);

            // 특정 구문 강조
            if (EnableOverrideColor)
            {
                if (OverrideColorOnValueScaled)
                {
                    if (modifer != 0)
                        str.Replace(str.ToString(), GetColorEX.ColorString(str.ToString(), OverrideColor));
                }
                else
                {
                    str.Replace(str.ToString(), GetColorEX.ColorString(str.ToString(), OverrideColor));
                }

            }

            return str.ToString();
        }
    }
}

