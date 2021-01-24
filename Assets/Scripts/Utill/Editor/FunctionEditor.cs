using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

//[CustomEditor(typeof(MonsterAsset))]
public class FunctionEditor : Editor
{
    GUIContent iconPlus; 
    GUIContent iconMinus; 
    GUIStyle buttonStyle = GUIStyle.none;

    ReorderableList OuterList;
    Dictionary<string, ReorderableList> innerListDict = new Dictionary<string, ReorderableList>();

    private void OnEnable()
    {

        var prop = serializedObject.FindProperty("Pattern");
        float SpriteHeight;
        OuterList = new ReorderableList(serializedObject, prop, false, true, true, true);

        OuterList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Action");

        };
        OuterList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            EditorGUI.indentLevel++;

            var ActionItem = prop.GetArrayElementAtIndex(index);
            var FuncList = ActionItem.FindPropertyRelative("Function");

            EditorGUI.PropertyField(new Rect(rect.x,rect.y,rect.width, EditorGUIUtility.singleLineHeight), ActionItem.FindPropertyRelative("Effect"));

            PreviewSpriteDrawer spriteDrawer = new PreviewSpriteDrawer();
            var customLabel = new GUIContent("Effect");
            SpriteHeight = spriteDrawer.GetPropertyHeight(ActionItem.FindPropertyRelative("Effect"), customLabel);

            EditorGUI.PropertyField(new Rect(rect.x, rect.y+ SpriteHeight, rect.width, EditorGUIUtility.singleLineHeight), ActionItem.FindPropertyRelative("Intent"));

            string listKey = ActionItem.propertyPath;

            ReorderableList InnerList;
            if (innerListDict.ContainsKey(listKey))
            {
                // fetch the reorderable list in dict
                InnerList = innerListDict[listKey];
                if (FuncList.arraySize == 0)
                {
                    InnerList.elementHeight = EditorGUIUtility.singleLineHeight;
                }
            }
            else
            {
                InnerList = new ReorderableList(ActionItem.serializedObject, FuncList, false, true, true, true);

                InnerList.drawHeaderCallback = (Rect innerRect) =>
                {
                    EditorGUI.LabelField(innerRect, "Function");
                };
                InnerList.drawElementCallback = (innerRect, innerIndex, innerA, innerH) =>
                {
                    InnerList.elementHeight = EditorGUIUtility.singleLineHeight * 5f;
                    // Get element of inner list
                    var FuncItem = FuncList.GetArrayElementAtIndex(innerIndex);

                    var Func = FuncItem.FindPropertyRelative("Func");
                    EditorGUI.PropertyField(new Rect(innerRect.x, innerRect.y, innerRect.width, EditorGUIUtility.singleLineHeight), Func);
                    var Value = FuncItem.FindPropertyRelative("Value");
                    EditorGUI.PropertyField(new Rect(innerRect.x, innerRect.y + EditorGUIUtility.singleLineHeight * 1.5f, innerRect.width, EditorGUIUtility.singleLineHeight), Value);
                    var Repeat = FuncItem.FindPropertyRelative("Repeat");
                    EditorGUI.PropertyField(new Rect(innerRect.x, innerRect.y + EditorGUIUtility.singleLineHeight * 3f, innerRect.width, EditorGUIUtility.singleLineHeight), Repeat);
                };

                innerListDict[listKey] = InnerList;
            }
            var height = (FuncList.arraySize + 3) * EditorGUIUtility.singleLineHeight+ SpriteHeight;
            InnerList.DoList(new Rect(rect.x, rect.y + 3f* EditorGUIUtility.singleLineHeight+ SpriteHeight, rect.width, height));
            EditorGUI.indentLevel--;
        };

        OuterList.elementHeightCallback = index =>
        {
            var ActionItem = prop.GetArrayElementAtIndex(index);
            var FuncList = ActionItem.FindPropertyRelative("Function");

            PreviewSpriteDrawer spriteDrawer = new PreviewSpriteDrawer();
            var customLabel = new GUIContent("Effect");
            SpriteHeight = spriteDrawer.GetPropertyHeight(ActionItem.FindPropertyRelative("Effect"), customLabel);

            return (EditorGUIUtility.singleLineHeight * 5f) + (EditorGUIUtility.singleLineHeight * 6f * FuncList.arraySize+ SpriteHeight);
        };

    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MonsterName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Description"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Prefab"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MinHp"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxHp"));
        OuterList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

}

//[CustomEditor(typeof(CardAsset))]
public class CardEditor : Editor
{
    GUIContent iconPlus;
    GUIContent iconMinus;
    GUIStyle buttonStyle = GUIStyle.none;

    ReorderableList OuterList;
    //Dictionary<string, ReorderableList> innerListDict = new Dictionary<string, ReorderableList>();

    private void OnEnable()
    {

        var prop = serializedObject.FindProperty("FunctionAndValue");

        OuterList = new ReorderableList(serializedObject, prop, false, true, true, true);

        OuterList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Function");

        };
        OuterList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            EditorGUI.indentLevel++;

            var ActionItem = prop.GetArrayElementAtIndex(index);
            var FuncList = ActionItem.FindPropertyRelative("FunctionModule");

            var Func = ActionItem.FindPropertyRelative("Func");
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), Func);
            var Value = ActionItem.FindPropertyRelative("Value");
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 1.5f, rect.width, EditorGUIUtility.singleLineHeight), Value);
            var Repeat = ActionItem.FindPropertyRelative("Repeat");
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 3f, rect.width, EditorGUIUtility.singleLineHeight), Repeat);
            var Enchant = ActionItem.FindPropertyRelative("EnchantRate");
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 4.5f, rect.width, EditorGUIUtility.singleLineHeight), Enchant);
            var Description = ActionItem.FindPropertyRelative("Description");
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 6f, rect.width, EditorGUIUtility.singleLineHeight), Enchant);


            EditorGUI.indentLevel--;
        };

        OuterList.elementHeight = (EditorGUIUtility.singleLineHeight * 8f);

    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("IsExtinct"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("charType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("CardName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Rarity"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Targets"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Cost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("CardImage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("SkillEffect"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MultipleEnchant"));
        OuterList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

}
