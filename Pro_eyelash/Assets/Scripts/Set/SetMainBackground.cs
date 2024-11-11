using UnityEngine;
using chataan.Scripts.Enums;

namespace chataan.Scripts.Utils.Background
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 메인 배경
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class BackgroundRoot : MonoBehaviour
    {
        [SerializeField] private BackgroundTypes backgroundType;

        public BackgroundTypes BackgroundType => backgroundType;
    }
}