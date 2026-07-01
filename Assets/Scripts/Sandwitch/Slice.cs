using UnityEngine;
public enum SType { Bread, Other}
//classe per gestire le singole Slice
public class Slice : MonoBehaviour
{
    public SType type;
    public float speed;
    [HideInInspector] public Vector2Int gridPos;
    [HideInInspector] public Vector3 tPos;

    Quaternion tRot;

    void Awake()
    {
        tPos = transform.position;
        tRot = transform.rotation;
    }
    //apena gli arrivano le nuove posizione esegue i Lerp
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, tPos, Time.deltaTime * speed);
        transform.rotation = Quaternion.Lerp(transform.rotation, tRot, Time.deltaTime * speed);
    }
    //sostituisce le vecchie posizioni con le nuove posizioni
    public void NewTarget(Vector3 newPos, Quaternion newRot)
    {
        tPos = newPos;
        tRot = newRot;
    }
    //flippa la Slice prendendo i riferimenti nel momento in cui eseguiamo l'azione
    public void Flip(Vector2Int newCell, Vector3 anchor, int stackHeight, int stackIndex, Vector2Int direction, float height)
    {
        this.gridPos = newCell;

        Vector3 rotationAxis = direction.x != 0 ? Vector3.forward : Vector3.right;

        float newY = anchor.y + (stackHeight + stackIndex) * height;
        Vector3 newPosTarget = new(anchor.x, newY, anchor.z);

        Quaternion newRotTarget = Quaternion.AngleAxis(180, rotationAxis) * tRot;

        NewTarget(newPosTarget, newRotTarget);
    }
}
