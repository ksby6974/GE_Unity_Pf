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
    // ����������������������������������������������������
    // �� ��ü
    // ����������������������������������������������������
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

        // ��������������������������������������������������
        // �� ����
        // ��������������������������������������������������
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

        // ��������������������������������������������������
        // �� ���
        // ��������������������������������������������������
        protected override void OnDeath()
        {
            base.OnDeath();
            BattleManager.OnAllyTurnStarted -= ShowNextAbility;
            BattleManager.OnEnemyTurnStarted -= CharacterStats.TriggerAllStatus;

            BattleManager.OnEnemyDeath(this);
            SoundManager.PlayOneShot(DeathSoundProfileData.GetRandomClip());
            Destroy(gameObject);
        }

        // ��������������������������������������������������
        // ���� �ɷ�
        // ��������������������������������������������������
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

        // ��������������������������������������������������
        // �� �ൿ
        // ��������������������������������������������������
        public virtual IEnumerator ActionRoutine()
        {
            // �� ���� ������ ��� �̹� �ൿ ����
            if (CharacterStats.IsStunned)
            {
                yield break;
            }

            // ������ ǥ���ص� �� �ǵ� ����
            EnemyCanvas.IntentImage.gameObject.SetActive(false);

            // �ɵ��� �ൿ
            if (NextAbility.Intention.EnemyIntentionType == EnemyIntentionType.Attack || NextAbility.Intention.EnemyIntentionType == EnemyIntentionType.Debuff)
            {
                yield return StartCoroutine(AttackRoutine(NextAbility));
            }
            // ������ �ൿ
            else
            {
                yield return StartCoroutine(BuffRoutine(NextAbility));
            }
        }

        // ��������������������������������������������������
        // �� �ൿ : �ɵ���
        // ��������������������������������������������������
        protected virtual IEnumerator AttackRoutine(EnemyAbilityData targetAbility)
        {
            var waitFrame = new WaitForEndOfFrame();

            // ���� ��Ȳ �ƴ� (�÷��̾� ��� ��)
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

        // ��������������������������������������������������
        // �� �ൿ : ������
        // ��������������������������������������������������
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

        // ��������������������������������������������������
        // �ൿ ���� �κ�
        // ��������������������������������������������������
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
