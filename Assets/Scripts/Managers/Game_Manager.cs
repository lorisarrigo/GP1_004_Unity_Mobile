using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//classe che gestisce il GameFlow principale
public class Game_Manager : MonoBehaviour
{
    [SerializeField] float winTimer; //tempo usato nella coroutine per non cambiare immediatamente la scena
    Grid_Manager grid;
    List<Slice> all;
    void Awake()
    {
        grid = FindAnyObjectByType<Grid_Manager>();
    }
    private void Start()
    {
        grid.InitializeGrid(); //prende le coordinate dei pezzi "Slice" che si trovano in scena e li ordina creando una griglia immaginaria
        all = grid.GetAllSlice();
    }
    //Metodo richiamato nell'input Manager per eseguire una mossa
    public void TryMove(Vector2Int origin, Vector2Int direction)
    {
        //recupera le informazioni di destinazione e le liste delle 2 pile di ingredienti di questa mossa (quella di partenza e di arrivo)
        Vector2Int destination = origin + direction;
        List<Slice> originalStack = grid.GetStackPos(origin);
        List<Slice> destinationStack = grid.GetStackPos(destination);
        //nel caso sono vuote non esegue la mossa, se nella pila di inizio il tipo di ingrediente è un pane ed è da solo non esegue la mossa
        if (originalStack.Count == 0 || destinationStack.Count == 0) return;
        if (originalStack[0].type == SType.Bread && originalStack.Count == 1) return;

        //controlla se l'ingrediente è un pane, nel caso lo è e gli ingredienti non sono stati impilati sui pani, non esegue la mossa
        bool moveBread = false;
        foreach (Slice sl in originalStack)
        {
            if (sl.type == SType.Bread) moveBread = true;
        }
        if (moveBread && !MoveBreadAndSlice()) return;

        //setta le nuove reference per eseguire la mossa nel costruttore
        Vector3 anchor = destinationStack[0].transform.position;
        int stackHeight = destinationStack.Count;
        float height = grid.sliceHeight;

        MoveCommand newMove = new(originalStack, origin, destination, direction, anchor, stackHeight, height);

        //controlla se hai finito il livello, nel caso vai al livello successivo
        Win();
    }
    
    //controlla se ci sono fette nella griglia, nel caso ci sono e non sono pani ritorna false 
    bool MoveBreadAndSlice()
    {
        List<Vector2Int> slicePos = new();
        foreach (Slice sl in all)
        {
            List<Slice> currentStack = grid.GetStackPos(sl.gridPos);
            if (currentStack[0].type != SType.Bread) return false;
        }
        return true;
    }
    //permette di andare al livello successivo
    void Win()
    {
        //se tutte le fette sono impilate in mezzo alle 2 di pane allora fa partire la Coroutine
        if (all.Count == 0) return;
        Vector2Int firstPos = all[0].gridPos;

        bool allInOne = true;
        foreach (Slice sl in all)
        {
            if (sl.gridPos != firstPos) allInOne = false;
        }
        if (!allInOne) return;
        List<Slice> finalStack = grid.GetStackPos(firstPos);

        if (finalStack[0].type == SType.Bread && finalStack[finalStack.Count - 1].type == SType.Bread) StartCoroutine(WinTransition());
    }
    //aspetta il tempo dichiarato nell'inspector e poi cambia scena
    IEnumerator WinTransition()
    {
        yield return new WaitForSeconds(winTimer);

        UI_Manager UI = FindAnyObjectByType<UI_Manager>();
        if (UI != null) UI.SkipLvl();
    }
}
