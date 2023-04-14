using UnityEngine;
using UnityEditor;

namespace MyCustomAttribute
{
    //custom atrr read only (sử dụng để ẩn chỉnh sửa trên inspector)
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // disables elements in the gui
            GUI.enabled = false;
            // creates the property, which will be disabled because of the above line
            EditorGUI.PropertyField(position, property, new GUIContent(label + "(read only)"), true);
            // re-enables the gui so that not all properties are greyed out
            GUI.enabled = true;
        }

    }

    //custom attr lable (chỉnh sửa tiêu đề của field)
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LabelAttribute labelAttribute = attribute as LabelAttribute;
            EditorGUI.PropertyField(position, property, new GUIContent(labelAttribute.label), true);
        }

    }

    [CustomPropertyDrawer(typeof(MinMaxAttribute))]
    public class MinToDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MinMaxAttribute minMaxAttribute = attribute as MinMaxAttribute;
            var propertyType = property.propertyType;


            if (propertyType == SerializedPropertyType.Vector2)
            {
                Rect controlRect = EditorGUI.PrefixLabel(position, label);
                Rect[] splittedRect = Utility.SplitRect(controlRect, 3);
                EditorGUI.BeginChangeCheck();

                Vector2 vector = property.vector2Value;
                float minVal = vector.x;
                float maxVal = vector.y;

                minVal = EditorGUI.FloatField(splittedRect[0], minVal);
                maxVal = EditorGUI.FloatField(splittedRect[2], maxVal);

                EditorGUI.MinMaxSlider(splittedRect[1], ref minVal, ref maxVal,
                minMaxAttribute.min, minMaxAttribute.max);


                if (minVal < minMaxAttribute.min)
                {
                    minVal = minMaxAttribute.min;
                }

                if (maxVal > minMaxAttribute.max)
                {
                    maxVal = minMaxAttribute.max;
                }

                vector = new Vector2(minVal > maxVal ? maxVal : minVal, maxVal);

                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2Value = vector;
                }
            }
            else if (propertyType == SerializedPropertyType.Vector2Int)
            {
                Rect controlRect = EditorGUI.PrefixLabel(position, label);
                Rect[] splittedRect = Utility.SplitRect(controlRect, 3);
                EditorGUI.BeginChangeCheck();

                Vector2Int vector = property.vector2IntValue;
                float minVal = vector.x;
                float maxVal = vector.y;

                minVal = EditorGUI.FloatField(splittedRect[0], minVal);
                maxVal = EditorGUI.FloatField(splittedRect[2], maxVal);

                EditorGUI.MinMaxSlider(splittedRect[1], ref minVal, ref maxVal,
                minMaxAttribute.min, minMaxAttribute.max);

                if (minVal < minMaxAttribute.min)
                {
                    maxVal = minMaxAttribute.min;
                }

                if (minVal > minMaxAttribute.max)
                {
                    maxVal = minMaxAttribute.max;
                }

                vector = new Vector2Int(Mathf.FloorToInt(minVal > maxVal ? maxVal : minVal), Mathf.FloorToInt(maxVal));

                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2IntValue = vector;
                }

            }
            else
            {
                EditorGUI.PropertyField(position, property, new GUIContent(label), true);
                EditorGUILayout.HelpBox("MinMax Attribute only use with vector2 of vector2Int field", MessageType.Warning);
            }
        }
    }
}

