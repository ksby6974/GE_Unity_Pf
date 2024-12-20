using chataan.Scripts.Data.Card;
using chataan.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace chataan.Scripts.Managers
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // UI 총괄 클래스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [DefaultExecutionOrder(-4)]
    public class UIManager : Singleton<UIManager>
    {
        protected MouseManager MouseManager => MouseManager.Instance;

        // 정보 표시용
        [Header("Canvas")]
        [SerializeField] private BattleCanvas battleCanvas;
        [SerializeField] private InfoCanvas infoCanvas;
        [SerializeField] private RewardCanvas rewardCanvas;
        [SerializeField] private InvenCanvas invenCanvas;

        // 페이드용
        [Header("Fader")]
        [SerializeField] private CanvasGroup fader;
        [SerializeField] private float fadeSpeed = 1f;

        public BattleCanvas BattleCanvas => battleCanvas;
        public InfoCanvas InfoCanvas => infoCanvas;
        public RewardCanvas RewardCanvas => rewardCanvas;
        public InvenCanvas InvenCanvas => invenCanvas;

        // ─────────────────────────
        // 창 보여주기
        // ─────────────────────────
        public void OpenInventory(List<CardData> cardList, string title)
        {
            SetCanvas(InvenCanvas, true, true);
            InvenCanvas.ChangeTitle(title);
            InvenCanvas.SetCards(cardList);
        }

        // ─────────────────────────
        // 캔버스 설정
        // ─────────────────────────
        public void SetCanvas(CanvasBase targetCanvas, bool open, bool reset = false)
        {
            if (reset)
                targetCanvas.ResetCanvas();

            if (open)
                targetCanvas.OpenCanvas();
            else
                targetCanvas.CloseCanvas();
        }

        // ─────────────────────────
        // 장면 전환
        // ─────────────────────────
        public void ChangeScene(int iIndex)
        {
            StartCoroutine(ChangeSceneRoutine(iIndex));
        }

        // ─────────────────────────
        // Scene 불러오기
        // ─────────────────────────
        private IEnumerator ChangeSceneRoutine(int iIndex)
        {
            SceneManager.LoadScene(iIndex);
            yield return StartCoroutine(FadeInOut(false));
        }

        // ─────────────────────────
        // 페이드 인아웃
        // true = 인
        // false = 아웃
        // ─────────────────────────
        public IEnumerator FadeInOut(bool bOn)
        {
            var LimitFrame = new WaitForEndOfFrame();
            float fTime = bOn ? 0f : 1f;

            while (true)
            {
                fTime += Time.deltaTime * (bOn ? fadeSpeed * 1 : fadeSpeed * -1);
                fader.alpha = fTime;

                if (fTime >= 1f)
                {
                    break;
                }

                yield return LimitFrame;
            }
        }
    }

}