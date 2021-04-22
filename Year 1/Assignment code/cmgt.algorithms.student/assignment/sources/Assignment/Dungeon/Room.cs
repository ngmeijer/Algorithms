using System;
using System.Collections.Generic;
using System.Drawing;
using GXPEngine;

/**
 * This class represents (the data for) a Room, at this moment only a rectangle in the dungeon.
 */
class Room
{
    public Rectangle originalSize;
    public int minX;
    public int maxX;
    public int minY;
    public int maxY;

    public float randomSplitValue;

    public Room(Rectangle pOriginalSize)
    {
        originalSize = pOriginalSize;
        minX = originalSize.X;
        maxX = originalSize.X + originalSize.Width;

        minY = originalSize.Y;
        maxY = originalSize.Y + originalSize.Height;
    }

    //TODO: Implement a toString method for debugging?
    //Return information about the type of object and it's data
    //eg Room: (x, y, width, height)

    public override string ToString()
    {
        return String.Format("X-position:{0}, Y-position:{1}, width:{2}, height:{3}", originalSize.X, originalSize.Y, originalSize.Width, originalSize.Height);
    }

    public Room[] Split(float pRandomMultiplication)
    {
        randomSplitValue = pRandomMultiplication;
        AXIS splitAxis = checkLargerAxis();
        Room[] newRooms = defineRooms(splitAxis);

        return newRooms;
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
                roomSizes[1].X = minX + roomSizes[0].Width - 1;
                break;
            case AXIS.VERTICAL:
                roomSizes[0].Height = (int)(originalSize.Height * randomSplitValue);

                roomSizes[1].Height = originalSize.Height - roomSizes[0].Height + 1;
                roomSizes[1].Y = originalSize.Y + roomSizes[0].Height - 1;
                break;
        }

        newRooms[0] = new Room(roomSizes[0]);
        newRooms[1] = new Room(roomSizes[1]);

        return newRooms;
    }

    private Rectangle[] defineSizes()
    {
        Rectangle[] roomSizes = new Rectangle[2];
        roomSizes[0] = new Rectangle(minX, minY, originalSize.Width, originalSize.Height);
        roomSizes[1] = new Rectangle(minX, minY, originalSize.Width, originalSize.Height);

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

    private List<Room> findNeighbourRooms()
    {
        List<Room> neighbourRooms = new List<Room>();

        return neighbourRooms;
    }
}