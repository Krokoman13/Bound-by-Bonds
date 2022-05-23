using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneToolbar : Editor
{
    [MenuItem("LeadTheWay/Scenes/Sep/Scene1")]
    static void SepScene1()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) //Ask user if they want to save
            EditorSceneManager.OpenScene("Assets/_USERS/Sep/SepScene1.unity", OpenSceneMode.Single);
    }


    [MenuItem("LeadTheWay/Scenes/Quint/Scene1")]
    static void QuintScene1()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) //Ask user if they want to save
            EditorSceneManager.OpenScene("Assets/_USERS/Quint/Scene/Texture_Test_Scene.unity", OpenSceneMode.Single);
    }


    [MenuItem("LeadTheWay/Scenes/Bram/Scene1.unity")]
    static void BramScene1()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) //Ask user if they want to save
            EditorSceneManager.OpenScene("Assets/_USERS/Bram/BramScene.unity", OpenSceneMode.Single);
    }
}