using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject lifeIconPref;
    public Transform lifeDisplay;
    private List<GameObject> currentLifeIcons = new List<GameObject>();
    public TextMeshProUGUI shotTypeText;
    public GameObject bombIconPref;
    public Transform bombDisplay;
    private List<GameObject> currentBombIcons = new List<GameObject>();

    public void UpdateLifes(int currentLifes)
    {
        foreach (GameObject icon in currentLifeIcons)
        {
            Destroy(icon);
        }
        currentLifeIcons.Clear();

        for (int i = 0; i < currentLifes; i++)
        {
            GameObject icon = Instantiate(lifeIconPref, lifeDisplay);
            currentLifeIcons.Add(icon);
        }
    }

    public void UpdateShotType(string type)
    {
        shotTypeText.text = $"Shot: {type}";
    }

    public void UpdateBombCount(int currentBombs)
    {
        foreach (GameObject icon in currentBombIcons)
        {
            Destroy(icon);
        }
        currentBombIcons.Clear();

        for (int i = 0; i < currentBombs; i++)
        {
            GameObject icon = Instantiate(bombIconPref, bombDisplay);
            currentBombIcons.Add(icon);
        }
    }
}
