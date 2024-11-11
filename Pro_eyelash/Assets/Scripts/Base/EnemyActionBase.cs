using chataan.Scripts.Battle;
using chataan.Scripts.Chara;
using chataan.Scripts.Enums;
using chataan.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.EnemyAction
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 적 행동 기본 추상 클래스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public abstract class EnemyActionBase
    {
        protected EnemyActionBase() { }
        public abstract EnemyActionType ActionType { get; }
        public abstract void DoAction(EnemyActionParameters actionParameters);

        protected FxManager FxManager => FxManager.Instance;
        protected SoundManager SoundManager => SoundManager.Instance;
        protected CoreManager CoreManager => CoreManager.Instance;
        protected BattleManager CombatManager => BattleManager.Instance;
        protected PlayerManager CollectionManager => PlayerManager.Instance;

    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 적 행동을 적용시키려는 대상
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class EnemyActionParameters
    {
        public readonly float Value;
        public readonly CharaBase TargetCharacter;
        public readonly CharaBase SelfCharacter;

        public EnemyActionParameters(float value, CharaBase target, CharaBase self)
        {
            Value = value;
            TargetCharacter = target;
            SelfCharacter = self;
        }
    }
}
