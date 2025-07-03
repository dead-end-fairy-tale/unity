using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputReceiver : MonoBehaviour
{
    public Vector3 position { get; private set; }
    public Vector3 dir { get; private set; }
    public bool tapPressed { get; private set; }

    public event Action<Vector3> OnTapPosition;
    
    private GameInput inputActions;
    private Vector2 touchPos;

    void Awake()
    {
        inputActions = new GameInput();

        inputActions.MobileGamePlay.Point.performed += ctx => touchPos = ctx.ReadValue<Vector2>();
        inputActions.MobileGamePlay.Point.canceled  += ctx => touchPos = Vector2.zero;

        inputActions.MobileGamePlay.Tap.performed   += _ => tapPressed = true;
    }

    void OnEnable()  => inputActions.MobileGamePlay.Enable();
    void OnDisable() => inputActions.MobileGamePlay.Disable();

    void Update()
    {
        if (tapPressed)
        {
            if (IsTouchOverUI())
            {
                tapPressed = false;
                return;
            }
            
            MoveToTap(touchPos);
            tapPressed = false;
        }
    }

    private void MoveToTap(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            position = hit.point;
            dir    = (position - transform.position).normalized;
            
            OnTapPosition?.Invoke(position);
        }
    }
    
    private bool IsTouchOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}