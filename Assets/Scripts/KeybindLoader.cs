using UnityEngine;

public class KeybindLoader : MonoBehaviour
{
    private void Awake()
    {
        CombatKeybindManager.LoadKeybinds();
    }
}
