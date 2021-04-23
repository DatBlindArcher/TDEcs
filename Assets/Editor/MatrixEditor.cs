using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TableMatrix), true)]
public class MatrixEditor : PropertyDrawer
{
    private bool foldout = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var currentRect = position;
        currentRect.height = EditorGUIUtility.singleLineHeight;

        if (foldout = EditorGUI.Foldout(currentRect, foldout, label))
        {
            currentRect.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.indentLevel++;
            var sizeRect = EditorGUI.PrefixLabel(currentRect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Size"));
            EditorGUI.indentLevel--;

            float half = sizeRect.width / 2f;
            var xRect = new Rect(sizeRect.x, sizeRect.y, half, sizeRect.height);
            var yRect = new Rect(sizeRect.x + half, sizeRect.y, half, sizeRect.height);

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(xRect, property.FindPropertyRelative("width"), GUIContent.none);
            EditorGUI.PropertyField(yRect, property.FindPropertyRelative("height"), GUIContent.none);

            var width = property.FindPropertyRelative("width").intValue;
            var height = property.FindPropertyRelative("height").intValue;

            if (EditorGUI.EndChangeCheck())
            {
                var array = property.FindPropertyRelative("data");
                array.arraySize = width * height;
            }

            float startPos = currentRect.x;
            currentRect.width /= width;

            for (int y = 0; y < height; y++)
            {
                currentRect.x = startPos;
                currentRect.y += EditorGUIUtility.singleLineHeight;

                for (int x = 0; x < width; x++)
                {
                    EditorGUI.PropertyField(currentRect, property.FindPropertyRelative("data").GetArrayElementAtIndex(y * width + x), GUIContent.none);
                    currentRect.x += currentRect.width;
                }
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (foldout)
        {
            return (2 + property.FindPropertyRelative("height").intValue) * EditorGUIUtility.singleLineHeight;
        }

        return base.GetPropertyHeight(property, label);
    }
}