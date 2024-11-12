using UnityEngine;

namespace chataan.Scripts.UI
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 보상 패널
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class ChoicePanel : MonoBehaviour
    {
        public void DisablePanel()
        {
            gameObject.SetActive(false);
        }
    }
}