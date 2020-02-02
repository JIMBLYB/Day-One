using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    private int nextFrame;
    public Sprite[] moveFrames;

    public Sprite idleFrame;

    void Awake()
    {
        nextFrame = 0;
    }

    public void MoveAnim()
    {
        GetComponent<SpriteRenderer>().sprite = moveFrames[nextFrame];
        nextFrame++;
        if (nextFrame >= moveFrames.Length)
        {
            nextFrame = 0;
        }
    }
}
