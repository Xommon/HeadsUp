using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;
using UnityEditor;

[ExecuteAlways]
public class Pack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private TextMeshProUGUI textDisplay;
    private bool isPointerDown = false;
    private float pointerDownTime = 0f;
    private float longPressThreshold = 0.75f; // Time in seconds for long press detection
    public string category;
    [TextArea(10, 20)]
    public string items;
    public float fontSize = 24;
    private GameScreen gameScreen;
    private GameManager gameManager;

    void Update()
    {
        // Find Game Manager
        gameManager = gameManager == null ? FindObjectOfType<GameManager>() : gameManager;

        // Update text
        textDisplay = textDisplay == null ? GetComponentInChildren<TextMeshProUGUI>() : textDisplay;
        gameObject.name = category == "" ? "Pack" : "Pack: " + category;
        textDisplay.text = category;
        textDisplay.fontSize = fontSize;

        // Check for long press
        if (isPointerDown)
        {
            pointerDownTime += Time.deltaTime;
            if (pointerDownTime >= longPressThreshold)
            {
                // Start a new game
                AudioManager.Play("tap");
                SetLandscape();
                gameScreen = gameManager.gameScreen;
                gameScreen.gameObject.SetActive(true);
                gameScreen.packItems = items;
                gameScreen.items = items.Split(",").ToList();
                gameScreen.NewGame();

                isPointerDown = false;  // Reset after detecting long press
            }
        }
    }

    public void SetLandscape()
    {
        //Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        pointerDownTime = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        /*if (pointerDownTime < longPressThreshold)
        {
            Debug.Log("Click detected on: " + gameObject.name);
        }*/
    }
}
