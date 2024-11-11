using chataan.Scripts.Data.Card;
using System.Collections.Generic;
using UnityEngine;
using chataan.Scripts.Battle;

namespace chataan.Scripts.Managers
{
    // ����������������������������������������������������
    // �÷��̾� Ŭ����
    // ī��Ǯ ����
    // ����������������������������������������������������
    public class PlayerManager : MonoBehaviour
    {
        public PlayerManager() { }

        public static PlayerManager Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private HandManager handManager;

        public List<CardData> DrawPile { get; private set; } = new List<CardData>();
        public List<CardData> HandPile { get; private set; } = new List<CardData>();
        public List<CardData> DiscardPile { get; private set; } = new List<CardData>();

        public List<CardData> ExhaustPile { get; private set; } = new List<CardData>();
        public HandManager HandManager => handManager;
        protected FxManager FxManager => FxManager.Instance;
        protected SoundManager SoundManager => SoundManager.Instance;
        protected CoreManager CoreManager => CoreManager.Instance;
        protected BattleManager BattleManager => BattleManager.Instance;

        protected UIManager UIManager => UIManager.Instance;


        // ��������������������������������������������������
        // Awake
        // ��������������������������������������������������
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
        }

        // ��������������������������������������������������
        // ī�� �̱�
        // ��������������������������������������������������
        public void DrawCards(int targetDrawCount)
        {
            var currentDrawCount = 0;

            for (var i = 0; i < targetDrawCount; i++)
            {
                // �п� ���� �� �ִ� �ִ� ī���� ���� ����
                if (CoreManager.PlayData.MaxCardOnHand <= HandPile.Count)
                {
                    return;
                }

                // ���� ī�� ���̰� ����ٸ� ���� ī�� ���̿��� ��ȯ
                if (DrawPile.Count <= 0)
                {
                    var nDrawCount = targetDrawCount - currentDrawCount;

                    if (nDrawCount >= DiscardPile.Count)
                        nDrawCount = DiscardPile.Count;

                    ReshuffleDiscardPile();
                    DrawCards(nDrawCount);
                    break;
                }

                var randomCard = DrawPile[Random.Range(0, DrawPile.Count)];
                var _randomCard = CoreManager.BuildAndGetCard(randomCard, HandManager.drawTransform);
                HandManager.AddCardToHand(_randomCard);

                // �п� �߰�
                HandPile.Add(randomCard);

                // ���� ī�� ���̿��� ����
                DrawPile.Remove(randomCard);

                // ���� ���� �� ����
                currentDrawCount++;

                //
                UIManager.BattleCanvas.SetPileTexts();
            }

            // ��ü �簻��
            foreach (var cardObject in HandManager.hand)
            {
                cardObject.UpdateCardText();
            }
        }

        // ��������������������������������������������������
        // ���� ���� �� �� ������
        // ��������������������������������������������������
        public void DiscardHand()
        {
            foreach (var cardBase in HandManager.hand)
            {
                cardBase.Discard();
            }

            HandManager.hand.Clear();
        }

        // ��������������������������������������������������
        // ī�� ������
        // ��������������������������������������������������
        public void OnCardDiscarded(Card targetCard)
        {
            HandPile.Remove(targetCard.CardData);
            DiscardPile.Add(targetCard.CardData);
            UIManager.BattleCanvas.SetPileTexts();
        }

        // ��������������������������������������������������
        // ī�� ��
        // ��������������������������������������������������
        public void OnCardExhausted(CardBase targetCard)
        {
            HandPile.Remove(targetCard.CardData);
            ExhaustPile.Add(targetCard.CardData);
            UIManager.BattleCanvas.SetPileTexts();
        }

        // ��������������������������������������������������
        // ī�� ���
        // ��������������������������������������������������
        public void OnCardPlayed(CardBase targetCard)
        {
            if (targetCard.CardData.ExhaustAfterPlay)
                targetCard.Exhaust();
            else
                targetCard.Discard();

            foreach (var cardObject in HandManager.hand)
                cardObject.UpdateCardText();
        }

        // ��������������������������������������������������
        // ī�带 ���� �߰�
        // ��������������������������������������������������
        public void SetGameDeck()
        {
            foreach (var i in CoreManager.SavePlayData.CurrentCardsList)
            {
                DrawPile.Add(i);
            }
        }

        // ��������������������������������������������������
        // ���� ���� �� ī�� ���̵� �ʱ�ȭ
        // ��������������������������������������������������
        public void ClearPiles()
        {
            DiscardPile.Clear();
            DrawPile.Clear();
            HandPile.Clear();
            ExhaustPile.Clear();
            HandManager.hand.Clear();
        }

        // ��������������������������������������������������
        // ���� ī�� ���� ����
        // ��������������������������������������������������
        private void ReshuffleDiscardPile()
        {
            foreach (var i in DiscardPile)
            {
                DrawPile.Add(i);
            }

            DiscardPile.Clear();
        }

        // ��������������������������������������������������
        // ���� ī�� ���� ����
        // ��������������������������������������������������
        private void ReshuffleDrawPile()
        {
            foreach (var i in DrawPile)
            {
                DiscardPile.Add(i);
            }
 
            DrawPile.Clear();
        }
    }
}
