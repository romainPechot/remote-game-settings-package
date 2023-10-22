namespace MadBox.Exercice
{
    using UnityEngine;
    using UnityEngine.Networking;

    partial class RemoteRequestManager
    {
        private partial class RemoteRequest : System.IDisposable
        {
            public readonly string URL;
            public readonly OnSuccess OnSuccess;
            public readonly OnError OnError;
            public readonly UnityWebRequest UnityWebRequest;

            public RemoteRequest(string url, OnSuccess onSuccess, OnError onError, UnityWebRequest unityWebRequest)
            {
                Debug.Assert(!string.IsNullOrEmpty(url));
                Debug.Assert(onSuccess != null);
                Debug.Assert(onError != null);
                Debug.Assert(unityWebRequest != null);

                this.URL = url;
                this.OnSuccess = onSuccess;
                this.OnError = onError;
                this.UnityWebRequest = unityWebRequest;
            }

            public static RemoteRequest Get(string url, OnSuccess onSuccess, OnError onError)
            {
                Debug.Assert(!string.IsNullOrEmpty(url));
                Debug.Assert(onSuccess != null);
                Debug.Assert(onError != null);

                UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
                RemoteRequest remoteRequest = new RemoteRequest(url, onSuccess, onError, unityWebRequest);
                return remoteRequest;
            }

            public void Dispose()
            {
                UnityWebRequest.Dispose();
            }

            public bool Update()
            {
                if (this.UnityWebRequest.isDone)
                {
                    if (this.UnityWebRequest.result == UnityWebRequest.Result.Success)
                    {
                        this.OnSuccess(this.UnityWebRequest.downloadHandler.text);
                    }
                    else
                    {
                        this.OnError(this.URL, this.UnityWebRequest.responseCode, this.UnityWebRequest.error);
                    }
                }

                return this.UnityWebRequest.isDone;
            }
        }
    }
}
