using UnityEngine;
using chataan.Scripts.Enums;

namespace chataan.Scripts.UI
{
    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
    // 詭檣 寡唳
    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
    public class BackgroundBase : MonoBehaviour
    {
        [SerializeField] private BackgroundTypes backgroundType;

        public BackgroundTypes BackgroundType => backgroundType;
    }
}