using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRouter : MonoBehaviour
{
    public const string MenuScene = "Menu";
    public const string FirstLevelScene = "Level01";
    public const string SecondLevelScene = "Level02";

    public void LoadMenu() => SceneManager.LoadScene(MenuScene);
    public void LoadFirstLevel() => SceneManager.LoadScene(FirstLevelScene);

    public void QuitGame()
    {
        Debug.Log("quit requested");
        Application.Quit();
    }
}
