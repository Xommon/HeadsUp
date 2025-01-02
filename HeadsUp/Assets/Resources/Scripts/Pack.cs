using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

[ExecuteAlways]
public class Pack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private TextMeshProUGUI text;
    private bool isPointerDown = false;
    private float pointerDownTime = 0f;
    private float longPressThreshold = 0.75f; // Time in seconds for long press detection
    [TextArea(10, 20)]
    public string items;
    public GameScreen gameScreen;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        gameObject.name = "Pack: " + text.text;

        // Check for long press
        if (isPointerDown)
        {
            pointerDownTime += Time.deltaTime;
            if (pointerDownTime >= longPressThreshold)
            {
                // Start a new game
                gameScreen.gameObject.SetActive(true);
                gameScreen.packItems = items;
                gameScreen.items = items.Split(",").ToList();
                gameScreen.NewGame();

                isPointerDown = false;  // Reset after detecting long press
            }
        }
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
        if (pointerDownTime < longPressThreshold)
        {
            Debug.Log("Click detected on: " + gameObject.name);
        }
    }
}
