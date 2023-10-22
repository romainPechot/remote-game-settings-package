namespace MadBox.Exercice
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.Networking;

    public partial class RemoteRequestManager : SingletonManager<RemoteRequestManager>
    {
        public delegate void OnSuccess(string result);
        public delegate void OnError(string url, long responseCode, string errorMessage);

        private readonly List<RemoteRequest> remoteRequests = new List<RemoteRequest>();

        public void Get(string url, OnSuccess onSuccess, OnError onError)
        {
            Debug.Assert(!string.IsNullOrEmpty(url));
            Debug.Assert(onSuccess != null);
            Debug.Assert(onError != null);

            RemoteRequest remoteRequest = RemoteRequest.Get(url, onSuccess, onError);
            this.remoteRequests.Add(remoteRequest);
            remoteRequest.UnityWebRequest.SendWebRequest();
        }

        private void Update()
        {
            for (int index = 0; index < this.remoteRequests.Count; index++)
            {
                RemoteRequest remoteRequest = this.remoteRequests[index];

                if (remoteRequest.Update())
                {
                    this.remoteRequests.RemoveAt(index);
                    index--;

                    remoteRequest.Dispose();
                }
            }
        }
    }
}
