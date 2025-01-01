using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public float duration = 3f;  // Time in seconds for the lerp
    private RectTransform titleRect;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float startFontSize = 100f;
    private float endFontSize = 48f;
    private float elapsedTime = 0f;
    private bool moveTitle = false;
    public GameObject content;
    public GameObject scrollView;
    [HideInInspector]
    public Image[] packs;
    public Color[] packColours;
    public GameObject playersWindow;
    public Button playersButton;
    public TMP_InputField[] playerNames;
    public int totalRounds;
    public float roundTime;

    void Start()
    {
        // Get the RectTransform component
        titleRect = titleText.GetComponent<RectTransform>();

        // Set initial positions
        startPosition = titleRect.anchoredPosition;
        endPosition = new Vector3(0, Screen.height / 2 - 50, 0);  // Moves to top with padding

        // Disable content at the start
        scrollView.SetActive(false);
        titleText.gameObject.SetActive(true);

        // Start coroutine to delay the movement
        StartCoroutine(MoveTitle());
    }

    void Update()
    {
        if (moveTitle)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Lerp position
            titleRect.anchoredPosition = Vector3.Lerp(startPosition, endPosition, t);

            // Lerp font size
            titleText.fontSize = Mathf.Lerp(startFontSize, endFontSize, t);

            // Stop lerping and enable content after completion
            if (t >= 1f)
            {
                moveTitle = false;

                // Enable content once the animation is done
                if (scrollView != null)
                {
                    scrollView.SetActive(true);
                }
            }
        }
    }

    IEnumerator MoveTitle()
    {
        yield return new WaitForSeconds(2.0f);
        moveTitle = true;
        elapsedTime = 0f;  // Reset elapsed time to start lerp fresh
    }

    public void OpenPlayers()
    {
        playersWindow.SetActive(!playersWindow.activeInHierarchy);
    }
}
