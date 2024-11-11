using chataan.Scripts.Data.Card;
using System.Collections.Generic;
using UnityEngine;
using chataan.Scripts.Battle;

namespace chataan.Scripts.Managers
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 플레이어 클래스
    // 카드풀 관리
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
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


        // ─────────────────────────
        // Awake
        // ─────────────────────────
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

        // ─────────────────────────
        // 카드 뽑기
        // ─────────────────────────
        public void DrawCards(int targetDrawCount)
        {
            var currentDrawCount = 0;

            for (var i = 0; i < targetDrawCount; i++)
            {
                // 패에 잡을 수 있는 최대 카드의 수를 넘음
                if (CoreManager.PlayData.MaxCardOnHand <= HandPile.Count)
                {
                    return;
                }

                // 뽑을 카드 더미가 비었다면 버린 카드 더미에서 순환
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

                // 패에 추가
                HandPile.Add(randomCard);

                // 뽑을 카드 더미에서 빼기
                DrawPile.Remove(randomCard);

                // 현재 뽑은 수 증가
                currentDrawCount++;

                //
                UIManager.BattleCanvas.SetPileTexts();
            }

            // 전체 재갱신
            foreach (var cardObject in HandManager.hand)
            {
                cardObject.UpdateCardText();
            }
        }

        // ─────────────────────────
        // 차례 종료 시 패 버리기
        // ─────────────────────────
        public void DiscardHand()
        {
            foreach (var cardBase in HandManager.hand)
            {
                cardBase.Discard();
            }

            HandManager.hand.Clear();
        }

        // ─────────────────────────
        // 카드 버리기
        // ─────────────────────────
        public void OnCardDiscarded(Card targetCard)
        {
            HandPile.Remove(targetCard.CardData);
            DiscardPile.Add(targetCard.CardData);
            UIManager.BattleCanvas.SetPileTexts();
        }

        // ─────────────────────────
        // 카드 고갈
        // ─────────────────────────
        public void OnCardExhausted(CardBase targetCard)
        {
            HandPile.Remove(targetCard.CardData);
            ExhaustPile.Add(targetCard.CardData);
            UIManager.BattleCanvas.SetPileTexts();
        }

        // ─────────────────────────
        // 카드 사용
        // ─────────────────────────
        public void OnCardPlayed(CardBase targetCard)
        {
            if (targetCard.CardData.ExhaustAfterPlay)
                targetCard.Exhaust();
            else
                targetCard.Discard();

            foreach (var cardObject in HandManager.hand)
                cardObject.UpdateCardText();
        }

        // ─────────────────────────
        // 카드를 덱에 추가
        // ─────────────────────────
        public void SetGameDeck()
        {
            foreach (var i in CoreManager.SavePlayData.CurrentCardsList)
            {
                DrawPile.Add(i);
            }
        }

        // ─────────────────────────
        // 전투 시작 시 카드 더미들 초기화
        // ─────────────────────────
        public void ClearPiles()
        {
            DiscardPile.Clear();
            DrawPile.Clear();
            HandPile.Clear();
            ExhaustPile.Clear();
            HandManager.hand.Clear();
        }

        // ─────────────────────────
        // 버린 카드 더미 섞기
        // ─────────────────────────
        private void ReshuffleDiscardPile()
        {
            foreach (var i in DiscardPile)
            {
                DrawPile.Add(i);
            }

            DiscardPile.Clear();
        }

        // ─────────────────────────
        // 뽑을 카드 더미 섞기
        // ─────────────────────────
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
