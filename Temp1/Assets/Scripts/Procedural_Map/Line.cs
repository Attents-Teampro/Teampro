#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;

using UnityEngine;

public class Line
{
    // 공간 분할에 사용되는 선의 정보를 저장하는 클래스


    Orientation orientation;
    Vector2Int coordinates;

    // Line 생성자를 만듬
    public Line(Orientation orientation, Vector2Int coordinates)
    {
        this.orientation = orientation;
        this.coordinates = coordinates;
    }

    public Orientation Orientation { get => orientation; set => orientation = value; }
    public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }
}


// enum = 열거형식... 값을 입력하지 않아도 자동으로 0,1,2,3.... 이 대입된다. 
public enum Orientation 
{
    // 즉, 현재 orientation 의 enum 을 대입해서
    // Horizontal 은 0번
    // Vertical 은 1번이 대입된다
    Horizontal = 0,
    Vertical = 1
}
#endif