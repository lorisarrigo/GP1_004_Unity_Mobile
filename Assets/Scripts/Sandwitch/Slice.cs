using UnityEngine;

public enum SType { Bread, Meat, Lettuce, Cheese}
public class Slice : MonoBehaviour
{
    public SType type;
    public Vector2Int gridPos;
    public Vector3 tPos;
    public Quaternion tRot;

    public float speed;

    void Awake()
    {
        tPos = transform.position;
        tRot = transform.rotation;
    }
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, tPos, Time.deltaTime * speed);
        transform.rotation = Quaternion.Lerp(transform.rotation, tRot, Time.deltaTime * speed);
    }
    public void NewTarget(Vector3 newPos, Quaternion newRot)
    {
        tPos = newPos;
        tRot = newRot;
    }
    public void Flip(Vector2Int newCell, Vector3 anchor, int stackHeight, int stackIndex, Vector2Int direction, float width)
    {
        this.gridPos = newCell; ;

        Vector3 rotationAxis = direction.x != 0 ? Vector3.forward : Vector3.right;

        float newY = anchor.y + (stackHeight + stackIndex) * width;
        Vector3 newPosTarget = new(anchor.x, newY, anchor.z);

        Quaternion newRotTarget = Quaternion.AngleAxis(180, rotationAxis) * tRot;

        NewTarget(newPosTarget, newRotTarget);
    }
}
