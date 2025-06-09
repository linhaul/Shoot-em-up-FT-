using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CombatKeybindUI : MonoBehaviour
{
    public Button shootButton;
    public Button shootTypeButton;
    public Button bombButton;
    public Button superMeterButton;

    private Button waitingForKeyButton = null;
    private string waitingForAction = "";

    void Start()
    {
        CombatKeybindManager.LoadKeybinds();

        shootButton.onClick.AddListener(() => StartRebind("Fire", shootButton));
        shootTypeButton.onClick.AddListener(() => StartRebind("SwitchFire", shootTypeButton));
        bombButton.onClick.AddListener(() => StartRebind("Bomb", bombButton));
        superMeterButton.onClick.AddListener(() => StartRebind("Super", superMeterButton));

        UpdateButtonLabels();
    }

    void Update()
    {
        if (waitingForKeyButton != null && !string.IsNullOrEmpty(waitingForAction))
        {
            foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    CombatKeybindManager.SetKey(waitingForAction, kcode);

                    waitingForKeyButton.GetComponentInChildren<TMPro.TMP_Text>().text = kcode.ToString();

                    waitingForKeyButton = null;
                    waitingForAction = "";
                    break;
                }
            }
        }
    }

    void StartRebind(string actionName, Button button)
    {
        waitingForKeyButton = button;
        waitingForAction = actionName;
        button.GetComponentInChildren<TMPro.TMP_Text>().text = "Press...";
    }

    void UpdateButtonLabels()
    {
        shootButton.GetComponentInChildren<TMPro.TMP_Text>().text = CombatKeybindManager.FireKey.ToString();
        shootTypeButton.GetComponentInChildren<TMPro.TMP_Text>().text = CombatKeybindManager.SwitchFireModeKey.ToString();
        bombButton.GetComponentInChildren<TMPro.TMP_Text>().text = CombatKeybindManager.BombKey.ToString();
        superMeterButton.GetComponentInChildren<TMPro.TMP_Text>().text = CombatKeybindManager.SuperKey.ToString();
    }
}
