using chataan.Scripts.Data.Card;
using chataan.Scripts.Data.Reward;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Data.Reward
{

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 금액 보상 데이터
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [CreateAssetMenu(fileName = "Gold RewardData", menuName = "Chataan/Collection/Rewards/GoldRW", order = 0)]
    public class GoldRewardData : RewardDataBase
    {
        [SerializeField] private int minGold;
        [SerializeField] private int maxGold;
        public int MinGold => minGold;
        public int MaxGold => maxGold;
    }
}