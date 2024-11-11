using UnityEngine;
using chataan.Scripts.Enums;

namespace chataan.Scripts.UI
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 메인 배경
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class BackgroundBase : MonoBehaviour
    {
        [SerializeField] private BackgroundTypes backgroundType;

        public BackgroundTypes BackgroundType => backgroundType;
    }
}