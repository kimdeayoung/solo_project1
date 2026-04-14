using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform leverArea;
    [SerializeField] private RectTransform lever;

    private Vector3 leverInitialPos;
    private float areaRadius;
    private float sqrAreaRadius;

    private Vector2 leverPos;
    private float moveIntensity;

    public event Action<Vector2, float> OnUpdateDirection;

    private void Start()
    {
        Debug.Assert(leverArea != null);
        Debug.Assert(lever != null);

        leverInitialPos = lever.transform.localPosition;
        areaRadius = leverArea.rect.width * 0.5f;

        sqrAreaRadius = areaRadius * areaRadius;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ComputeLeverTransform(eventData);
        OnUpdateDirection?.Invoke(leverPos.normalized, moveIntensity);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        lever.transform.localPosition = leverInitialPos;

        OnUpdateDirection?.Invoke(Vector3.zero, 0f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        ComputeLeverTransform(eventData);
        OnUpdateDirection?.Invoke(leverPos.normalized, moveIntensity);
    }

    private void ComputeLeverTransform(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(leverArea, eventData.position, eventData.pressEventCamera, out leverPos);
        leverPos = Vector2.ClampMagnitude(leverPos, areaRadius);// 조이스틱 이동범위를 넘지않도록 조정
        lever.localPosition = leverPos;

        moveIntensity = leverPos.sqrMagnitude / sqrAreaRadius;
    }
}
