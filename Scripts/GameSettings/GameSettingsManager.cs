namespace MadBox.Exercice
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public partial class GameSettingsManager : SingletonManager<GameSettingsManager>
    {
        public delegate void OnSuccess(GameSettings gameSettings);
        public delegate void OnError(string error);

        private static readonly string GameSettingsURL = "https://script.googleusercontent.com/macros/echo?user_content_key=zWU9PHHVxJ5LwNIN9HDUVczPsIInItmhPa88wl1ZN1mFw_TFJmdJYPy9iE0G5dIg6K_0ADHVs1TzbzXqfCRIHN7plHF0iTstm5_BxDlH2jW0nuo2oDemN9CCS2h10ox_1xSncGQajx_ryfhECjZEnOGZtukqWqhi0eu1OHj48ckfXhec17BuHwVsbaV3uqRbzaB3dC8fRKvruR0DjScjtSBoVNiPTONBl-5D9-Nq7_efZohciHsDvQ&lib=M5YioJdcbZ146fiZV43VHb_-qERZM9JSA";
        private static string gameSettingsFilePath = null;

        private Status status = Status.None;
        private string errorMessage = null;

        private readonly List<Tuple<OnSuccess, OnError>> requests = new List<Tuple<OnSuccess, OnError>>();

        private GameSettings gameSettings = new GameSettings();

        private static string GameSettingsFilePath
        {
            get
            {
                if (GameSettingsManager.gameSettingsFilePath == null)
                {
                    GameSettingsManager.gameSettingsFilePath = Application.persistentDataPath + "settings/game_settings.json";
                }

                return GameSettingsManager.gameSettingsFilePath;
            }
        }

        public enum Status
        {
            None,
            RemoteFetching,
            Loaded,
            Error,
        }

#if UNITY_EDITOR
        public static void OpenLocalFileFolder()
        {
            string localFileFolderPath = System.IO.Path.GetDirectoryName(GameSettingsManager.GameSettingsFilePath);
            if (!System.IO.Directory.Exists(localFileFolderPath))
            {
                Debug.Log("The local folder does not exists");
                return;
            }

            System.Diagnostics.Process.Start(localFileFolderPath);
        }

        public static void DeleteLocalFile()
        {
            if (!System.IO.File.Exists(GameSettingsManager.GameSettingsFilePath))
            {
                Debug.Log("The local file does not exists");
                return;
            }

            bool confirm = UnityEditor.EditorUtility.DisplayDialog(
                "Delete Local File",
                "Do you really want to delete the local copy of the game's settings?",
                "Delete");
            if (confirm)
            {
                try
                {
                    System.IO.File.Delete(GameSettingsManager.GameSettingsFilePath);
                }
                catch (System.IO.IOException e)
                {
                    Debug.LogException(e);
                }
            }
        }
#endif

        public void GetGameSettings(OnSuccess onSuccess, OnError onError)
        {
            Debug.Assert(onSuccess != null);
            Debug.Assert(onError != null);

            switch (this.status)
            {
                case Status.None:
                    {
                        // Store the request(s) if multiple are made before the game settings are retrieved.
                        this.requests.Add(new Tuple<OnSuccess, OnError>(onSuccess, onError));

                        // Try remote fetch first.
                        RemoteRequestManager.GetManager().Get(GameSettingsManager.GameSettingsURL, this.OnRemoteFetchGameSettingSuccess, this.OnRemoteFetchGameSettingsError);

                        // Set status.
                        this.status = Status.RemoteFetching;
                        return;
                    }

                case Status.RemoteFetching:
                    {
                        // Store the request(s) if multiple are made before the game settings are retrieved.
                        this.requests.Add(new Tuple<OnSuccess, OnError>(onSuccess, onError));
                        return;
                    }

                case Status.Loaded:
                    onSuccess(this.gameSettings);
                    break;

                case Status.Error:
                    onError(this.errorMessage);
                    break;

                default:
                    break;
            }
        }

        private void OnRemoteFetchGameSettingSuccess(string result)
        {
            this.status = Status.Loaded;
            this.gameSettings = GameSettings.FromJson(result);
            this.TryWriteToLocalFile(this.gameSettings);

            for (int index = 0; index < this.requests.Count; index++)
            {
                Tuple<OnSuccess, OnError> request = this.requests[index];
                request.Item1(this.gameSettings);
            }

            this.requests.Clear();
        }

        private void OnRemoteFetchGameSettingsError(string url, long responseCode, string error)
        {
            Debug.LogWarning(
                $"Unable to fetch the remote game's settings.\n" +
                $"URL: {url}\n" +
                $"Response Code: {responseCode}\n" +
                $"Error: {error}");

            // Remote failed, let's fallback to local file (if possible).

            if (!this.TryLoadFromLocalFile())
            {
                Debug.LogError("Unable to retrieve local backup of game's settings, the game should works but will have default values");
            }

            this.status = Status.Loaded;

            for (int index = 0; index < this.requests.Count; index++)
            {
                Tuple<OnSuccess, OnError> request = this.requests[index];
                request.Item1(this.gameSettings);
            }

            this.requests.Clear();
        }

        private bool TryLoadFromLocalFile()
        {
            if (!System.IO.File.Exists(GameSettingsManager.GameSettingsFilePath))
            {
                return false;
            }

            try
            {
                string content = System.IO.File.ReadAllText(GameSettingsManager.GameSettingsFilePath, System.Text.Encoding.UTF8);
                this.gameSettings = GameSettings.FromJson(content);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        private bool TryWriteToLocalFile(GameSettings gameSettings)
        {
            string content = gameSettings.ToJson();

            try
            {
                string gameSettingsFolderPath = System.IO.Path.GetDirectoryName(GameSettingsManager.GameSettingsFilePath);
                if (!System.IO.Directory.Exists(gameSettingsFolderPath))
                {
                    System.IO.Directory.CreateDirectory(gameSettingsFolderPath);
                }

                System.IO.File.WriteAllText(GameSettingsManager.GameSettingsFilePath, content, System.Text.Encoding.UTF8);
                return true;
            }
            catch (System.IO.IOException e)
            {
                Debug.LogException(e);
                return false;
            }
        }
    }
}
