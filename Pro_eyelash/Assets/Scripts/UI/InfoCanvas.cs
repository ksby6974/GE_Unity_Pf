using chataan.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace chataan.Scripts.UI
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 정보창 클래스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class InfoCanvas : CanvasBase
    {
        [Header("Settings")]
        [SerializeField] private GameObject randomizedDeckObject;
        [SerializeField] private TextMeshProUGUI roomTextField;
        [SerializeField] private TextMeshProUGUI goldTextField;
        [SerializeField] private TextMeshProUGUI nameTextField;
        [SerializeField] private TextMeshProUGUI healthTextField;

        public GameObject RandomizedDeckObject => randomizedDeckObject;
        public TextMeshProUGUI RoomTextField => roomTextField;
        public TextMeshProUGUI GoldTextField => goldTextField;
        public TextMeshProUGUI NameTextField => nameTextField;
        public TextMeshProUGUI HealthTextField => healthTextField;

        // ─────────────────────────
        // Awake
        // ─────────────────────────
        private void Awake()
        {
            ResetCanvas();
        }

        // ─────────────────────────
        // 방
        // ─────────────────────────
        public void SetRoomText(int roomNumber, bool useStage = false, int stageNumber = -1) => RoomTextField.text = useStage ? $"Room {stageNumber}/{roomNumber}" : $"Room {roomNumber}";

        // ─────────────────────────
        // 돈
        // ─────────────────────────
        public void SetGoldText(int value) => GoldTextField.text = $"{value}";

        // ─────────────────────────
        // 플레이어 이름
        // ─────────────────────────
        public void SetNameText(string name) => NameTextField.text = $"{name}";

        // ─────────────────────────
        // 체력
        // ─────────────────────────
        public void SetHealthText(int currentHealth, int maxHealth) => HealthTextField.text = $"{currentHealth}/{maxHealth}";

        // ─────────────────────────
        // 갱신
        // ─────────────────────────
        public override void ResetCanvas()
        {
            RandomizedDeckObject.SetActive(CoreManager.SavePlayData.IsRandomHand);
            SetHealthText(CoreManager.SavePlayData.AllyList[0].MyCharaData.MaxHealth, CoreManager.SavePlayData.AllyList[0].MyCharaData.MaxHealth);
            SetNameText(CoreManager.PlayData.DefaultName);
            SetRoomText(CoreManager.SavePlayData.CurrentEncounterId + 1, CoreManager.PlayData.UseStageSystem, CoreManager.SavePlayData.CurrentStageId + 1);
            UIManager.InfoCanvas.SetGoldText(CoreManager.SavePlayData.CurrentGold);
        }


    }
}