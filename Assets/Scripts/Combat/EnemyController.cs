using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Objects from grid
    private GameObject grid;
    private MainArray array;

    private GameObject player;

    // Range of spawn delay
    public float moveTimeMin;
    public float moveTimeMax;

    // Grid spaces to move per movement
    public int moveAmount;
    // Amount of characters to kill
    public int charLimit;
    // Amount of damage player will take
    public int attack;

    private void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid");
        array = grid.GetComponent<MainArray>();
        player = array.player;

        StartCoroutine("MoveTimer");
    }

    private void Update()
    {
        if (charLimit <= 0)
        {
            player.GetComponent<Combat>().getEnemy();
            array.totalEnemies--;
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        int positionX = GetComponent<CurrentPosition>().positionX;
        int positionY = GetComponent<CurrentPosition>().positionY;

        if (positionX > 5 || positionX < 22 || positionY > 10 || positionY < 17)
        {
            player.GetComponent<Combat>().getEnemy(this.gameObject);
        }       
    }

    /// <summary>
    /// Handles direction of movement and also delay between moves
    /// </summary>
    private IEnumerator MoveTimer()
    {
        // Finds distance from player on both axes
        int xDiff = player.GetComponent<CurrentPosition>().positionX - GetComponent<CurrentPosition>().positionX;
        int yDiff = player.GetComponent<CurrentPosition>().positionY - GetComponent<CurrentPosition>().positionY;

        // Checks if within 1 move of the player
        if ((xDiff > moveAmount || xDiff < -moveAmount) || (yDiff > moveAmount || yDiff < -moveAmount))
        {
            // Further away in the X
            if (Mathf.Abs(xDiff) > Mathf.Abs(yDiff))
            {
                // Moves with a bias to getting closer on X
                if (Random.value > 0.25f)
                {
                    // Checks which direction to move on axis

                    // Needs to move right
                    if (xDiff > 0)
                    {
                        array.Move(moveAmount, 0, gameObject);
                    }
                    // Needs to move left
                    else
                    {
                        array.Move(-moveAmount, 0, gameObject);
                    }
                }
                else
                {
                    // Checks which direction to move on axis

                    // Needs to move down
                    if (yDiff > 0)
                    {
                        array.Move(0, moveAmount, gameObject);
                    }
                    // Needs to move up
                    else
                    {
                        array.Move(0, -moveAmount, gameObject);
                    }
                }
            }

            // Further away in the Y
            else if (Mathf.Abs(xDiff) < Mathf.Abs(yDiff))
            {
                // Moves with a bias to getting closer on Y
                if (Random.value > 0.25f)
                {
                    // Checks which direction to move on axis

                    // Needs to move down
                    if (yDiff > 0)
                    {
                        array.Move(0, moveAmount, gameObject);
                    }
                    // Needs to move up
                    else
                    {
                        array.Move(0, -moveAmount, gameObject);
                    }
                }
                else
                {
                    // Checks which direction to move on axis

                    // Needs to move right
                    if (xDiff > 0)
                    {
                        array.Move(moveAmount, 0, gameObject);
                    }
                    // Needs to move left
                    else
                    {
                        array.Move(-moveAmount, 0, gameObject);
                    }
                }
            }

            // Equidistant in X and Y (Random chance of either)
            else
            {
                // Creates 50/50 chance of evaluating true
                if (Random.value > .5f)
                {
                    // 3/4 chance of evaluating true
                    if (Random.value > 0.25f)
                    {
                        // Checks which direction to move on axis

                        // Needs to move right
                        if (xDiff > 0)
                        {
                            array.Move(moveAmount, 0, gameObject);
                        }
                        // Needs to move left
                        else
                        {
                            array.Move(-moveAmount, 0, gameObject);
                        }
                    }
                    else
                    {
                        // Checks which direction to move on axis

                        // Needs to move down
                        if (yDiff > 0)
                        {
                            array.Move(0, moveAmount, gameObject);
                        }
                        // Needs to move up
                        else
                        {
                            array.Move(0, -moveAmount, gameObject);
                        }
                    }
                }
                else
                {
                    if (Random.value > 0.25f)
                    {
                        // Checks which direction to move on axis

                        // Needs to move down
                        if (yDiff > 0)
                        {
                            array.Move(0, moveAmount, gameObject);
                        }
                        // Needs to move up
                        else
                        {
                            array.Move(0, -moveAmount, gameObject);
                        }
                    }
                    else
                    {
                        // Checks which direction to move on axis

                        // Needs to move left
                        if (xDiff > 0)
                        {
                            array.Move(moveAmount, 0, gameObject);
                        }
                        // Needs to move right
                        else
                        {
                            array.Move(-moveAmount, 0, gameObject);
                        }
                    }
                }
            }
        }
        else
        {
            Attack(xDiff, yDiff);
        }

        float moveWaitTime = Random.Range(moveTimeMin, moveTimeMax);
        yield return new WaitForSeconds(moveWaitTime);
        StartCoroutine("MoveTimer");
    }

    // Handles attacking
    private void Attack(int xDiff, int yDiff)
    {
        float xMove = xDiff;
        xMove = xMove / 2;
        float yMove = yDiff;
        yMove = yMove / 2;


        StartCoroutine(AttackMove(xMove, yMove));
    }

    // IEnumerator for waiting
    private IEnumerator AttackMove(float xMove, float yMove)
    {
        transform.position = new Vector2(transform.position.x + xMove, transform.position.y + yMove);
        player.GetComponent<Combat>().TakeDamage(attack);
        yield return new WaitForSeconds(moveTimeMin / 2);
        transform.position = new Vector2(transform.position.x - xMove, transform.position.y - yMove);
    }
}
