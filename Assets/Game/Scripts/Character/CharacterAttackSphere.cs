using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackSphere : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    protected virtual void OnTriggerEnter(Collider other)
    {
    }
    
    protected virtual void OnTriggerExit(Collider other)
    {
    }
}
