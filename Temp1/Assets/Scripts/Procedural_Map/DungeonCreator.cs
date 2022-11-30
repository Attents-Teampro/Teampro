using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
    int count, count_BossCheck;
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
        // CreateDungeon(); // 던전 생성 매서드

        //11.10 추가 by 손동욱
        //네브메쉬 활성화하는게 주석 중에 뭔지 몰라서 다 활성화 했습니다.
        surfaces = GetComponents<NavMeshSurface>();
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
        //Debug.Log("ㅁ");

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
        
        //11.10 추가 및 수정 by 손동욱
        //{
        //콜리더 지정에 필요한 변수 초기화
        count = 0;
        roomCollider = new BoxCollider[listOfRooms.Count];

        //보스 생성 용 카운트 변수 초기화
        count_BossCheck = (int) ((listOfRooms.Count - 1) *0.5f +1);

        //방과 문 생성 구분
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            //문 생성
            if ((listOfRooms.Count - 1) * 0.5f + 1 <= i)
            {
                CreateMesh_Door(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
            }
            else
            //방 생성
            {
                CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
            }
        }
        // 벽만들기
        CreateWalls(wallParent);

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
        wallPrefab.AddComponent<BoxCollider>();
        
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

        
        //룸 콜리더를 연결
        roomCollider[count] = dungeonFloor.AddComponent<BoxCollider>();



        //11.10 추가 by 손동욱
        //{

        //룸 스크립트 추가
        Room r = dungeonFloor.AddComponent<Room>();

        //룸 콜리더의 위치를 이용해서 해당 방의 position 좌표 구하고, Room스크립트 변수에 대입
        dungeonFloor.GetComponent<Room>().roomPosition = roomCollider[count].center;

        //룸에 본인 생성 순서 저장해놓기(첫 맵 찾아야함)
        r.index = count;
        //Debug.Log(count);

        //몇 번째로 생성되었는지 카운트
        count++;


        //태그랑 레이어 지정
        dungeonFloor.tag = "Floor";
        dungeonFloor.layer = 7;

        //메인매니저 스폰매니저의 스폰 배열 변수
        Spawner[] sp = MainManager.instance.spawnManager.spawners;

        r.isBossIndex = count_BossCheck + 2;
        r.spawners = sp[sp.Length - 1];

        //만약 방이 마지막 방이면
        //if (count == count_BossCheck)
        //{
        //    r.spawners = new Spawner[1];
        //    r.spawners[0] = sp[sp.Length - 1];
        //    r.spawners[0].bossPosition = roomCollider[count - 1].center;
        //}
        //else
        //{
        //    r.spawners = new Spawner[sp.Length - 1];
        //    for (int i = 0; i < sp.Length - 1; i++)
        //    {
        //        r.spawners[i] = sp[i];
        //    }
        //}
        
        //if(count == count_BossCheck)
        //{

        //}
        //else
        //{
        //    r.spawners = new Spawner[sp.Length - 1];
        //    for(int i=0; i < sp.Length; i++)
        //    {
        //        r.spawners[i] = sp(i);
        //    }
        //}


        //랜덤으로 스폰 가져오기
        ////메인매니저 스폰매니저의 스폰 배열 변수
        //Spawner[] sp = MainManager.instance.spawnManager.spawners;

        ////룸에 할당할 스포너 종류의 개수 정하기
        //int rand = Random.Range(1, sp.Length);

        ////정한 개수 대입
        //r.spawners = new Spawner[rand];

        ////정한 개수 만큼의 스포너를 대입. 대입할 스포너는 매니저의 스포너 배열에서 랜덤으로 정함.
        //for (int i =0; i<r.spawners.Length; i++)
        //{
        //    //보스방을 스폰매니저 맨 마지막에 둘 예정이라 아래 코드가 딱 맞음
        //    rand = Random.Range(0, sp.Length-1);

        //    r.spawners[i] = sp[rand];
        //}
        //}




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
        //{
        roomCollider[count] = dungeonFloor.AddComponent<BoxCollider>();

        //문 프리팹 추가.
        GameObject objDoor = Instantiate(door, dungeonFloor.transform);
        objDoor.transform.position = roomCollider[count].center + Vector3.up * 2.5f;

        //문 프리팹의 방 찾기 함수 실행
        
        for (int i = 0; i < 4; i++)
        {
            bool result = objDoor.transform.GetChild(i).GetComponent<Door>().CheckRoom();
            //Debug.Log($"{i} {dungeonFloor.name}의 방 찾기 결과 = {result}");
        }
        //몇 번째로 생성되었는지 카운트
        count++;

        //}
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
