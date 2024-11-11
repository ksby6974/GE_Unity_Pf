using chataan.Scripts.Enums;
using chataan.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace chataan.Scripts.UI
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 전투 시 표시창 클래스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class BattleCanvas : CanvasBase
    {
        // 표시
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI drawPileTextField;
        [SerializeField] private TextMeshProUGUI discardPileTextField;
        [SerializeField] private TextMeshProUGUI exhaustPileTextField;
        [SerializeField] private TextMeshProUGUI manaTextTextField;

        // 패널
        [Header("Panels")]
        [SerializeField] private GameObject combatWinPanel;
        [SerializeField] private GameObject combatLosePanel;

        public TextMeshProUGUI DrawPileTextField => drawPileTextField;
        public TextMeshProUGUI DiscardPileTextField => discardPileTextField;
        public TextMeshProUGUI ManaTextTextField => manaTextTextField;
        public GameObject CombatWinPanel => combatWinPanel;
        public GameObject CombatLosePanel => combatLosePanel;

        public TextMeshProUGUI ExhaustPileTextField => exhaustPileTextField;

        // ─────────────────────────
        // Awake
        // ─────────────────────────
        private void Awake()
        {
            CombatWinPanel.SetActive(false);
            CombatLosePanel.SetActive(false);
        }

        // ─────────────────────────
        // 플레이어 카드풀
        // ─────────────────────────
        public void SetPileTexts()
        {
            DrawPileTextField.text = $"{PlayerManager.DrawPile.Count.ToString()}";
            DiscardPileTextField.text = $"{PlayerManager.DiscardPile.Count.ToString()}";
            ExhaustPileTextField.text = $"{PlayerManager.ExhaustPile.Count.ToString()}";
            ManaTextTextField.text = $"{CoreManager.SavePlayData.CurrentMana.ToString()}/{CoreManager.SavePlayData.MaxMana}";
        }

        // ─────────────────────────
        // 표시창 재설정
        // ─────────────────────────
        public override void ResetCanvas()
        {
            base.ResetCanvas();
            CombatWinPanel.SetActive(false);
            CombatLosePanel.SetActive(false);
        }

        // ─────────────────────────
        // 차례 종료
        // ─────────────────────────
        public void EndTurn()
        {
            // 현재 전투 상황이 플레이어 차례 == 차례 종료
            if (BattleManager.CurrentBattlePhase == BattlePhaseType.MyTurn)
            {
                BattleManager.EndTurn();
            }
        }
    }
}
