using System;
using System.Collections;
using System.Collections.Generic;
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

    // 
    public Material material;

    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;

    [Range(0.7f, 1.0f)]
    public float roomTopCornerModifier;

    [Range(0.0f, 2.0f)]
    public int roomOffset;

    public BoxCollider[] roomCollider;

    public GameObject wallVertical, walllHorizontal;
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;

    //11.10 추가 by 손동욱
    //네브메쉬 활성화하는게 주석 중에 뭔지 몰라서 다 했습니다.
    NavMeshSurface surface;
    NavMeshSurface[] surfaces;
    int count;
    public GameObject player;
    public GameObject door;

    private void Awake()
    {
        //11.10 추가 by 손동욱
        //네브메쉬 활성화하는게 주석 중에 뭔지 몰라서 다 활성화 했습니다.
        surface = GetComponent<NavMeshSurface>();

        //11.10 추가 by 손동욱

    }
    private void Start()
    {
        CreateDungeon(); // 던전 생성 매서드

        //11.10 추가 by 손동욱
        //네브메쉬 활성화하는게 주석 중에 뭔지 몰라서 다 활성화 했습니다.
        surfaces = GetComponents<NavMeshSurface>();
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
        Debug.Log("ㅁ");

        //11.10 추가 by 손동욱
        //플레이어와 카메라를 활성화하고, 시작 위치로 이동시키는 코드
        player.SetActive(true);
        player.transform.position = roomCollider[0].center;
        
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

        //11.10 추가 by 손동욱
        //콜리더 지정에 필요한 변수 초기화
        count = 0;
        roomCollider = new BoxCollider[listOfRooms.Count];


        for (int i = 0; i < listOfRooms.Count; i++)
        {
            //11.10 추가 by 손동욱
            //문 생성 인덱스 체크
            //문 생성이면 실행
            if ((listOfRooms.Count - 1) * 0.5f + 1 <= i)
            {
                CreateMesh_Door(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
            }
            else
            //방 생성이면 실행
            {
                CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
            }



        }
        //11.10 주석 by 손동욱
        //벽 만드는거 잠시 멈췄습니다.
        //CreateWalls(wallParent); // 벽만들기


        //surface.BuildNavMesh();

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

        wallParent.AddComponent<BoxCollider>();
        //11.10 추가 by 손동욱
        //주석처리되어 있는데 네브매쉬에 필요한건지 잘 모르겠어서 활성화해뒀습니다.
        // NavMesh를 위한 정적으로 만들기
        surface.BuildNavMesh();
        
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        // 벽 생성... 프리팹 이용
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);

        // NavMesh를 위한 정적으로 만들기 
        wallPrefab.isStatic = true;
        // wallPrefab.AddComponent<BoxCollider>();
        
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

        
        GameObject dungeonFloor = new GameObject("Mesh" +bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));
        
        if (dungeonFloor.name == "Mesh")
        {

        }

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.transform.parent = transform;

        // dungeonFloor.isStatic = true;
        roomCollider[count] = dungeonFloor.AddComponent<BoxCollider>(); ;
        //몇 번째로 생성되었는지 카운트
        count++;
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

    /// <summary>
    /// 11.10 추가 by 손동욱
    /// 문 생성 시 실행될 코드
    /// 문에 컴포넌트 추가 및 프리팹 추가
    /// </summary>
    /// <param name="bottomLeftCorner"></param>
    /// <param name="topRightCorner"></param>
    private void CreateMesh_Door(Vector2 bottomLeftCorner, Vector2 topRightCorner)
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


        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        if (dungeonFloor.name == "Mesh")
        {

        }

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.transform.parent = transform;


        //dungeonFloor.isStatic = true;

        //11.10 추가 by 손동욱
        roomCollider[count] = dungeonFloor.AddComponent<BoxCollider>();

        //11.10 추가 by 손동욱
        //문 프리팹 추가.
        GameObject objDoor = Instantiate(door, dungeonFloor.transform);
        objDoor.transform.position = roomCollider[count].center + Vector3.up * 2.5f;


        //11.10 추가 by 손동욱
        //몇 번째로 생성되었는지 카운트
        count++;
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
