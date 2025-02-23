using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ExitGame(){
        Application.Quit();
        Debug.Log("Game Closed");
    }
    public void PlayGame(){
        SceneManager.LoadScene("Action Scene");
    }
}