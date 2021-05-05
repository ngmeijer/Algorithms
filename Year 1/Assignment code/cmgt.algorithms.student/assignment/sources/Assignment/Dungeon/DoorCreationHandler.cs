using System;
using System.Collections.Generic;
using System.Drawing;
using GXPEngine;

public class DoorCreationHandler
{
    public enum DoorMaster
    {
        THIS_ROOM = 0,
        NEIGHBOUR_ROOM = 1,
        UNDEFINED = 2,
    };

    private const int MAX_DOOR_COUNT = 2;
    private const int OFFSET = 0;

    private int doorCount;
    private RoomArea roomArea;
    private RoomContainer parentRoom;

    public DoorCreationHandler(RoomContainer pParentRoom, RoomArea pRoomArea)
    {
        parentRoom = pParentRoom;
        roomArea = pRoomArea;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                   void InitiateDoorHandling()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    public void InitiateDoorHandling(List<RoomContainer> pFinishedRooms)
    {
        List<RoomContainer> neighbourRooms = findNeighbourRooms(pFinishedRooms);

        foreach (RoomContainer neighbour in neighbourRooms)
        {
            Console.WriteLine($"This room ID: {parentRoom.ID}. Other ID: {neighbour.ID}");
            //DoorMaster master = defineDoorResponsibility(room);
            //if (master == DoorMaster.UNDEFINED)
            //    continue;

            //checkDoorResponsibility(master, room);
        }
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                   void checkDoorResponsibility()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    private void checkDoorResponsibility(DoorMaster pMaster, RoomContainer pOtherRoom)
    {
        switch (pMaster)
        {
            case DoorMaster.THIS_ROOM:
                placeDoor(pOtherRoom.RoomArea);
                break;
            case DoorMaster.NEIGHBOUR_ROOM:
                pOtherRoom.DoorCreator.placeDoor(roomArea);
                break;
        }

        incrementDoorCount();
        pOtherRoom.DoorCreator.incrementDoorCount();
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                            Door placeDoor()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Door</returns>
    private Door placeDoor(RoomArea pOtherRoomArea)
    {
        Point newDoorLocation = new Point();

        AXIS usedAxis = determineDoorAxis(pOtherRoomArea);

        switch (usedAxis)
        {
            case AXIS.HORIZONTAL:
                newDoorLocation.X = ((pOtherRoomArea.rightSide - pOtherRoomArea.leftSide) / 2) + roomArea.leftSide;
                newDoorLocation.Y = roomArea.topSide;
                break;
            case AXIS.VERTICAL:
                break;
        }
        return new Door(newDoorLocation);
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                      AXIS  determineDoorAxis()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>AXIS</returns>
    private AXIS determineDoorAxis(RoomArea pOtherRoom)
    {
        AXIS usedAxis = AXIS.UNDEFINED;

        if (pOtherRoom.bottomSide == roomArea.topSide + 1 || pOtherRoom.topSide == roomArea.bottomSide - 1)
            usedAxis = AXIS.HORIZONTAL;

        if (pOtherRoom.leftSide == roomArea.rightSide - 1 || pOtherRoom.rightSide == roomArea.leftSide + 1)
            usedAxis = AXIS.VERTICAL;

        return usedAxis;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                     void incrementDoorCount()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    private void incrementDoorCount() => doorCount++;

    //------------------------------------------------------------------------------------------------------------------------
    //			                                List<Room> findNeighbourRooms()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>List<RoomContainer></returns>
    private List<RoomContainer> findNeighbourRooms(List<RoomContainer> pFinishedRooms)
    {
        List<RoomContainer> neighbourRooms = new List<RoomContainer>();
        //neighbourRooms.Remove(parentRoom);

        foreach (RoomContainer otherRoom in pFinishedRooms)
        {
            //Learned something new: break is for completely exiting the loop, continue is used to skip this iteration!
            if (neighbourRooms.Contains(otherRoom)) continue;
            if (otherRoom.ID == parentRoom.ID) continue;
            RoomArea other = otherRoom.RoomArea;

            bool leftSideAligned = checkHorizontalNeighbourAlignment(other.leftSide + 1);
            bool rightSideAligned = checkHorizontalNeighbourAlignment(other.rightSide - 1);

            bool topSideAligned = checkVerticalNeighbourAlignment(other.topSide + 1);
            bool bottomSideAligned = checkVerticalNeighbourAlignment(other.bottomSide - 1);

            if ((leftSideAligned || rightSideAligned) && (topSideAligned || bottomSideAligned))
                neighbourRooms.Add(otherRoom);
        }

        return neighbourRooms;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                             DoorMaster defineDoorResponsibility()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>DoorMaster</returns>
    private DoorMaster defineDoorResponsibility(RoomContainer pOtherRoom)
    {
        int otherRoomDoorCount = pOtherRoom.DoorCreator.doorCount;

        //Both rooms have max amount of doors
        if (doorCount >= MAX_DOOR_COUNT && otherRoomDoorCount >= MAX_DOOR_COUNT)
            return DoorMaster.UNDEFINED;

        //This room has not reached the max yet, the other one has.
        if (doorCount < MAX_DOOR_COUNT && otherRoomDoorCount >= MAX_DOOR_COUNT)
            return DoorMaster.THIS_ROOM;

        //This room has reached the max, the other one has not.
        if (doorCount >= MAX_DOOR_COUNT && otherRoomDoorCount < MAX_DOOR_COUNT)
            return DoorMaster.NEIGHBOUR_ROOM;

        if (doorCount == otherRoomDoorCount)
            return (DoorMaster)Utils.Random(0, 2);

        if (doorCount < otherRoomDoorCount)
            return DoorMaster.THIS_ROOM;

        if (doorCount > otherRoomDoorCount)
            return DoorMaster.NEIGHBOUR_ROOM;

        return DoorMaster.UNDEFINED;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                           bool checkNeighbourRoomBoundaryConditions()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Bool</returns>
    private bool checkVerticalNeighbourAlignment(int pOtherSide)
    {
        if (checkIfOnExactBorder(pOtherSide, roomArea.topSide, roomArea.bottomSide) ||
            checkIfInsideAreaWithOffset(pOtherSide, roomArea.topSide, roomArea.bottomSide))
            return true;
        return false;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                           bool checkNeighbourRoomBoundaryConditions()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Bool</returns>
    private bool checkHorizontalNeighbourAlignment(int pOtherSide)
    {
        if (checkIfOnExactBorder(pOtherSide, roomArea.leftSide, roomArea.rightSide) ||
            checkIfInsideAreaWithOffset(pOtherSide, roomArea.leftSide, roomArea.rightSide))
            return true;
        return false;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                           bool checkIfOnExactBorder()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Bool</returns>
    private bool checkIfOnExactBorder(int pOtherSide, int pMainSide0, int pMainSide1)
        => pOtherSide == pMainSide0 || pOtherSide == pMainSide1;

    //------------------------------------------------------------------------------------------------------------------------
    //			                                      bool checkIfInsideAreaWithOffset()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Bool</returns>
    private bool checkIfInsideAreaWithOffset(int pOtherSide, int pMainSide0, int pMainSide1)
        => pOtherSide > (pMainSide0 + OFFSET) && pOtherSide < (pMainSide1 - OFFSET);
}