using chataan.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace chataan.Scripts.UI
{
    // ����������������������������������������������������
    // ����â Ŭ����
    // ����������������������������������������������������
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

        // ��������������������������������������������������
        // Awake
        // ��������������������������������������������������
        private void Awake()
        {
            ResetCanvas();
        }

        // ��������������������������������������������������
        // ��
        // ��������������������������������������������������
        public void SetRoomText(int roomNumber, bool useStage = false, int stageNumber = -1) => RoomTextField.text = useStage ? $"Room {stageNumber}/{roomNumber}" : $"Room {roomNumber}";

        // ��������������������������������������������������
        // ��
        // ��������������������������������������������������
        public void SetGoldText(int value) => GoldTextField.text = $"{value}";

        // ��������������������������������������������������
        // �÷��̾� �̸�
        // ��������������������������������������������������
        public void SetNameText(string name) => NameTextField.text = $"{name}";

        // ��������������������������������������������������
        // ü��
        // ��������������������������������������������������
        public void SetHealthText(int currentHealth, int maxHealth) => HealthTextField.text = $"{currentHealth}/{maxHealth}";

        // ��������������������������������������������������
        // ����
        // ��������������������������������������������������
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