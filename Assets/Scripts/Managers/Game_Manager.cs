using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    Grid_Manager grid;
    void Awake()
    {
        grid = FindAnyObjectByType<Grid_Manager>();

        grid.InitializeGrid();
    }

    public void TryMove(Vector2Int origin, Vector2Int direction)
    {
        Vector2Int destination = origin + direction;
        List<Slice> originalStack = grid.GetStackPos(origin);
        List<Slice> destinationStack = grid.GetStackPos(destination);
        if (originalStack.Count == 0 || destinationStack.Count == 0) return;

        if (originalStack[0].type == SType.Bread)
        {
            if (originalStack.Count == 1) return;
        }

        bool moveBread = false;
        foreach (Slice sl in originalStack)
        {
            if(sl.type == SType.Bread) moveBread = true;
        }
        if (moveBread)
        {
            if (!MoveBreadAndSlice())
            {
                Debug.Log("mossa bloccata: aggiungere ingredienti");
                return;
            }
        }
        Vector3 anchor = destinationStack[0].transform.position;
        int stackHeight = destinationStack.Count;
        float height = grid.sliceHeight;

        MoveCommand newMove = new (originalStack, origin, destination, direction, anchor, stackHeight, height);

        Win();
    }
    bool MoveBreadAndSlice()
    {
        List<Slice> all = grid.GetAllSlice();
        List<Vector2Int> BreakCell = new();
        foreach (Slice sl in all)
        {
            if(sl.type != SType.Bread)
            {
                if(!BreakCell.Contains(sl.gridPos)) BreakCell.Add(sl.gridPos);
            }

        }
        foreach (Slice sl in all)
        {
            if(sl.type != SType.Bread)
            {
                if (!BreakCell.Contains(sl.gridPos)) return false;
            }
        }
        return true; 
    }
    void Win()
    {
        List<Slice> all = grid.GetAllSlice();
        if(all.Count == 0) return;
        Vector2Int firstPos = all[0].gridPos;

        bool allInOne = true;
        foreach(Slice sl in all)
        {
            if(sl.gridPos != firstPos) allInOne = false;
        }
        if (!allInOne) return;
        List<Slice> finalStack = grid.GetStackPos(firstPos);
        
        if (finalStack[0].type == SType.Bread && finalStack[finalStack.Count - 1].type == SType.Bread) { Debug.Log("Lvl complete"); }
    }
}
