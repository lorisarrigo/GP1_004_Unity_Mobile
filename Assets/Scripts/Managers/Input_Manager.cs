using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

//una classe che gestisce l'input (lo swipe) del giocatore
public class Input_Manager : MonoBehaviour
{
    [SerializeField] float minPixelSwipe; //distanza per compleetare uno swipe
    Vector2 initialRaycastPos; //punto dove tocchiamo lo schermo
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
        //controlla se stiamo toccando (fa partire il raycast) e trascinando il dito sullo schermo (e calcola il delta per eseguire lo swipe)
        if (Touch.activeTouches.Count <= 0) return;

        Touch touch = Touch.activeTouches[0];

        if (touch.phase == TouchPhase.Began)
        {
            initialRaycastPos = touch.screenPosition;
            isMoved = false;
        }

        if (touch.phase == TouchPhase.Moved && !isMoved)
        {
            Vector2 swipeDelta = touch.delta;
            if (swipeDelta.magnitude > minPixelSwipe)
            {
                Swipe(swipeDelta);
                isMoved = true;
            }
        }
    }
    //lo swipe
    void Swipe(Vector2 swipeDir)
    {
        //calcola la direzione
        Vector2Int gridPos = Vector2Int.zero;
        if (Mathf.Abs(swipeDir.x) > Mathf.Abs(swipeDir.y)) { gridPos.x = swipeDir.x > 0 ? 1 : -1; }
        else { gridPos.y = swipeDir.y > 0 ? 1 : -1; }
        
        Ray ray = Camera.main.ScreenPointToRay(initialRaycastPos);
        RaycastHit hit;
        //controlla cosa colpisce il raycast, se colpisce una fetta prova a eseguire la mossa nella direzione dello swipe
        if (Physics.Raycast(ray, out hit))
        {
            Slice hitSlice = hit.collider.GetComponentInParent<Slice>();
            if (hitSlice != null) gm.TryMove(hitSlice.gridPos, gridPos); 
        }
        else return;
    }
}
