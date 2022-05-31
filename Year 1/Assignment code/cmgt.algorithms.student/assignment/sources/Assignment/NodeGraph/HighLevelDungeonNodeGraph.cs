using System;
using System.Collections.Generic;
using DoorCreation;
using Dungeon;
using RoomCreation;
using RoomCreation.DoorCreation;

class HighLevelDungeonNodeGraph : SampleDungeonNodeGraph
{
    public HighLevelDungeonNodeGraph(BaseDungeon pDungeon) : base(pDungeon)
    {
    }

    protected override void generate()
    {
        createNodes();
        connectNodes();
        
        _dungeon.UpdateDebugInformation();
    }

    private void createNodes()
    {
        foreach (RoomContainer room in _dungeon.finishedRooms)
        {
            Node roomCenterNode = new Node(getRoomCenter(room));

            //First of all, a node for every single room.
            room.CreatedNodes.Add(room, roomCenterNode);
            nodes.Add(room, roomCenterNode);

            //Loop over all doors.
            foreach (KeyValuePair<RoomContainer, Door> door in room.CreatedDoors)
            {
                Node doorNode = null;
                //Check if a node has already been created for this door.
                if (!nodes.ContainsKey(door.Value))
                {
                    doorNode = new Node(getDoorCenter(door.Value));
                    nodes.Add(door.Value, doorNode);
                    
                }

                if (doorNode != null)
                {
                    room.CreatedNodes.Add(door.Value, doorNode);
                    door.Value.RoomContainerB.CreatedNodes.Add(door.Value, doorNode);
                }
            }
        }
    }

    private void connectNodes()
    {
    }
}