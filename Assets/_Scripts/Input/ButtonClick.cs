using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _pointerDown;
    private bool PointerDown
    {
        get
        {
            return _pointerDown;
        }
        set
        {
            _pointerDown = value;
            InputController.Instance.PointerDown = _pointerDown;
        }
    }

    [SerializeField] private Image _image;
    [SerializeField] private Color _color;
    [SerializeField] private Color _beforeClickedColor;

    public UnityEvent OnButtonHold;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("pointerdown");
        PointerDown = true;
        _image.color = _color;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
    }

    private void Update()
    {
        if (PointerDown)
        {
            OnButtonHold.Invoke();
        }
    }

    private void Reset()
    {
        _image.color = _beforeClickedColor;
        PointerDown = false;
    }
}
