using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator 
{
    // 방과 복도를 생성하고 던전공간들을 나누는 메인 클래스

    
    List<RoomNode> allNodesCollection = new List<RoomNode>();

    private int dungeonWidth;
    private int dungeonLength;

    public DungeonGenerator(int dungeonWidth, int dungeonLength)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;
    }

    public List<Node> CalculateDungeon (int maxIterations, int roomWidthMin, int roomLengthMin, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidth) // 방 크기 계산, 리스트 형식 메써드
    {
        // 이중 공간 분할법 (던전 세로, 가로) 길이를 사용해서 만듬
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength); 
        
        // allSpaceNodes 는 방 노드의 리스트 형식이며, 이중공간 분할법에 PrepareNodesCollection 에 방 갯수, 방 세로, 가로 길이를 사용해서 방의 크기를 계산함
        allNodesCollection = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLengthMin);

        // 방을 먼저 더 작게 나누어야한다.
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeaves(bsp.RootNode);

        // 나누어진 공간에 실제 방을 만든다.
        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomLengthMin, roomWidthMin);

        // 
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces, roomBottomCornerModifier, roomTopCornerModifier, roomOffset);

        CorridorGenerator corridorGenerator = new CorridorGenerator();
        var corridorList = corridorGenerator.CreateCorridor(allNodesCollection, corridorWidth);

        return new List<Node>(roomList).Concat(corridorList).ToList();
    }



}