////namespace MadBox.Exercice
////{
////    using System.Collections.Generic;

////    using UnityEditor;
////    using UnityEngine;

////    partial class RemoteRequestManager : MonoBehaviour
////    {
////        private static readonly List<RemoteRequest> editorRemoteRequests = new List<RemoteRequest>();

////        public static void EditorGet(string url, OnSuccess onSuccess, OnError onError)
////        {
////            Debug.Assert(!string.IsNullOrEmpty(url));
////            Debug.Assert(onSuccess != null);
////            Debug.Assert(onError != null);

////            RemoteRequest remoteRequest = RemoteRequest.Get(url, onSuccess, onError);
////            RemoteRequestManager.editorRemoteRequests.Add(remoteRequest);

////            // First request?
////            if (RemoteRequestManager.editorRemoteRequests.Count == 1)
////            {
////                EditorApplication.update += RemoteRequestManager.EditorUpdate;
////            }

////            remoteRequest.UnityWebRequest.SendWebRequest();
////        }

////        private static void EditorUpdate()
////        {
////            for (int index = 0; index < RemoteRequestManager.editorRemoteRequests.Count; index++)
////            {
////                RemoteRequest editorRemoteRequest = RemoteRequestManager.editorRemoteRequests[index];

////                if (editorRemoteRequest.Update())
////                {
////                    RemoteRequestManager.editorRemoteRequests.RemoveAt(index);
////                    index--;

////                    editorRemoteRequest.Dispose();

////                    // Last request?
////                    if (RemoteRequestManager.editorRemoteRequests.Count == 0)
////                    {
////                        EditorApplication.update -= RemoteRequestManager.EditorUpdate;
////                    }
////                }
////            }
////        }
////    }
////}
