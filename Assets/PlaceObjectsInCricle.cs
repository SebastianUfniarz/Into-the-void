using UnityEngine;

public class ArrangeUIInCircle : MonoBehaviour
{
    public RectTransform[] items; // Sloty do rozmieszczenia
    public RectTransform panel;   // Panel, w kt�rym ma by� okr�g
    public float radiusOffset = 0.4f; // Skala promienia wzgl�dem panelu

    void Start()
    {
        ArrangeInCircle();
    }

    void ArrangeInCircle()
    {
        if (panel == null || items.Length == 0) return;

        // Oblicz �rodek panelu w lokalnych wsp�rz�dnych
        Vector2 panelCenter = Vector2.zero; // �rodek panelu (zak�adaj�c, �e pivot = (0.5, 0.5))
        if (panel.pivot != new Vector2(0.5f, 0.5f))
        {
            panelCenter = new Vector2(panel.rect.width * (0.5f - panel.pivot.x),
                                      panel.rect.height * (0.5f - panel.pivot.y));
        }

        // Oblicz promie� wzgl�dem panelu
        float radius = Mathf.Min(panel.rect.width, panel.rect.height) * radiusOffset;

        float angleStep = 360f / items.Length; // R�wny podzia� k�ta

        for (int i = 0; i < items.Length; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad; // Konwersja na radiany

            Vector2 position = new Vector2(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius
            );

            // Ustawienie pozycji wzgl�dem �rodka panelu
            items[i].anchoredPosition = panelCenter + position;
        }
    }
}
