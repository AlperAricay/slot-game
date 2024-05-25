using Gameplay.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(BaseButtonUI), true)]
    public class BaseButtonUIEditor : ButtonEditor
    {
        private BaseButtonUI TargetButton => (BaseButtonUI) target;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TargetButton.isAnimatedButton = EditorGUILayout.Toggle("Is Animated Button", TargetButton.isAnimatedButton);
            if (TargetButton.isAnimatedButton)
            {
                var objectField = (Transform)EditorGUILayout.ObjectField("Target Scale Transform",
                    TargetButton.targetScaleTransform, typeof(Transform));
                if (objectField == null)
                {
                    var hasNull = TargetButton.targetScaleTransform == null;
                    TargetButton.targetScaleTransform = TargetButton.transform;
                    if (!hasNull)
                    {
                        Save();
                    }
                }
                else
                {
                    var hasNull = TargetButton.targetScaleTransform == null;
                    TargetButton.targetScaleTransform = objectField;
                    if (hasNull)
                    {
                        Save();
                    }
                }
            }
            else
            {
                var hasNull = TargetButton.targetScaleTransform == null;
                TargetButton.targetScaleTransform = null;
                if (!hasNull)
                {
                    Save();
                }
            }

            if (GUI.changed)
            {
                Save();
            }
        }

        private void Save()
        {
            EditorUtility.SetDirty(TargetButton);
            serializedObject.ApplyModifiedProperties();
        }
    }
}