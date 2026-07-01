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

        bool moveBread = false;
        foreach (Slice sl in originalStack)
        {
            if(sl.type == SType.Bread) moveBread = true;
        }
        if (moveBread)
        {
            if (!moveBreadAndSlice())
            {
                Debug.Log("mossa bloccata: aggiungere ingredienti");
                return;
            }
        }
        Vector3 anchor = destinationStack[0].transform.position;
        int stackHeight = destinationStack.Count;
        float width = grid.sliceWidth;

        List<Slice> reverseStack = new(originalStack);
        reverseStack.Reverse();
        for (int i = 0; i < reverseStack.Count; i++)
        {
            reverseStack[i].Flip(destination, anchor, stackHeight, i, direction, width);
        }

        Win();
    }
    bool moveBreadAndSlice()
    {
        var all = grid.GetAllSlice();
        List<Slice> secondary = new ();
        foreach (Slice sl in all)
        {
            if(sl.type != SType.Bread) secondary.Add(sl);
        }
        if (secondary.Count == 0) return true;

        Vector2Int commonCell = secondary[0].gridPos;
        
        foreach (Slice sl in secondary)
        {
            if (sl.gridPos != commonCell) return false;
        }
        List<Slice> cellStack = grid.GetStackPos(commonCell);
        bool foundBread = false;
        foreach (Slice sl in cellStack)
        {
            if(sl.type == SType.Bread) foundBread = true;
        }
        return foundBread;
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
