using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StructureHelper
{
    public static List<Node> TraverseGraphToExtractLowestLeaves(Node parentNode)
    {
        // 이중 공간 분할법을 사용할때 마지막 방까지 만들어내는 클래스

        Queue<Node> nodesToCheck = new Queue<Node>(); // 
        List<Node> listToReturn = new List<Node>();


        if (parentNode.ChildrenNodeList.Count == 0) // 부모 노드의 자식 노드가 없는경우...
        {
            return new List<Node>() { parentNode }; // 부모노드를 자식노드가 없다는 리스트로 리턴하기
        }
        foreach (var child in parentNode.ChildrenNodeList) // 노드를 확인할때
        {
            nodesToCheck.Enqueue(child); // 노드들을 큐 형식으로 나열한다.
        }

        // 자식노드가 없는 자식노드들을 찾아낸다
        while (nodesToCheck.Count > 0) 
        {
            var currentNode = nodesToCheck.Dequeue();
            if (currentNode.ChildrenNodeList.Count == 0)
            {
                listToReturn.Add(currentNode);
            }
            else
            {
                foreach (var child in currentNode.ChildrenNodeList)
                {
                    nodesToCheck.Enqueue(child);
                }
            }
        }
        return listToReturn;
    }


    // pointModifier => 방의 크기를 랜덤하게 만들기 위해 사용하는 float
    // offset => int 로 만들어야지 벽이랑 기타 오브젝트를 더 쉽게 배치할수있기 때문이다. 
    // offset 의 용도...? 
    // bottomleft corner 와 top right corner 을 따로 계산하는 이유...?

    public static Vector2Int GenerateBottomLeftCornerBetween(
        Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        

        // 방의 최소와 최대 가로 세로 길이를 계산하기
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;

        return new Vector2Int(
            Random.Range(minX, (int)(minX + (maxX - minX) * pointModifier)),
            Random.Range(minY, (int)(minY + (maxY - minY) * pointModifier)));
    }
    public static Vector2Int GenerateTopRightCornerBetween(
        Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        // 방의 최소와 최대 가로 세로 길이를 계산하기
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;

        // 
        return new Vector2Int(
            Random.Range((int)(minX + (maxX - minX)* pointModifier),maxX),
            Random.Range((int)(minY + (maxY - minY)* pointModifier),maxY)
            );
    }

    public static Vector2Int CalculateMiddlePoint(Vector2Int v1, Vector2Int v2)
    {
        Vector2 sum = v1 + v2;
        Vector2 tempVector = sum / 2;
        return new Vector2Int((int)tempVector.x, (int)tempVector.y);
    }
}
public enum RelativePosition
{
    Up,
    Down,
    Right,
    Left
}