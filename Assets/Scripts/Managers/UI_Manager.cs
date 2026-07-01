using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//una classe che gestisce i bottoni in scena
public class UI_Manager : MonoBehaviour
{
    [SerializeField] string nextLvl;
    [SerializeField] Button UndoBtn, ResetBtn;
    
    public static UI_Manager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }
    private void Update()
    {
        //rendo interagibili/non interagibili i bottoni
        if (UndoBtn != null && ResetBtn & Command_manager.instance != null)
        {
            ResetBtn.interactable = Command_manager.instance.hasMoved;
            UndoBtn.interactable = Command_manager.instance.canUndo;
        }
    }
    //queste sono le funzioni che metto nei bottoni di Skip, Reset e Quit;
    public void SkipLvl()
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
