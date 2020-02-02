using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamManagement : MonoBehaviour
{
    public GameObject player;

    public void SetHealth(float scale)
    {
        transform.GetChild(0).localScale = new Vector3(scale, 1, 1);
    }
}
