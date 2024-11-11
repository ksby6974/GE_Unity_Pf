using chataan.Scripts.Data.Card;
using chataan.Scripts.Gets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Data.Reward
{
    // ����������������������������������������������������
    // ���� ������ Ŭ����
    // ����������������������������������������������������
    [CreateAssetMenu(fileName = "Reward Data", menuName = "chataan/Containers/RewardData", order = 4)]
    public class RewardData : ScriptableObject
    {
        [SerializeField] private List<CardRewardData> cardRewardDataList;
        [SerializeField] private List<GoldRewardData> goldRewardDataList;
        public List<CardRewardData> CardRewardDataList => cardRewardDataList;
        public List<GoldRewardData> GoldRewardDataList => goldRewardDataList;

        // ��������������������������������������������������
        // ī�� ���� : �������� ������ ī�带 ȹ��
        // ��������������������������������������������������
        public List<CardData> GetRandomCardRewardList(out CardRewardData rewardData)
        {
            rewardData = CardRewardDataList.GetRandomItem();

            List<CardData> cardList = new List<CardData>();

            foreach (var cardData in rewardData.RewardCardList)
            {
                // ���� �߰�
                cardList.Add(cardData);
            }

            return cardList;
        }

        // ��������������������������������������������������
        // �� ���� : �������� ������ ��ġ�� �ݾ��� ȹ��
        // ��������������������������������������������������
        public int GetRandomGoldReward(out GoldRewardData rewardData)
        {
            rewardData = GoldRewardDataList.GetRandomItem();
            var value = Random.Range(rewardData.MinGold, rewardData.MaxGold);

            return value;
        }

    }

}