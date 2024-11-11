using chataan.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Utils.Background
{
    // ����������������������������������������������������
    // ��� ������ Ŭ����
    // ����������������������������������������������������

    public class SetBackground : MonoBehaviour
    {
        [SerializeField] private List<BackgroundRoot> backgroundRootList;
        public List<BackgroundRoot> BackgroundRootList => backgroundRootList;

        private BattleManager BattleManager => BattleManager.Instance;

        // ��������������������������������������������������
        // ��� ǥ��
        // ��������������������������������������������������
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
