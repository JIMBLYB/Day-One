using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueScreen : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            gameObject.SetActive(false);
        }
    }
}
