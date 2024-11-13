using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSprites : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Sprite MainSprite;
    [SerializeField] private int currentFrame;
    [SerializeField] private float frameRate;
    [SerializeField] private Sprite[] sprites;

    private float frameTimer;
    private int frameCount;

    private void Start()
    {
        currentFrame = 0;
        MainSprite = sprites[currentFrame];
        frameRate = 0.1f;
        frameCount = sprites.Length;
    }

    private void Update()
    {
        SetImage();
    }

    private void SetImage()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            ResetSprite();
        }
    }

    private void ResetSprite()
    {
        MainSprite = sprites[currentFrame];
    }
}
