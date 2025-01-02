using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class PlayersWindow : MonoBehaviour
{
    public GameManager gameManager;
    public Slider turnTimeSlider;
    public Slider roundsSlider;
    public TextMeshProUGUI turnTimeText;
    public TextMeshProUGUI roundsText;

    void Update()
    {
        // Turns
        turnTimeText.text = (int)(turnTimeSlider.value / 6) + ":" + (turnTimeSlider.value % 6).ToString() + "0";
        gameManager.turnTime = turnTimeSlider.value * 10;

        // Rounds
        roundsText.text = roundsSlider.value.ToString();
        gameManager.totalRounds = (int)roundsSlider.value;
    }
}
