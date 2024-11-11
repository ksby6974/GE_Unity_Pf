using System.Collections.Generic;
using chataan.Scripts.Chara;
using chataan.Scripts.Data.Card;
using UnityEngine;

namespace chataan.Scripts.Data.Settings
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 시작 플레이 데이터
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [CreateAssetMenu(fileName = "PlayData", menuName = "Chataan/Settings/PlayData", order = 0)]
    public class PlayData : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private int drawCount = 4;
        [SerializeField] private int maxMana = 3;
        [SerializeField] private List<MyBase> initalAllyList;

        [Header("Deck")]
        [SerializeField] private DeckData initalDeck;
        [SerializeField] private int maxCardOnHand;

        [Header("Card")]
        [SerializeField] private List<CardData> allCardsList;
        [SerializeField] private CardBase cardPrefab;

        [Header("Custom")]
        [SerializeField] private string defaultName = "Nue";
        [SerializeField] private bool useStageSystem;

        [Header("Modifier")]
        [SerializeField] private bool isRandomHand = false;
        [SerializeField] private int randomCardCount;

        #region Encapsulation
        public int DrawCount => drawCount;
        public int MaxMana => maxMana;
        public bool IsRandomHand => isRandomHand;
        public List<MyBase> InitalAllyList => initalAllyList;
        public DeckData InitalDeck => initalDeck;
        public int RandomCardCount => randomCardCount;
        public int MaxCardOnHand => maxCardOnHand;
        public List<CardData> AllCardsList => allCardsList;
        public CardBase CardPrefab => cardPrefab;
        public string DefaultName => defaultName;
        public bool UseStageSystem => useStageSystem;
        #endregion
    }
}