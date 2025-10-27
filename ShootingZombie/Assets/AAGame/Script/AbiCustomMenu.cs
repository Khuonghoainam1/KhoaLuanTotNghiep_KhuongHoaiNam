#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;

namespace Nubg.Editor
{
    public class AbiCustomMenu
    {
        [MenuItem("Abi/Clear All Data", priority = 40)]
        private static void ClearALLData()
        {
            DataPersistent.ClearAll();
            PlayerPrefs.DeleteAll();
        }
        [MenuItem("Abi/Open Save Folder", priority = 40)]
        private static void OpenPersistentFolder()
        {
            //EditorUtility.RevealInFinder(Application.persistentDataPath);
            Process process = new Process();
            process.StartInfo.FileName = ((Application.platform == RuntimePlatform.WindowsEditor) ? "explorer.exe" : "open");
            process.StartInfo.Arguments = "file://" + Application.persistentDataPath;
            process.Start();
        }

        [MenuItem("Abi/SPRITE MANAGER", priority = 1)]
        private static void OpenSpriteManager()
        {
            Selection.activeObject = SpriteManager.Instance;
        }


    }


    public class ToyMenu : OdinEditorWindow
    {

        public double ADouble;

        [MenuItem("Abi/Remove Missing Reference In Scene", priority = 100)]
        private static void ShowWindow()
        {
            var window = GetWindow<ToyMenu>();

            // Nifty little trick to quickly position the window in the middle of the editor.
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 700);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        [FoldoutGroup("Remove Missing Reference In Scene")]
        [Sirenix.OdinInspector.Button(ButtonHeight = 50)]
        void RemoveAllMissingScript()
        {
            var go_all = FindObjectsOfType<Transform>();
            for (int i = 0; i < go_all.Length; i++)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go_all[i].gameObject);
            }
            var meshAll = FindObjectsOfType<MeshFilter>();
            var meshToDelete = new List<GameObject>();
            for (int i = 0; i < meshAll.Length; i++)
            {
                if (meshAll[i].sharedMesh == null)
                    meshToDelete.Add(meshAll[i].gameObject);
            }
            Debug.LogError(meshToDelete.Count);
            for (int i = 0; i < meshToDelete.Count; i++)
            {
                DestroyImmediate(meshToDelete[i]);
            }
        }
    }
}
#endif