﻿using System;
using PixelCrew.Components.Dialogs;
using PixelCrew.Utils.Editor;
using UnityEditor;

namespace PixelCrew.UI.HUD.Dialogs.Editor
{
    [CustomEditor(typeof(ShowDialogComponent))]
    public class ShowDialogComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;
        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("_mode");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modeProperty);

            if (_modeProperty.GetEnum(out ShowDialogComponent.Mode mode))
            {
                switch (mode)
                {
                    case ShowDialogComponent.Mode.Bound:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_bound"));
                        break;
                    case ShowDialogComponent.Mode.External:
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("_external"));
                            break;
                }
            }
            
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}