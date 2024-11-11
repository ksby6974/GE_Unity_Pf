using chataan.Scripts.Chara;
using chataan.Scripts.Data.Card;
using chataan.Scripts.Enums;
using chataan.Scripts.Managers;
using UnityEngine;

namespace chataan.Scripts.Card
{
    // ����������������������������������������������������
    // ī�� ȿ�� ������
    // ����������������������������������������������������
    public class CardActionParameters
    {
        public readonly float Value;
        public readonly CharaBase TargetCharacter;
        public readonly CharaBase SelfCharacter;
        public readonly CardData CardData;
        public readonly CardBase CardBase;

        // ��������������������������������������������������
        // ������
        // ��������������������������������������������������
        public CardActionParameters(float value, CharaBase target, CharaBase self, CardData cardData, CardBase cardBase)
        {
            Value = value;
            TargetCharacter = target;
            SelfCharacter = self;
            CardData = cardData;
            CardBase = cardBase;
        }
    }

    // ����������������������������������������������������
    // ī�� ȿ�� �߻� Ŭ����
    // ����������������������������������������������������
    public abstract class CardActionBase
    {
        // ��������������������������������������������������
        // ������
        // ��������������������������������������������������
        protected CardActionBase() { }
        public abstract CardEffectType ActionType { get; }

        // ��������������������������������������������������
        // ȿ�� �ߵ�
        // ��������������������������������������������������
        public abstract void DoAction(CardActionParameters actionParameters);

        protected FxManager FxManager => FxManager.Instance;
        protected SoundManager SoundManager => SoundManager.Instance;
        protected CoreManager CoreManager => CoreManager.Instance;
        protected BattleManager BattleManager => BattleManager.Instance;
        protected PlayerManager PlayerManager => PlayerManager.Instance;

    }
}