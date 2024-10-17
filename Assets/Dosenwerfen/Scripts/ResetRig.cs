using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public struct TrackingOrigin
{
    public Vector3 position;
    public Quaternion rotation;

    public TrackingOrigin(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}

/// <summary>
/// This script must be put on the XR CameraRig gameobject.
/// For user-defined rig orientation (from HMD view direction), call <see cref="SetOrigin(UnityEngine.InputSystem.InputAction.CallbackContext)"/>. 
/// </summary>
public class ResetRig : MonoBehaviour
{
    [Header("Scene settings")]

    /// <summary>
    /// Reference to the user-defined origin of the VR experience.
    /// </summary>
    [SerializeField] private Transform virtualSceneOrigin;

    /// <summary>
    /// XR Camera rig
    /// </summary>
    [SerializeField] private Transform cameraRig;

    /// <summary>
    /// HMD pose reference transform
    /// </summary>
    [SerializeField] private Transform hmdTransform;

    /// <summary>
    /// Input Action to reset the rig.
    /// </summary>
    [SerializeField] private InputActionReference inputActionReference;

    /// <summary>
    /// Struct for saving rig transform in the PlayerPrefs
    /// </summary>
    protected TrackingOrigin trackingOrigin;

    protected void Awake()
    {
        CheckParameters();
    }

    /// <summary>
    /// At startup, set last saved player origin.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Start()
    {
        yield return WaitForXR();
        LoadTrackingOrigin();
        inputActionReference.action.performed += SetOrigin;
    }

    /// <summary>
    /// Set virtual scene origin.
    /// Available input:
    /// Transform of <see cref="ResetRig.cameraRig"/>
    /// Transform of <see cref="ResetRig.hmdTransform"/>
    /// Transform of <see cref="virtualSceneOrigin"/>
    /// </summary>
    /// <param name="inputContext"></param>
    public void SetOrigin(InputAction.CallbackContext inputContext)
    {
        Debug.Log("Reset");

        if (!inputContext.performed)
        {
            return;
        }

        cameraRig.rotation *= Quaternion.FromToRotation(Vector3ProjectXZ(hmdTransform.forward).normalized, Vector3ProjectXZ(virtualSceneOrigin.forward).normalized);
        cameraRig.position -= Vector3ProjectXZ(hmdTransform.position - virtualSceneOrigin.position);


        // save
        SaveTrackingOrigin(new TrackingOrigin(cameraRig.position, cameraRig.rotation));
    }

    /// <summary>
    /// Load last saved tracking origin.
    /// </summary>
    protected void LoadTrackingOrigin()
    {
        if (PlayerPrefs.GetString("OriginJSON") == "")
        {
            return;
        }

        trackingOrigin = JsonUtility.FromJson<TrackingOrigin>(PlayerPrefs.GetString("OriginJSON"));

        cameraRig.SetPositionAndRotation(trackingOrigin.position, trackingOrigin.rotation);
    }

    /// <summary>
    /// Save tracking origin to player prefs.
    /// </summary>
    /// <param name="trackingOrigin"></param>
    protected void SaveTrackingOrigin(TrackingOrigin trackingOrigin)
    {
        PlayerPrefs.SetString("OriginJSON", JsonUtility.ToJson(trackingOrigin));
    }

    /// <summary>
    /// Remove y component from Vector3.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    protected static Vector3 Vector3ProjectXZ(Vector3 v) => new Vector3(v.x, 0, v.z);

    /// <summary>
    /// Wait for XR subsystem to start
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForXR()
    {
        bool XRisRunning = false;

        // wait untit XR has started 
        while (!XRisRunning)
        {
            List<XRDisplaySubsystem> displays = new List<XRDisplaySubsystem>();
            SubsystemManager.GetInstances(displays);

            foreach (XRDisplaySubsystem d in displays)
            {
                if (d.running)
                {
                    XRisRunning = true;
                    //Debug.Log(d.GetType().ToString() + " running at " + Time.frameCount);
                    break;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Check inspector parameter configuration.
    /// </summary>
    private void CheckParameters()
    {
        if (cameraRig == null)
        {
            Debug.LogWarning("ResetRig: no cameraRig configured, using this gameobject (" + gameObject.name + ")");
            cameraRig = gameObject.transform;
        }

        if (hmdTransform == null)
        {
            Debug.LogError("ResetRig: hmdTransform is null");
        }
    }
}

