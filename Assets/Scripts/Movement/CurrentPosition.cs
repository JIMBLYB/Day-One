using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPosition : MonoBehaviour
{
    public int positionX;
    public int positionY;

    private void Update()
    {
        if (positionX <= 5 || positionX >= 21 || positionY <= 9 || positionY >= 17)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (positionX > 5 || positionX < 21 || positionY > 10 || positionY < 17)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
