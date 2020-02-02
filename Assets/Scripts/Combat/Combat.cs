using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    public bool inCombat;
    Transform target;
    public GameObject letter;
    public Canvas canvas;
    Transform player;
    public float ram;
    public GameObject ramStick;

    public GameObject gameWindow;
    public GameObject levelWindow;
    public GameObject blueScreen;

    void Start()
    {
        player = this.transform;
        canvas = GameObject.Find("TextCanvas").GetComponent<Canvas>();
        ramStick = GameObject.FindGameObjectWithTag("Ram");
        ramStick.GetComponent<RamManagement>().player = gameObject;

        gameWindow = GameObject.Find("Game Window");
        levelWindow = GameObject.Find("Level Window");
    }

    void Update()
    {
        if (inCombat)
        {
            if (Input.anyKeyDown)
            {
                string keyPressed = Input.inputString;
                if (target.gameObject.GetComponent<EnemyController>() != null)
                {
                    target.gameObject.GetComponent<EnemyController>().charLimit -= keyPressed.Length;
                }
                else
                {
                    target.gameObject.GetComponent<AntEnemyController>().charLimit -= keyPressed.Length;
                }
                    

                GameObject newLetter = Instantiate(letter, player.position, player.rotation, canvas.transform);
                newLetter.GetComponent<TMPro.TextMeshProUGUI>().text = keyPressed;
                newLetter.GetComponent<TextController>().endPos = target.position;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        ram -= damage * 0.25f;
        if (ram <= 0)
        {
            ram = 0;
        }
        ramStick.GetComponent<RamManagement>().SetHealth(ram);
        if (ram <= 0)
        {
            ram = 1;
            ramStick.GetComponent<RamManagement>().SetHealth(ram);
            Instantiate(blueScreen, new Vector2(0, 0), Quaternion.identity);

            Destroy(gameWindow.transform.Find("Grid(Clone)").gameObject);
            levelWindow.SetActive(false);
            gameWindow.SetActive(false);           
        }
    }

    public void getEnemy(GameObject enemy)
    {
        if (!inCombat)
        {
            inCombat = true;
        }

        target = enemy.transform;
    }

    public void getEnemy()
    {
        inCombat = false;
    }
}
