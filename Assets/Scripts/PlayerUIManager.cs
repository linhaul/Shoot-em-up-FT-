using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject lifeIconPref;
    public Transform lifeDisplay;
    private List<GameObject> currentIcons = new List<GameObject>();
    public TextMeshProUGUI shotTypeText;

    public void UpdateLifes(int currentLifes)
    {
        foreach (GameObject icon in currentIcons)
        {
            Destroy(icon);
        }
        currentIcons.Clear();

        for (int i = 0; i < currentLifes; i++)
        {
            GameObject icon = Instantiate(lifeIconPref, lifeDisplay);
            currentIcons.Add(icon);
        }
    }

    public void UpdateShotType(string type)
    {
        shotTypeText.text = $"Shot: {type}";
    }
}
