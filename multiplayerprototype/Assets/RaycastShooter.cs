using UnityEngine;
using Unity.Netcode;

public class RaycastShooter : NetworkBehaviour
{
    [Header("Settings")]
    public float range = 100f;
    public LayerMask hitLayers;
    public Camera playerCamera;

    [Header("Effects")]
    public LineRenderer lineRenderer;
    public float lineDuration = 0.05f;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log($"[Local] Fire1 pressed by client {OwnerClientId}");
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 hitPoint;

        if (Physics.Raycast(ray, out hit, range, hitLayers))
        {
            hitPoint = hit.point;
            Debug.Log($"[Local] Ray hit: {hit.collider.name} at {hitPoint}");

            // Notify server about the hit
            ReportHitServerRpc(hit.collider.name, hitPoint);
        }
        else
        {
            hitPoint = ray.origin + ray.direction * range;
            Debug.Log($"[Local] Ray missed. Firing into empty space at {hitPoint}");
        }

        ShowLaser(ray.origin, hitPoint);
    }

    void ShowLaser(Vector3 start, Vector3 end)
    {
        if (lineRenderer == null) return;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.enabled = true;

        Debug.Log($"[Visual] Laser shown from {start} to {end}");

        CancelInvoke(nameof(HideLaser));
        Invoke(nameof(HideLaser), lineDuration);
    }

    void HideLaser()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    [ServerRpc]
    void ReportHitServerRpc(string hitObjectName, Vector3 hitPosition)
    {
        Debug.Log($"[ServerRpc] Client {OwnerClientId} hit: {hitObjectName} at {hitPosition}");

        // Optional: Apply damage or logic here

        ShowHitClientRpc(hitPosition);
    }

    [ClientRpc]
    void ShowHitClientRpc(Vector3 position)
    {
        Debug.Log($"[ClientRpc] Showing global hit effect at {position}");
        Debug.DrawRay(position, Vector3.up, Color.red, 1f);
    }
}
