using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class GameScreen : MonoBehaviour
{
    public GameManager gameManager;
    public Dictionary<string, int> players = new Dictionary<string, int>();
    public List<string> items = new List<string>();
    [HideInInspector]
    public string packItems;
    private int itemIndex;
    public int turn;
    public int round;
    public TextMeshProUGUI beginText;
    public TextMeshProUGUI itemText;
    public Animator animator;
    public Image backgroundImage;

    private Vector2 startTouchPosition;
    private float touchStartTime;
    private bool isTouching = false;
    private bool longTapTriggered = false;
    private bool gameStarted;

    public float longTapThreshold = 0.5f;  // Seconds for long tap
    public float swipeThreshold = 50f;     // Minimum swipe distance

    void Update()
    {
        DetectTouchOrClick();
    }

    public void NewGame()
    {
        // Reset variables
        turn = 0;
        round = 0;
        animator.enabled = false;

        // Rebuild the players list
        players.Clear();
        foreach (TMP_InputField playerName in gameManager.playerNames)
        {
            if (playerName.text != "")
            {
                players.Add(playerName.text, 0);
            }
        }

        // Shuffle the order of the players
        players = players.OrderBy(x => Guid.NewGuid()).ToDictionary(x => x.Key, x => x.Value);

        // Begin with the first player
        beginText.text = players.Keys.ElementAt(0) + "\n<size=24>Long Tap to Begin";
    }

    private void DetectTouchOrClick()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isTouching = true;
                    longTapTriggered = false;
                    startTouchPosition = touch.position;
                    touchStartTime = Time.time;
                    break;

                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    // Detect long tap while holding
                    if (isTouching && !longTapTriggered)
                    {
                        if (Time.time - touchStartTime > longTapThreshold)
                        {
                            longTapTriggered = true;
                            HandleLongTap();
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    isTouching = false;
                    if (!longTapTriggered)
                    {
                        DetectSwipeOrTap(touch.position);
                    }
                    break;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            isTouching = true;
            longTapTriggered = false;
            startTouchPosition = Input.mousePosition;
            touchStartTime = Time.time;
        }
        else if (Input.GetMouseButton(0))
        {
            // Detect long tap with mouse while holding down
            if (isTouching && !longTapTriggered)
            {
                if (Time.time - touchStartTime > longTapThreshold)
                {
                    longTapTriggered = true;
                    HandleLongTap();
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && isTouching)
        {
            isTouching = false;
            if (!longTapTriggered)
            {
                DetectSwipeOrTap(Input.mousePosition);
            }
        }
    }

    private void DetectSwipeOrTap(Vector2 endPosition)
    {
        Vector2 swipeVector = endPosition - startTouchPosition;

        if (swipeVector.magnitude > swipeThreshold)
        {
            /*if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
            {
                if (swipeVector.x > 0)
                    Debug.Log("Swipe Right Detected");
                else
                    Debug.Log("Swipe Left Detected");
            }
            else
            {
                if (swipeVector.y > 0)
                    Debug.Log("Swipe Up Detected");
                else
                    Debug.Log("Swipe Down Detected");
            }*/
            HandleSwipe();
        }
        else
        {
            HandleTap();
        }
    }

    private void HandleLongTap()
    {
        // Start new turn
        if (beginText.text.Contains("Long Tap to Begin"))
        {
            StartCoroutine(Countdown(5));
            
        }
        else if (round > 0 && !gameStarted)
        {
            // Return to home screen
            gameObject.SetActive(false);
        }
    }

    private void HandleSwipe()
    {
        if (gameStarted)
        {
            // Pass
            NextItem();
        }
    }

    private void HandleTap()
    {
        if (gameStarted)
        {
            // Correct
            players[players.Keys.ElementAt(turn)]++;
            items.RemoveAt(itemIndex);
            NextItem();
        }
    }

    IEnumerator Countdown(int number)
    {
        beginText.text = "<size=50>Get Ready!\n<size=150>" + number.ToString();
        yield return new WaitForSeconds(1.0f);
        
        if (number > 1)
        {
            StartCoroutine(Countdown(number - 1));
        }
        else
        {
            // Start the round
            gameStarted = true;
            NextItem();
            StartCoroutine(RoundTimer(gameManager.turnTime));
        }
    }

    IEnumerator RoundTimer(float time)
    {
        animator.enabled = false;
        backgroundImage.color = new Color(0, 0.8573186f, 1);
        yield return new WaitForSeconds(time - 5);
        animator.enabled = true;
        animator.Play("colour_shift", -1, 0f);
        yield return new WaitForSeconds(5.0f);
        gameStarted = false;
        beginText.text = "Time's Up!";
        yield return new WaitForSeconds(5.0f);

        // Turn or round up
        turn++;
        if (turn < players.Count)
        {
            beginText.text = players.Keys.ElementAt(turn) + "\n<size=24>Long Tap to Begin";
        }
        else
        {
            round++;

            if (round == gameManager.totalRounds)
            {
                // End the game
                beginText.text = "Game Over!";
                StartCoroutine(EndGame());
            }
            else
            {
                turn = 0;
                beginText.text = players.Keys.ElementAt(turn) + "\n<size=24>Long Tap to Begin";
            }
        }
    }

    public void NextItem()
    {
        // Replenish the list if it's getting too low
        if (items.Count < 2)
        {
            items = packItems.Split(",").ToList();
        }

        itemIndex = UnityEngine.Random.Range(0, items.Count);
        beginText.text = items[itemIndex];
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3.0f);

        // Sort players by score
        players = players.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        // Display scores
        beginText.text = "<size=40>";

        foreach (KeyValuePair<string, int> entry in players)
        {
            beginText.text += $"{entry.Key}: {entry.Value}\n";
        }
    }
}
