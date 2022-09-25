using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Camera mainCamera;
    public Camera MainCam
    {
        get
        {
            return mainCamera;
        }
    }
    

    [SerializeField] private CinemachineVirtualCamera followerCine;
    // public CinemachineVirtualCamera FollowerCine => followerCine;
    public void SetFollowCam(Transform follow,Transform lookAt)
    {
        followerCine.Follow = follow;
        followerCine.LookAt = lookAt;
    }

    public void AddCamOverLay(Camera cam)
    {
        var uACD= mainCamera.GetUniversalAdditionalCameraData();
        uACD.cameraStack.Add(cam);
    }

    public void RemoveCamOverlay(Camera cam)
    {
        var uACD= mainCamera.GetUniversalAdditionalCameraData();
        uACD.cameraStack.Remove(cam);
    }
}
