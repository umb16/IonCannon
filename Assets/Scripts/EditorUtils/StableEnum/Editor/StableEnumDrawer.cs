using System;
using UnityEditor;
using UnityEngine;
using StableEnumUtilities;
using System.Linq;

[CustomPropertyDrawer(typeof(BaseStableEnum), true)]
public class StableEnumDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = base.GetPropertyHeight(property, label);

        //if (property.propertyType == SerializedPropertyType.Enum)
            height = height * 2 + EditorGUIUtility.standardVerticalSpacing;

        return height;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var proxyProp = property.FindPropertyRelative(BaseStableEnum.kProxyPropName);
        var indexProp = property.FindPropertyRelative(BaseStableEnum.kIndexPropName);
        var valueProp = property.FindPropertyRelative(BaseStableEnum.kValuePropName);
        var propObject = EditorHelper.GetTargetObjectOfProperty(property);

        var enumValue = (Enum)((BaseStableEnum)propObject).valueObject;
        if (searchTextFieldStyle == null)
            searchTextFieldStyle = GUI.skin.FindStyle("ToolbarSeachTextField");
        Rect searchRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect popupRect = new Rect(position.x, searchRect.y + searchRect.height + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);
        DrawSearchBar(searchRect, label, Enum.GetNames(enumValue.GetType()));
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

        

        if (options == null)
            UpdateOptions(Enum.GetNames(enumValue.GetType()));

        
        


        
        //enumValue = EditorGUI.EnumPopup(popupRect, label, enumValue);
        

        //DrawEnumPopup(popupRect, property);

        EditorGUI.showMixedValue = false;
        
        //=========================================
        Rect fieldRect = EditorGUI.PrefixLabel(popupRect, new GUIContent(" "));
        int currentIndex = Array.IndexOf(options, Enum.GetNames(enumValue.GetType())[(int)Convert.ChangeType(enumValue, enumValue.GetType())]);
        int selectedIndex = EditorGUI.Popup(fieldRect, currentIndex, options);
        if (selectedIndex >= 0)
        {
            int newIndex = Array.IndexOf(Enum.GetNames(enumValue.GetType()), options[selectedIndex]);

            if (EditorGUI.EndChangeCheck())// if (newIndex != currentIndex)
            {
                int intValue = newIndex;
                //int intValue = (int)Convert.ChangeType(enumValue, enumValue.GetType());
                valueProp.intValue = intValue;
                indexProp.intValue = intValue;
                proxyProp.stringValue = Enum.GetNames(enumValue.GetType())[intValue];
                //property.enumValueIndex = newIndex;
                search = string.Empty;
                UpdateOptions(Enum.GetNames(enumValue.GetType()));
            }
        }
        //Debug.Log("selectedIndex " + selectedIndex);
    }

    private string search;
    private string[] options;
    private GUIStyle searchTextFieldStyle;

    private void DrawEnumPopup(Rect position, SerializedProperty property)
    {

        Rect fieldRect = EditorGUI.PrefixLabel(position, new GUIContent(" "));
        int currentIndex = Array.IndexOf(options, property.enumDisplayNames[property.enumValueIndex]);
        int selectedIndex = EditorGUI.Popup(fieldRect, currentIndex, options);
        if (selectedIndex >= 0)
        {
            int newIndex = Array.IndexOf(property.enumDisplayNames, options[selectedIndex]);
            if (newIndex != currentIndex)
            {
                property.enumValueIndex = newIndex;
                search = string.Empty;
                UpdateOptions(property.enumDisplayNames);
            }
        }
    }

    private void DrawSearchBar(Rect position, GUIContent label, string[] allOptions)
    {
        EditorGUI.BeginChangeCheck();
        search = EditorGUI.TextField(position, label, search, searchTextFieldStyle);
        if (EditorGUI.EndChangeCheck())
            UpdateOptions(allOptions);
    }

    private void UpdateOptions(string[] allOptions)
    {
        options = Array.FindAll(allOptions, name => string.IsNullOrEmpty(search) || name.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0);
    }
}
