using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSanity : MonoBehaviour
{
    void OnEnable() => SceneManager.sceneLoaded += OnLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnLoaded;

    void Start() { OnLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single); }

    void OnLoaded(Scene s, LoadSceneMode m)
    {
        // Make sure time is running
        if (Time.timeScale == 0f) { Time.timeScale = 1f; Debug.LogWarning("[SceneSanity] Time was paused. Unpausing."); }

        // Log active scene
        Debug.Log($"[SceneSanity] Active Scene: {SceneManager.GetActiveScene().name}");

        // Find/ensure a camera
        var cam = Camera.main;
        if (!cam || !cam.enabled)
        {
            foreach (var c in FindObjectsOfType<Camera>(true))
                if (c.enabled) { cam = c; break; }
        }
        if (!cam)
        {
            var go = new GameObject("RescueCamera");
            cam = go.AddComponent<Camera>();
            cam.tag = "MainCamera";
            cam.clearFlags = CameraClearFlags.Skybox;
            cam.cullingMask = ~0; // everything
            go.transform.SetPositionAndRotation(new Vector3(0, 2, -10), Quaternion.Euler(10, 0, 0));
            Debug.LogWarning("[SceneSanity] No camera found, spawned RescueCamera.");
        }
        else
        {
            cam.enabled = true;
            cam.cullingMask = ~0; // everything
        }

        // Rebind any Screen Space - Camera canvases
        foreach (var canvas in FindObjectsOfType<Canvas>(true))
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                canvas.worldCamera = cam;
                Debug.Log($"[SceneSanity] Rebound canvas '{canvas.name}' to {cam.name}");
            }
        }

        // Log cameras/canvases found
        var cams = FindObjectsOfType<Camera>(true);
        Debug.Log($"[SceneSanity] Cameras in scene: {cams.Length} (Main: {cam.name})");
        foreach (var c in cams) Debug.Log($"  - {c.name} (enabled={c.enabled}, cullingMask={c.cullingMask})");

        var canvases = FindObjectsOfType<Canvas>(true);
        foreach (var cv in canvases)
            Debug.Log($"  Canvas: {cv.name} mode={cv.renderMode} worldCam={(cv.worldCamera ? cv.worldCamera.name : "null")}");
    }
}

