using UnityEngine;
using UnityEditor;


// Give this script a custom UI interface used in conjunction with the new Stats ssystem (see AlterStatNode and StatsManager)
[CustomEditor(typeof(ChoiceNode))]
public class ChoiceNodeEditor : Editor
{
    float default_label_width;

    override public void OnInspectorGUI()
    {
        default_label_width = EditorGUIUtility.labelWidth;

        var choices = target as ChoiceNode;

        choices.Name_Of_Choice = EditorGUILayout.TextField("Name of Choice:", choices.Name_Of_Choice);
        choices.Number_Of_Choices = Mathf.Abs(Mathf.Min(ChoiceNode.max_number_of_buttons, EditorGUILayout.IntField("Number of Choices:", choices.Number_Of_Choices)));
        EditorGUILayout.LabelField("");

        // Create editor interface by looping through all of the buttons
        for (int x = 0; x < choices.Number_Of_Choices; x++)
        {
            int button_number = x + 1;
            EditorGUILayout.LabelField("Choice Button " + button_number);
            choices.Requirement_Type[x] = (Choice_Stat_Requirement)EditorGUILayout.EnumPopup("Choice requires Stats?", choices.Requirement_Type[x]);
            EditorGUIUtility.labelWidth = 75;
            choices.Button_Text[x] = EditorGUILayout.TextField("Button Text:", choices.Button_Text[x]);
            EditorGUIUtility.labelWidth = default_label_width;
            switch (choices.Requirement_Type[x])
            {
                case Choice_Stat_Requirement.No_Requirement:
                    break;
                case Choice_Stat_Requirement.Float_Stat_Requirement:
                    GUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 40;
                    choices.Stat_Name[x] = EditorGUILayout.TextField("Stat:", choices.Stat_Name[x]);//, GUILayout.Width(200));
                    choices.Float_Stat_Is[x] = (Float_Stat_Comparator)EditorGUILayout.EnumPopup("is", choices.Float_Stat_Is[x]);
                    choices.Float_Compare_Value[x] = EditorGUILayout.FloatField(choices.Float_Compare_Value[x]);
                    GUILayout.EndHorizontal();
                    EditorGUIUtility.labelWidth = default_label_width;

                    choices.Requirement_Not_Met_Actions[x] = (Requirement_Not_Met_Action)EditorGUILayout.EnumPopup("Requirement not met?", choices.Requirement_Not_Met_Actions[x]);
                    switch (choices.Requirement_Not_Met_Actions[x])
                    {
                        case Requirement_Not_Met_Action.Disable_Button:
                            choices.Disabled_Text[x] = EditorGUILayout.TextField("Disabled Button Text:", choices.Disabled_Text[x]);
                            break;
                        case Requirement_Not_Met_Action.Hide_Choice:
                            break;
                    }
                    break;
                case Choice_Stat_Requirement.Bool_Stat_Requirement:
                    GUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 40;
                    choices.Stat_Name[x] = EditorGUILayout.TextField("Stat:", choices.Stat_Name[x]);
                    choices.Bool_Compare_Value[x] = EditorGUILayout.Toggle("is", choices.Bool_Compare_Value[x]);
                    GUILayout.EndHorizontal();
                    EditorGUIUtility.labelWidth = default_label_width;

                    choices.Requirement_Not_Met_Actions[x] = (Requirement_Not_Met_Action)EditorGUILayout.EnumPopup("Requirement not met?", choices.Requirement_Not_Met_Actions[x]);
                    switch (choices.Requirement_Not_Met_Actions[x])
                    {
                        case Requirement_Not_Met_Action.Disable_Button:
                            EditorGUIUtility.labelWidth = 130;
                            choices.Disabled_Text[x] = EditorGUILayout.TextField("Disabled Button Text:", choices.Disabled_Text[x]);
                            EditorGUIUtility.labelWidth = default_label_width;
                            break;
                        case Requirement_Not_Met_Action.Hide_Choice:
                            break;
                    }
                    break;
                case Choice_Stat_Requirement.Object_Is_Null:
                    EditorGUIUtility.labelWidth = 45;
                    GUILayout.BeginHorizontal();
                    choices.Check_Null_Object[x] = (GameObject)EditorGUILayout.ObjectField("Object ", choices.Check_Null_Object[x], typeof(GameObject), true);
                    choices.Bool_Compare_Value[x] = EditorGUILayout.Toggle("is null: ", choices.Bool_Compare_Value[x]);
                    GUILayout.EndHorizontal();
                    break;
            }
            EditorGUIUtility.labelWidth = default_label_width;
            EditorGUILayout.LabelField("");
        }
        EditorGUIUtility.labelWidth = default_label_width;

        base.OnInspectorGUI();
    }
}