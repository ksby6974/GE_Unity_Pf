using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using chataan.Scripts.Enums;
using chataan.Scripts.Utils.Background;
using chataan.Scripts.Data.Encounter;
using chataan.Scripts.Chara;

namespace chataan.Scripts.Managers
{   
    // ����������������������������������������������������
    // ���� �Ѱ� Ŭ����
    // ����������������������������������������������������
    public class BattleManager : MonoBehaviour
    {
        private BattleManager() { }

        public static BattleManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private SetBackground setBackground; // ��� �̹���
        [SerializeField] private List<Transform> enemyPosList;  // �� ��ġ
        [SerializeField] private List<Transform> allyPosList;   // �Ʊ� ��ġ

        public List<EnemyBase> CurrentEnemiesList { get; private set; } = new List<EnemyBase>();
        public List<MyBase> CurrentAlliesList { get; private set; } = new List<MyBase>();

        public Action OnAllyTurnStarted;
        public Action OnEnemyTurnStarted;
        public List<Transform> EnemyPosList => enemyPosList;

        public List<Transform> AllyPosList => allyPosList;

        public MyBase CurrentMainAlly => CurrentAlliesList.Count > 0 ? CurrentAlliesList[0] : null;

        public EnemyEncounter CurrentEncounter { get; private set; }

        // ��������������������������������������������������
        // ���� ���� ������ ������Ȳ
        // ��������������������������������������������������
        public BattlePhaseType CurrentBattlePhase
        {
            get => _CurrentBattle;
            private set
            {
                PlayBattlePhase(value);
                _CurrentBattle = value;
            }
        }

        private BattlePhaseType _CurrentBattle;

        protected FxManager FxManager => FxManager.Instance;
        protected SoundManager SoundManager => SoundManager.Instance;
        protected CoreManager CoreManager => CoreManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        protected PlayerManager PlayerManager => PlayerManager.Instance;

