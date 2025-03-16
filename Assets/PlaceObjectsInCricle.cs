using UnityEngine;

public class ArrangeUIInCircle : MonoBehaviour
{
    public RectTransform[] items; // Sloty do rozmieszczenia
    public RectTransform panel;   // Panel, w którym ma byæ okr¹g
    public float radiusOffset = 0.4f; // Skala promienia wzglêdem panelu

    void Start()
    {
        ArrangeInCircle();
    }

    void ArrangeInCircle()
    {
        if (panel == null || items.Length == 0) return;

        // Oblicz œrodek panelu w lokalnych wspó³rzêdnych
        Vector2 panelCenter = Vector2.zero; // Œrodek panelu (zak³adaj¹c, ¿e pivot = (0.5, 0.5))
        if (panel.pivot != new Vector2(0.5f, 0.5f))
        {
            panelCenter = new Vector2(panel.rect.width * (0.5f - panel.pivot.x),
                                      panel.rect.height * (0.5f - panel.pivot.y));
        }

        // Oblicz promieñ wzglêdem panelu
        float radius = Mathf.Min(panel.rect.width, panel.rect.height) * radiusOffset;

        float angleStep = 360f / items.Length; // Równy podzia³ k¹ta

        for (int i = 0; i < items.Length; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad; // Konwersja na radiany

            Vector2 position = new Vector2(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius
            );

            // Ustawienie pozycji wzglêdem œrodka panelu
            items[i].anchoredPosition = panelCenter + position;
        }
    }
}
