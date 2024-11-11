using chataan.Scripts.Chara;
using chataan.Scripts.Data.Card;
using chataan.Scripts.Enums;
using chataan.Scripts.Managers;
using UnityEngine;

namespace chataan.Scripts.Card
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 카드 효과 변수들
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class CardActionParameters
    {
        public readonly float Value;
        public readonly CharaBase TargetCharacter;
        public readonly CharaBase SelfCharacter;
        public readonly CardData CardData;
        public readonly CardBase CardBase;

        // ─────────────────────────
        // 생성자
        // ─────────────────────────
        public CardActionParameters(float value, CharaBase target, CharaBase self, CardData cardData, CardBase cardBase)
        {
            Value = value;
            TargetCharacter = target;
            SelfCharacter = self;
            CardData = cardData;
            CardBase = cardBase;
        }
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 카드 효과 추상 클래스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public abstract class CardActionBase
    {
        // ─────────────────────────
        // 생성자
        // ─────────────────────────
        protected CardActionBase() { }
        public abstract CardEffectType ActionType { get; }

        // ─────────────────────────
        // 효과 발동
        // ─────────────────────────
        public abstract void DoAction(CardActionParameters actionParameters);

        protected FxManager FxManager => FxManager.Instance;
        protected SoundManager SoundManager => SoundManager.Instance;
        protected CoreManager CoreManager => CoreManager.Instance;
        protected BattleManager BattleManager => BattleManager.Instance;
        protected PlayerManager PlayerManager => PlayerManager.Instance;

    }
}