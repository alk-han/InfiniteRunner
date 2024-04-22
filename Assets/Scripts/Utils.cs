using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    private static GameManager gameManager;


    public static bool IsPositionOccupied(Vector3 position, Vector3 detectionHalfExtend, string occupationCheckTag)
    {
        Collider[] colliders = Physics.OverlapBox(position, detectionHalfExtend);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(occupationCheckTag) || collider.CompareTag("NoSpawn"))
            {
                return true;
            }
        }
        return false;
    }


    public static GameManager GetGameManager()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
        }

        return gameManager;
    }
}
