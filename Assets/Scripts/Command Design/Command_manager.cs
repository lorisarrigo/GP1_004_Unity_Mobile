using System.Collections.Generic;
using UnityEngine;

//classe che gestisce l'Undo
public class Command_manager : MonoBehaviour
{
    Stack<ICommand> undoStack = new();
    //bool per l'interagibilità
    [HideInInspector] public bool hasMoved = false;
    [HideInInspector] public bool canUndo = false;

    public static Command_manager instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }
    //aggiorna la stack di azioni
    public void AddCommand(ICommand cmd)
    {
        undoStack.Clear();
        hasMoved = true;
        canUndo = true;
        undoStack.Push(cmd);
        cmd.Excute();
    }
    //torna indietro di un'azione e rende non interagibile l'Undo
    public void UndoCommand()
    {
        if (undoStack.Count <= 0) return;
        undoStack.Pop().Undo();
        canUndo = false;
    }
}
