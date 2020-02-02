using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    Vector2 startPos;
    public Vector2 endPos;
    Transform text;
    float time;

    void Start()
    {
        text = this.transform;
        startPos = text.position;
        time = 0;
    }

    void Update()
    {
        time += 0.1f;

        text.position = Vector2.Lerp(startPos, endPos, time);

        if (new Vector2(text.position.x, text.position.y) == endPos)
        {
            Destroy(this.gameObject);
        }
    }
}
