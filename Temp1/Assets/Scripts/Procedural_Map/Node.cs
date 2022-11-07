using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    // 이중 트리 구조를 형성하는데에 사용할 클래스


    private List<Node> childrenNodeList; // 자식 노드는 리스트 형식

    public List<Node> ChildrenNodeList { get => childrenNodeList; }

    // 방문했다는 bool type
    public bool Visited { get; set; }

    // 방의 4가지 구석쪽을 표현
    public Vector2Int BottomLeftAreaCorner { get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopLeftAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }

    // 부모 노드
    public Node Parent { get; set; }

    // 트리 형식
    public int TreeLayerIndex { get; set; }

    public Node(Node parentNode)
    {
        childrenNodeList = new List<Node>();
        this.Parent = parentNode;
        if (parentNode != null) // 만약 부모 노드가 있으면
        {
            parentNode.AddChild(this); // 자식 노드를 추가해라
        }
    }

    public void AddChild(Node node) // 자식 노드 
    {
        childrenNodeList.Add(node); // 자식 노드 추가
    }

    public void RemoveChild(Node node)
    {
        childrenNodeList.Remove(node); // 자식 노드 삭제
    }

}