using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
    [Serializable]
    public struct RoadBlockSpawn
    {
        public GameObject RoadBlock;
        public float Weight;
    }

    [Header("Road Blocks")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private RoadBlockSpawn[] roadBlocks;
    private float roadWeightTotal;

    [Header("Buildings")]
    [SerializeField] private GameObject[] buildingPrefabs;
    [SerializeField] private Transform[] buildingSpawnPoints;
    [SerializeField] private Vector2 buildingSpawnScaleRange = new Vector2(0.6f, 0.8f);

    [Header("Street Lights")]
    [SerializeField] private GameObject streetLightPrefab;
    [SerializeField] private Transform[] streetLightsSpawnPoints;
    
    [Header("Obstacles")]
    [SerializeField] private Obstacle[] obstacles;
    [SerializeField] private Transform[] laneTransforms;
    [SerializeField] private Vector3 detectionHalfExtend;

    [Header("Collectibles")]
    [SerializeField] private Collectible[] collectibles;

    private Vector3 moveDirection;
    
    private bool isSpawning = false;


    private void Start()
    {
        roadWeightTotal = 0;
        for (int i = 0; i < roadBlocks.Length; i++)
        {
            roadWeightTotal += roadBlocks[i].Weight;
        }

        moveDirection = (endPoint.position - startPoint.position).normalized;
        float endPointDistance = Vector3.Distance(startPoint.position, endPoint.position);
        Vector3 nextBlockPosition = startPoint.position;
        while (Vector3.Distance(startPoint.position, nextBlockPosition) < endPointDistance) 
        {
            GameObject pickedBlock = roadBlocks[0].RoadBlock;
            GameObject newRoadBlock = Instantiate(pickedBlock);
            newRoadBlock.transform.position = nextBlockPosition;

            Movement movement = newRoadBlock.GetComponent<Movement>();
            if (movement != null)
            {
                movement.SetDestination(endPoint.position);
                movement.SetMoveDirection(moveDirection);
            }

            SpawnBuildings(newRoadBlock);
            SpawnStreetLights(newRoadBlock);

            float roadBlockLength = newRoadBlock.GetComponent<Renderer>().bounds.size.z;
            nextBlockPosition += moveDirection * roadBlockLength;
        }

        StartSpawningElements();

        Collectible collectible = Instantiate(collectibles[0], startPoint.position, Quaternion.identity);
        collectible.GetComponent<Movement>().SetDestination(endPoint.position);
        collectible.GetComponent<Movement>().SetMoveDirection(moveDirection);
    }


    private void StartSpawningElements()
    {
        isSpawning = true;
        foreach (var obstacle in obstacles)
        {
            StartCoroutine(SpawnElements(obstacle));
        }
        foreach (var collectible in collectibles)
        {
            StartCoroutine(SpawnElements(collectible));
        }
    }


    private IEnumerator SpawnElements(Spawnable spawnable)
    {
        while (isSpawning)
        {
            if (RandomSpawnPoint(out Vector3 spawnPoint, spawnable.gameObject.tag))
            {
                Spawnable newObstacle = Instantiate(spawnable, spawnPoint, Quaternion.identity);
                newObstacle.Movement.SetDestination(endPoint.position);
                newObstacle.Movement.SetMoveDirection(moveDirection);

            }
            yield return new WaitForSeconds(spawnable.SpawnInterval);
        }
    }


    private bool RandomSpawnPoint(out Vector3 spawnPoint, string occupationCheckTag)
    {
        Vector3[] spawnPoints = GetAvailableSpawnPoints(occupationCheckTag);
        if (spawnPoints.Length == 0)
        {
            spawnPoint = Vector3.zero;
            return false;
        }

        int rnd = Random.Range(0, spawnPoints.Length);
        spawnPoint = spawnPoints[rnd];

        return true;
    }


    private Vector3[] GetAvailableSpawnPoints(string occupationCheckTag)
    {
        List<Vector3> spawnPoints = new List<Vector3>();

        foreach (Transform spawnTransform in laneTransforms)
        {
            Vector3 spawnPoint = spawnTransform.position + new Vector3(0, 0, startPoint.position.z);
            if (!Utils.IsPositionOccupied(spawnPoint, detectionHalfExtend, occupationCheckTag))
            {
                spawnPoints.Add(spawnPoint);
            }
        }
        return spawnPoints.ToArray();
    }


    private void Update()
    {
        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != null && other.CompareTag("RoadBlock")) 
        {
            GameObject newBlock = SpawnNewBlock(other.transform.position, moveDirection);
            float newBlockHalfWidth = newBlock.GetComponent<Renderer>().bounds.size.z / 2f;
            float previousBlockHalfWidth = other.GetComponent<Renderer>().bounds.size.z / 2f;

            Vector3 newBlockSpawnOffset = -(newBlockHalfWidth + previousBlockHalfWidth) * moveDirection;
            newBlock.transform.position += newBlockSpawnOffset;
        }
    }


    private GameObject SpawnNewBlock(Vector3 spawnPosition, Vector3 moveDirection)
    {
        GameObject pickedBlock = GetRandomBlock();
        GameObject newRoadBlock = Instantiate(pickedBlock);
        newRoadBlock.transform.position = spawnPosition;

        Movement movement = newRoadBlock.GetComponent<Movement>();
        if (movement != null)
        {
            movement.SetDestination(endPoint.position);
            movement.SetMoveDirection(moveDirection);
        }

        SpawnBuildings(newRoadBlock);

        SpawnStreetLights(newRoadBlock);

        return newRoadBlock;
    }


    private GameObject GetRandomBlock()
    {
        float rndWeight = Random.Range(0f, roadWeightTotal);
        float total = 0;
        int rnd = 0;
        for (int i = 0; i < roadBlocks.Length; i++)
        {
            total += roadBlocks[i].Weight;
            if (rndWeight < total)
            {
                rnd = i;
                break;
            }
        }

        return roadBlocks[rnd].RoadBlock;
    }


    private void SpawnStreetLights(GameObject parentBlock)
    {
        foreach (Transform streetLightSpawnPoint in streetLightsSpawnPoints) 
        {
            Vector3 spawnPoint = parentBlock.transform.position + (streetLightSpawnPoint.position - startPoint.position);
            Quaternion spawnRotation = Quaternion.LookRotation((startPoint.position - streetLightSpawnPoint.position), Vector3.up) * Quaternion.Euler(0, -90, 0);
            GameObject newStreetLight = Instantiate(streetLightPrefab, spawnPoint, spawnRotation, parentBlock.transform);
        }
    }


    private void SpawnBuildings(GameObject parentBlock)
    {
        foreach (Transform buildingSpawnPoint in buildingSpawnPoints)
        {
            // Vector3 buildingSpawnPosition = new Vector3(parentBlock.transform.position.x + buildingSpawnPoint.position.x, 0, parentBlock.transform.position.z);
            Vector3 buildingSpawnPosition = parentBlock.transform.position + (buildingSpawnPoint.position - startPoint.position);

            // Debug.Log(buildingSpawnPosition);

            // int rotationOffsetBy90 = Random.Range(0, 3);
            int rotationOffset = -90;
            if (buildingSpawnPoint.transform.position.x < 0)
            {
                rotationOffset = 90;
            }
            Quaternion buildingSpawnRotation = Quaternion.Euler(0, rotationOffset, 0);
            Vector3 buildingSpawnSize = Vector3.one * Random.Range(buildingSpawnScaleRange.x, buildingSpawnScaleRange.y);
            int buildingPick = Random.Range(0, buildingPrefabs.Length);

            GameObject newBuilding = Instantiate(buildingPrefabs[buildingPick], buildingSpawnPosition, buildingSpawnRotation, parentBlock.transform);
            newBuilding.transform.localScale = buildingSpawnSize;
        }
    }
}
