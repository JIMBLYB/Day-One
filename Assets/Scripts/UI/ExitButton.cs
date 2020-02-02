using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void OnMouseDown()
    {
        if (transform.parent.Find("Grid(Clone)") != null)
        {
            Destroy(transform.parent.Find("Grid(Clone)").gameObject);
        }
        transform.parent.gameObject.SetActive(false);
    }
}
