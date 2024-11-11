using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace chataan.Scripts.Chara
{
    // ����������������������������������������������������
    // ���� ���� ���� ǥ��â
    // ����������������������������������������������������
    public class EnemyCanvas : CharaCanvas
    {
        [Header("Enemy Canvas Settings")]
        [SerializeField] private Image intentImage;
        [SerializeField] private TextMeshProUGUI nextActionValueText;
        public Image IntentImage => intentImage;
        public TextMeshProUGUI NextActionValueText => nextActionValueText;
    }
}