        // ��������������������������������������������������
        // Awake
        // �̱��� �ƴ�
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
                CurrentBattlePhase = BattlePhaseType.Pre;
                // ó������ ���� ���� ���������
            }
        }

        // ��������������������������������������������������
        // �����Ӹ���
        // ��������������������������������������������������
        private void Start()
        {
            StartBattle();
        }

        // ��������������������������������������������������
        // ���� ����
        // ��������������������������������������������������

        public void StartBattle()
            {
                // �� �Ʊ� ��� ����
                CreateEnemy();
                CreateMy();
                setBackground.OpenSelectedBackground();

                PlayerManager.SetGameDeck();

                UIManager.BattleCanvas.gameObject.SetActive(true);
                UIManager.InfoCanvas.gameObject.SetActive(true);
                CurrentBattlePhase = BattlePhaseType.MyTurn;
            }

        // ��������������������������������������������������
        // ���� ������ ����
        // ��������������������������������������������������
        public void PlayBattlePhase(BattlePhaseType phaseType)
        {
            switch (phaseType)
            {
                // ���� ����
                case BattlePhaseType.Pre:
                    break;

                // �÷��̾� ����
                case BattlePhaseType.MyTurn:

                    OnAllyTurnStarted?.Invoke();

                    if (CurrentMainAlly.CharacterStats.IsStunned)
                    {
                        EndTurn();
                        return;
                    }

                    CoreManager.SavePlayData.CurrentMana = CoreManager.SavePlayData.MaxMana;

                    // �÷��̾� ���� ���� ��ο�
                    PlayerManager.DrawCards(CoreManager.SavePlayData.DrawCount);

                    CoreManager.SavePlayData.CanSelectCards = true;

                    break;

                // �� ����
                case BattlePhaseType.EnemyTurn:

                    OnEnemyTurnStarted?.Invoke();

                    PlayerManager.DiscardHand();

                    StartCoroutine(nameof(EnemyTurnRoutine));

                    CoreManager.SavePlayData.CanSelectCards = false;

                    break;

                // ���� ����

                case BattlePhaseType.End:
                    CoreManager.SavePlayData.CanSelectCards = false;
                    break;

                // �� ��
                default:
                    throw new ArgumentOutOfRangeException(nameof(phaseType), phaseType, null);
            }
        }

        // ��������������������������������������������������
        // ���� ����
        // ��������������������������������������������������
        public void EndTurn()
        {
            CurrentBattlePhase = BattlePhaseType.EnemyTurn;
        }

        // ��������������������������������������������������
        // �Ʊ� �ı�
        // ��������������������������������������������������
        public void OnAllyDeath(MyBase targetAlly)
        {
            var targetAllyData = CoreManager.SavePlayData.AllyList.Find(x => x.MyCharaData.CharacterID == targetAlly.MyCharaData.CharacterID);
            if (CoreManager.SavePlayData.AllyList.Count > 1)
            {
                CoreManager.SavePlayData.AllyList.Remove(targetAllyData);
            }

            CurrentAlliesList.Remove(targetAlly);
            UIManager.InfoCanvas.ResetCanvas();

            // ���� �й�
            if (CurrentAlliesList.Count <= 0)
            {
                LoseBattle();
            }
        }

        // ��������������������������������������������������
        // �� �ı�
        // ��������������������������������������������������
        public void OnEnemyDeath(EnemyBase targetEnemy)
        {
            CurrentEnemiesList.Remove(targetEnemy);

            // ���� �¸�
            if (CurrentEnemiesList.Count <= 0)
            {
                WinBattle();
            }
        }

        // ��������������������������������������������������
        // ī�� ���� ��Ȱ��ȭ
        // ��������������������������������������������������
        public void DeactivateCardHighlights()
        {
            foreach (var currentEnemy in CurrentEnemiesList)
                currentEnemy.EnemyCanvas.SetHighlight(false);

            foreach (var currentAlly in CurrentAlliesList)
                currentAlly.MyCanvas.SetHighlight(false);
        }

        // ��������������������������������������������������
        // �ൿ�� ����
        // ��������������������������������������������������
        public void IncreaseCost(int target)
        {
            CoreManager.SavePlayData.CurrentMana += target;
            UIManager.BattleCanvas.SetPileTexts();
        }

        // ��������������������������������������������������
        // ������
        // ��������������������������������������������������
        public void HighlightCardTarget(TargetType targetTypeTargetType)
        {
            switch (targetTypeTargetType)
            {
                case TargetType.Enemy:
                    foreach (var currentEnemy in CurrentEnemiesList)
                        currentEnemy.EnemyCanvas.SetHighlight(true);
                    break;
                case TargetType.Ally:
                    foreach (var currentAlly in CurrentAlliesList)
                        currentAlly.MyCanvas.SetHighlight(true);
                    break;
                case TargetType.AllEnemies:
                    foreach (var currentEnemy in CurrentEnemiesList)
                        currentEnemy.EnemyCanvas.SetHighlight(true);
                    break;
                case TargetType.AllAllies:
                    foreach (var currentAlly in CurrentAlliesList)
                        currentAlly.MyCanvas.SetHighlight(true);
                    break;
                case TargetType.RandomEnemy:
                    foreach (var currentEnemy in CurrentEnemiesList)
                        currentEnemy.EnemyCanvas.SetHighlight(true);
                    break;

                    //
                case TargetType.RandomAlly:
                    foreach (var currentAlly in CurrentAlliesList)
                        currentAlly.MyCanvas.SetHighlight(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetTypeTargetType), targetTypeTargetType, null);
            }
        }

        // ��������������������������������������������������
        // �� ����
        // ��������������������������������������������������
        private void CreateEnemy()
        {
            CurrentEncounter = CoreManager.EncounterData.GetEnemyEncounter(
                CoreManager.SavePlayData.CurrentStageId,
                CoreManager.SavePlayData.CurrentEncounterId,
                CoreManager.SavePlayData.IsFinalEncounter);

            var enemyList = CurrentEncounter.EnemyList;
            for (var i = 0; i < enemyList.Count; i++)
            {
                var clone = Instantiate(enemyList[i].EnemyPrefab, EnemyPosList.Count >= i ? EnemyPosList[i] : EnemyPosList[0]);
                clone.BuildCharacter();
                CurrentEnemiesList.Add(clone);
            }
        }

        // ��������������������������������������������������
        // �Ʊ� ����
        // ��������������������������������������������������
        private void CreateMy()
        {
            for (var i = 0; i < CoreManager.SavePlayData.AllyList.Count; i++)
            {
                var clone = Instantiate(CoreManager.SavePlayData.AllyList[i], AllyPosList.Count >= i ? AllyPosList[i] : AllyPosList[0]);
                clone.BuildCharacter();
                CurrentAlliesList.Add(clone);
            }
        }

        // ���� ���
        #region ResultBattle

            // ��������������������������������������������������
            // ���� �й�
            // ��������������������������������������������������
            private void LoseBattle()
            {
                // ������ ����Ǿ��ٸ� ��ȿ ó��
                if (CurrentBattlePhase == BattlePhaseType.End)
                    {
                        return;
                    }

                CurrentBattlePhase = BattlePhaseType.End;

                PlayerManager.DiscardHand();
                PlayerManager.DiscardPile.Clear();
                PlayerManager.DrawPile.Clear();
                PlayerManager.HandPile.Clear();
                PlayerManager.HandManager.hand.Clear();
                UIManager.BattleCanvas.gameObject.SetActive(true);
                UIManager.BattleCanvas.CombatLosePanel.SetActive(true);
            }

            // ��������������������������������������������������
            // ���� �¸�
            // ��������������������������������������������������
            private void WinBattle()
            {
                // ������ ����Ǿ��ٸ� ��ȿ ó��
                if (CurrentBattlePhase == BattlePhaseType.End)
                {
                    return;
                }

                CurrentBattlePhase = BattlePhaseType.End;

                foreach (var allyBase in CurrentAlliesList)
                {
                    CoreManager.SavePlayData.SetAllyHealthData(allyBase.MyCharaData.CharacterID,
                        allyBase.CharacterStats.CurrentHealth, allyBase.CharacterStats.MaxHealth);
                }

                PlayerManager.ClearPiles();


                if (CoreManager.SavePlayData.IsFinalEncounter)
                {
                    UIManager.BattleCanvas.CombatWinPanel.SetActive(true);
                }
                else
                {
                    CurrentMainAlly.CharacterStats.ClearAllStatus();
                    CoreManager.SavePlayData.CurrentEncounterId++;
                    UIManager.BattleCanvas.gameObject.SetActive(false);
                    UIManager.RewardCanvas.gameObject.SetActive(true);
                    UIManager.RewardCanvas.PrepareCanvas();
                    UIManager.RewardCanvas.BuildReward(RewardType.Gold);
                    UIManager.RewardCanvas.BuildReward(RewardType.Card);
                }

            }
        #endregion

        private IEnumerator EnemyTurnRoutine()
        {
            var waitDelay = new WaitForSeconds(0.1f);

            foreach (var currentEnemy in CurrentEnemiesList)
            {
                yield return currentEnemy.StartCoroutine(nameof(EnemyExample.ActionRoutine));
                yield return waitDelay;
            }

            if (CurrentBattlePhase != BattlePhaseType.End)
                CurrentBattlePhase = BattlePhaseType.MyTurn;
        }
    }
}
