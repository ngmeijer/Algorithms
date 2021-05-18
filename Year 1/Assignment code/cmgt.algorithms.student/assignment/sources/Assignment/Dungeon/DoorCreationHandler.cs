using GXPEngine;
using System.Collections.Generic;
using System.Drawing;

public class DoorCreationHandler
{
    public enum DoorMaster
    {
        THIS_ROOM = 0,
        NEIGHBOUR_ROOM = 1,
        UNDEFINED = 2,
    };

    private const int MAX_DOOR_COUNT = 2;
    public const int OFFSET = 0;

    private int doorCount;
    private RoomArea roomArea;
    private RoomContainer parentRoom;
    private NeighbourRoomFinder roomFinder;

    public DoorCreationHandler(RoomContainer pParentRoom, RoomArea pRoomArea)
    {
        parentRoom = pParentRoom;
        roomArea = pRoomArea;

        roomFinder = new NeighbourRoomFinder(roomArea);
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                   void InitiateDoorHandling()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    public Door[] InitiateDoorHandling(List<RoomContainer> pFinishedRooms)
    {
        List<Door> newDoors = new List<Door>();
        List<RoomContainer> neighbourRooms = roomFinder.findNeighbourRooms(parentRoom, pFinishedRooms);

        foreach (RoomContainer neighbour in neighbourRooms)
        {
            DoorMaster master = defineDoorResponsibility(neighbour);
            if (master == DoorMaster.UNDEFINED)
                continue;

            Door newDoor = createNewDoor(master, neighbour);
            newDoors.Add(newDoor);
        }

        //To array, because in advance I do not know how many doors there will be (and if there would be a limit, if the limit would be reached.
        //So for that reason, create a temp List, once all doors are added, convert to array
        return newDoors.ToArray();
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                   void checkDoorResponsibility()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    private Door createNewDoor(DoorMaster pMaster, RoomContainer pOtherRoom)
    {
        Point newDoorPosition = new Point();
        switch (pMaster)
        {
            case DoorMaster.THIS_ROOM:
                newDoorPosition = defineDoorPosition(pOtherRoom.RoomArea);
                break;
            case DoorMaster.NEIGHBOUR_ROOM:
                newDoorPosition = pOtherRoom.DoorCreator.defineDoorPosition(parentRoom.RoomArea);
                break;
        }

        incrementDoorCount();
        pOtherRoom.DoorCreator.incrementDoorCount();

        Door newDoor = new Door(newDoorPosition)
        {
            RoomContainerA = parentRoom,
            RoomContainerB = pOtherRoom
        };

        return newDoor;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //			                                            Door placeDoor()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Door</returns>
    private Point defineDoorPosition(RoomArea pOtherRoomArea)
    {
        AXIS usedAxis = determineDoorAxis(pOtherRoomArea);

        int xPos = 0, yPos = 0;

        switch (usedAxis)
        {
            case AXIS.HORIZONTAL:
                xPos = 0;
                yPos = calculateAxisPosition(usedAxis, pOtherRoomArea);
                break;
            case AXIS.VERTICAL:
                xPos = calculateAxisPosition(usedAxis, pOtherRoomArea);
                yPos = 0;
                break;
        }

        return new Point(xPos, yPos);
    }

    private int calculateHorizontalRoomOverlap(RoomArea pOther)
    {
        int overlap = 0;

        //Check if either left or right side is inside this room, horizontally.
        if (pOther.leftSide >= this.roomArea.leftSide && pOther.leftSide <= (this.roomArea.rightSide - OFFSET))
            overlap = this.roomArea.rightSide - pOther.leftSide;
        if (pOther.rightSide >= (this.roomArea.leftSide + OFFSET) && pOther.rightSide <= this.roomArea.rightSide)
            overlap = pOther.rightSide - this.roomArea.leftSide;

        //Check if both sides are inside this room, horizontally.
        if (pOther.leftSide >= this.roomArea.leftSide && pOther.rightSide <= this.roomArea.rightSide)
            overlap = pOther.rightSide - pOther.leftSide;

        //Check if both sides are outside this room (but still overlap), horizontally.
        if (pOther.leftSide < this.roomArea.leftSide && pOther.rightSide > this.roomArea.leftSide)
        {
            overlap = this.roomArea.rightSide - this.roomArea.leftSide;
        }

        return overlap;
    }

    private int calculateVerticalRoomOverlap(RoomArea pOther, out int pDoorSide)
    {
        int overlap = 0;
        int doorSide = 0;

        //Check if either top or bottom side is inside this room, vertically.
        if (pOther.topSide >= (this.roomArea.topSide + OFFSET) && pOther.topSide <= (this.roomArea.bottomSide - OFFSET))
        {
            doorSide = this.roomArea.topSide;
            overlap = this.roomArea.topSide - pOther.bottomSide;
        }

        if (pOther.bottomSide <= (this.roomArea.bottomSide - OFFSET) && pOther.bottomSide >= (this.roomArea.topSide + OFFSET))
        {
            doorSide = pOther.topSide;
            overlap = pOther.topSide - this.roomArea.bottomSide;
        }

        //Check if both other sides are inside this room, vertically.
        if (pOther.bottomSide <= this.roomArea.bottomSide && pOther.topSide >= this.roomArea.topSide)
        {
            doorSide = pOther.topSide;
            overlap = pOther.topSide - pOther.bottomSide;
        }

        //Check if both sides are outside this room (but still overlap), horizontally.
        if (pOther.bottomSide < this.roomArea.bottomSide && pOther.topSide > this.roomArea.topSide)
        {
            doorSide = this.roomArea.topSide;
            overlap = this.roomArea.topSide - this.roomArea.bottomSide;
        }

        pDoorSide = doorSide;

        return overlap;
    }

    private int calculateAxisPosition(AXIS pAxis, RoomArea pRoomArea)
    {
        int overlap = 0;
        int side = 0;

        if (pAxis == AXIS.HORIZONTAL)
            overlap = calculateHorizontalRoomOverlap(pRoomArea);
        if (pAxis == AXIS.VERTICAL)
            overlap = calculateVerticalRoomOverlap(pRoomArea, out side);

        int position = side + (overlap / 2);

        return position;
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

        if (pOtherRoom.leftSide == roomArea.rightSide - 1 || pOtherRoom.rightSide == roomArea.leftSide + 1)
            usedAxis = AXIS.HORIZONTAL;

        if (pOtherRoom.bottomSide == roomArea.topSide + 1 || pOtherRoom.topSide == roomArea.bottomSide - 1)
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
}