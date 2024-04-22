using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private TrainSegment segmentPrefab;
    [SerializeField] private Vector2 segmentCountRange;

    private Obstacle obstacle;

    private void Start()
    {
        obstacle = GetComponent<Obstacle>();
        GenerateTrainBody();
    }


    private void GenerateTrainBody()
    {
        int segmentCount = Random.Range((int)segmentCountRange.x, (int)segmentCountRange.y);

        for (int i = 0; i < segmentCount; i++)
        {
            Vector3 spawnPosition = transform.position + transform.forward * segmentPrefab.GetSegmentLength() * i;
            TrainSegment newSegment = Instantiate(segmentPrefab, spawnPosition, Quaternion.identity);
            if (i == 0)
            {
                newSegment.SetHead();
            }
            newSegment.GetComponent<Movement>().CopyFrom(obstacle.GetComponent<Movement>());
        }
    }
}
