using System.Collections;
using UnityEngine;

public class XRayController : MonoBehaviour
{
    [Header("X-Ray Vision using terrain details")]
    [SerializeField] private Terrain terrain;   
    [SerializeField] private float duration = 1f;

    private bool busy;
    private bool unlocked;

    private void Update()
    {
        if (unlocked && Input.GetKeyDown(KeyCode.E))
        {
            ActivateXRay();
        }
    }
    //Called once when player first clicks the glasses in inventory
    public void UnlockXRay()
    {
        unlocked = true;
        Debug.Log("X-Ray unlocked!");
    }

    public void OnUseGlassesButton()
    {
        if (!unlocked)
        {
            Debug.Log("X-Ray not unlocked yet.");
            return;
        }
        if (busy || terrain == null)
        {
            return; 
        }

        ActivateXRay();
    }
    public void DisableXRay()
    {
        unlocked = false;
    }

    public void ActivateXRay()
    {
        if (!isActiveAndEnabled || busy || terrain == null) return;
        StartCoroutine(HideGrassCoroutine());
    }

    private IEnumerator HideGrassCoroutine()
    {
        busy = true;

        float prevDist = terrain.detailObjectDistance;
        float prevDensity = terrain.detailObjectDensity;

        // hide grass
        terrain.detailObjectDistance = 0f;
        terrain.detailObjectDensity = 0f;

        yield return new WaitForSeconds(duration);

        // restore
        terrain.detailObjectDistance = prevDist;
        terrain.detailObjectDensity = prevDensity;

        busy = false;
    }
}

