using UnityEngine;
using System.Collections.Generic;
//classe che esegue l'azione o fa l'Undo
public class MoveCommand : ICommand
{
    //variabili per immagazzinare le informazioni delle celle
    List<Slice> move;
    Vector2Int originalCell;
    Vector2Int destinationCell;
    Vector2Int directionMove;
    Vector3 destAnchor;
    Vector3 undoAnchor;
    int heightDest;
    float heightSlice;
    //costruttore che esegue la mossa prendendo i dati dal Game_Manager
    public MoveCommand(List<Slice> originalStack, Vector2Int origin, Vector2Int destination, Vector2Int direction, Vector3 anchor, int destHeight, float Height)
    {
        move = new List<Slice>(originalStack);
        originalCell = origin;
        destinationCell = destination;
        directionMove = direction;

        destAnchor = anchor;
        heightDest = destHeight;
        heightSlice = Height;

        if (originalStack.Count > 0) undoAnchor = originalStack[0].transform.position;
        Command_manager.instance.AddCommand(this);
    }
    //l'eseguitore della mossa
    public void Excute()
    {
        List<Slice> reverseStack = new (move);
        reverseStack.Reverse();
        for(int i = 0; i< reverseStack.Count; i++)
        {
            reverseStack[i].Flip(destinationCell, destAnchor, heightDest, i, directionMove, heightSlice);
        }
    }
    //l'Undo 
    public void Undo()
    {
        Grid_Manager grid = Object.FindAnyObjectByType<Grid_Manager>();
        List<Slice> originalStack = grid.GetStackPos(originalCell);

        Vector3 backAnchor;
        int undoHeight = 0;
        if(originalStack.Count > 0)
        {
            backAnchor = originalStack[0].tPos;
            undoHeight = originalStack.Count;
        }
        else
        {
            backAnchor = undoAnchor;
            backAnchor.y = 0;
        }
        Vector2Int invDir = -directionMove;
        for(int i = 0; i< move.Count; i++)
        {
            move[i].Flip(originalCell, backAnchor, undoHeight, i, invDir, heightSlice);
        }
    }
}
