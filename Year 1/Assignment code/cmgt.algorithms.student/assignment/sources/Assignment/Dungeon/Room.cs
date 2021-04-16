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

        RNG gen = new RNG();
        int axis = gen.RandomInt(0, 2);

        Rectangle newRoomSize = new Rectangle(area.X, area.Y, area.Width, area.Height);
        newRooms[0] = new Room(newRoomSize);

        if (axis == 0)
        {
            //A
            newRooms[0].area.X = area.X;
            newRooms[0].area.Y = area.Y;
            newRooms[0].area.Width /= 2;
            newRooms[0].area.Height = area.Height;
        }

        if (axis == 1)
        {
            newRooms[0].area.X = area.X;
            newRooms[0].area.Y = area.Y;
            newRooms[0].area.Width = area.Width;
            newRooms[0].area.Height /= 2;
        }

        Console.WriteLine($"Room 1 area: {newRooms[0].area}");

        Rectangle newRoom2Size = new Rectangle(0, 0, 0, 0);
        newRooms[1] = new Room(newRoom2Size);
        if (axis == 0)
        {
            //B
            newRooms[1].area.X = newRooms[0].area.Width;
            newRooms[1].area.Y = area.Y;
            newRooms[1].area.Width = area.Width - newRooms[0].area.Width;
            newRooms[1].area.Height = area.Height;
        }

        if (axis == 1)
        {
            newRooms[1].area.X = area.X;
            newRooms[1].area.Y = newRooms[0].area.Height;
            newRooms[1].area.Width = area.Width;
            newRooms[1].area.Height = area.Height - newRooms[0].area.Height;
        }

        Console.WriteLine($"Room 2 area: {newRooms[1].area}");

        return newRooms;
    }
}

