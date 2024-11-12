using chataan.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ━━━━━━━━━━━━━━━━━━━━━━━━━━
// 장면 전환 클래스
// ━━━━━━━━━━━━━━━━━━━━━━━━━━
public class SetScene : MonoBehaviour
{
    private enum SceneType
    {
        Menu,
        Map,
        Battle,
    }

    [SerializeField] UIManager UIManager  => UIManager.Instance;
    [SerializeField] CoreManager CoreManager => CoreManager.Instance;

    // 장면 전환
    private IEnumerator ChangeScene(SceneType type)
    {
        UIManager.SetCanvas(UIManager.Instance.InvenCanvas, false, true);
        yield return StartCoroutine(UIManager.Instance.FadeInOut(true));

        switch (type)
        {
            case SceneType.Menu:
                UIManager.ChangeScene(CoreManager.SceneData.mainMenuSceneIndex);
                CoreManager.InitPlayData();
                break;

            case SceneType.Map:
                //UIManager.ChangeScene(GameManager.SceneData.mapSceneIndex);
                break;

            case SceneType.Battle:
                UIManager.ChangeScene(CoreManager.SceneData.combatSceneIndex);
                break;

            default:
                break;

                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    // 메뉴로
    public void OpenMenuPhase()
    {
        StartCoroutine(ChangeScene(SceneType.Menu));
    }

    // 자리
    public void OpenMapPhase()
    {
        StartCoroutine(ChangeScene(SceneType.Map));
    }

    // 전투
    public void OpenBattlePhase()
    {
        StartCoroutine(ChangeScene(SceneType.Battle));
    }

    //게임 종료
    public void ExitGame()
    {
        CoreManager.ExitGame();
    }
}
