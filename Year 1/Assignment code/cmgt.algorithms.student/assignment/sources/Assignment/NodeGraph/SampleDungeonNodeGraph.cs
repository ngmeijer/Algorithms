using System.Diagnostics;
using System.Drawing;
using DoorCreation;
using Dungeon;
using RoomCreation;

/**
 * An example of a dungeon nodegraph implementation.
 * 
 * This implementation places only three nodes and only works with the SampleDungeon.
 * Your implementation has to do better :).
 * 
 * It is recommended to subclass this class instead of NodeGraph so that you already 
 * have access to the helper methods such as getRoomCenter etc.
 * 
 * TODO:
 * - Create a subclass of this class, and override the generate method, see the generate method below for an example.
 */
class SampleDungeonNodeGraph : NodeGraph
{
    protected BaseDungeon _dungeon;

    public SampleDungeonNodeGraph(BaseDungeon pDungeon) : base((int)(pDungeon.size.Width * pDungeon.scale), (int)(pDungeon.size.Height * pDungeon.scale), (int)pDungeon.scale / 3)
    {
        Debug.Assert(pDungeon != null, "Please pass in a dungeon.");

        _dungeon = pDungeon;
    }

    protected override void generate()
    {
        //Generate nodes, in this sample node graph we just add to nodes manually
        //of course in a REAL nodegraph (read:yours), node placement should 
        //be based on the rooms in the dungeon

        //We assume (bad programming practice 1-o-1) there are two rooms in the given dungeon.
        //The getRoomCenter is a convenience method to calculate the screen space center of a room
        // nodes.Add(new Node(getRoomCenter(_dungeon.finishedRooms[0])));
        // nodes.Add(new Node(getRoomCenter(_dungeon.finishedRooms[1])));
        // //The getDoorCenter is a convenience method to calculate the screen space center of a door
        // nodes.Add(new Node(getDoorCenter(_dungeon.doors[0])));
        //
        // //create a connection between the two rooms and the door...
        // AddConnection(nodes[0], nodes[2]);
        // AddConnection(nodes[1], nodes[2]);
    }

    /**
	 * A helper method for your convenience so you don't have to meddle with coordinate transformations.
	 * @return the location of the center of the given room you can use for your nodes in this class
	 */
    protected Point getRoomCenter(RoomContainer pRoomContainer)
    {
        float centerX = ((pRoomContainer.OriginalSize.Left + pRoomContainer.OriginalSize.Right) / 2.0f) * _dungeon.scale;
        float centerY = ((pRoomContainer.OriginalSize.Top + pRoomContainer.OriginalSize.Bottom) / 2.0f) * _dungeon.scale;
        return new Point((int)centerX, (int)centerY);
    }

    /**
	 * A helper method for your convenience so you don't have to meddle with coordinate transformations.
	 * @return the location of the center of the given door you can use for your nodes in this class
	 */
    protected Point getDoorCenter(Door pDoor)
    {
        return getPointCenter(pDoor.location);
    }

    /**
	 * A helper method for your convenience so you don't have to meddle with coordinate transformations.
	 * @return the location of the center of the given point you can use for your nodes in this class
	 */
    protected Point getPointCenter(Point pLocation)
    {
        float centerX = (pLocation.X + 0.5f) * _dungeon.scale;
        float centerY = (pLocation.Y + 0.5f) * _dungeon.scale;
        return new Point((int)centerX, (int)centerY);
    }

}
