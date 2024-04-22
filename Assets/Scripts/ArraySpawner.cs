using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArraySpawner : MonoBehaviour
{
    [SerializeField] private int amount = 10;
    [SerializeField] private float offSet = 1f;


    private void Start()
    {
        for (int i = 1; i <= amount; i++)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, 0f, transform.position.z) + transform.forward * i * offSet;
            GameObject nextObject = Instantiate(gameObject, spawnPosition, Quaternion.identity);
            nextObject.GetComponent<ArraySpawner>().enabled = false; // do not repeat the Start()
        }
    }

    
    private void Update()
    {
        
    }
}
