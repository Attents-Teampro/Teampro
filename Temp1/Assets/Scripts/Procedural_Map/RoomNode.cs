using UnityEngine;

public class RoomNode : Node
{
    // 방을 생성할때 필요한 클래스


    // 방 노드 생성할때 
    public RoomNode(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreacorner, Node parentNode, int index) : base(parentNode)
    {
        this.BottomLeftAreaCorner = bottomLeftAreaCorner;
        this.TopRightAreaCorner = topRightAreacorner;
        this.BottomRightAreaCorner = new Vector2Int(TopRightAreaCorner.x, bottomLeftAreaCorner.y);
        this.TopLeftAreaCorner = new Vector2Int(bottomLeftAreaCorner.x, TopRightAreaCorner.y);
        this.TreeLayerIndex = index;
    }

    public int Width { get => (int)(TopRightAreaCorner.x - BottomLeftAreaCorner.x); }

    public int Length { get => (int)(TopRightAreaCorner.y - BottomLeftAreaCorner.y); }
}