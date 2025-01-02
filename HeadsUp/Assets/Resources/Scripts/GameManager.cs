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
    public float turnTime;

    void Start()
    {
        // Disable content at the start
        scrollView.SetActive(false);
        titleText.gameObject.SetActive(true);
        StartCoroutine(MoveTitle());
    }

    IEnumerator MoveTitle()
    {
        yield return new WaitForSeconds(3.0f);
        scrollView.SetActive(true);
    }

    public void OpenPlayersWindow()
    {
        playersWindow.SetActive(!playersWindow.activeInHierarchy);
    }
}
