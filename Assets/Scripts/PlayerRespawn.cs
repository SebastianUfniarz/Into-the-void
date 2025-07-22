using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    private static PlayerRespawn instance;

    private static Vector3 savedPosition;
    private static string savedScene;
    private static bool hasCheckpoint = false;

    private Damageable damageable;

    private void Awake()
    {
        // Jeœli instancja ju¿ istnieje i to nie jest ten sam obiekt, usuñ tego nowego
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        damageable = GetComponent<Damageable>();
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        savedPosition = checkpoint.position + new Vector3(0, 4, 0);
        savedScene = SceneManager.GetActiveScene().name;
        hasCheckpoint = true;
        Debug.Log($"Checkpoint ustawiony: scena {savedScene}, pozycja {savedPosition}");
    }

    public void Respawn()
    {
        if (!hasCheckpoint)
        {
            Debug.LogWarning("Brak checkpointu - nie mo¿na zrespawnowaæ.");
            return;
        }

        if (SceneManager.GetActiveScene().name != savedScene)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(savedScene);
        }
        else
        {
            RespawnHere();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        RespawnHere();
    }

    private void RespawnHere()
    {
        transform.position = savedPosition;

        if (damageable != null)
        {
            damageable.Health = damageable.Maxhealth;
            damageable.IsAlive = true;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 1f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        Debug.Log("Gracz zosta³ odrodzony.");
    }
}
