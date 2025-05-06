using UnityEngine;

public class ArrangeUIInCircle : MonoBehaviour
{
    public RectTransform[] items;
    public float radius = 400f;
    public Vector2 centerOffset = Vector2.zero;

    void Start()
    {
        ArrangeInCircle();
    }

    void ArrangeInCircle()
    {
        if (items.Length == 0) return;

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 center = screenCenter + centerOffset;

        float angleStep = 360f / items.Length;

        for (int i = 0; i < items.Length; i++)
        {
            float angle = angleStep * i;
            Vector2 position = GetPositionOnCircle(angle, radius, center);

            items[i].position = position;

            Vector2 directionToCenter = (center - position).normalized;
            float rotationAngle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
            items[i].localRotation = Quaternion.Euler(0, 0, rotationAngle - 90f);
        }
    }

    Vector2 GetPositionOnCircle(float angleDegrees, float radius, Vector2 center)
    {
        float angleRad = angleDegrees * Mathf.Deg2Rad;
        return new Vector2(
            center.x + Mathf.Cos(angleRad) * radius,
            center.y + Mathf.Sin(angleRad) * radius
        );
    }

}