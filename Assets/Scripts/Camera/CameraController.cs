using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera shopCamera;// to render 3d object
    public Camera MainCam
    {
        get
        {
            return mainCamera;
        }
    }
    public Camera ShopCamera
    {
        get
        {
            return shopCamera;
        }
    }

}
