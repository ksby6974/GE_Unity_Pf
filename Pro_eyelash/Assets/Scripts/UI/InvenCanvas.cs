using chataan.Scripts.Card;
using chataan.Scripts.Data.Card;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace chataan.Scripts.UI
{
    public class InvenCanvas : CanvasBase
    {
        // ����������������������������������������������������
        // ���� ǥ��â
        // ����������������������������������������������������
        [SerializeField] private TextMeshProUGUI titleTextField;
        [SerializeField] private LayoutGroup cardSpawnRoot;
        [SerializeField] private CardBase cardUIPrefab;

        public TextMeshProUGUI TitleTextField => titleTextField;
        public LayoutGroup CardSpawnRoot => cardSpawnRoot;

        private List<CardBase> _spawnedCardList = new List<CardBase>();

        public void ChangeTitle(string newTitle) => TitleTextField.text = newTitle;

        public void SetCards(List<CardData> cardDataList)
        {
            var count = 0;
            for (int i = 0; i < _spawnedCardList.Count; i++)
            {
                count++;
                if (i >= cardDataList.Count)
                {
                    _spawnedCardList[i].gameObject.SetActive(false);
                }
                else
                {
                    _spawnedCardList[i].SetCard(cardDataList[i], false);
                    _spawnedCardList[i].gameObject.SetActive(true);
                }

            }

            var cal = cardDataList.Count - count;
            if (cal > 0)
            {
                for (var i = 0; i < cal; i++)
                {
                    var cardData = cardDataList[count + i];
                    var cardBase = Instantiate(cardUIPrefab, CardSpawnRoot.transform);
                    cardBase.SetCard(cardData, false);
                    _spawnedCardList.Add(cardBase);
                }
            }


        }

        public override void OpenCanvas()
        {
            base.OpenCanvas();
            if (PlayerManager)
                PlayerManager.HandManager.DisableDragging();
        }

        public override void CloseCanvas()
        {
            base.CloseCanvas();
            if (PlayerManager)
                PlayerManager.HandManager.EnableDragging();
        }

        public override void ResetCanvas()
        {
            base.ResetCanvas();
        }
    }
}
