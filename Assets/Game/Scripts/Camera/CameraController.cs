using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Camera mainCamera;
    public Camera ShopCam
    {
        get
        {
            var uACD= mainCamera.GetUniversalAdditionalCameraData();

            return 
                uACD.cameraStack[0];
        }
    }
    public Camera MainCam
    {
        get
        {
            return mainCamera;
        }
    }
    [SerializeField] private CinemachineVirtualCamera followerCine;
    [SerializeField] private CinemachineVirtualCamera skinShopCine;
    [SerializeField] private CinemachineVirtualCamera initCam;


    public void Init()
    {
        initCam.Priority = 11;
        skinShopCine.Priority = 10;
        followerCine.Priority = 9;
    }
    
    
    public void SetFollowCam(Transform follow,Transform lookAt)
    {
        followerCine.Follow = follow;
        followerCine.LookAt = lookAt;
        initCam.Follow = follow;
        initCam.LookAt = lookAt;
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


    public void SetSkinShopCam(Transform fl,Transform lookAt)
    {
        skinShopCine.Follow = fl;
        skinShopCine.LookAt = lookAt;
    }
    
    public void EnterSkinShop()
    {
        skinShopCine.Priority = 11;
        initCam.Priority = 10;
    }

    public void ExitSkinShop()
    {
        skinShopCine.Priority = 10;
        initCam.Priority = 11;
    }

    public void Play()
    {
        followerCine.Priority = 12;
    }
}
