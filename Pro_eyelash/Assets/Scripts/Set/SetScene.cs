using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // ��� ��ȯ
    private IEnumerator ChangeScene(SceneType type)
    {
       // UIManager.SetCanvas(UIManager.Instance.InventoryCanvas, false, true);
        yield return StartCoroutine(UIManager.Instance.FadeInOut(true));

        switch (type)
        {
            case SceneType.Menu:
                //UIManager.ChangeScene(GameManager.SceneData.mainMenuSceneIndex);
                CoreManager.InitAllData();
                break;

            case SceneType.Map:
                //UIManager.ChangeScene(GameManager.SceneData.mapSceneIndex);

                break;
            case SceneType.Battle:
                //UIManager.ChangeScene(GameManager.SceneData.combatSceneIndex);

                break;
            default:
                break;

                //throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    // ����
    public void OpenBattlePhase()
    {
        StartCoroutine(ChangeScene(SceneType.Menu));
    }

    // �޴�
    public void OpenMenuPhase()
    {
        StartCoroutine(ChangeScene(SceneType.Menu));
    }

    //���� ����
    public void ExitGame()
    {
        CoreManager.ExitGame();
        Application.Quit();
    }
}
