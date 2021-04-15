using System;
using System.Drawing;

/**
 * This class represents (the data for) a Room, at this moment only a rectangle in the dungeon.
 */
class Room
{
    public Rectangle area;

    public Room(Rectangle pArea)
    {
        area = pArea;
        ToString();
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

        Rectangle room0NewSize = new Rectangle(0, 0, area.Width / 2, area.Height);
        Rectangle room1NewSize = new Rectangle(room0NewSize.Width, 0, area.Width / 2, area.Height);

        newRooms[0] = new Room(room0NewSize);
        newRooms[1] = new Room(room1NewSize);

        return newRooms;
    }
}
