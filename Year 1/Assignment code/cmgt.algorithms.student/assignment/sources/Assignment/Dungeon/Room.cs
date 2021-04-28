using System;
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
/**
 * This class represents (the data for) a Room, at this moment only a rectangle in the dungeon.
 */
class Room : GameObject
{
    public Rectangle originalSize;

    public RoomArea roomArea;

    public int ID;
    public float randomSplitValue;

    //"Worldspace" coordinates
    private Vec2 screenPosition;
    private EasyDraw idText;

    public Room(Rectangle pOriginalSize)
    {
        AlgorithmsAssignment.OnGenerateDestroyPrevious += handleDestroy;
        originalSize = pOriginalSize;

        roomArea.leftSide = originalSize.X;
        roomArea.rightSide = originalSize.X + originalSize.Width;
        roomArea.topSide = originalSize.Y;
        roomArea.bottomSide = originalSize.Y + originalSize.Height;

        screenPosition.x = (roomArea.leftSide + 1) * AlgorithmsAssignment.SCALE;
        screenPosition.y = (roomArea.topSide + 4) * (AlgorithmsAssignment.SCALE);

        idText = new EasyDraw(game.width, game.height);
        AddChild(idText);
        idText.SetColor(0, 255, 0);
        idText.SetScaleXY(0.1f, 0.1f);
    }

    //TODO: Implement a toString method for debugging?
    //Return information about the type of object and it's data
    //eg Room: (x, y, width, height)

    public override string ToString()
    {
        return String.Format("Room ID: {0}\nLeft side:{1}, right side:{2}, \ntop side:{3}, bottom side:{4}", ID, roomArea.leftSide, roomArea.rightSide, roomArea.topSide, roomArea.bottomSide);
    }

    private void handleDestroy()
    {
        AlgorithmsAssignment.OnGenerateDestroyPrevious -= handleDestroy;
        Destroy();
    }

    public Room[] Split(float pRandomMultiplication)
    {
        randomSplitValue = pRandomMultiplication;
        AXIS splitAxis = checkLargerAxis();
        Room[] newRooms = defineRooms(splitAxis);

        return newRooms;
    }

    public void UpdateRoomID(int pID)
    {
        ID = pID;
        idText.Text($"ID: {pID}", screenPosition.x, screenPosition.y);
    }

    private Room[] defineRooms(AXIS pSplitAxis)
    {
        Room[] newRooms = new Room[2];
        Rectangle[] roomSizes = defineSizes();

        switch (pSplitAxis)
        {
            case AXIS.HORIZONTAL:
                roomSizes[0].Width = (int)(originalSize.Width * randomSplitValue);

                roomSizes[1].Width = originalSize.Width - roomSizes[0].Width + 1;
                roomSizes[1].X = roomArea.leftSide + roomSizes[0].Width - 1;
                break;
            case AXIS.VERTICAL:
                roomSizes[0].Height = (int)(originalSize.Height * randomSplitValue);

                roomSizes[1].Height = originalSize.Height - roomSizes[0].Height + 1;
                roomSizes[1].Y = originalSize.Y + roomSizes[0].Height - 1;
                break;
        }

        newRooms[0] = new Room(roomSizes[0]);
        newRooms[1] = new Room(roomSizes[1]);

        newRooms[0].x = this.x * randomSplitValue;
        newRooms[0].y = this.y * randomSplitValue;

        newRooms[1].x = this.x * randomSplitValue;
        newRooms[1].y = this.y * randomSplitValue;

        return newRooms;
    }

    private Rectangle[] defineSizes()
    {
        Rectangle[] roomSizes = new Rectangle[2];
        roomSizes[0] = new Rectangle(roomArea.leftSide, roomArea.topSide, originalSize.Width, originalSize.Height);
        roomSizes[1] = new Rectangle(roomArea.leftSide, roomArea.topSide, originalSize.Width, originalSize.Height);

        return roomSizes;
    }

    public bool ShouldSplit()
    {
        int minSize = AlgorithmsAssignment.MIN_ROOM_SIZE;
        if (originalSize.Width > minSize && originalSize.Height > minSize)
            return true;

        return false;
    }

    private AXIS checkLargerAxis()
    {
        AXIS axis = AXIS.VERTICAL;

        if (originalSize.Width > originalSize.Height)
            axis = AXIS.HORIZONTAL;

        return axis;
    }

    public void PlaceDoors(List<Room> pFinishedRooms)
    {
        List<Room> neighbourRooms = findNeighbourRooms(pFinishedRooms);

        foreach (Room room in neighbourRooms)
        {
            Console.WriteLine($"\n\nNeighbour room ID: {room.ID} \nThis ID: {ID}.\nFound neighbours: {neighbourRooms.Count}");
        }
    }

    private List<Room> findNeighbourRooms(List<Room> pFinishedRooms)
    {
        List<Room> neighbourRooms = new List<Room>();

        //Create for/foreach loop, iterate over all rooms to check if they have an xPos or yPos that's next to this room. If it is, communicate back to that room that this room is going to place a door to prevent double doors?

        foreach (Room otherRoom in pFinishedRooms)
        {
            bool horizontallyAligned = false;
            bool verticallyAligned = false;

            RoomArea neighbourArea = otherRoom.roomArea;

            //Horizontal alignment
            {
                //Left side room
                if (neighbourArea.leftSide >= this.roomArea.leftSide && neighbourArea.leftSide <= this.roomArea.rightSide)
                    horizontallyAligned = true;

                //Right side room
                if (neighbourArea.rightSide <= this.roomArea.rightSide && neighbourArea.rightSide >= this.roomArea.leftSide)
                    horizontallyAligned = true;
            }

            //Vertical alignment
            {
                //Top side
                if (neighbourArea.topSide >= this.roomArea.topSide && neighbourArea.topSide <= this.roomArea.bottomSide)
                    verticallyAligned = true;

                if (neighbourArea.bottomSide <= this.roomArea.bottomSide && neighbourArea.bottomSide >= this.roomArea.topSide)
                    verticallyAligned = true;
            }

            if (horizontallyAligned && verticallyAligned)
                if (otherRoom != this)
                    if (!neighbourRooms.Contains(otherRoom))
                        neighbourRooms.Add(otherRoom);
        }

        return neighbourRooms;
    }
}