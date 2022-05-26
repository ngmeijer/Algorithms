using System.Collections.Generic;
using DoorCreation;
using Dungeon;
using RoomCreation;

class HighLevelDungeonNodeGraph : SampleDungeonNodeGraph
{
    public HighLevelDungeonNodeGraph(BaseDungeon pDungeon) : base(pDungeon)
    {
    }

    protected override void generate()
    {
        foreach (RoomContainer room in _dungeon.finishedRooms)
        {
            nodes.Add(new Node(getRoomCenter(room)));
            foreach (KeyValuePair<RoomContainer, Door> door in room.CreatedDoors)
            {
                nodes.Add(new Node(getDoorCenter(door.Value)));
            }
        }
    }
}