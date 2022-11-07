using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class BinarySpacePartitioner
{
    RoomNode rootNode; // 루트 노드 (맨 처음 노드/방)

    public RoomNode RootNode { get => rootNode; }
    public BinarySpacePartitioner(int dungeonWidth, int dungeonLength) // 이중 공간 분할법, int 던전배경의 가로 세로 길이 활용
    {
        this.rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(dungeonWidth, dungeonLength), null, 0);
    }

    public List<RoomNode> PrepareNodesCollection(int maxIterations, int roomWidthMin, int roomLengthMin)
    {
        Queue<RoomNode> graph = new Queue<RoomNode>();// 큐 형식의 룸 노드를 graph 이름으로 새로 불러오기
        List<RoomNode> listToReturn = new List<RoomNode>();// 리스트 형식 의 룸 노드를 listToReturn 이름으로 새로 불러오기
        graph.Enqueue(this.rootNode);// 큐의 끝부분에 루트 노드를 추가한다
        listToReturn.Add(this.rootNode);// 리스트에 루트노드를 추가한다
        int iterations = 0;// 반복 = 0;
        while (iterations < maxIterations && graph.Count > 0)// 반복 횟수가 최대 반복횟수 보다 작고, 큐 형식 크래프 횟수가 0보다 클때마다...
        {
            // 반복을 하나씩 올리고
            iterations++;

            // 룸 노드의 현재 노드는 큐 형식 graph 의 시작 부분 개체를 제거하고 반환한다
            RoomNode currentNode = graph.Dequeue();

            // 만약 현재 노드(방)의 가로-세로 길이가 만들수있는 방의 최소 가로 세로 길이의 2배보다 작다면...
            if (currentNode.Width >= roomWidthMin * 2 || currentNode.Length >= roomLengthMin * 2)
            {
                // 방을 나눠라 (현재 방, 리스트 형식 listToReturn, 방의 가로 세로길이, 큐 형식 graph)
                SplitTheSpace(currentNode, listToReturn, roomLengthMin, roomWidthMin, graph);
            }
        }
        return listToReturn; // 모든 방의 리스트를 return 한다
    }

    private void SplitTheSpace(RoomNode currentNode, List<RoomNode> listToReturn, int roomLengthMin, int roomWidthMin, Queue<RoomNode> graph)
    {

        // 방을 나누는 방법
        // 라인 클래스를 이용해 선이 공간을 나누게끔 만들기
        Line line = GetLineDividingSpace(
            currentNode.BottomLeftAreaCorner,
            currentNode.TopRightAreaCorner,
            roomWidthMin,
            roomLengthMin);

        // 나눈 공간을 2개로 더 작게 만들기
        // 정확한 설명...
        RoomNode node1, node2;
        if (line.Orientation == Orientation.Horizontal)
        {
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(currentNode.TopRightAreaCorner.x, line.Coordinates.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);
            node2 = new RoomNode(new Vector2Int(currentNode.BottomLeftAreaCorner.x, line.Coordinates.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }
        else
        {
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(line.Coordinates.x, currentNode.TopRightAreaCorner.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);
            node2 = new RoomNode(new Vector2Int(line.Coordinates.x, currentNode.BottomLeftAreaCorner.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }

        // 만들어진 방들을 graph 로 추가하는 메써드
        AddNewNodeToCollections(listToReturn, graph, node1);
        AddNewNodeToCollections(listToReturn, graph, node2);
    }

    private void AddNewNodeToCollections(List<RoomNode> listToReturn, Queue<RoomNode> graph, RoomNode node)
    {
        listToReturn.Add(node);
        graph.Enqueue(node);
    }

    private Line GetLineDividingSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLengthMin)
    {

        // bool 타입으로 가로 세로길이를 먼저 확인하고 공간을 나눈다.
        Orientation orientation; // Orientation을 가져오고

        // 가로길이 확인방법 = 두 지점의 y 값을 빼면 그 사이의 길이가 나온다. 이게 만약 만들어질 방의 최소 가로 길이의 2배보다 작으면 가로길이가 나눌 수 있는 길이가 된다.
        bool lengthStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y) >= 2 * roomLengthMin;
        // 세로길이 확인방법 = 두 지점의 x 값을 빼면 그 사이의 길이가 나온다. 이게 만약 만들어질 방의 최소 세로 길이의 2배보다 작으면 세로길이가 나눌 수 있는 길이가 된다.
        bool widthStatus = (topRightAreaCorner.x - bottomLeftAreaCorner.x) >= 2 * roomWidthMin;

        if (lengthStatus && widthStatus)// 만약 가로 세로 길이 둘다 나눌 수 있는 길이라면..
        {
            orientation = (Orientation)(Random.Range(0, 2));// 둘중에 하나를 랜덤으로 지정해서 나눈다
        }
        else if (widthStatus) // 만약 세로 길이가 더 크다면...
        {
            orientation = Orientation.Vertical;// 가로로 한번 나눈다.
        }
        else// 만약 가로 길이가 더 크다면...
        {
            orientation = Orientation.Horizontal;// 세로로 한번 나눈다
        }
        // 다시 라인을 리턴해서 새로운 공간을 나누기 시작한다.
        // GetCoordinatesForOrientation 은 다음 공간을 나누는 것에 방향을 잡아주는 역할을 한다
        return new Line(orientation, GetCoordinatesFororientation(
            orientation,
            bottomLeftAreaCorner,
            topRightAreaCorner,
            roomWidthMin,
            roomLengthMin));
    }

    private Vector2Int GetCoordinatesFororientation(Orientation orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLengthMin)
    {
        // 공간을 나눌때 방향을 잡아준다.

        Vector2Int coordinates = Vector2Int.zero; // Vector2Int 제로를 만든후

        if (orientation == Orientation.Horizontal)// 만약 orientation 이 가로길이랑 같다면...
        {
            // 가로 길이를 랜덤으로 지정해서 만든다
            // << 왜 + - 하는지 모름...>>
            coordinates = new Vector2Int(
                0,
                Random.Range(
                (bottomLeftAreaCorner.y + roomLengthMin),
                (topRightAreaCorner.y - roomLengthMin)));
        }
        else // 만약 orientation 이 세로길이랑 같다면
        {
            // 세로길이를 랜덤으로 지정해서 만든다
            coordinates = new Vector2Int(
                Random.Range(
                (bottomLeftAreaCorner.x + roomWidthMin),
                (topRightAreaCorner.x - roomWidthMin))
                , 0);
        }
        return coordinates;
    }
}