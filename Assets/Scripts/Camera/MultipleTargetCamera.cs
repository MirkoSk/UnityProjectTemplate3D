using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this on your cameraRig to make it follow multiple targets.
/// It puts a bounding box around all targets and orients itself at it's center point. Zoom in and out is achieved by adjusting the field of view.
/// This script was written for our GGJ2018 project.
/// Found on Brackeys Youtube channel: https://www.youtube.com/watch?v=aLpixrPvlB8
/// </summary>
public class MultipleTargetCamera : MonoBehaviour {

    #region Variable Declarations

    public List<Transform> targets;

    [SerializeField] float smoothTime = 0.5f;
    [SerializeField] float minZoom = 40f;
    [SerializeField] float maxZoom = 10f;
    [SerializeField] float maxDistance = 100f;

    private Vector3 velocity;
    private Camera[] cams;
    private Vector3 offset;
    #endregion



    #region Unity Event Functions
    private void Start() {
        cams = transform.GetComponentsInChildren<Camera>();
        offset = transform.position;
	}
	
	private void LateUpdate() {
        if (targets.Count == 0) return;

        Move();
        Zoom();
	}

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GetCenterPoint(), 1f);
    }
    #endregion



    #region Private Functions
    void Move() {
        Vector3 centerPoint = GetCenterPoint();

        transform.position = Vector3.SmoothDamp(transform.position, centerPoint + offset, ref velocity, smoothTime);
    }

    void Zoom() {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / maxDistance);
        foreach (Camera cam in cams) {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
        }
    }

    float GetGreatestDistance() {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++) {
            bounds.Encapsulate(targets[i].position);
        }

        if (bounds.size.x > (1.778f * bounds.size.z)) {
            return bounds.size.x;
        }
        else {
            return bounds.size.z * 1.778f;
        }
    }

    Vector3 GetCenterPoint() {
        if (targets.Count == 1) {
            return targets[0].position;
        }
        else {
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for (int i = 0; i < targets.Count; i++) {
                bounds.Encapsulate(targets[i].position);
            }

            return bounds.center;
        }
    }
	#endregion
}
