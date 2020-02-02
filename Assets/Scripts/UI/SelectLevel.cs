using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevel : MonoBehaviour
{
    public GameObject gridPrefab;

    public GameObject blueScreen;
    public GameObject gameWindow;
    private GameObject grid;

    public GameObject Text;
    public GameObject nextLevel;
    public bool active;

    // How many of each enemy to spawn
    public int ticksToSpawn;
    public int fliesToSpawn;
    public int spidersToSpawn;
    public int antsToSpawn;

    private bool doubleClick;
    private float clickTimer;

    private bool createNew;
    private float createDelay;

    public void Start()
    {
        doubleClick = false;
        clickTimer = 0.2f;
    }

    public void Update()
    {    
        if (active)
        {
            BecomeActive();
        }

        if (createNew == true && createDelay < 0)
        {
            gameWindow.transform.position = new Vector3(transform.parent.parent.transform.position.x + 1,
                                                    transform.parent.parent.transform.position.y + 1, 0);

            grid = Instantiate(gridPrefab, gameWindow.transform);
            grid.GetComponent<MainArray>().levelSelector = gameObject;


            gameWindow.SetActive(true);

            createNew = false;
        }

        createDelay -= Time.deltaTime;
    }

    public void BeginLevel()
    {
        if (gameWindow.activeSelf == true)
        {
            Destroy(gameWindow.transform.Find("Grid(Clone)").gameObject);
            gameWindow.SetActive(true);
        }

        createNew = true;
        createDelay = 0.5f;              
    }

    private void BecomeActive()
    {
        this.GetComponent<BoxCollider2D>().enabled = true;
        Text.GetComponent<TMPro.TextMeshProUGUI>().text = this.gameObject.name;
    }

    public void ActivateNext()
    {
        if (nextLevel != null)
        {
            nextLevel.GetComponent<SelectLevel>().active = true;
        }
    }

    void OnMouseOver()
    {
        clickTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            if (doubleClick)
            {
                BeginLevel();
            }

            doubleClick = true;
            clickTimer = 0.2f;
        }

        if (clickTimer <= 0)
        {
            doubleClick = false;
        }
    }
}
