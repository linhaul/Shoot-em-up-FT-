using UnityEngine;

public static class CombatKeybindManager
{
    public static KeyCode FireKey { get; private set; } = KeyCode.Z;
    public static KeyCode SwitchFireModeKey { get; private set; } = KeyCode.X;
    public static KeyCode BombKey { get; private set; } = KeyCode.C;
    public static KeyCode SuperKey { get; private set; } = KeyCode.Space;

    public static void SetKey(string action, KeyCode key)
    {
        switch (action)
        {
            case "Fire":
                FireKey = key;
                PlayerPrefs.SetString("FireKey", key.ToString());
                break;
            case "SwitchFire":
                SwitchFireModeKey = key;
                PlayerPrefs.SetString("SwitchFireModeKey", key.ToString());
                break;
            case "Bomb":
                BombKey = key;
                PlayerPrefs.SetString("BombKey", key.ToString());
                break;
            case "Super":
                SuperKey = key;
                PlayerPrefs.SetString("SuperKey", key.ToString());
                break;
        }

        PlayerPrefs.Save();
    }

    public static void LoadKeybinds()
    {
        FireKey = GetSavedKey("FireKey", KeyCode.Z);
        SwitchFireModeKey = GetSavedKey("SwitchFireModeKey", KeyCode.X);
        BombKey = GetSavedKey("BombKey", KeyCode.C);
        SuperKey = GetSavedKey("SuperKey", KeyCode.Space);
    }

    private static KeyCode GetSavedKey(string keyName, KeyCode defaultKey)
    {
        string keyString = PlayerPrefs.GetString(keyName, defaultKey.ToString());
        return System.Enum.TryParse(keyString, out KeyCode parsedKey) ? parsedKey : defaultKey;
    }
}
