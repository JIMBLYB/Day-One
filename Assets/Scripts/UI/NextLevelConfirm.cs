using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelConfirm : MonoBehaviour
{
    public GameObject levelSelect;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            levelSelect.GetComponent<SelectLevel>().ActivateNext();
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
