using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    GameObject levelSelect;
    bool doubleClick;
    float clickTimer;
    public GameObject levelWindow;
    bool draging;

    void Start()
    {
        levelSelect = this.gameObject;
    }

    void Update()
    {
        if (draging && Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            levelSelect.transform.position = mousePos;
        }



        if (Input.GetMouseButtonUp(0))
        {
            draging = false;
            fixPos();
        }
    }

    void OnMouseOver()
    {       
        clickTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            draging = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (doubleClick)
            {
                levelWindow.SetActive(true);
            }

            doubleClick = true;
            clickTimer = 0.2f;
        }

        if (clickTimer <= 0)
        {
            doubleClick = false;
        }
    }

    void fixPos()
    {
        Vector2 newPos = levelSelect.transform.position;
        newPos.x = Mathf.RoundToInt(newPos.x / 5) * 5;
        newPos.y = Mathf.RoundToInt(newPos.y / 5) * 5;

        if (newPos.x >= 15)
        {
            newPos.x = 10;
        }

        else if (newPos.x <= -15)
        {
            newPos.x = -10;
        }

        if (newPos.y >= 10)
        {
            newPos.y = 5;
        }

        if (newPos.y <= -10)
        {
            newPos.y = -5;
        }

        levelSelect.transform.position = newPos;
    }
}
