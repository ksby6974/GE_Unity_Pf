using chataan.Scripts.Data.Card;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Data.Deck
{
    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
    // Ы溯檜橫 策 識婪 贗楚蝶
    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
    [CreateAssetMenu(fileName = "Deck Data", menuName = "Chataan/Collection/Deck", order = 1)]
    public class DeckData : ScriptableObject
    {
        [SerializeField] private string deckId;
        [SerializeField] private string deckName;

        [SerializeField] private List<CardData> cardList;
        public List<CardData> CardList => cardList;

        public string DeckId => deckId;

        public string DeckName => deckName;
    }
}
