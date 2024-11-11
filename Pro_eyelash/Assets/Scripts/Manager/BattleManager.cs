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
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 전투 총괄 클래스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class BattleManager : MonoBehaviour
    {
        private BattleManager() { }

        public static BattleManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private SetBackground setBackground; // 배경 이미지
        [SerializeField] private List<Transform> enemyPosList;  // 적 위치
        [SerializeField] private List<Transform> allyPosList;   // 아군 위치

        public List<EnemyBase> CurrentEnemiesList { get; private set; } = new List<EnemyBase>();
        public List<MyBase> CurrentAlliesList { get; private set; } = new List<MyBase>();

        public Action OnAllyTurnStarted;
        public Action OnEnemyTurnStarted;
        public List<Transform> EnemyPosList => enemyPosList;

        public List<Transform> AllyPosList => allyPosList;

        public MyBase CurrentMainAlly => CurrentAlliesList.Count > 0 ? CurrentAlliesList[0] : null;

        public EnemyEncounter CurrentEncounter { get; private set; }

        // ─────────────────────────
        // 현재 전투 페이즈 진행현황
        // ─────────────────────────
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

        // ─────────────────────────
        // Awake
        // 싱글톤 아님
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
                CurrentBattlePhase = BattlePhaseType.Pre;
                // 처음에는 전투 시작 페이즈부터
            }
        }

        // ─────────────────────────
        // 프레임마다
        // ─────────────────────────
        private void Start()
        {
            StartBattle();
        }

        // ─────────────────────────
        // 전투 시작
        // ─────────────────────────

        public void StartBattle()
            {
                // 적 아군 모두 생성
                CreateEnemy();
                CreateMy();
                setBackground.OpenSelectedBackground();

                PlayerManager.SetGameDeck();

                UIManager.BattleCanvas.gameObject.SetActive(true);
                UIManager.InfoCanvas.gameObject.SetActive(true);
                CurrentBattlePhase = BattlePhaseType.MyTurn;
            }

        // ─────────────────────────
        // 전투 페이즈 진행
        // ─────────────────────────
        public void PlayBattlePhase(BattlePhaseType phaseType)
        {
            switch (phaseType)
            {
                // 전투 돌입
                case BattlePhaseType.Pre:
                    break;

                // 플레이어 차례
                case BattlePhaseType.MyTurn:

                    OnAllyTurnStarted?.Invoke();

                    if (CurrentMainAlly.CharacterStats.IsStunned)
                    {
                        EndTurn();
                        return;
                    }

                    CoreManager.SavePlayData.CurrentMana = CoreManager.SavePlayData.MaxMana;

                    // 플레이어 차례 정기 드로우
                    PlayerManager.DrawCards(CoreManager.SavePlayData.DrawCount);

                    CoreManager.SavePlayData.CanSelectCards = true;

                    break;

                // 적 차례
                case BattlePhaseType.EnemyTurn:

                    OnEnemyTurnStarted?.Invoke();

                    PlayerManager.DiscardHand();

                    StartCoroutine(nameof(EnemyTurnRoutine));

                    CoreManager.SavePlayData.CanSelectCards = false;

                    break;

                // 전투 종료

                case BattlePhaseType.End:
                    CoreManager.SavePlayData.CanSelectCards = false;
                    break;

                // 그 외
                default:
                    throw new ArgumentOutOfRangeException(nameof(phaseType), phaseType, null);
            }
        }

        // ─────────────────────────
        // 차례 종료
        // ─────────────────────────
        public void EndTurn()
        {
            CurrentBattlePhase = BattlePhaseType.EnemyTurn;
        }

        // ─────────────────────────
        // 아군 파괴
        // ─────────────────────────
        public void OnAllyDeath(MyBase targetAlly)
        {
            var targetAllyData = CoreManager.SavePlayData.AllyList.Find(x => x.MyCharaData.CharacterID == targetAlly.MyCharaData.CharacterID);
            if (CoreManager.SavePlayData.AllyList.Count > 1)
            {
                CoreManager.SavePlayData.AllyList.Remove(targetAllyData);
            }

            CurrentAlliesList.Remove(targetAlly);
            UIManager.InfoCanvas.ResetCanvas();

            // 전투 패배
            if (CurrentAlliesList.Count <= 0)
            {
                LoseBattle();
            }
        }

        // ─────────────────────────
        // 적 파괴
        // ─────────────────────────
        public void OnEnemyDeath(EnemyBase targetEnemy)
        {
            CurrentEnemiesList.Remove(targetEnemy);

            // 전투 승리
            if (CurrentEnemiesList.Count <= 0)
            {
                WinBattle();
            }
        }

        // ─────────────────────────
        // 카드 강조 비활성화
        // ─────────────────────────
        public void DeactivateCardHighlights()
        {
            foreach (var currentEnemy in CurrentEnemiesList)
                currentEnemy.EnemyCanvas.SetHighlight(false);

            foreach (var currentAlly in CurrentAlliesList)
                currentAlly.MyCanvas.SetHighlight(false);
        }

        // ─────────────────────────
        // 행동력 증가
        // ─────────────────────────
        public void IncreaseCost(int target)
        {
            CoreManager.SavePlayData.CurrentMana += target;
            UIManager.BattleCanvas.SetPileTexts();
        }

        // ─────────────────────────
        // 강조문
        // ─────────────────────────
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

        // ─────────────────────────
        // 적 생성
        // ─────────────────────────
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

        // ─────────────────────────
        // 아군 생성
        // ─────────────────────────
        private void CreateMy()
        {
            for (var i = 0; i < CoreManager.SavePlayData.AllyList.Count; i++)
            {
                var clone = Instantiate(CoreManager.SavePlayData.AllyList[i], AllyPosList.Count >= i ? AllyPosList[i] : AllyPosList[0]);
                clone.BuildCharacter();
                CurrentAlliesList.Add(clone);
            }
        }

        // 전투 결과
        #region ResultBattle

            // ─────────────────────────
            // 전투 패배
            // ─────────────────────────
            private void LoseBattle()
            {
                // 전투가 종료되었다면 무효 처리
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

            // ─────────────────────────
            // 전투 승리
            // ─────────────────────────
            private void WinBattle()
            {
                // 전투가 종료되었다면 무효 처리
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
