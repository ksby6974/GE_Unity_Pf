using chataan.Scripts.Managers;
using chataan.Scripts.UI;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Utils.Background
{
    // ����������������������������������������������������
    // ��� ������ Ŭ����
    // ����������������������������������������������������

    public class SetBackground : MonoBehaviour
    {
        [SerializeField] private List<BackgroundBase> backgroundBaseList;
        public List<BackgroundBase> BackgroundBaseList => backgroundBaseList;

        private BattleManager BattleManager => BattleManager.Instance;

        // ��������������������������������������������������
        // ��� ǥ��
        // ��������������������������������������������������
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
