using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    #region SerializedFields
    [SerializeField] private float handleRange = 1;
    [SerializeField] private float _deadZone = 0;
    [SerializeField] private AxisOptions _axisOptions = AxisOptions.Both;
    [SerializeField] private bool _snapX = false;
    [SerializeField] private bool _snapY = false;
    [SerializeField] protected RectTransform background = null;
    [SerializeField] private RectTransform _handle = null;
    #endregion
    #region References
    private RectTransform _baseRect = null;
    private Canvas _canvas;
    private Camera _cam;
    public Vector2 input = Vector2.zero;
    #endregion
    #region Properties
    public bool SnapX { get => _snapX; set => _snapX = value; }
    public bool SnapY { get => _snapY; set => _snapY = value; }
    public  float HandleRange { get => handleRange; set => handleRange = Mathf.Abs(value); }
    public float DeadZone { get => _deadZone; set => _deadZone = Mathf.Abs(value); }
    public AxisOptions AxisOption { get => _axisOptions; set => _axisOptions = value; }
    public float Horizontal { get => SnapX ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x; }
    public float Vertical { get => SnapY ? SnapFloat(input.y, AxisOptions.Vertical) : input.y; }
    public Vector2 Direction { get => new Vector2(Horizontal, Vertical); }
    #endregion
    #region Interface
    protected virtual void Start()
    {
        //not sure of atm...
        HandleRange = handleRange;
        DeadZone = _deadZone;

        //this script will be attached to the Background Image
        _baseRect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
        {
            Debug.LogError("The Joystick is not placed inside the canvas! or the script is not on the background image!");
            
        }
        Vector2 center = new Vector2(0.5f,0.5f);
        background.pivot = center;
        _handle.anchorMin = center;
        _handle.anchorMax = center;
        _handle.pivot = center;
        _handle.anchoredPosition = Vector2.zero;
    }
    private float SnapFloat(float value, AxisOptions snapAxis)
    {
        if (value == 0)
        {
            return value;
        }
        if (AxisOption == AxisOptions.Both)
        {
            float angle = Vector2.Angle(input, Vector2.up);
            if (snapAxis == AxisOptions.Horizontal)
            {
                if (angle < 22.5f || angle > 157.5f)
                {
                    return 0f;
                }
                else
                {
                    return (value > 0) ? 1 : -1;
                }
            }
            if (snapAxis == AxisOptions.Vertical)
            {
                if (angle < 67.5f || angle > 112.5f)
                {
                    return 0f;
                }
                else
                {
                    return (value > 0) ? 1 : -1;
                }
            }

            return value;
        }
        else
        {
            if (value > 0)
            {
                return 1;
            }

            if (value < 0)
            {
                return -1;
            }
        }
        return 0;
    }
    private void FormatInput()
    {
        if (AxisOption == AxisOptions.Horizontal)
        {
            input = new Vector2(input.x, 0f);
        }
        else if (AxisOption == AxisOptions.Vertical)
        {
            input = new Vector2(0f, input.y);
        }
    }
    protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > DeadZone)
        {
            if (magnitude > 1)
            {
                input = normalised;
            }
        }
        else
        {
            input = Vector2.zero;
        }
    }
    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, screenPosition, _cam, out localPoint))
        {
            return localPoint - (background.anchorMax * _baseRect.sizeDelta);
        }
        return Vector2.zero;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_cam = null)
        {
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                _cam = _canvas.worldCamera;
            }
            else
            {
                _cam = Camera.main;
            }
        }
        
        Vector2 position = RectTransformUtility.WorldToScreenPoint(_cam, background.position);
        Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * _canvas.scaleFactor);
        HandleInput(input.magnitude, input.normalized, radius, _cam);
        _handle.anchoredPosition = input * radius * HandleRange;
        //if this plays up we will be changing HandleRange to _handleRange
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
    }
    #endregion

    public enum AxisOptions
    {
        Both, Horizontal, Vertical
    }
}
