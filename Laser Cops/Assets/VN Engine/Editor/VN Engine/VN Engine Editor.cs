using UnityEngine;
using System.Collections;
using UnityEditor;

// Creates a new menu when you right click in the Hierarchy pane.
// Allows the user to easily create dialogue elements
public class VNEngineEditor : MonoBehaviour
{
    [MenuItem("GameObject/VN Engine/Create DialogueCanvas", false, 0)]
    private static void CreateDialogueCanvas(MenuCommand menuCommand)
    {
        GameObject go = Instantiate(Resources.Load("VN Engine/DialogueCanvas", typeof(GameObject))) as GameObject;     // Create new object
        go.name = "DialogueCanvas";
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;
    }


    [MenuItem("GameObject/VN Engine/New Conversation", false, 0)]
    private static void CreateConversation(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Conversation");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ConversationManager>();
    }


    [MenuItem("GameObject/VN Engine/Add Dialogue", false, 0)]
    private static void AddDialogue(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Dialogue");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<DialogueNode>();
        go.AddComponent<AudioSource>();
        go.GetComponent<AudioSource>().playOnAwake = false;
    }


    [MenuItem("GameObject/VN Engine/Enter Actor", false, 0)]
    private static void EnterActor(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Enter Actor");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<EnterActorNode>();
    }


    [MenuItem("GameObject/VN Engine/Exit Actor", false, 0)]
    private static void ExitActor(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Exit Actor");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ExitActorNode>();
    }


    [MenuItem("GameObject/VN Engine/Exit All Actors", false, 0)]
    private static void ExitAllActors(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Exit All Actors");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ExitAllActorsNode>();
    }


    [MenuItem("GameObject/VN Engine/Change Conversation", false, 0)]
    private static void ChangeConversation(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Change Conversation");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ChangeConversationNode>();
    }


    [MenuItem("GameObject/VN Engine/Change Actor Image", false, 0)]
    private static void ChangeActorImage(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Change Actor Image");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ChangeActorImageNode>();
    }


    [MenuItem("GameObject/VN Engine/Wait", false, 0)]
    private static void Wait(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Wait");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<WaitNode>();
    }


    [MenuItem("GameObject/VN Engine/Set Background", false, 0)]
    private static void SetBackground(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Set Background");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<SetBackground>();
    }


    [MenuItem("GameObject/VN Engine/Fade in from Black", false, 0)]
    private static void FadeInFromBlack(MenuCommand menuCommand)
    {
        GameObject go = Instantiate(Resources.Load("VN Engine/Conversation Pieces/Fade in from Black", typeof(GameObject))) as GameObject;     // Create new object
        go.name = "Fade in from Black";
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;
    }


    [MenuItem("GameObject/VN Engine/Fade out to Black", false, 0)]
    private static void FadeToBlack(MenuCommand menuCommand)
    {
        GameObject go = Instantiate(Resources.Load("VN Engine/Conversation Pieces/Fade out to Black", typeof(GameObject))) as GameObject;     // Create new object
        go.name = "Fade out to Black";
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;
    }


    [MenuItem("GameObject/VN Engine/Show Choices", false, 0)]
    private static void ShowChoicesNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Show Choices");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ChoiceNode>();
    }


    [MenuItem("GameObject/VN Engine/Clear Text", false, 0)]
    private static void ClearTextNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Clear Text");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<ClearTextNode>();
    }


    [MenuItem("GameObject/VN Engine/Move Actor Inwards", false, 0)]
    private static void MoveActorInwardsNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Move Actor Inwards");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<MoveActorInwardsNode>();
    }


    [MenuItem("GameObject/VN Engine/Move Actor Outwards", false, 0)]
    private static void MoveActorOutwardsNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Move Actor Outwards");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<MoveActorOutwardsNode>();
    }


    [MenuItem("GameObject/VN Engine/Bring Actor Forward", false, 0)]
    private static void BringActorForwardNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Bring Actor Forward");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<BringActorForwardNode>();
    }


    [MenuItem("GameObject/VN Engine/Bring Actor Backward", false, 0)]
    private static void BringActorBackwardNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Bring Actor Backward");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<BringActorBackwardNode>();
    }


    [MenuItem("GameObject/VN Engine/Darken Actor", false, 0)]
    private static void DarkenActorNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Darken Actor");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<DarkenActorNode>();
    }


    [MenuItem("GameObject/VN Engine/Brighten Actor", false, 0)]
    private static void BrightenActorNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Brighten Actor");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<BrightenActorNode>();
    }


    [MenuItem("GameObject/VN Engine/Flip Actor", false, 0)]
    private static void FlipActorFacingNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Flip Actor");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<FlipActorFacingNode>();
    }


    [MenuItem("GameObject/VN Engine/Play Sound Effect", false, 0)]
    private static void PlaySoundEffectNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Play Sound Effect");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<PlaySoundEffectNode>();
        go.AddComponent<AudioSource>();
        go.GetComponent<AudioSource>().playOnAwake = false;
    }


    [MenuItem("GameObject/VN Engine/Play Music", false, 0)]
    private static void PlayMusicNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Play Music");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<SetMusicNode>();
    }


    [MenuItem("GameObject/VN Engine/Fade out Music", false, 0)]
    private static void FadeOutMusicNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Fade out Music");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<SetMusicNode>();
        go.GetComponent<SetMusicNode>().fadeOutPreviousMusic = true;
        go.GetComponent<SetMusicNode>().fadeOutTime = 5.0f;
    }

    [MenuItem("GameObject/VN Engine/Load Scene", false, 0)]
    private static void LoadSceneNode(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Load Scene");     // Create new object
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject); // Parent the new object
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);    // Register the creation in the undo system
        Selection.activeObject = go;

        go.AddComponent<LoadSceneNode>();
    }
}
