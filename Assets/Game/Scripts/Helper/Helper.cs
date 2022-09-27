using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static Vector3 RandomNavmeshLocation(float radius, Vector3 center, Vector3 playerPosision)
    {
        UnityEngine.AI.NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        for (int i = 0; i < 100; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius+center;
            if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
                if((finalPosition-playerPosision).magnitude>FixVariable.AI_MINDISTANCE_TOPLAYER){
                    break;
                }
            }
        }
        return finalPosition;
    }

    public static Vector3 GetposisionOnNavmesh(Vector3 sourcePosision, float radius=20)
    {
        UnityEngine.AI.NavMeshHit hit;
        Vector3 finalPosition = sourcePosision;
        if (UnityEngine.AI.NavMesh.SamplePosition(sourcePosision, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }

        return finalPosition;
    }

}
