using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Command_manager : MonoBehaviour
{
    Stack<ICommand> undoStack = new();
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
