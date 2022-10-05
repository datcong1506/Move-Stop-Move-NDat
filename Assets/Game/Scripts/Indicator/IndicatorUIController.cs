using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorUIController : MonoBehaviour
{
    private Transform _playerTransform;
    private Camera _mainCamera;
    [SerializeField] private TextMeshProUGUI _killCountText;
    private Transform _refEnemy;
    [SerializeField]private CharacterController characterController;
    private Transform _transform;
    [SerializeField] private Transform _arrow;
    [SerializeField] private Image killCountPanel;
    [SerializeField] private Image arrowPanel;
    
    public void Init(IndicatorManager indicatorManager,CharacterController characterController)
    {
        transform.SetParent(indicatorManager.transform);
        _transform=transform;
        _refEnemy = characterController.transform;
        this.characterController = characterController;
        _mainCamera = CameraController.Instance.MainCam;
        _playerTransform = GameManager.Instance.PlayerController.transform;
        //0.7 is fit scale 
        transform.localScale=Vector3.one *0.5f;
        killCountPanel.color = characterController.SkinColor;
        arrowPanel.color=characterController.SkinColor;
    }

    private void LateUpdate()
    {
        HandleBound();
        UpdateKillCount();
    }
    private void HandleBound()
    {
        var playerScreenPoint = _mainCamera.WorldToViewportPoint(_playerTransform.position);
        var enemyScreenPoint = _mainCamera.WorldToViewportPoint(_refEnemy.position);
        var direcFromPlayerToEnemy = enemyScreenPoint - playerScreenPoint;
        var cutEdgePoint = CalCutEdge(direcFromPlayerToEnemy, playerScreenPoint);
        var cutEdgePointScreen = _mainCamera.ViewportToScreenPoint(new Vector3(cutEdgePoint.x, cutEdgePoint.y));
        var boundWidth = (int)(Screen.width * (float)(100 - FixVariable.CONDICATE_PERCENT * ((float)Screen.height / Screen.width)) / 100);
        var boundHeight = (int)(Screen.height * (float)(100 - FixVariable.CONDICATE_PERCENT) / 100);
        if (cutEdgePointScreen.x == Screen.width)
        {
            cutEdgePointScreen.x -= (Screen.width - boundWidth) / 2;
        }
        else
        {
            cutEdgePointScreen.x += (Screen.width - boundWidth) / 2;
        }
        if (cutEdgePointScreen.y == Screen.height)
        {
            cutEdgePointScreen.y -= (Screen.height - boundHeight) / 2;
        }
        else
        {
            cutEdgePointScreen.y += (Screen.height - boundHeight) / 2;
        }
        _transform.position = cutEdgePointScreen;
        var angle = Vector3.Angle(Vector3.right, direcFromPlayerToEnemy);
        if (direcFromPlayerToEnemy.y <0)
        {
            angle = -angle;
        }
        var eulerAngle = new Vector3(0, 0, angle);
        _arrow.eulerAngles = eulerAngle;

    }

    private void UpdateKillCount()
    {
        _killCountText.text = characterController.KillCount.ToString();

    }
    private Vector2 CalCutEdge(Vector3 direc, Vector3 _playerPosision)
    {
        // -direc.y(x-x0)+direc.x*(y-y0)=0;
        // t*direc.x+x0=x;
        // t* direc.y+y0=y;
        Vector3 point = Vector3.zero;
        Vector3 currentDirec = Vector3.zero;
        if (direc.y == 0) // done this case
        {
            // case 01 : edge y=0; not exsit
            // case 02 : edge y=1; not exsit
            // case 03 : edge x=0;  
            if (direc.x < 0)
            {
                point.x = 0;
                point.y = _playerPosision.y;
                return point;
            }
            // case 03 : edge x=1;  
            point.x = 1;
            point.y = _playerPosision.y;
            return point;
        }
        else // not done
        {
            if (direc.x == 0)
            {
                // case  y=0;
                if (direc.y < 0)
                {
                    point.y = 0;
                    point.x = _playerPosision.x;
                    return point;
                }
                point.y = 1;
                point.x = _playerPosision.x;
                return point;
            }
            // case 01 : edge y = 0;
            point.y = 0;
            point.x = (direc.x * (0 - _playerPosision.y) / (direc.y)) + _playerPosision.x;
            if (point.x >= 0 && point.x <= 1)
            {
                currentDirec = point - _playerPosision;
                if (currentDirec.x * direc.x > 0)
                {
                    return point;
                }
            }
            // case 02 : edge y=1; n
            point.y = 1;
            point.x = (direc.x * (1 - _playerPosision.y) / (direc.y)) + _playerPosision.x;
            if (point.x >= 0 && point.x <= 1)
            {
                currentDirec = point - _playerPosision;
                if (currentDirec.x * direc.x > 0)
                {
                    return point;
                }
            }
            // case 03: edge x= 0;
            point.y = (direc.y * (0 - _playerPosision.x) / (direc.x)) + _playerPosision.y;
            point.x = 0;
            if (point.y >= 0 && point.x <= 1)
            {
                currentDirec = point - _playerPosision;
                if (currentDirec.x * direc.x > 0)
                {
                    return point;
                }
            }
            // case 03: edge x= 1;
            point.y = (direc.y * (1 - _playerPosision.x) / (direc.x)) + _playerPosision.y;
            point.x = 1;
            if (point.y >= 0 && point.x <= 1)
            {
                currentDirec = point - _playerPosision;
                if (currentDirec.x * direc.x > 0)
                {
                    return point;
                }
            }
        }

        return point;
    }
}
