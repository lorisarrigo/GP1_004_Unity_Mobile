using UnityEngine;
using System.Collections.Generic;
public class Grid_Manager : MonoBehaviour
{
    
    public float sliceHeight;
    List<Slice> allSlice = new();
    public void InitializeGrid()
    {
        allSlice = new List<Slice>(FindObjectsByType<Slice>());

        List<float> xCoord = new();
        List<float> zCoord = new();
        foreach (Slice sl in allSlice)
        {
            if(!xCoord.Contains(sl.transform.position.x)) xCoord.Add(sl.transform.position.x);
            if(!zCoord.Contains(sl.transform.position.z)) zCoord.Add(sl.transform.position.z);
        }
        xCoord.Sort();
        zCoord.Sort();
        List<float> xGrid = GroupAndFilter(xCoord);
        List<float> zGrid = GroupAndFilter(zCoord);

        foreach (Slice sl in allSlice)
        {
            int xLogic = FindIndex(xGrid, sl.transform.position.x);
            int zLogic = FindIndex(zGrid, sl.transform.position.z);
            sl.gridPos = new Vector2Int(xLogic,zLogic);
        }
    }
    List<float> GroupAndFilter(List<float> coordinates)
    {
        List<float> coord = new();
        foreach (float cord in coordinates)
        {
            bool close = false;
            foreach (float c in coord)
            {
                if (Mathf.Abs(c - cord)<0.5f) close = true;
                break;
            }
            if(!close) coord.Add(cord);
        }

        coord.Sort();
        return coord;
    }
    int FindIndex(List<float> Index, float val)
    {
        for(int i = 0; i<Index.Count; i++)
        {
            if (Mathf.Abs(Index[i] - val) < 0.5f) return i;
        }
        return 0;
    }
    public List<Slice> GetStackPos(Vector2Int stPos) 
    {
        List<Slice> result = new();
        foreach(Slice sl in allSlice)
        {
            if (sl.gridPos == stPos) result.Add(sl);
        }
        result.Sort((a,b)=> a.tPos.y.CompareTo(b.tPos.y));
        return result;
    }

    public List<Slice> GetAllSlice() => allSlice;
}
