using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : Singleton<MouseManager>
{
    [SerializeField] private Texture2D[] cursorTextureArray;
    [SerializeField] private int currentFrame;
    [SerializeField] private float frameRate;
    [SerializeField] private Sprite[] sprites;

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
        currentFrame = 0;
        frameRate = 0.1f;
        frameCount = sprites.Length;
        Cursor.SetCursor(cursorTextureArray[0], Vector2.zero, CursorMode.Auto);
    }

    //https://www.youtube.com/watch?v=8Fm37H1Mwxw

    private void Update()
    {
        SetCursorImage();
    }

    private void ResetTexture()
    {
        cursorTextureArray = new Texture2D[sprites.Length]; 

        for (int i = 0; i < cursorTextureArray.Length; i++)
        {
            cursorTextureArray[i] = TextureFromSprite(sprites[i]);
        }
    }

    private void SetCursorImage()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorTextureArray[currentFrame], Vector2.zero, CursorMode.Auto);
        }
    }

    public static Texture2D TextureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                            (int)sprite.textureRect.y,
                                                            (int)sprite.textureRect.width,
                                                            (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
        {
            return sprite.texture;
        }
    }
}
