using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    Transform window;
    Vector2 lastPos;
    public bool drag;
    public bool front;
    public GameObject[] levelWindow;
    public Canvas levelCanvas;
    public GameObject gameWindow;

    void Start()
    {
        window = this.transform;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (lastPos != new Vector2(0, 0) && drag && front)
            {
                Vector2 thisPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 posDiff = lastPos - thisPos;

                window.position = new Vector3(window.position.x - posDiff.x,
                                              window.position.y - posDiff.y,
                                              window.position.z);
            }

            lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        else
        {
            lastPos = new Vector2(0, 0);
            drag = false;
        }

        Vector2 mouseView = new Vector2(Camera.main.ScreenToViewportPoint(Input.mousePosition).x,
                                        Camera.main.ScreenToViewportPoint(Input.mousePosition).y);

        if (drag && front)
        {
            if (mouseView.x > 0.9)
            {
                window.position = new Vector3(5, window.position.y, window.position.z);
            }

            else if (mouseView.y > 0.9)
            {
                window.position = new Vector3(window.position.x, 3, window.position.z);
            }

            else if (mouseView.x < 0.1)
            {
                window.position = new Vector3(-5, window.position.y, window.position.z);
            }

            else if (mouseView.y < 0.5)
            {
                window.position = new Vector3(window.position.x, -3, window.position.z);
            }
        }
    }

    void OnMouseOver()
    {
        drag = true;

        if (Input.GetMouseButtonDown(0))
        {
            if (window.gameObject.name == "Game Window" && levelWindow[0].GetComponent<SpriteRenderer>().sortingOrder == 21)
            {
                for (int i = 0; i < levelWindow.Length; i++)
                {
                    levelWindow[i].GetComponent<SpriteRenderer>().sortingOrder -= 20;                   
                }

                levelCanvas.sortingOrder -= 20;

                gameWindow.GetComponent<WindowController>().front = true;
                levelWindow[0].GetComponent<WindowController>().front = false;
            }

            else if (window.GetComponent<SpriteRenderer>().sortingOrder == 1)
            {
                for (int i = 0; i < levelWindow.Length; i++)
                {
                    levelWindow[i].GetComponent<SpriteRenderer>().sortingOrder += 20;
                }

                levelCanvas.sortingOrder += 20;

                gameWindow.GetComponent<WindowController>().front = false;
                levelWindow[0].GetComponent<WindowController>().front = true;
            }
        }
    }
}
