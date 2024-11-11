using chataan.Scripts.Managers;
using chataan.Scripts.UI;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Utils.Background
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 배경 설정용 클래스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━

    public class SetBackground : MonoBehaviour
    {
        [SerializeField] private List<BackgroundBase> backgroundBaseList;
        public List<BackgroundBase> BackgroundBaseList => backgroundBaseList;

        private BattleManager BattleManager => BattleManager.Instance;

        // ─────────────────────────
        // 배경 표시
        // ─────────────────────────
        public void OpenSelectedBackground()
        {
            var encounter = BattleManager.CurrentEncounter;
            foreach (var backgroundRoot in BackgroundBaseList)
            {
                backgroundRoot.gameObject.SetActive(encounter.TargetBackgroundType == backgroundRoot.BackgroundType);
            }
                
        }
    }
}
