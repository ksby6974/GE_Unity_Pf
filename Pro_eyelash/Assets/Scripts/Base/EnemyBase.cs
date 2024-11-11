using chataan.Scripts.Data.Chara;
using chataan.Scripts.Data.Sound;
using chataan.Scripts.EnemyAction;
using chataan.Scripts.Enums;
using chataan.Scripts.Gets;
using chataan.Scripts.Interface;
using System.Collections;
using UnityEngine;

namespace chataan.Scripts.Chara
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 적 객체
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class EnemyBase : CharaBase, IEnemy
    {
        [Header("Settings")]
        [SerializeField] protected EnemyData enemyData;
        [SerializeField] protected EnemyCanvas enemyCanvas;
        [SerializeField] protected SoundProfileData deathSoundProfileData;
        protected EnemyAbilityData NextAbility;

        public EnemyData EnemyData => enemyData;
        public EnemyCanvas EnemyCanvas => enemyCanvas;
        public SoundProfileData DeathSoundProfileData => deathSoundProfileData;

        // ─────────────────────────
        // 적 생성
        // ─────────────────────────
        public override void BuildCharacter()
        {
            base.BuildCharacter();
            EnemyCanvas.InitCanvas();
            CharacterStats = new CharacterStats(EnemyData.MaxHealth, EnemyCanvas);
            CharacterStats.OnDeath += OnDeath;
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);
            BattleManager.OnAllyTurnStarted += ShowNextAbility;
            BattleManager.OnEnemyTurnStarted += CharacterStats.TriggerAllStatus;
        }

        // ─────────────────────────
        // 적 사망
        // ─────────────────────────
        protected override void OnDeath()
        {
            base.OnDeath();
            BattleManager.OnAllyTurnStarted -= ShowNextAbility;
            BattleManager.OnEnemyTurnStarted -= CharacterStats.TriggerAllStatus;

            BattleManager.OnEnemyDeath(this);
            SoundManager.PlayOneShot(DeathSoundProfileData.GetRandomClip());
            Destroy(gameObject);
        }

        // ─────────────────────────
        // 다음 능력
        // ─────────────────────────
        private int _usedAbilityCount;
        private void ShowNextAbility()
        {
            NextAbility = EnemyData.GetAbility(_usedAbilityCount);
            EnemyCanvas.IntentImage.sprite = NextAbility.Intention.IntentionSprite;

            if (NextAbility.HideActionValue)
            {
                EnemyCanvas.NextActionValueText.gameObject.SetActive(false);
            }
            else
            {
                EnemyCanvas.NextActionValueText.gameObject.SetActive(true);
                EnemyCanvas.NextActionValueText.text = NextAbility.ActionList[0].ActionValue.ToString();
            }

            _usedAbilityCount++;
            EnemyCanvas.IntentImage.gameObject.SetActive(true);
        }

        // ─────────────────────────
        // 적 행동
        // ─────────────────────────
        public virtual IEnumerator ActionRoutine()
        {
            // 적 기절 상태일 경우 이번 행동 생략
            if (CharacterStats.IsStunned)
            {
                yield break;
            }

            // 이전에 표시해둔 적 의도 제거
            EnemyCanvas.IntentImage.gameObject.SetActive(false);

            // 능동적 행동
            if (NextAbility.Intention.EnemyIntentionType == EnemyIntentionType.Attack || NextAbility.Intention.EnemyIntentionType == EnemyIntentionType.Debuff)
            {
                yield return StartCoroutine(AttackRoutine(NextAbility));
            }
            // 수동적 행동
            else
            {
                yield return StartCoroutine(BuffRoutine(NextAbility));
            }
        }

        // ─────────────────────────
        // 적 행동 : 능동적
        // ─────────────────────────
        protected virtual IEnumerator AttackRoutine(EnemyAbilityData targetAbility)
        {
            var waitFrame = new WaitForEndOfFrame();

            // 전투 상황 아님 (플레이어 사망 등)
            if (BattleManager == null)
            {
                yield break;
            }

            var target = BattleManager.CurrentAlliesList.GetRandomItem();

            var startPos = transform.position;
            var endPos = target.transform.position;

            var startRot = transform.localRotation;
            var endRot = Quaternion.Euler(60, 0, 60);

            yield return StartCoroutine(MoveToTargetRoutine(waitFrame, startPos, endPos, startRot, endRot, 5));

            targetAbility.ActionList.ForEach(x => SetEnemyAction.GetAction(x.ActionType).DoAction(new EnemyActionParameters(x.ActionValue, target, this)));

            yield return StartCoroutine(MoveToTargetRoutine(waitFrame, endPos, startPos, endRot, startRot, 5));
        }

        // ─────────────────────────
        // 적 행동 : 수동적
        // ─────────────────────────
        protected virtual IEnumerator BuffRoutine(EnemyAbilityData targetAbility)
        {
            var waitFrame = new WaitForEndOfFrame();

            var target = BattleManager.CurrentEnemiesList.GetRandomItem();

            var startPos = transform.position;
            var endPos = startPos + new Vector3(0, 0.2f, 0);

            var startRot = transform.localRotation;
            var endRot = transform.localRotation;

            yield return StartCoroutine(MoveToTargetRoutine(waitFrame, startPos, endPos, startRot, endRot, 5));

            targetAbility.ActionList.ForEach(x => SetEnemyAction.GetAction(x.ActionType).DoAction(new EnemyActionParameters(x.ActionValue, target, this)));

            yield return StartCoroutine(MoveToTargetRoutine(waitFrame, endPos, startPos, endRot, startRot, 5));
        }

        // ─────────────────────────
        // 행동 공통 부분
        // ─────────────────────────
        private IEnumerator MoveToTargetRoutine(WaitForEndOfFrame waitFrame, Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, float speed)
        {
            var timer = 0f;
            while (true)
            {
                timer += Time.deltaTime * speed;

                transform.position = Vector3.Lerp(startPos, endPos, timer);
                transform.localRotation = Quaternion.Lerp(startRot, endRot, timer);
                if (timer >= 1f)
                {
                    break;
                }

                yield return waitFrame;
            }
        }
    }
}
