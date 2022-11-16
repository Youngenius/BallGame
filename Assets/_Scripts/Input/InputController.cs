using System;
using UnityEngine;

public class InputController : Singleton<InputController>
{
    private bool _canGetButtonUp;
    [HideInInspector] public bool PointerDown;

    public event Action OnMouseButtonDown;
    public event Action OnMouseButtonUp;

    private void Update()
    {
#if UNITY_EDITOR


        if (!PointerDown && GameFlowController.Instance.CurrentState == GameFlowController.State.Waiting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseButtonDown?.Invoke();
                _canGetButtonUp = true;
            }
        }

        if (_canGetButtonUp)
        {
            if (Input.GetMouseButtonUp(0))
            {
                OnMouseButtonUp?.Invoke();
                _canGetButtonUp = false;
            }
        }
#endif

        if (!PointerDown && GameFlowController.Instance.CurrentState == GameFlowController.State.Waiting)
        {
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                OnMouseButtonDown?.Invoke();
                _canGetButtonUp = true;
            }
        }

        if (_canGetButtonUp)
        {
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
            {
                OnMouseButtonUp?.Invoke();
                _canGetButtonUp = false;
            }
        }
    }
}
