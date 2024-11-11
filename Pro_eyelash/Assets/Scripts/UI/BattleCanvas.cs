using chataan.Scripts.Enums;
using chataan.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace chataan.Scripts.UI
{
    // ����������������������������������������������������
    // ���� �� ǥ��â Ŭ����
    // ����������������������������������������������������
    public class BattleCanvas : CanvasBase
    {
        // ǥ��
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI drawPileTextField;
        [SerializeField] private TextMeshProUGUI discardPileTextField;
        [SerializeField] private TextMeshProUGUI exhaustPileTextField;
        [SerializeField] private TextMeshProUGUI manaTextTextField;

        // �г�
        [Header("Panels")]
        [SerializeField] private GameObject combatWinPanel;
        [SerializeField] private GameObject combatLosePanel;

        public TextMeshProUGUI DrawPileTextField => drawPileTextField;
        public TextMeshProUGUI DiscardPileTextField => discardPileTextField;
        public TextMeshProUGUI ManaTextTextField => manaTextTextField;
        public GameObject CombatWinPanel => combatWinPanel;
        public GameObject CombatLosePanel => combatLosePanel;

        public TextMeshProUGUI ExhaustPileTextField => exhaustPileTextField;

        // ��������������������������������������������������
        // Awake
        // ��������������������������������������������������
        private void Awake()
        {
            CombatWinPanel.SetActive(false);
            CombatLosePanel.SetActive(false);
        }

        // ��������������������������������������������������
        // �÷��̾� ī��Ǯ
        // ��������������������������������������������������
        public void SetPileTexts()
        {
            DrawPileTextField.text = $"{PlayerManager.DrawPile.Count.ToString()}";
            DiscardPileTextField.text = $"{PlayerManager.DiscardPile.Count.ToString()}";
            ExhaustPileTextField.text = $"{PlayerManager.ExhaustPile.Count.ToString()}";
            ManaTextTextField.text = $"{CoreManager.SavePlayData.CurrentMana.ToString()}/{CoreManager.SavePlayData.MaxMana}";
        }

        // ��������������������������������������������������
        // ǥ��â �缳��
        // ��������������������������������������������������
        public override void ResetCanvas()
        {
            base.ResetCanvas();
            CombatWinPanel.SetActive(false);
            CombatLosePanel.SetActive(false);
        }

        // ��������������������������������������������������
        // ���� ����
        // ��������������������������������������������������
        public void EndTurn()
        {
            // ���� ���� ��Ȳ�� �÷��̾� ���� == ���� ����
            if (BattleManager.CurrentBattlePhase == BattlePhaseType.MyTurn)
            {
                BattleManager.EndTurn();
            }
        }
    }
}
