using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    [field: SerializeField] public float    SpawnInterval   { get; private set; } = 2f;
    [field: SerializeField] public Movement Movement        { get; private set; }
}
