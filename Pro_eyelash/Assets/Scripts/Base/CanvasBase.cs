using chataan.Scripts.Managers;
using UnityEngine;

namespace chataan.Scripts.UI
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 표시창 기본 클래스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class CanvasBase : MonoBehaviour
    {
        protected BattleManager BattleManager => BattleManager.Instance;
        protected PlayerManager PlayerManager => PlayerManager.Instance;
        protected CoreManager CoreManager => CoreManager.Instance;
        protected UIManager UIManager => UIManager.Instance;


        // ─────────────────────────
        // 열기
        // ─────────────────────────
        public virtual void OpenCanvas()
        {
            gameObject.SetActive(true);
        }

        // ─────────────────────────
        // 닫기
        // ─────────────────────────
        public virtual void CloseCanvas()
        {
            gameObject.SetActive(false);
        }

        // ─────────────────────────
        // 재설정
        // ─────────────────────────
        public virtual void ResetCanvas()
        {

        }
    }
}
