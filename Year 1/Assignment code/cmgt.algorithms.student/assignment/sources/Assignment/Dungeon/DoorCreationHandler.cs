using System.Collections.Generic;
using System.Drawing;
using GXPEngine;

public class DoorCreationHandler
{
    private const int MAX_DOOR_COUNT = 2;
    private const int OFFSET = 3;

    private int doorCount;
    private RoomArea roomArea;
    private RoomContainer parent;

    public DoorCreationHandler(RoomContainer pParent, RoomArea pRoomArea)
    {
        parent = pParent;
        roomArea = pRoomArea;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                   void InitiateDoorHandling()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    public void InitiateDoorHandling(List<RoomContainer> pFinishedRooms)
    {
        List<RoomContainer> neighbourRooms = findNeighbourRooms(pFinishedRooms);

        foreach (RoomContainer room in neighbourRooms)
        {
            DoorMaster master = defineDoorResponsibility(room);
            communicateDoorResponsibility(master, room);
            //Console.WriteLine($"\n\nNeighbour room ID: {room.ID} \nThis ID: {ID}.\nFound neighbours: {neighbourRooms.Count}");
        }
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                              void communicateDoorResponsibility()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private void communicateDoorResponsibility(DoorMaster pResponsibleRoom, RoomContainer pOtherRoom)
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
    private void validateDoorMaster(DoorMaster pMaster, RoomContainer pOtherRoom)
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
    /// <returns>String</returns>
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
    /// <returns>String</returns>
    private void incrementDoorCount() => doorCount++;

    //------------------------------------------------------------------------------------------------------------------------
    //			                                List<Room> findNeighbourRooms()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private List<RoomContainer> findNeighbourRooms(List<RoomContainer> pFinishedRooms)
    {
        List<RoomContainer> neighbourRooms = new List<RoomContainer>();

        foreach (RoomContainer otherRoom in pFinishedRooms)
        {
            if (otherRoom == parent)
                break;
            if (neighbourRooms.Contains(otherRoom))
                break;

            bool horizontallyAligned = false;
            bool verticallyAligned = false;

            //Readability purposes
            RoomArea other = otherRoom.RoomArea;
            RoomArea main = roomArea;

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
    //			                             DoorMaster defineDoorResponsibility()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private DoorMaster defineDoorResponsibility(RoomContainer pOtherRoom)
    {
        int thisRoomDoorCount = doorCount;
        int otherRoomDoorCount = pOtherRoom.DoorCreator.doorCount;

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