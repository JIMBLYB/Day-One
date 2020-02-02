using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainArray : MonoBehaviour
{
    // Prefab objects
    public GameObject gridMarkerPrefab;
    public GameObject playerPrefab;
    // Enemy prefabs
    public GameObject tickPrefab;
    public GameObject flyPrefab;
    public GameObject spiderPrefab;
    public GameObject antPrefab;

    // Enemies left to spawn
    public List<GameObject> enemiesLeft = new List<GameObject>();

    // How many of each enemy to spawn
    public int ticksToSpawn;
    public int fliesToSpawn;
    public int spidersToSpawn;
    public int antsToSpawn;

    public int totalEnemies;
    public bool allPatched;

    // Size of grid
    // KEEP ODD
    private int gridSizeX = 27;
    private int gridSizeY = 27;

    // Offsets to keep grid centred
    private int gridOffsetX;
    private int gridOffsetY;

    // 2D Array for placement
    public int[,] gridPositions;

    // Enemy spawn delay range values
    public float spawnDelayMin;
    public float spawnDelayMax;

    // UI canvas
    public GameObject canvas;
    // Player Object
    public GameObject player;
    // Holds most recently instantiated grid marker
    private GameObject newGridMarker;
    // Level instantiated from
    public GameObject levelSelector;
    // Level complete
    public GameObject done;

    /// <summary>
    /// Instantiates grid and player
    /// </summary>
    public void Start()
    {
        allPatched = false;
        done = transform.parent.Find("Done").gameObject;
        done.SetActive(false);

        ticksToSpawn = levelSelector.GetComponent<SelectLevel>().ticksToSpawn;
        fliesToSpawn = levelSelector.GetComponent<SelectLevel>().fliesToSpawn;
        spidersToSpawn = levelSelector.GetComponent<SelectLevel>().spidersToSpawn;
        antsToSpawn = levelSelector.GetComponent<SelectLevel>().antsToSpawn;

        while (ticksToSpawn > 0)
        {
            enemiesLeft.Add(tickPrefab);
            ticksToSpawn--;
            totalEnemies++;
        }
        while (fliesToSpawn > 0)
        {
            enemiesLeft.Add(flyPrefab);
            fliesToSpawn--;
            totalEnemies++;
        }
        while (spidersToSpawn > 0)
        {
            enemiesLeft.Add(spiderPrefab);
            spidersToSpawn--;
            totalEnemies++;
        }
        while (antsToSpawn > 0)
        {
            enemiesLeft.Add(antPrefab);
            antsToSpawn--;
            totalEnemies++;
        }

        // Sets array size
        gridPositions = new int[gridSizeX, gridSizeY];

        // Finds midpoint of array
        gridOffsetX = (gridSizeX - 1) / 2;
        gridOffsetY = (gridSizeY - 1) / 2;

        // Instantiates full grid of markers
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                // Instantiates new grid marker, sets it's name to it's co-ordinates on gridPositions variable
                newGridMarker = Instantiate(gridMarkerPrefab, new Vector2(i - gridOffsetX, j - gridOffsetY), Quaternion.identity, gameObject.transform);
                newGridMarker.name = (i.ToString() + ", " + j.ToString());

                // Sets current position to starting position
                newGridMarker.GetComponent<CurrentPosition>().positionX = i;
                newGridMarker.GetComponent<CurrentPosition>().positionY = j;
            }
        }

        // Instantiates player in the centre of the grid
        player = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity, gameObject.transform.Find(gridOffsetX.ToString() + ", " + gridOffsetY.ToString()));
        player.GetComponent<CurrentPosition>().positionX = gridOffsetX;
        player.GetComponent<CurrentPosition>().positionY = gridOffsetY;

        transform.position += new Vector3(transform.parent.transform.position.x,
                                          transform.parent.transform.position.y, 0);

        // Begins spawning enemies
        StartCoroutine("EnemySpawn");
    }

    /// <summary>
    /// Handles keyboard events for movement
    /// </summary>
    public void Update()
    {
        // Checks inputs while not in combat for movement keys
        if (player.GetComponent<Combat>().inCombat == false)
        {
            // Parses correct ints to PlayerMove to allow player movement across the board (Clamped to visible grid)
            if (Input.GetKeyDown(KeyCode.W) && player.transform.parent.GetComponent<CurrentPosition>().positionY < 16)
            {
                // UP
                Move(0, 1, player);
            }
            if (Input.GetKeyDown(KeyCode.A) && player.transform.parent.GetComponent<CurrentPosition>().positionX > 6)
            {
                // LEFT
                Move(-1, 0, player);
            }
            if (Input.GetKeyDown(KeyCode.S) && player.transform.parent.GetComponent<CurrentPosition>().positionY > 10)
            {
                // DOWN
                Move(0, -1, player);
            }
            if (Input.GetKeyDown(KeyCode.D) && player.transform.parent.GetComponent<CurrentPosition>().positionX < 20)
            {
                // RIGHT
                Move(1, 0, player);
            }
        }

        if (totalEnemies <= 0)
        {
            done.SetActive(true);
            done.GetComponent<NextLevelConfirm>().levelSelect = levelSelector;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Coroutine for spawning enemies
    /// </summary>
    public IEnumerator EnemySpawn()
    {
        if (enemiesLeft.Count > 0)
        {
            int indexToInstantiate = Random.Range(0, enemiesLeft.Count);

            // Finds random X pos to spawn at (outside visible grid spaces)
            int spawnXPos = 13;
            while (spawnXPos >= 5 && spawnXPos <= 21)
            {
                spawnXPos = Random.Range(1, 26);
            }

            // Finds random Y pos to spawn at (outside visible grid spaces)
            int spawnYPos = 13;
            while (spawnYPos >= 9 && spawnYPos <= 17)
            {
                spawnYPos = Random.Range(1, 26);
            }

            // Finds parent at index on grid
            GameObject newParent = GameObject.Find(spawnXPos.ToString() + ", " + spawnYPos.ToString());

            // Checks if parent of instantiation already has a child
            if (newParent.transform.childCount == 0)
            {
                // Instantiates enemy and removes it from the list
                GameObject newEnemy = Instantiate(enemiesLeft[indexToInstantiate], newParent.transform);
                enemiesLeft.RemoveAt(indexToInstantiate);

                newEnemy.GetComponent<CurrentPosition>().positionX = spawnXPos;
                newEnemy.GetComponent<CurrentPosition>().positionY = spawnYPos;

                // Randomises (within range) the delay between spawning
                float spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
                yield return new WaitForSeconds(spawnDelay);
                StartCoroutine("EnemySpawn");
            }
            // If there is a child already, doesn't instantiate new enemy and restarts the coroutine
            else
            {
                yield return null;
                StartCoroutine("EnemySpawn");
            }
        }
        else
        {
            yield return null;
        }
       
    }

    /// <summary>
    /// Moves the parsed object around the grid
    /// </summary>
    /// <param name="x">Translation across the X axis (- = left, + = right)</param>
    /// <param name="y">Translation across the Y axis (- = down, + = up)</param>
    /// <param name="toMove">Object to perform translation on</param>
    public void Move(int x, int y, GameObject toMove)
    {
        // Finds new co-ordinates on grid array
        int newX = toMove.transform.parent.GetComponent<CurrentPosition>().positionX + x;
        int newY = toMove.transform.parent.GetComponent<CurrentPosition>().positionY + y;

        // Finds new parent grid marker
        GameObject newParent = GameObject.Find(newX.ToString() + ", " + newY.ToString());

        // If new parent has no existing children then move the object
        if (newParent != null && newParent.transform.childCount == 0)
        {
            // Reparents object to new array position
            toMove.transform.parent = newParent.transform;

            // Resets transform
            toMove.transform.localPosition = Vector2.zero;

            // Corrects Current Position
            toMove.GetComponent<CurrentPosition>().positionX = newX;
            toMove.GetComponent<CurrentPosition>().positionY = newY;

            // Calls next frame of animation
            toMove.GetComponent<Animation>().MoveAnim();
        }
    }
}
