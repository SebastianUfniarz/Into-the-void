using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class AutoFollowPlayer : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;

    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        SetPlayerAsTarget();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetPlayerAsTarget();
    }

    void SetPlayerAsTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            vcam.Follow = player.transform;
            vcam.LookAt = player.transform;
            Debug.Log("Kamera śledzi gracza: " + player.name);
        }
        else
        {
            Debug.LogWarning("Nie znaleziono gracza z tagiem 'Player'.");
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
