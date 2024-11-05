using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : Singleton<MouseManager>
{
    [SerializeField] private Texture2D[] cursorTextureArray;
    [SerializeField] private int currentFrame;
    [SerializeField] private float frameRate;
    [SerializeField] private Sprite[] sprites = Resources.LoadAll<Sprite>("Resources/UI/Cursor");

    private float frameTimer;
    private int frameCount;

    public enum CursorType
    { 
        normal = 0,
        check = 1,
    }

    private void Start()
    {
        
        ResetTexture();
        Cursor.SetCursor(cursorTextureArray[0], Vector2.zero, CursorMode.Auto);
    }

    //https://www.youtube.com/watch?v=8Fm37H1Mwxw

    private void Update()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorTextureArray[currentFrame], Vector2.zero, CursorMode.Auto);
        }
    }

    private void ResetTexture()
    {
        Debug.Log(sprites);
    }
}
