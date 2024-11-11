using chataan.Scripts.Data.Card;
using chataan.Scripts.Data.Containers;
using chataan.Scripts.Enums;
using chataan.Scripts.Gets;
using chataan.Scripts.UI.Reward;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.UI
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 결과 표시창 클래스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━

    public class RewardCanvas : CanvasBase
    {
        [Header("References")]
        [SerializeField] private RewardData rewardContainerData;
        [SerializeField] private Transform rewardRoot;
        [SerializeField] private SetReward rewardContainerPrefab;
        [SerializeField] private Transform rewardPanelRoot;

        [Header("Reward")]
        [SerializeField] private Transform choice2DCardSpawnRoot;
        [SerializeField] private SetChoiceCard choiceCardUIPrefab;
        [SerializeField] private ChoicePanel choicePanel;

        private readonly List<SetReward> _currentRewardsList = new List<SetReward>();
        private readonly List<SetChoiceCard> _spawnedChoiceList = new List<SetChoiceCard>();
        private readonly List<CardData> _cardRewardList = new List<CardData>();
        public ChoicePanel ChoicePanel => choicePanel;

        // ─────────────────────────
        // 딜레이 후 활성화
        // ─────────────────────────
        public void PrepareCanvas()
        {
            rewardPanelRoot.gameObject.SetActive(true);
        }

        // ─────────────────────────
        // 보상 항목
        // ─────────────────────────
        public void BuildReward(RewardType rewardType)
        {
            var rewardClone = Instantiate(rewardContainerPrefab, rewardRoot);
            _currentRewardsList.Add(rewardClone);

            switch (rewardType)
            {
                // 돈 보상
                case RewardType.Gold:
                    var rewardGold = rewardContainerData.GetRandomGoldReward(out var goldRewardData);
                    rewardClone.BuildReward(goldRewardData.RewardSprite, goldRewardData.RewardDescription);
                    rewardClone.RewardButton.onClick.AddListener(() => GetGoldReward(rewardClone, rewardGold));
                    break;

                // 카드 보상
                case RewardType.Card:
                    var rewardCardList = rewardContainerData.GetRandomCardRewardList(out var cardRewardData);
                    _cardRewardList.Clear();
                    foreach (var cardData in rewardCardList)
                        _cardRewardList.Add(cardData);
                    rewardClone.BuildReward(cardRewardData.RewardSprite, cardRewardData.RewardDescription);
                    rewardClone.RewardButton.onClick.AddListener(() => GetCardReward(rewardClone, 3));
                    break;

                // 유물 보상
                case RewardType.Relic:
                    break;

                // 디폴트 
                default:
                    throw new ArgumentOutOfRangeException(nameof(rewardType), rewardType, null);
            }
        }

        // ─────────────────────────
        // 표시창 갱신
        // ─────────────────────────
        public override void ResetCanvas()
        {
            ResetRewards();

            ResetChoice();
        }

        // ─────────────────────────
        // 보상 갱신
        // ─────────────────────────
        private void ResetRewards()
        {
            foreach (var rewardContainer in _currentRewardsList)
                Destroy(rewardContainer.gameObject);

            _currentRewardsList?.Clear();
        }

        // ─────────────────────────
        // 선택 재설정
        // ─────────────────────────
        private void ResetChoice()
        {
            foreach (var choice in _spawnedChoiceList)
            {
                Destroy(choice.gameObject);
            }

            _spawnedChoiceList?.Clear();
            ChoicePanel.DisablePanel();
        }

        // ─────────────────────────
        // 돈 보상
        // ─────────────────────────
        private void GetGoldReward(SetReward rewardContainer, int amount)
        {
            CoreManager.SavePlayData.CurrentGold += amount;
            _currentRewardsList.Remove(rewardContainer);
            UIManager.InformationCanvas.SetGoldText(CoreManager.SavePlayData.CurrentGold);
            Destroy(rewardContainer.gameObject);
        }

        // ─────────────────────────
        // 카드 보상
        // ─────────────────────────
        private void GetCardReward(SetReward rewardContainer, int amount = 3)
        {
            ChoicePanel.gameObject.SetActive(true);

            for (int i = 0; i < amount; i++)
            {
                Transform spawnTransform = choice2DCardSpawnRoot;

                var choice = Instantiate(choiceCardUIPrefab, spawnTransform);

                var reward = _cardRewardList.GetRandomItem();
                choice.BuildReward(reward);
                choice.OnCardChose += ResetChoice;

                _cardRewardList.Remove(reward);
                _spawnedChoiceList.Add(choice);
                _currentRewardsList.Remove(rewardContainer);

            }

            Destroy(rewardContainer.gameObject);
        }
    }
}
