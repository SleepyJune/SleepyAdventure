using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabCollection))]
public class PrefabCollectionEditor : Editor
{
    //private PrefabCollection prefabCollection;

    //private bool[] showPrefabSlots;

    //private SerializedProperty prefabImagesProperty;
    private SerializedProperty prefabsProperty;
    private SerializedProperty collectionHeightProperty;

    private const float addButtonWidth = 30f;

    //private const string collectionPropPrefabImagesName = "images";
    private const string collectionPropPrefabsName = "objects";
    private const string collectionPropHeightName = "height";

    private void OnEnable()
    {
        //prefabCollection = (PrefabCollection)target;

        //showPrefabSlots = new bool[prefabCollection.objects.Length];

        //prefabImagesProperty = serializedObject.FindProperty(collectionPropPrefabImagesName);
        prefabsProperty = serializedObject.FindProperty(collectionPropPrefabsName);
        collectionHeightProperty = serializedObject.FindProperty(collectionPropHeightName);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(collectionHeightProperty);

        for (int i = 0; i < prefabsProperty.arraySize; i++)
        {
            ItemSlotGUI(i);
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+", GUILayout.Width(addButtonWidth)))
        {
            prefabsProperty.arraySize++;
            //prefabImagesProperty.arraySize++;
        }

        if (GUILayout.Button("-", GUILayout.Width(addButtonWidth)))
        {
            prefabsProperty.arraySize--;
            //prefabImagesProperty.arraySize--;
        }

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
    private void ItemSlotGUI(int index)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        /*showPrefabSlots[index] = EditorGUILayout.Foldout(showPrefabSlots[index], "Item slot " + index);
        if (showPrefabSlots[index])
        {
            EditorGUILayout.PropertyField(prefabImagesProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(prefabsProperty.GetArrayElementAtIndex(index));
        }*/

        //EditorGUILayout.PropertyField(prefabImagesProperty.GetArrayElementAtIndex(index));
        EditorGUILayout.PropertyField(prefabsProperty.GetArrayElementAtIndex(index));

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}