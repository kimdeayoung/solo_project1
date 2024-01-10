using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private RectTransform leverArea;
    [SerializeField]
    private RectTransform lever;

    private Vector3 leverInitialPos;
    private float areaRadius;

    private Vector2 leverPos;

    private bool isTouchJoyStick;

    
    public Vector2 LeverPos { get => leverPos; }
    public bool IsTouchJoyStick { get => isTouchJoyStick; }

    private void Awake()
    {
        Assert.IsNotNull(leverArea);
        Assert.IsNotNull(lever);

        leverInitialPos = lever.transform.position;
        areaRadius = leverArea.rect.width * 0.5f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouchJoyStick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouchJoyStick = false;

        lever.transform.position = leverInitialPos;
        leverPos = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        leverPos = eventData.position - (Vector2)leverArea.position;
        leverPos = Vector2.ClampMagnitude(leverPos, areaRadius);// 조이스틱 이동범위를 넘지않도록 조정
        lever.localPosition = leverPos;
    }
}
