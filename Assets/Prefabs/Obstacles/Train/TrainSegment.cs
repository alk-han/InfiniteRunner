using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSegment : MonoBehaviour
{
    [SerializeField] private Mesh           headMesh;
    [SerializeField] private Mesh[]         segmentMeshes;
    [SerializeField] private MeshFilter     trainMesh;
    [SerializeField] private BoxCollider    trainCollider;

    private bool isHead = false;

    private void Start()
    {
        CreateTrainMesh();
    }


    private void CreateTrainMesh()
    {
        if (isHead) return;
        int rnd = Random.Range(0, segmentMeshes.Length);
        trainMesh.mesh = segmentMeshes[rnd];
    }

    
    public float GetSegmentLength()
    {
        return trainCollider.size.z;
    }


    public void SetHead()
    {
        trainMesh.mesh = headMesh;
        isHead = true;
    }
}
