namespace MadBox.Exercice
{
    using UnityEngine;

    public class MonoBehaviorThatNeedsGameSettings : MonoBehaviour
    {
        private void Start()
        {
            // Fetch the game settings.
            GameSettingsManager.GetManager().GetGameSettings(this.OnGetGameSettingsSuccess, this.OnGetGameSettingsError);
        }

        private void OnGetGameSettingsSuccess(GameSettings gameSettings)
        {
            int entitiesCount = gameSettings.Entities.Length;
            for (int index = 0; index < entitiesCount; index++)
            {
                GameObject entity = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                entity.name = gameSettings.Entities[index].Name;
                entity.transform.position = Vector3.right * 1.5f * index;
                entity.GetComponent<Renderer>().material.color = Random.ColorHSV();
            }
             
            Debug.Log("<color=green>SUCCESS</color>");
        }

        private void OnGetGameSettingsError(string error)
        {
            GameObject entity = GameObject.CreatePrimitive(PrimitiveType.Cube);
            entity.GetComponent<Renderer>().material.color = Color.red;
            Debug.Log($"<color=red>ERROR: {error}</color>");
        }
    }
}
