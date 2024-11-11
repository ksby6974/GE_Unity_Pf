using chataan.Scripts.Managers;
using UnityEngine;

namespace chataan.Scripts.UI
{
    // ����������������������������������������������������
    // ǥ��â �⺻ Ŭ����
    // ����������������������������������������������������
    public class CanvasBase : MonoBehaviour
    {
        protected BattleManager BattleManager => BattleManager.Instance;
        protected PlayerManager PlayerManager => PlayerManager.Instance;
        protected CoreManager CoreManager => CoreManager.Instance;
        protected UIManager UIManager => UIManager.Instance;


        // ��������������������������������������������������
        // ����
        // ��������������������������������������������������
        public virtual void OpenCanvas()
        {
            gameObject.SetActive(true);
        }

        // ��������������������������������������������������
        // �ݱ�
        // ��������������������������������������������������
        public virtual void CloseCanvas()
        {
            gameObject.SetActive(false);
        }

        // ��������������������������������������������������
        // �缳��
        // ��������������������������������������������������
        public virtual void ResetCanvas()
        {

        }
    }
}
