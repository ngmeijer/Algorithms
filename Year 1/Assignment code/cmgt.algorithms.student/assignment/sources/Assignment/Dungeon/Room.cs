using System;
using System.Drawing;
using GXPEngine;

/**
 * This class represents (the data for) a Room, at this moment only a rectangle in the dungeon.
 */
class Room
{
    public Rectangle area;

    public Room(Rectangle pArea)
    {
        area = pArea;

    }

    //TODO: Implement a toString method for debugging?
    //Return information about the type of object and it's data
    //eg Room: (x, y, width, height)

    public override string ToString()
    {
        return String.Format("X-position:{0}, Y-position:{1}, width:{2}, height:{3}", area.X, area.Y, area.Width, area.Height);
    }

    public Room[] Split(float pRandomDivision)
    {
        Room[] newRooms = new Room[2];
        Rectangle newRoomSize = new Rectangle(area.X, area.Y, area.Width, area.Height);
        Rectangle newRoom2Size = newRoomSize;

        AXIS splitAxis = checkLargerAxis();

        switch (splitAxis)
        {
            case AXIS.HORIZONTAL:
                newRoomSize.Height = (int)(newRoomSize.Height * pRandomDivision);
                newRoom2Size.Y = newRoomSize.Height;
                newRoom2Size.Height = area.Height - newRoomSize.Height;
                break;
            case AXIS.VERTICAL:
                newRoomSize.Width = (int)(newRoomSize.Width * pRandomDivision);
                newRoom2Size.X = newRoomSize.Width;
                newRoom2Size.Width = area.Width - newRoomSize.Width;
                break;
        }


        newRooms[0] = new Room(newRoomSize);
        newRooms[1] = new Room(newRoom2Size);

        return newRooms;
    }

    public bool ShouldSplit(Rectangle pRect)
    {
        int minSize = AlgorithmsAssignment.MIN_ROOM_SIZE;
        if (pRect.Width > minSize && pRect.Height > minSize)
            return true;

        return false;
    }

    private AXIS checkLargerAxis()
    {
        AXIS axis = AXIS.VERTICAL;
        if (area.Width > area.Height)
            //Vertical, because the width is larger than the height, meaning it has to be split vertically in order to decrease the width.
            axis = AXIS.VERTICAL;
        if (area.Height > area.Width)
            //Vertical, because the width is larger than the height, meaning it has to be split vertically in order to decrease the width.
            axis = AXIS.HORIZONTAL;

        return axis;
    }
}

