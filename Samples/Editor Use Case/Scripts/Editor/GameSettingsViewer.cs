namespace MadBox.Exercice.Editor
{
    using UnityEditor;
    using UnityEngine;

    public class GameSettingsViewer : EditorWindow
    {
        [SerializeField]
        private Editor gameSettingsContainerEditor = null;

        [SerializeField]
        private Vector2 gameSettingsContainerEditorScrollPosition;

        [SerializeField]
        private bool fetchingGameSettings = false;

        [SerializeField]
        private string fetchErrorMessage = null;

        [MenuItem("Tools/Game's Settings Viewer")]
        [InitializeOnLoadMethod]
        public static void OpenWindow()
        {
            if (!EditorWindow.HasOpenInstances<GameSettingsViewer>())
            {
                EditorApplication.delayCall += () =>
                {
                    EditorWindow.GetWindow<GameSettingsViewer>();
                };
            }
        }

        private void OnEnable()
        {
            this.titleContent = new GUIContent("Game's Settings Viewer");
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox("This tool help you see and explore the game's settings.", MessageType.Info);

            if (this.gameSettingsContainerEditor != null)
            {
                using (GUILayout.ScrollViewScope scrollViewScope = new GUILayout.ScrollViewScope(this.gameSettingsContainerEditorScrollPosition))
                {
                    this.gameSettingsContainerEditor.OnInspectorGUI();
                    this.gameSettingsContainerEditorScrollPosition = scrollViewScope.scrollPosition;
                }
            }
            else
            {
                Rect warningNoGameSettingsArea = EditorGUILayout.GetControlRect(false, GUILayout.ExpandWidth(true), GUILayout.Height(EditorGUIUtility.singleLineHeight * 5f));
                GUIContent warningNoGameSettingsContent = EditorGUIUtility.TrTempContent(this.fetchingGameSettings ? "Fetching game's settings..." : !string.IsNullOrEmpty(this.fetchErrorMessage) ? this.fetchErrorMessage : "Please fetch the game's settings in order to view them.");
                GUI.Label(warningNoGameSettingsArea, warningNoGameSettingsContent, EditorStyles.centeredGreyMiniLabel);
            }

            GUILayout.FlexibleSpace();

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("The application needs to be playing in order to fetch the game's settings.*\n\n* I could've done a version that works without the play mode (using EditorApplication.update delegate) but it was not in the specs.", MessageType.Warning);
            }

            using (new EditorGUI.DisabledScope(this.fetchingGameSettings || !Application.isPlaying))
            {
                if (GUILayout.Button("Fetch Game's Settings", GUILayout.Height(EditorGUIUtility.singleLineHeight * 2f)))
                {
                    this.fetchingGameSettings = true;
                    GameSettingsManager.GetManager().GetGameSettings(this.OnFetchGameSettingsSuccess, this.OnFetchGameSettingsError);
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Local File");

            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Open Folder"))
                {
                    GameSettingsManager.OpenLocalFileFolder();
                }

                if (GUILayout.Button("Delete File"))
                {
                    GameSettingsManager.DeleteLocalFile();
                }
            }
        }

        private void OnFetchGameSettingsSuccess(GameSettings gameSettings)
        {
            this.fetchingGameSettings = false;
            this.fetchErrorMessage = null;
            this.UpdateGameSettingsContainer(gameSettings);
        }

        private void OnFetchGameSettingsError(string errorMessage)
        {
            this.fetchingGameSettings = false;
            this.fetchErrorMessage = errorMessage;
            this.UpdateGameSettingsContainer(null);
        }

        private void UpdateGameSettingsContainer(GameSettings gameSettings)
        {
            if (gameSettings != null)
            {
                GameSettingsContainer gameSettingsContainer;
                if (this.gameSettingsContainerEditor == null)
                {
                    gameSettingsContainer = ScriptableObject.CreateInstance<GameSettingsContainer>();
                    this.gameSettingsContainerEditor = Editor.CreateEditor(gameSettingsContainer);
                }
                else
                {
                    gameSettingsContainer = (GameSettingsContainer)this.gameSettingsContainerEditor.target;
                }

                gameSettingsContainer.GameSettings = gameSettings;
            }
            else if (this.gameSettingsContainerEditor != null)
            {
                Object gameSettingsContainer = this.gameSettingsContainerEditor.target;
                Object.DestroyImmediate(this.gameSettingsContainerEditor);
                Object.DestroyImmediate(gameSettingsContainer);
                this.gameSettingsContainerEditor = null;
            }
        }
    }
}
