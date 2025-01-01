using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class PackManager : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        // Alternate the colours of the packs
        for (int i = 0; i < gameManager.packs.Length; i++)
        {
            gameManager.packs[i].color = gameManager.packColours[i % gameManager.packColours.Length];
        }

        gameManager.packs = gameManager.content.GetComponentsInChildren<Image>();
    }
}
