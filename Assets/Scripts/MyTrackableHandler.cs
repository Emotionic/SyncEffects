﻿using UnityEngine;
using Vuforia;

public class MyTrackableHandler : MonoBehaviour, ITrackableEventHandler
{
    public LogViewer Viewer;
    public EffectManager effectManager;

    private EffectManager _effmgr;
    private TrackableBehaviour mTrackableBehaviour;
    private LogViewer _viewer;

    private bool isTracking = false;

    private void Update()
    {
        if (isTracking)
        {
            var obj = new GameObject();
            obj.transform.position = new Vector3(-150.0f, -150.0f, -50.0f);
            obj.transform.rotation = Quaternion.Euler(0, -90, 0);
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            _effmgr.PlayEffect(obj, "Laser01", false);
        }
        
    }

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        _viewer = Viewer.GetComponent<LogViewer>();
        _effmgr = effectManager.GetComponent<EffectManager>();
    }

    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }

    private void OnTrackingFound()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        isTracking = true;

    }

    private void OnTrackingLost()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Disable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }

        // Disable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = false;
        }

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        isTracking = false;
    }

}