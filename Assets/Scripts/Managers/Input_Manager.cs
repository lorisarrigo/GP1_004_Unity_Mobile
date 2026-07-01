using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
public class Input_Manager : MonoBehaviour
{
    public float minPixelSwipe;
    Vector2 initialRaycastPos;
    bool isMoved = false;
    Game_Manager gm;

    void Awake()
    {
        gm = FindAnyObjectByType<Game_Manager>();
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }
    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        if (Touch.activeTouches.Count <= 0) return;

        Touch touch = Touch.activeTouches[0];

        if (touch.phase == TouchPhase.Began)
        {
            Debug.Log(" Schermo toccato");
            initialRaycastPos = touch.screenPosition;
            isMoved = false;
        }

        if (touch.phase == TouchPhase.Moved && !isMoved)
        {
            Vector2 swipeDelta = touch.delta;
            Debug.Log($" dito in movimento. Delta: {swipeDelta.magnitude}");
            if (swipeDelta.magnitude > minPixelSwipe)
            {
                Debug.Log("swipe rilevato oltre la soglia");
                DeltaMove(swipeDelta);
                isMoved = true;
            }
        }
    }

    void DeltaMove(Vector2 delta)
    {
        Vector2Int gridDir = Vector2Int.zero;
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) { gridDir.x = delta.x > 0 ? 1 : -1; }
        else { gridDir.y = delta.y > 0 ? 1 : -1; }

        Ray ray = Camera.main.ScreenPointToRay(initialRaycastPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Slice hitSlice = hit.collider.GetComponentInParent<Slice>();
            if (hitSlice != null)
            {
                Debug.Log($" raycast ha colpito la fetta: {hitSlice.gameObject.name} in posizione {hitSlice.gridPos}");
                gm.TryMove(hitSlice.gridPos, gridDir);
            }
            else
            {
                Debug.Log("Raycast ha colpito qualcosa");
            }
        }
        else Debug.Log("RayCast nel vuoto, nessun collider colpito");
    }
}
