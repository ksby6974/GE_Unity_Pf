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
    // ����������������������������������������������������
    // ī�� �⺻ ������
    // Card Ŭ������ ���ԵǴ� �ش� ī���� ���� ������
    // ����������������������������������������������������
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

        // ��������������������������������������������������
        // ī�� ���� ����
        // ���� ��ȭ�� ���� ���� ������ ������ ���ŵ� �� ���� 
        // ��������������������������������������������������
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

        // ��������������������������������������������������
        // ����Ƽ �����Ϳ�
        // ��������������������������������������������������
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

    // ����������������������������������������������������
    // ī�� ȿ�� ������
    // ����������������������������������������������������
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

        // ��������������������������������������������������
        // ����Ƽ ������ �׽�Ʈ��
        // ��������������������������������������������������
#if UNITY_EDITOR
        public void EditActionType(CardEffectType newType) => cardEffectType = newType;
        public void EditActionTarget(TargetType newTargetType) => targetType = newTargetType;
        public void EditActionValue(float newValue) => actionValue = newValue;
        public void EditActionDelay(float newValue) => actionDelay = newValue;

#endif


        #endregion
    }

    // ����������������������������������������������������
    // ī�� ���� ������
    // ����������������������������������������������������
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

        // ��������������������������������������������������
        // ���� ��������
        // ��������������������������������������������������
        public string GetDescription()
        {
            // ���� ���ϸ� ���̱� ���� ��Ʈ������ ���
            var str = new StringBuilder();

            str.Append(DescriptionText);

            if (EnableOverrideColor && !string.IsNullOrEmpty(str.ToString()))
                str.Replace(str.ToString(), GetColorEX.ColorString(str.ToString(), OverrideColor));

            return str.ToString();
        }

        // ��������������������������������������������������
        // ������ 
        // ��������������������������������������������������
        public string GetModifiedValue(CardData cardData)
        {
            // �� �̻� ī�� ȿ�� ������ ���� ����
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


            // ���� ������ ��
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

            // Ư�� ���� ����
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

