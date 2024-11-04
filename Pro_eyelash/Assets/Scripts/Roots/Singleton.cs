using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        private set { }
        get { return instance; }
    }

    protected virtual private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            transform.parent = null;
            Instance = (T)FindObjectOfType(typeof(T));
            DontDestroyOnLoad(gameObject);
        }
    }
}