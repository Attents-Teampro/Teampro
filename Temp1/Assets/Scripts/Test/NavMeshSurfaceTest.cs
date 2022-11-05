using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSurfaceTest : MonoBehaviour
{
    NavMeshSurface surface;
    // Start is called before the first frame update

    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
    }
    void Start()
    {
        surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
