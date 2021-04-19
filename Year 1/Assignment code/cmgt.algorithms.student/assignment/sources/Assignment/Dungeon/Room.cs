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

    public Room[] Split(float pRandomDivision, AXIS pSplitAxis)
    {
        Room[] newRooms = new Room[2];
        Rectangle newRoomSize = new Rectangle(area.X, area.Y, area.Width, area.Height);
        Rectangle newRoom2Size = new Rectangle(area.X, area.Y, area.Width, area.Height);
        newRooms[0] = new Room(newRoomSize);
        newRooms[1] = new Room(newRoom2Size);

        if (pSplitAxis == AXIS.VERTICAL)
        {
            newRooms[1].area.X = newRooms[0].area.Width;
            newRooms[0].area.Width = (int)(newRooms[0].area.Width * pRandomDivision);
            newRooms[1].area.Width = area.Width - newRooms[0].area.Width;
        }

        if (pSplitAxis == AXIS.HORIZONTAL)
        {
            newRooms[1].area.Y = newRooms[0].area.Height;
            newRooms[0].area.Height = (int)(newRooms[0].area.Height * pRandomDivision);
            newRooms[1].area.Height = area.Height - newRooms[0].area.Height;
        }

        return newRooms;
    }

    public bool ShouldSplit(Rectangle pRect, int pMinSize)
    {
        if (pRect.Width > pMinSize && pRect.Height > pMinSize)
        {
            return true;
        }

        return false;
    }

    private AXIS checkLargerAxis(Rectangle pArea)
    {
        AXIS axis = AXIS.VERTICAL;
        if (pArea.Width > pArea.Height)
            //Vertical, because the width is larger than the height, meaning it has to be split vertically in order to decrease the width.
            axis = AXIS.VERTICAL;
        if (pArea.Height > pArea.Width)
            //Vertical, because the width is larger than the height, meaning it has to be split vertically in order to decrease the width.
            axis = AXIS.HORIZONTAL;

        return axis;
    }
}

