using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Vector3 targetPosition;
    public RectTransform pointerRectTransform;
    public GameObject objective;

    [SerializeField]
    public Camera uiCamera;

    private void Awake()
    {
        targetPosition = objective.transform.position;
        pointerRectTransform = transform.Find("Arrow").GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0.0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        pointerRectTransform.localEulerAngles = new Vector3(0f, 0f, angle);
        Vector3 targetPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool offScreen = targetPoint.x <= 0 || targetPoint.x >= Screen.width || targetPoint.y <= 0 || targetPoint.y >= Screen.height;

        if (offScreen)
        {
            Vector3 capped = targetPoint;
            if (capped.x <= 0) capped.x = 0f;
            if (capped.x >= Screen.width) capped.x = Screen.width;
            if (capped.y <= 0) capped.y = 0f;
            if (capped.y >= Screen.width) capped.y = Screen.height;

            Vector3 worldPos = uiCamera.ScreenToWorldPoint(capped);
            pointerRectTransform.position = worldPos;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0.0f);
        }
    }
}
