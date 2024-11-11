using chataan.Scripts.Data.Card;
using chataan.Scripts.Gets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Data.Reward
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 보상 데이터 클래스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [CreateAssetMenu(fileName = "Reward Data", menuName = "chataan/Containers/RewardData", order = 4)]
    public class RewardData : ScriptableObject
    {
        [SerializeField] private List<CardRewardData> cardRewardDataList;
        [SerializeField] private List<GoldRewardData> goldRewardDataList;
        public List<CardRewardData> CardRewardDataList => cardRewardDataList;
        public List<GoldRewardData> GoldRewardDataList => goldRewardDataList;

        // ─────────────────────────
        // 카드 보상 : 보상으로 무작위 카드를 획득
        // ─────────────────────────
        public List<CardData> GetRandomCardRewardList(out CardRewardData rewardData)
        {
            rewardData = CardRewardDataList.GetRandomItem();

            List<CardData> cardList = new List<CardData>();

            foreach (var cardData in rewardData.RewardCardList)
            {
                // 덱에 추가
                cardList.Add(cardData);
            }

            return cardList;
        }

        // ─────────────────────────
        // 돈 보상 : 보상으로 무작위 수치의 금액을 획득
        // ─────────────────────────
        public int GetRandomGoldReward(out GoldRewardData rewardData)
        {
            rewardData = GoldRewardDataList.GetRandomItem();
            var value = Random.Range(rewardData.MinGold, rewardData.MaxGold);

            return value;
        }

    }

}