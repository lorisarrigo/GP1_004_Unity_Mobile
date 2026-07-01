using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] string nextLvl;
    [SerializeField] Button UndoBtn, ResetBtn;
    

    private void Update()
    {
        ResetBtn.interactable = Command_manager.instance.hasMoved;
        UndoBtn.interactable = Command_manager.instance.canUndo;
    }
    public void NextLvl()
    {
        SceneManager.LoadScene(nextLvl);
    }
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
