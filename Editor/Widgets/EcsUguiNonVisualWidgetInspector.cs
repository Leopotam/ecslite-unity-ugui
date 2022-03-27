// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2022 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEditor.UI;

namespace Leopotam.EcsLite.Unity.Ugui.Editor {
    [CustomEditor (typeof (EcsUguiNonVisualWidget), false)]
    [CanEditMultipleObjects]
    sealed class EcsUguiNonVisualWidgetInspector : GraphicEditor {
        public override void OnInspectorGUI () {
            serializedObject.Update ();
            EditorGUILayout.PropertyField (m_Script);
            RaycastControlsGUI ();
            serializedObject.ApplyModifiedProperties ();
        }
    }
}