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

    public Room(Rectangle pOriginalSize)
    {
        originalSize = pOriginalSize;
        Console.WriteLine(originalSize);
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
        Room[] newRooms = new Room[2];
        Rectangle room1Size = new Rectangle(0, 0, 0, 0);
        Rectangle room2Size = new Rectangle(0, 0, 0, 0);

        AXIS splitAxis = checkLargerAxis();

        switch (splitAxis)
        {
            case AXIS.HORIZONTAL:
                room1Size.Width = (int)(originalSize.Width * pRandomMultiplication);
                room1Size.Height = originalSize.Height;
                room1Size.X = originalSize.X;
                room1Size.Y = originalSize.Y;

                room2Size.Width = originalSize.Width - room1Size.Width;
                room2Size.Height = originalSize.Height;
                room2Size.X = originalSize.X + room1Size.Width;
                room2Size.Y = originalSize.Y;
                break;
            case AXIS.VERTICAL:
                room1Size.Width = originalSize.Width;
                room1Size.Height = (int)(originalSize.Height * pRandomMultiplication);
                room1Size.X = originalSize.X;
                room1Size.Y = originalSize.Y;

                room2Size.Width = originalSize.Width;
                room2Size.Height = originalSize.Height - room1Size.Height;
                room2Size.X = originalSize.X;
                room2Size.Y = originalSize.Y + room1Size.Height;
                break;
        }

        newRooms[0] = new Room(room1Size);
        newRooms[1] = new Room(room2Size);

        return newRooms;
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

        if (originalSize.Height > originalSize.Width)
            axis = AXIS.VERTICAL;

        return axis;
    }

    private void setRoomSize(Rectangle pArea)
    {

    }
}

