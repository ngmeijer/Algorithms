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
        Room[] newRooms = new Room[2];

        Rectangle room0NewSize = new Rectangle(0, 0, (int)(area.Width / 2.5f), area.Height);

        newRooms[0] = new Room(room0NewSize);

        Rectangle room1NewSize = new Rectangle(newRooms[0].area.Width, 0, (area.Width - newRooms[0].area.Width), area.Height);
        newRooms[1] = new Room(room1NewSize);

        Console.WriteLine(newRooms[0].area.ToString());
        Console.WriteLine(newRooms[1].area.ToString());
        return newRooms;
    }
}
