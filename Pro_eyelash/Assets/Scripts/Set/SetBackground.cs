using chataan.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Utils.Background
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 배경 설정용 클래스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━

    public class SetBackground : MonoBehaviour
    {
        [SerializeField] private List<BackgroundRoot> backgroundRootList;
        public List<BackgroundRoot> BackgroundRootList => backgroundRootList;

        private BattleManager BattleManager => BattleManager.Instance;

        // ─────────────────────────
        // 배경 표시
        // ─────────────────────────
        public void OpenSelectedBackground()
        {
            var encounter = BattleManager.CurrentEncounter;
            foreach (var backgroundRoot in BackgroundRootList)
            {
                backgroundRoot.gameObject.SetActive(encounter.TargetBackgroundType == backgroundRoot.BackgroundType);
            }
                
        }
    }
}
