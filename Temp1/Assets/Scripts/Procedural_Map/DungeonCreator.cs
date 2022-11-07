using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class DungeonCreator : MonoBehaviour
{
    // 전체 던전 크기
    public int dungeonWidth;
    public int dungeonLength;

    // 각 방의 크기
    public int roomWidth;
    public int roomLength;

    // 생성 반복 횟수
    public int maxIterations;

    // 방 이어주는 복도의 크기
    public int corridorWidth;

    //네비매시 서페이스 사용을 위한 캐싱 : 박인엽
    NavMeshSurface surface;

    // 
    public Material material;

    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;

    [Range(0.7f, 1.0f)]
    public float roomTopCornerModifier;

    [Range(0.0f, 2.0f)]
    public int roomOffset;

    public GameObject wallVertical, walllHorizontal;
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;


    private void Awake()
    {
        //서페이스 객세 할당 : 박인엽
        surface = GetComponent<NavMeshSurface>();
    }


    private void Start()
    {
        CreateDungeon(); // 던전 생성 매서드
        surface.BuildNavMesh(); // 네비 매시 생성 from : 박인엽
    }


    public void CreateDungeon()
    {
        DestroyAllChildren();// 모든걸 지운다.
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonLength); // 던전을 생성하는 클래스

        // 방을 생성하는 var. 
        var listOfRooms = generator.CalculateDungeon(
            maxIterations,
            roomWidth,roomLength, 
            roomBottomCornerModifier, 
            roomTopCornerModifier, 
            roomOffset,
            corridorWidth);
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();

        for (int i=0; i<listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
                
        CreateWalls(wallParent); // 벽만들기
        surface.BuildNavMesh(); // 네비매시 생성 : 박인엽

    }

    private void CreateWalls(GameObject wallParent)
    {
        // 벽 생성

        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition, walllHorizontal);
            
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical);
        }

        // NavMesh를 위한 정적으로 만들기
        wallParent.isStatic = true;

    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        // 벽 생성... 프리팹 이용
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);

        // NavMesh를 위한 정적으로 만들기 
        wallPrefab.isStatic = true;
    }

    private void CreateMesh (Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        // 면 만들어주는 매써드
        // V = Vertices

        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            // 시계방향...?

            0,
            1,
            2,
            2,
            1,
            3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Mesh"+bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.transform.parent = transform;

        dungeonFloor.AddComponent<BoxCollider>(); //던전 바닥 박스 콜리더 추가
        //dungeonFloor.isStatic = true;

        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }


    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point)){
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }

    private void DestroyAllChildren()
    {
        // 모든 맵과 오브젝트를 지운다
        while(transform.childCount != 0)
        {
            foreach(Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}
