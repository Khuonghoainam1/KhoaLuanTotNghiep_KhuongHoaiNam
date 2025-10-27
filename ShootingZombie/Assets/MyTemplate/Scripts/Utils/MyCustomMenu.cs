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
    public class MyCustomMenu
    {
        [MenuItem("Custom/Open Save Folder", priority = 40)]
        private static void OpenPersistentFolder()
        {
            //EditorUtility.RevealInFinder(Application.persistentDataPath);
            Process process = new Process();
            process.StartInfo.FileName = ((Application.platform == RuntimePlatform.WindowsEditor) ? "explorer.exe" : "open");
            process.StartInfo.Arguments = "file://" + Application.persistentDataPath;
            process.Start();
        }

        [MenuItem("Custom/SPRITE MANAGER", priority = 1)]
        private static void OpenSpriteManager()
        {
            Selection.activeObject = SpriteManager.Instance;
        }

        //[MenuItem("Abi/CharacterData Manager", priority = 0)]
        //private static void OpenLevelManager()
        //{
        //    Selection.activeObject = CharacterDataManager.Instance;
        //}
    }


    
}
#endif