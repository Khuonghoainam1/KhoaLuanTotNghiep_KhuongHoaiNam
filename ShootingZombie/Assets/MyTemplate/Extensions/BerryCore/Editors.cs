#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Yurowm.EditorCore;
using Yurowm.GameCore;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.Reflection;

[BerryPanelGroup("Content")]
[BerryPanelTab("Audio", 3)]
public class AudioEditor : MetaEditor<AudioAssistant> {

    SoundTree soundTree;
    SoundFileTree soundFileTree;

    MusicTree trackTree;
    GUIHelper.LayoutSplitter splitter = null;
    GUIHelper.Scroll scroll = null;
    static object selected = null;

    public override bool Initialize() {
        if (!metaTarget) {
            Debug.LogError("AudioAssistant is missing");
            return false;
        }
        if (metaTarget.musics == null)
            metaTarget.musics = new List<AudioAssistant.MusicTrack>();

        if (metaTarget.sounds == null)
            metaTarget.sounds = new List<AudioAssistant.Sound>();

        selected = null;

        int id = 0;
        metaTarget.sounds.ForEach(x => {
            x.clips.RemoveAll(c => !c);
            x.id = id++;
        });
        metaTarget.musics.ForEach(x => x.id = id++);

        splitter = new GUIHelper.LayoutSplitter(OrientationLine.Horizontal, OrientationLine.Vertical, 220);
        splitter.drawCursor += x => GUI.Box(x, "", Styles.separator);

        scroll = new GUIHelper.Scroll(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

        soundTree = new SoundTree(metaTarget.sounds);
        soundTree.onSelectedItemChanged += x => {
            if (x.Count == 1) {
                selected = x[0];
                SelectionChanged();
            }
        };

        trackTree = new MusicTree(metaTarget.musics);
        trackTree.onSelectedItemChanged += x => {
            if (x.Count == 1) {
                selected = x[0];
                SelectionChanged();
            }
        };

        soundFileTree = null;

        return true;
    }

    private void SelectionChanged() {
        if (selected == null)
            return;
        if (selected is AudioAssistant.Sound) {
            AudioAssistant.Sound sound = (selected as AudioAssistant.Sound);
            soundFileTree = new SoundFileTree(sound.clips);
            return;
        }
    }

    public override void OnGUI() {
        Undo.RecordObject(metaTarget, "");

        using (splitter.Start(true, true)) {
            if (splitter.Area(Styles.area)) {
                using (scroll.Start()) {
                    using (new GUIHelper.Horizontal()) {
                        GUILayout.Label("Music", Styles.miniLabel, GUILayout.ExpandWidth(true));
                        if (GUILayout.Button("New", EditorStyles.miniButton, GUILayout.Width(35)))
                            trackTree.AddNewItem(null, "NewMusic{0}");
                    }
                    trackTree.OnGUI();
                    EditorGUILayout.Space();
                    using (new GUIHelper.Horizontal()) {
                        GUILayout.Label("SFX", Styles.miniLabel, GUILayout.ExpandWidth(true));
                        if (GUILayout.Button("New", EditorStyles.miniButton, GUILayout.Width(35)))
                            soundTree.AddNewItem(null, "NewSound{0}");
                    }
                    soundTree.OnGUI();
                }
            }

            if (splitter.Area(Styles.area)) {
                if (selected != null) {
                    if (selected is AudioAssistant.Sound) {
                        EditorGUILayout.HelpBox("Use drag and drop to add new AudioClip", MessageType.Info, true);
                        soundFileTree.OnGUI(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                    }
                    else if (selected is AudioAssistant.MusicTrack) {
                        AudioAssistant.MusicTrack music = selected as AudioAssistant.MusicTrack;
                        music.track = (AudioClip) EditorGUILayout.ObjectField("Audio Clip", music.track, typeof(AudioClip), false);
                        GUILayout.FlexibleSpace();
                    }
                } else
                    GUILayout.Label("Nothing Selected", Styles.centeredLabel, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            }
        }
    }

    public override AudioAssistant FindTarget() {
        return AudioAssistant.main;
    }

    public class SoundSelector {
        string[] sounds;
        string[] soundNames;

        public SoundSelector() {
            if (AudioAssistant.main) {
                AudioAssistant.main.UpdatePath();
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Set("<Null>", "");
                foreach (var sound in AudioAssistant.main.sounds)
                    if (!dictionary.ContainsKey(sound.fullName))
                        dictionary.Add(sound.fullName, sound.fullName);
                soundNames = dictionary.Keys.ToArray();
                sounds = dictionary.Values.ToArray();
            }
        }

        public string Select(string name, string selected) {
            Rect rect = EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            if (Event.current.type == EventType.Layout)
                return selected;

            Rect rect2 = EditorGUI.PrefixLabel(rect, new GUIContent(name));
            rect.xMin = rect2.x;

            Rect objectRect = new Rect(rect);
            return sounds.Get(EditorGUI.Popup(objectRect, sounds.IndexOf(selected), soundNames));
        }
    }

    public class TrackSelector {
        string[] tracks;
        string[] trackNames;

        public TrackSelector() {
            if (AudioAssistant.main) {
                AudioAssistant.main.UpdatePath();
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Set("-", "-");
                dictionary.Set("None", "None");
                foreach (var track in AudioAssistant.main.musics)
                    if (!dictionary.ContainsKey(track.fullName))
                        dictionary.Add(track.fullName, track.fullName);
                trackNames = dictionary.Keys.ToArray();
                tracks = dictionary.Values.ToArray();
            }
        }

        public string Select(string name, string selected) {
            Rect rect = EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            if (Event.current.type == EventType.Layout)
                return selected;

            Rect rect2 = EditorGUI.PrefixLabel(rect, new GUIContent(name));
            rect.xMin = rect2.x;

            Rect objectRect = new Rect(rect);
            return tracks.Get(EditorGUI.Popup(objectRect, tracks.IndexOf(selected), trackNames));
        }
    }

    class SoundTree : GUIHelper.HierarchyList<AudioAssistant.Sound> {
        static Texture2D icon = null;

        public SoundTree(List<AudioAssistant.Sound> collection) : base(collection, null, new TreeViewState()) {
            icon = EditorGUIUtility.FindTexture("AudioSource Icon");
        }

        internal const string newGroupNameFormat = "Folder{0}";
        internal const string newSoundNameFormat = "Sound{0}";
        public override void ContextMenu(GenericMenu menu, List<IInfo> selected) {
            if (selected.Count == 0) {
                menu.AddItem(new GUIContent("New Sound"), false, () => AddNewItem(null, newSoundNameFormat));
                menu.AddItem(new GUIContent("New Group"), false, () => AddNewFolder(null, newGroupNameFormat));
            } else {
                if (selected.Count == 1 && selected[0].isFolderKind) {
                    FolderInfo parent = selected[0].asFolderKind;

                    menu.AddItem(new GUIContent("New Sound"), false, () => AddNewItem(parent, newSoundNameFormat));
                    menu.AddItem(new GUIContent("New Group"), false, () => AddNewFolder(parent, newGroupNameFormat));

                } else {
                    FolderInfo parent = selected[0].parent;
                    if (selected.All(x => x.parent == parent))
                        menu.AddItem(new GUIContent("Group"), false, () => Group(selected, parent, newGroupNameFormat));
                    else
                        menu.AddItem(new GUIContent("Group"), false, () => Group(selected, root, newGroupNameFormat));

                }

                menu.AddItem(new GUIContent("Remove"), false, () => Remove(selected.ToArray()));
            }
        }

        public override AudioAssistant.Sound CreateItem() {
            var sound = new AudioAssistant.Sound();
            sound.id = UnityEngine.Random.Range(int.MinValue, -1);
            return sound;
        }

        public override void DrawItem(Rect rect, ItemInfo info) {
            Rect _rect = new Rect(rect.x, rect.y, 16, rect.height);
            GUI.DrawTexture(_rect, icon);
            _rect = new Rect(rect.x + 16, rect.y, rect.width - 16, rect.height);
            GUI.Label(_rect, info.name);
            if (info.content == selected) {
                Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Color.cyan);
                _rect = new Rect(rect);
                _rect.width = 20;
                _rect.x = rect.xMax - _rect.width;
                if (GUI.Button(_rect, ">")) {
                    if (info.content.clips.Count > 0) {
                        StopAllClips();
                        PlayClip(info.content.clips.GetRandom());
                    }
                }

            }
        }

        static void PlayClip(AudioClip clip) {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod("PlayClip", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(AudioClip) }, null);
            method.Invoke(null, new object[] { clip });
        }

        static void StopAllClips() {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod("StopAllClips",
                BindingFlags.Static | BindingFlags.Public, null, new Type[0], null);
            method.Invoke(null, new object[0]);
        }

        public override string GetPath(AudioAssistant.Sound element) {
            return element.path;
        }

        public override string GetName(AudioAssistant.Sound element) {
            return element.name;
        }

        public override void SetName(AudioAssistant.Sound element, string name) {
            element.name = name;
        }

        public override int GetUniqueID(AudioAssistant.Sound element) {
            return element.id;
        }

        public override bool ObjectToItem(UnityEngine.Object o, out IInfo result) {
            result = null;
            return false;
        }

        public override void SetPath(AudioAssistant.Sound element, string path) {
            element.path = path;
        }
    }

    class SoundFileTree : GUIHelper.NonHierarchyList<AudioClip> {
        public static Texture2D icon = null;

        public override void ContextMenu(GenericMenu menu, List<IInfo> selected) {
            selected = selected.Where(x => x.isItemKind).ToList();
            if (selected.Count > 0)
                menu.AddItem(new GUIContent("Remove"), false, () => Remove(selected.ToArray()));
        }

        protected override bool CanRename(TreeViewItem item) {
            return false;
        }

        public SoundFileTree(List<AudioClip> collection) : base(collection, new TreeViewState()) {
            icon = EditorGUIUtility.FindTexture("AudioClip Icon");
            onSelectedItemChanged += s => Selection.objects = s.ToArray();
        }

        public override AudioClip CreateItem() {
            return null;
        }

        public override void DrawItem(Rect rect, ItemInfo info) {
            Rect _rect = new Rect(rect.x, rect.y, 16, rect.height);
            GUI.DrawTexture(_rect, icon);
            _rect = new Rect(rect.x + 16, rect.y, rect.width - 16, rect.height);
            GUI.Label(_rect, info.content.name);
        }

        public override int GetUniqueID(AudioClip element) {
            return element.GetHashCode();
        }

        public override bool ObjectToItem(UnityEngine.Object o, out IInfo result) {
            if (o is AudioClip) {
                ItemInfo item = new ItemInfo(0, null);
                item.content = o as AudioClip;
                result = item;
                return true;
            }

            result = null;
            return false;
        }
    }

    class MusicTree : GUIHelper.HierarchyList<AudioAssistant.MusicTrack> {
        public static Texture2D icon = null;

        public MusicTree(List<AudioAssistant.MusicTrack> collection) : base(collection, null, new TreeViewState()) {
            icon = EditorGUIUtility.FindTexture("AudioListener Icon");
        }

        internal const string newGroupNameFormat = "Folder{0}";
        internal const string newMusicNameFormat = "Music{0}";
        public override void ContextMenu(GenericMenu menu, List<IInfo> selected) {
            if (selected.Count == 0) {
                menu.AddItem(new GUIContent("New Music"), false, () => AddNewItem(null, newMusicNameFormat));
                menu.AddItem(new GUIContent("New Group"), false, () => AddNewFolder(null, newGroupNameFormat));
            } else {
                if (selected.Count == 1 && selected[0].isFolderKind) {
                    FolderInfo parent = selected[0].asFolderKind;

                    menu.AddItem(new GUIContent("New Music"), false, () => AddNewItem(parent, newMusicNameFormat));
                    menu.AddItem(new GUIContent("New Group"), false, () => AddNewFolder(parent, newGroupNameFormat));
                } else {
                    FolderInfo parent = selected[0].parent;
                    if (selected.All(x => x.parent == parent))
                        menu.AddItem(new GUIContent("Group"), false, () => Group(selected, parent, newGroupNameFormat));
                    else
                        menu.AddItem(new GUIContent("Group"), false, () => Group(selected, root, newGroupNameFormat));

                }

                menu.AddItem(new GUIContent("Remove"), false, () => Remove(selected.ToArray()));
            }
        }

        public override AudioAssistant.MusicTrack CreateItem() {
            var sound = new AudioAssistant.MusicTrack();
            sound.id = UnityEngine.Random.Range(int.MinValue, -1);
            return sound;
        }

        public override void DrawItem(Rect rect, ItemInfo info) {
            if (info.content == selected)
                Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Color.cyan);
            Rect _rect = new Rect(rect.x, rect.y, 16, rect.height);
            GUI.DrawTexture(_rect, icon);
            _rect = new Rect(rect.x + 16, rect.y, rect.width - 16, rect.height);
            GUI.Label(_rect, info.name);
        }
        
        public override string GetPath(AudioAssistant.MusicTrack element) {
            return element.path;
        }

        public override string GetName(AudioAssistant.MusicTrack element) {
            return element.name;
        }

        public override void SetName(AudioAssistant.MusicTrack element, string name) {
            element.name = name;
        }

        public override int GetUniqueID(AudioAssistant.MusicTrack element) {
            return element.id;
        }

        public override bool ObjectToItem(UnityEngine.Object o, out IInfo result) {
            result = null;
            return false;
        }

        public override void SetPath(AudioAssistant.MusicTrack element, string path) {
            element.path = path;
        }
    }
}
#endif