using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ━━━━━━━━━━━━━━━━━━━━━━━━━━
// 게임 시작 클래스
// 이곳에서 시작해서 불러옴
// ━━━━━━━━━━━━━━━━━━━━━━━━━━

[DefaultExecutionOrder(-11)]
public class StartPhase : MonoBehaviour
{
    void Awake()
    {
        try
        {
            //
            if (!CoreManager.Instance)
            {
                SceneManager.LoadScene("Phase", LoadSceneMode.Additive);
            }

            // 해당 객체 파괴
            Destroy(gameObject);
        }
        catch (Exception exception)
        {
            Debug.LogError($"먼가 잘못됐음ㅋ : {exception}");
            throw;
        }
    }
}
