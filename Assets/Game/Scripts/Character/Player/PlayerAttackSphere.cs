using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSphere : CharacterAttackSphere
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        TransparentWall(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        UnTransparentWall(other);
    }

    private void TransparentWall(Collider other)
    {
        if (CacheComponentManager.Instance.WallCache.TryGet(other.gameObject, out var wall))
        {
            wall.OnPlayerEnter();
        }
    }
    
    
    
    private void UnTransparentWall(Collider other)
    {
        if (CacheComponentManager.Instance.WallCache.TryGet(other.gameObject, out var wall))
        {
            wall.OnPlayerExit();
        }
    }

}
