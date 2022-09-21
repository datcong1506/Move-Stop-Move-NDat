using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputController : Singleton<PlayerInputController>,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField] private Canvas _inputCanvas;
    public Vector2 direc { get; private set; }
    
    private Vector2 startPosision=Vector2.zero;
    private Vector2 currentPosision=Vector2.zero;
    
    private void Start()
    {
        Init();
    }
    private void Init(){
        direc=Vector2.zero;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosision = eventData.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        currentPosision = eventData.position;
        direc = currentPosision - startPosision;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        direc=Vector2.zero;
    }
    public void SetCanvas(bool value)
    {
        _inputCanvas.enabled = value;
    }
}