using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace chataan.Scripts.UI.Reward
{
    // ����������������������������������������������������
    // ���� ���� Ŭ����
    // ����������������������������������������������������
    public class SetReward : MonoBehaviour
    {
        [SerializeField] private Button rewardButton;
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardText;

        public Button RewardButton => rewardButton;

        // ��������������������������������������������������
        // ���� ����
        // ��������������������������������������������������
        public void BuildReward(Sprite rewardSprite, string rewardDescription)
        {
            rewardImage.sprite = rewardSprite;
            rewardText.text = rewardDescription;
        }

    }
}
