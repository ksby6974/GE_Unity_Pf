using UnityEngine;
using chataan.Scripts.Enums;

namespace chataan.Scripts.Utils.Background
{
    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
    // 詭檣 寡唳
    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
    public class BackgroundRoot : MonoBehaviour
    {
        [SerializeField] private BackgroundTypes backgroundType;

        public BackgroundTypes BackgroundType => backgroundType;
    }
}