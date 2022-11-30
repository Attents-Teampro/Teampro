using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class DungeonCreator : MonoBehaviour
{
    // ��ü ���� ũ��
    public int dungeonWidth;
    public int dungeonLength;

    // �� ���� ũ��
    public int roomWidth;
    public int roomLength;

    // ���� �ݺ� Ƚ��
    public int maxIterations;

    // �� �̾��ִ� ������ ũ��
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

    //11.10 �߰� by �յ���
    //�׺�޽� Ȱ��ȭ�ϴ°� �ּ� �߿� ���� ���� �� �߽��ϴ�.
    NavMeshSurface surface;
    NavMeshSurface[] surfaces;
    int count, count_BossCheck;
    public GameObject player;
    public GameObject door;

    private void Awake()
    {
        //11.10 �߰� by �յ���
        //�׺�޽� Ȱ��ȭ�ϴ°� �ּ� �߿� ���� ���� �� Ȱ��ȭ �߽��ϴ�.
        surface = GetComponent<NavMeshSurface>();

        //11.10 �߰� by �յ���

    }
    private void Start()
    {
        // CreateDungeon(); // ���� ���� �ż���

        //11.10 �߰� by �յ���
        //�׺�޽� Ȱ��ȭ�ϴ°� �ּ� �߿� ���� ���� �� Ȱ��ȭ �߽��ϴ�.
        surfaces = GetComponents<NavMeshSurface>();
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
        //Debug.Log("��");

        //11.10 �߰� by �յ���
        //�÷��̾�� ī�޶� Ȱ��ȭ�ϰ�, ���� ��ġ�� �̵���Ű�� �ڵ�
        player.SetActive(true);
        player.transform.position = roomCollider[0].center;
        
    }


    public void CreateDungeon()
    {
        DestroyAllChildren();// ���� �����.
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonLength); // ������ �����ϴ� Ŭ����

        // ���� �����ϴ� var. 
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
        
        //11.10 �߰� �� ���� by �յ���
        //{
        //�ݸ��� ������ �ʿ��� ���� �ʱ�ȭ
        count = 0;
        roomCollider = new BoxCollider[listOfRooms.Count];

        //���� ���� �� ī��Ʈ ���� �ʱ�ȭ
        count_BossCheck = (int) ((listOfRooms.Count - 1) *0.5f +1);

        //��� �� ���� ����
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            //�� ����
            if ((listOfRooms.Count - 1) * 0.5f + 1 <= i)
            {
                CreateMesh_Door(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
            }
            else
            //�� ����
            {
                CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
            }
        }
        // �������
        CreateWalls(wallParent);

        //surface.BuildNavMesh();

    }

    private void CreateWalls(GameObject wallParent)
    {
        // �� ����

        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition, walllHorizontal);
            
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical);
        }

        wallParent.AddComponent<BoxCollider>();
        //11.10 �߰� by �յ���
        //�ּ�ó���Ǿ� �ִµ� �׺�Ž��� �ʿ��Ѱ��� �� �𸣰ھ Ȱ��ȭ�ص׽��ϴ�.
        // NavMesh�� ���� �������� �����
        surface.BuildNavMesh();
        
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        // �� ����... ������ �̿�
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);

        // NavMesh�� ���� �������� ����� 
        wallPrefab.isStatic = true;
        wallPrefab.AddComponent<BoxCollider>();
        
    }

    private void CreateMesh (Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        // �� ������ִ� �Ž��
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
            // �ð����...?

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

        
        //�� �ݸ����� ����
        roomCollider[count] = dungeonFloor.AddComponent<BoxCollider>();



        //11.10 �߰� by �յ���
        //{

        //�� ��ũ��Ʈ �߰�
        Room r = dungeonFloor.AddComponent<Room>();

        //�� �ݸ����� ��ġ�� �̿��ؼ� �ش� ���� position ��ǥ ���ϰ�, Room��ũ��Ʈ ������ ����
        dungeonFloor.GetComponent<Room>().roomPosition = roomCollider[count].center;

        //�뿡 ���� ���� ���� �����س���(ù �� ã�ƾ���)
        r.index = count;
        //Debug.Log(count);

        //�� ��°�� �����Ǿ����� ī��Ʈ
        count++;


        //�±׶� ���̾� ����
        dungeonFloor.tag = "Floor";
        dungeonFloor.layer = 7;

        //���θŴ��� �����Ŵ����� ���� �迭 ����
        Spawner[] sp = MainManager.instance.spawnManager.spawners;

        r.isBossIndex = count_BossCheck + 2;
        r.spawners = sp[sp.Length - 1];

        //���� ���� ������ ���̸�
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


        //�������� ���� ��������
        ////���θŴ��� �����Ŵ����� ���� �迭 ����
        //Spawner[] sp = MainManager.instance.spawnManager.spawners;

        ////�뿡 �Ҵ��� ������ ������ ���� ���ϱ�
        //int rand = Random.Range(1, sp.Length);

        ////���� ���� ����
        //r.spawners = new Spawner[rand];

        ////���� ���� ��ŭ�� �����ʸ� ����. ������ �����ʴ� �Ŵ����� ������ �迭���� �������� ����.
        //for (int i =0; i<r.spawners.Length; i++)
        //{
        //    //�������� �����Ŵ��� �� �������� �� �����̶� �Ʒ� �ڵ尡 �� ����
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
    /// 11.10 �߰� by �յ���
    /// �� ���� �� ����� �ڵ�
    /// ���� ������Ʈ �߰� �� ������ �߰�
    /// </summary>
    /// <param name="bottomLeftCorner"></param>
    /// <param name="topRightCorner"></param>
    private void CreateMesh_Door(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        // �� ������ִ� �Ž��
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
            // �ð����...?

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

        //11.10 �߰� by �յ���
        //{
        roomCollider[count] = dungeonFloor.AddComponent<BoxCollider>();

        //�� ������ �߰�.
        GameObject objDoor = Instantiate(door, dungeonFloor.transform);
        objDoor.transform.position = roomCollider[count].center + Vector3.up * 2.5f;

        //�� �������� �� ã�� �Լ� ����
        
        for (int i = 0; i < 4; i++)
        {
            bool result = objDoor.transform.GetChild(i).GetComponent<Door>().CheckRoom();
            //Debug.Log($"{i} {dungeonFloor.name}�� �� ã�� ��� = {result}");
        }
        //�� ��°�� �����Ǿ����� ī��Ʈ
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
        // ��� �ʰ� ������Ʈ�� �����
        while(transform.childCount != 0)
        {
            foreach(Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}
