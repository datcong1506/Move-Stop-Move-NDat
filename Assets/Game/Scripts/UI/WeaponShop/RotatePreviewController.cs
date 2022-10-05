using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotatePreviewController : MonoBehaviour,IDragHandler
{

    [SerializeField] private WeaponShopUiController weaponShopUiController;

    public void OnDrag(PointerEventData eventData)
    {
        weaponShopUiController.RotatePreviewHandle(eventData.delta);
    }
}
