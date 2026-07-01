using System.Collections.Generic;
using UnityEngine;

public class Command_manager : MonoBehaviour
{
    private Stack<ICommand> undoStack = new();
    public static Command_manager instance;
    public bool hasMoved = false;
    public bool canUndo = false;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        hasMoved = false;
    }
    public void AddCommand(ICommand cmd)
    {
        undoStack.Clear();
        Debug.Log("stack pulita da altre mosse");
        hasMoved = true;
        canUndo = true;
        undoStack.Push(cmd);
        cmd.Excute();
    }
    public void UndoCommand()
    {
        if (undoStack.Count <= 0) return;
        undoStack.Pop().Undo();
        canUndo = false;
    }
}
