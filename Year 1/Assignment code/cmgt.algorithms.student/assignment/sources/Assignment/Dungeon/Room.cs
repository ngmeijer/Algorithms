﻿using System;
using System.Collections.Generic;
using System.Drawing;
using GXPEngine;

public struct RoomArea
{
    public int leftSide;
    public int rightSide;
    public int topSide;
    public int bottomSide;
}

public enum DoorMaster
{
    THIS_ROOM,
    NEIGHBOUR_ROOM,
    UNDEFINED,
};

/**
 * This class represents (the data for) a Room, at this moment only a rectangle in the dungeon.
 */
public class Room : GameObject
{
    public RoomCreator roomCreator;
    public Rectangle OriginalSize;
    public RoomArea RoomArea;

    private int doorCount;
    public int ID;
    public float RandomSplitValue;

    private const int MAX_DOOR_COUNT = 2;
    private const int OFFSET = 3;

    //"Worldspace" coordinates
    private Vec2 screenPosition;
    private EasyDraw idText;

    private delegate Door OnDoorPlacing(RoomArea pOtherArea);
    private OnDoorPlacing onDoor;

    public Room(Rectangle pOriginalSize)
    {
        AlgorithmsAssignment.OnGenerateDestroyPrevious += handleDestroy;
        OriginalSize = pOriginalSize;

        defineRoomArea();
        handleDebugTextInitalization();

        roomCreator = new RoomCreator(this, RoomArea, OriginalSize, RandomSplitValue);

        onDoor += placeDoor;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //										            string ToString()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    public override string ToString()
    {
        return String.Format("Room ID: {0}\nLeft side:{1}, right side:{2}, \ntop side:{3}, bottom side:{4}", ID,
            RoomArea.leftSide, RoomArea.rightSide, RoomArea.topSide, RoomArea.bottomSide);
    }

    //------------------------------------------------------------------------------------------------------------------------
    //										        void handleDestroy()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private void handleDestroy()
    {
        AlgorithmsAssignment.OnGenerateDestroyPrevious -= handleDestroy;
        Destroy();
    }

    //------------------------------------------------------------------------------------------------------------------------
    //										       void defineRoomArea()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private void defineRoomArea()
    {
        RoomArea.leftSide = OriginalSize.X;
        RoomArea.rightSide = OriginalSize.X + OriginalSize.Width;
        RoomArea.topSide = OriginalSize.Y;
        RoomArea.bottomSide = OriginalSize.Y + OriginalSize.Height;

        screenPosition.x = (RoomArea.leftSide + 1) * AlgorithmsAssignment.SCALE;
        screenPosition.y = (RoomArea.topSide + 4) * (AlgorithmsAssignment.SCALE);
    }

    //------------------------------------------------------------------------------------------------------------------------
    //									    void handleDebugTextInitalization()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private void handleDebugTextInitalization()
    {
        idText = new EasyDraw(game.width, game.height);
        AddChild(idText);
        idText.SetColor(0, 255, 0);
        idText.SetScaleXY(0.1f, 0.1f);
    }

    //------------------------------------------------------------------------------------------------------------------------
    //								                void UpdateRoomID(int pID)
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    public void UpdateRoomID(int pID)
    {
        ID = pID;
        idText.Text($"ID: {pID}." +
                    $"\nLeft: {RoomArea.leftSide}." +
                    $"\nRight: {RoomArea.rightSide}." +
                    $"\nTop: {RoomArea.topSide}." +
                    $"\nBottom:{RoomArea.bottomSide}", screenPosition.x, screenPosition.y + 115);
    }

    //------------------------------------------------------------------------------------------------------------------------
    //										         bool ShouldSplit()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    public bool ShouldSplit()
    {
        int minSize = AlgorithmsAssignment.MIN_ROOM_SIZE;
        if (OriginalSize.Width > minSize && OriginalSize.Height > minSize)
            return true;

        return false;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                   void InitiateDoorHandling()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    public void InitiateDoorHandling(List<Room> pFinishedRooms)
    {
        List<Room> neighbourRooms = findNeighbourRooms(pFinishedRooms);

        foreach (Room room in neighbourRooms)
        {
            DoorMaster master = defineDoorResponsibility(room);
            communicateDoorResponsibility(master, room);
            //Console.WriteLine($"\n\nNeighbour room ID: {room.ID} \nThis ID: {ID}.\nFound neighbours: {neighbourRooms.Count}");
        }
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                             DoorMaster defineDoorResponsibility()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private DoorMaster defineDoorResponsibility(Room pOtherRoom)
    {
        int thisRoomDoorCount = doorCount;
        int otherRoomDoorCount = pOtherRoom.doorCount;

        DoorMaster responsibleRoomIndex = DoorMaster.THIS_ROOM;

        //Both rooms have max amount of doors
        if (thisRoomDoorCount >= MAX_DOOR_COUNT && otherRoomDoorCount >= MAX_DOOR_COUNT)
            return DoorMaster.UNDEFINED;

        //This room has not reached the max yet, the other one has.
        if (thisRoomDoorCount < MAX_DOOR_COUNT && otherRoomDoorCount >= MAX_DOOR_COUNT)
            responsibleRoomIndex = DoorMaster.THIS_ROOM;

        //This room has reached the max, the other one has not.
        if (thisRoomDoorCount >= MAX_DOOR_COUNT && otherRoomDoorCount < MAX_DOOR_COUNT)
            responsibleRoomIndex = DoorMaster.NEIGHBOUR_ROOM;

        if (thisRoomDoorCount < otherRoomDoorCount)
            responsibleRoomIndex = DoorMaster.THIS_ROOM;

        if (thisRoomDoorCount > otherRoomDoorCount)
            responsibleRoomIndex = DoorMaster.NEIGHBOUR_ROOM;

        if (thisRoomDoorCount == otherRoomDoorCount)
            responsibleRoomIndex = (DoorMaster)Utils.Random((int)DoorMaster.THIS_ROOM, (int)DoorMaster.UNDEFINED);

        return responsibleRoomIndex;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                              void communicateDoorResponsibility()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private void communicateDoorResponsibility(DoorMaster pResponsibleRoom, Room pOtherRoom)
    {
        if (pResponsibleRoom == DoorMaster.NEIGHBOUR_ROOM)
            validateDoorMaster(DoorMaster.NEIGHBOUR_ROOM, pOtherRoom);

        if (pResponsibleRoom == DoorMaster.THIS_ROOM)
            validateDoorMaster(DoorMaster.THIS_ROOM, pOtherRoom);
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                     void validateDoorMaster()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private void validateDoorMaster(DoorMaster pMaster, Room pOtherRoom)
    {
        switch (pMaster)
        {
            case DoorMaster.THIS_ROOM:
                placeDoor(pOtherRoom.RoomArea);
                break;
            case DoorMaster.NEIGHBOUR_ROOM:
                pOtherRoom.placeDoor(this.RoomArea);
                break;
        }

        incrementDoorCount();
        pOtherRoom.incrementDoorCount();
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                     void incrementDoorCount()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private void incrementDoorCount() => doorCount++;

    //------------------------------------------------------------------------------------------------------------------------
    //			                                            Door placeDoor()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private Door placeDoor(RoomArea pOtherRoomArea)
    {
        Point newDoorLocation = new Point();

        AXIS usedAxis = determineDoorAxis(pOtherRoomArea);

        switch (usedAxis)
        {
            case AXIS.HORIZONTAL:
                newDoorLocation.X = ((pOtherRoomArea.rightSide - pOtherRoomArea.leftSide) / 2) + this.RoomArea.leftSide;
                newDoorLocation.Y = this.RoomArea.topSide;
                break;
            case AXIS.VERTICAL:
                break;
        }

        Door doorInstance = new Door(newDoorLocation);
        return doorInstance;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                      AXIS  determineDoorAxis()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private AXIS determineDoorAxis(RoomArea pOtherRoom)
    {
        AXIS usedAxis = AXIS.UNDEFINED;

        if (pOtherRoom.bottomSide == this.RoomArea.topSide + 1 || pOtherRoom.topSide == this.RoomArea.bottomSide - 1)
            usedAxis = AXIS.HORIZONTAL;

        if (pOtherRoom.leftSide == this.RoomArea.rightSide - 1 || pOtherRoom.rightSide == this.RoomArea.leftSide + 1)
            usedAxis = AXIS.VERTICAL;

        return usedAxis;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                List<Room> findNeighbourRooms()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private List<Room> findNeighbourRooms(List<Room> pFinishedRooms)
    {
        List<Room> neighbourRooms = new List<Room>();

        foreach (Room otherRoom in pFinishedRooms)
        {
            if (otherRoom == this)
                break;
            if (neighbourRooms.Contains(otherRoom))
                break;

            bool horizontallyAligned = false;
            bool verticallyAligned = false;

            //Readability purposes
            RoomArea other = otherRoom.RoomArea;
            RoomArea main = this.RoomArea;

            if (checkNeighbourRoomBoundaryConditions(other.leftSide, main.leftSide, main.rightSide)
                || checkNeighbourRoomBoundaryConditions(other.rightSide, main.leftSide, main.rightSide))
                horizontallyAligned = true;

            if (checkNeighbourRoomBoundaryConditions(other.topSide, main.topSide, main.bottomSide)
                || checkNeighbourRoomBoundaryConditions(other.bottomSide, main.topSide, main.bottomSide))
                verticallyAligned = true;

            if (horizontallyAligned && verticallyAligned)
                neighbourRooms.Add(otherRoom);
        }

        return neighbourRooms;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                           bool checkNeighbourRoomBoundaryConditions()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private bool checkNeighbourRoomBoundaryConditions(int pOtherSide, int pMainSide0, int pMainSide1)
        => checkIfOnExactBorder(pOtherSide, pMainSide0, pMainSide1) ||
          checkIfInsideAreaWithOffset(pOtherSide, pMainSide0, pMainSide1);

    //------------------------------------------------------------------------------------------------------------------------
    //			                                           bool checkIfOnExactBorder()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private bool checkIfOnExactBorder(int pOtherSide, int pMainSide0, int pMainSide1)
        => pOtherSide == pMainSide0 || pOtherSide == pMainSide1;

    //------------------------------------------------------------------------------------------------------------------------
    //			                                      bool checkIfInsideAreaWithOffset()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private bool checkIfInsideAreaWithOffset(int pOtherSide, int pMainSide0, int pMainSide1)
        => pOtherSide > (pMainSide0 + OFFSET) && pOtherSide < (pMainSide1 - OFFSET);
}