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

    public Room[] Split()
    {
        //Create returned array of (soon to be) split rooms.
        Room[] newRooms = new Room[2];

        //Define area for the first "new room". Set Position X equal to the previous's area X so it remains contained in the last room's boundaries.

        Rectangle newRoomSize = new Rectangle(area.X, area.Y, area.Width, area.Height);
        newRooms[0] = new Room(newRoomSize);
        Rectangle newRoom2Size = new Rectangle(0, 0, 0, 0);
        newRooms[1] = new Room(newRoom2Size);

        float randomDivision = Utils.Random(0.3f, 0.8f);

        newRooms[0].area = area;
        newRooms[1].area = area;
        AXIS splitAxis = checkLargerAxis(area);

        if (splitAxis == AXIS.VERTICAL)
        {
            newRooms[0].area.Width = (int) (newRooms[0].area.Width * randomDivision);
            newRooms[1].area.Width = area.Width - newRooms[0].area.Width;
            newRooms[1].area.X = newRooms[0].area.Width;
        }

        if (splitAxis == AXIS.HORIZONTAL)
        {
            newRooms[0].area.Height = (int) (newRooms[0].area.Height * randomDivision);
            newRooms[1].area.Height = area.Height - newRooms[0].area.Height;
            newRooms[1].area.Y = newRooms[0].area.Height;
        }

        Console.WriteLine($"Room 1 area: {newRooms[0].area}");
        Console.WriteLine($"Room 2 area: {newRooms[1].area}");

        return newRooms;
    }

    public bool ShouldSplit(Rectangle pRect, int pMinSize)
    {
        if (pRect.Width > pMinSize || pRect.Height > pMinSize)
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

