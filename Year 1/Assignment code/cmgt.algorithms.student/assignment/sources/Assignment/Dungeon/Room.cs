using System;
using System.Collections.Generic;
using System.Drawing;
using GXPEngine;

/**
 * This class represents (the data for) a Room, at this moment only a rectangle in the dungeon.
 */
class Room : GameObject
{
    private Vec2 originOfRoom;
    public Rectangle originalSize;
    public int leftSide;
    public int rightSide;
    public int topSide;
    public int bottomSide;

    public int ID;
    public float randomSplitValue;

    //"Worldspace" coordinates
    private Vec2 screenPosition;

    public Room(Rectangle pOriginalSize, int pID)
    {
        AlgorithmsAssignment.OnGenerateDestroyPrevious += handleDestroy;
        originalSize = pOriginalSize;
        originOfRoom = new Vec2(originalSize.Width / 2, originalSize.Height / 2);

        leftSide = originalSize.X;
        rightSide = originalSize.X + originalSize.Width;
        topSide = originalSize.Y;
        bottomSide = originalSize.Y + originalSize.Height;

        screenPosition.x = leftSide * AlgorithmsAssignment.SCALE;
        screenPosition.y = topSide * (AlgorithmsAssignment.SCALE + 1);

        ID = pID;

        Console.WriteLine($"Screen pos: {screenPosition} for room {ID}");

        EasyDraw text = new EasyDraw(game.width, game.height);
        AddChild(text);

        text.Text($"ID: {ID}", screenPosition.x, screenPosition.y);
        text.SetColor(0, 255, 0);
        text.SetScaleXY(0.1f, 0.1f);
    }

    //TODO: Implement a toString method for debugging?
    //Return information about the type of object and it's data
    //eg Room: (x, y, width, height)

    public override string ToString()
    {
        return String.Format("X-position:{0}, Y-position:{1}, width:{2}, height:{3}", originalSize.X, originalSize.Y, originalSize.Width, originalSize.Height);
    }

    private void handleDestroy()
    {
        AlgorithmsAssignment.OnGenerateDestroyPrevious -= handleDestroy;
        Destroy();
    }

    public Room[] Split(float pRandomMultiplication, int pID)
    {
        randomSplitValue = pRandomMultiplication;
        AXIS splitAxis = checkLargerAxis();
        Room[] newRooms = defineRooms(splitAxis, pID);

        return newRooms;
    }

    private Room[] defineRooms(AXIS pSplitAxis, int pID)
    {
        Room[] newRooms = new Room[2];
        Rectangle[] roomSizes = defineSizes();

        switch (pSplitAxis)
        {
            case AXIS.HORIZONTAL:
                roomSizes[0].Width = (int)(originalSize.Width * randomSplitValue);

                roomSizes[1].Width = originalSize.Width - roomSizes[0].Width + 1;
                roomSizes[1].X = leftSide + roomSizes[0].Width - 1;
                break;
            case AXIS.VERTICAL:
                roomSizes[0].Height = (int)(originalSize.Height * randomSplitValue);

                roomSizes[1].Height = originalSize.Height - roomSizes[0].Height + 1;
                roomSizes[1].Y = originalSize.Y + roomSizes[0].Height - 1;
                break;
        }

        newRooms[0] = new Room(roomSizes[0], pID);
        newRooms[1] = new Room(roomSizes[1], pID);

        newRooms[0].x = this.x * randomSplitValue;
        newRooms[0].y = this.y * randomSplitValue;

        newRooms[1].x = this.x * randomSplitValue;
        newRooms[1].y = this.y * randomSplitValue;

        return newRooms;
    }

    private Rectangle[] defineSizes()
    {
        Rectangle[] roomSizes = new Rectangle[2];
        roomSizes[0] = new Rectangle(leftSide, topSide, originalSize.Width, originalSize.Height);
        roomSizes[1] = new Rectangle(leftSide, topSide, originalSize.Width, originalSize.Height);

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
            Console.WriteLine($"Neighbour room ID: {room.ID}");
        }
    }

    private List<Room> findNeighbourRooms(List<Room> pFinishedRooms)
    {
        List<Room> neighbourRooms = new List<Room>();

        //Create for/foreach loop, iterate over all rooms to check if they have an xPos or yPos that's next to this room. If it is, communicate back to that room that this room is going to place a door to prevent double doors?

        foreach (Room room in pFinishedRooms)
        {
            bool horizontallyAligned = false;
            bool verticallyAligned = false;

            //Right side room
            if (room.leftSide == this.rightSide || room.rightSide == this.leftSide)
            {
                horizontallyAligned = true;
            }

            if (room.topSide == this.bottomSide || room.bottomSide == this.topSide)
            {
                verticallyAligned = true;
            }

            if (horizontallyAligned && verticallyAligned)
            {
                neighbourRooms.Add(room);
                Console.WriteLine($"Neighbour rooms found: {neighbourRooms.Count}");
            }
        }
        return neighbourRooms;
    }
}