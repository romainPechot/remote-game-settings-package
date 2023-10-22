namespace MadBox.Exercice
{
    using UnityEngine;

    /// <summary>
    /// Used to display the game's settings inside Unity Editor.
    /// </summary>
    public class GameSettingsContainer : ScriptableObject
    {
        public GameSettings GameSettings = new GameSettings();
    }
}
