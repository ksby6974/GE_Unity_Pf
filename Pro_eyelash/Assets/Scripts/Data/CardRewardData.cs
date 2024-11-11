using chataan.Scripts.Data.Card;
using chataan.Scripts.Data.Reward;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Data.Reward
{

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 카드 보상 데이터
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [CreateAssetMenu(fileName = "Card RewardData", menuName = "Chataan/Collection/Rewards/CardRW", order = 0)]
    public class CardRewardData : RewardDataBase
    {
        [SerializeField] private List<CardData> rewardCardList;
        public List<CardData> RewardCardList => rewardCardList;
    }
}