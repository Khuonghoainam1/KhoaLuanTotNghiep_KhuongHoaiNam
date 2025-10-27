using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using Yurowm.GameCore;
using Sound = ContentSound.Sound;

[CustomEditor(typeof(ContentSound))]
[CanEditMultipleObjects]
public class ContentSoundEditor : Editor
{

    ContentSound provider;
    AudioEditor.SoundSelector selector = null;

    IEnumerable<IGrouping<string, Sound>> clipGroups;

    void OnEnable()
    {
        selector = new AudioEditor.SoundSelector();

        foreach (ContentSound sound in serializedObject.targetObjects)
        {
            List<Sound> clips = sound.GetClipNames().Select(x => new Sound(x)).ToList();

            foreach (Sound clip in clips)
                if (!sound.clips.Contains(clip))
                    sound.clips.Add(clip);

            foreach (Sound clip in new List<Sound>(sound.clips))
                if (!clips.Contains(clip) && sound.clips.Contains(clip))
                    sound.clips.Remove(clip);

            sound.clips.Sort((a, b) => string.CompareOrdinal(a.name, b.name));
        }

        if (serializedObject.isEditingMultipleObjects)
            clipGroups = serializedObject.targetObjects.SelectMany(x => (x as ContentSound).clips).GroupBy(x => x.name);
        else
            provider = (ContentSound)target;
    }

    public override void OnInspectorGUI()
    {
        if (AudioAssistant.main == null)
        {
            EditorGUILayout.HelpBox("AudioAssistant is missing", MessageType.Error);
            return;
        }

        string value;
        if (serializedObject.isEditingMultipleObjects)
        {
            Undo.RecordObjects(serializedObject.targetObjects, "Chips is changed");


            foreach (IGrouping<string, Sound> group in clipGroups)
            {
                value = group.First().clip;
                EditorGUI.showMixedValue = !group.All(x => x.clip == value);
                EditorGUI.BeginChangeCheck();
                value = selector.Select(group.Key, value);
                if (EditorGUI.EndChangeCheck())
                    group.ForEach(x => x.clip = value);
                EditorGUI.showMixedValue = false;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("onlyDuringSession"));
        }
        else
        {
            Undo.RecordObject(target, "Chip is changed");
            foreach (Sound clip in provider.clips)
                clip.clip = selector.Select(clip.name, clip.clip);
            provider.onlyDuringSession = EditorGUILayout.Toggle("Only During Session", provider.onlyDuringSession);

            //EditorUtility.SetDirty(target);
        }
    }
}