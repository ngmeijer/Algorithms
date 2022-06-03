using System;
using System.Collections.Generic;
using System.Linq;
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

        //Seperating the creation and connecting process, because then I can be sure I'm not trying to connect to nodes that don't exist yet.
        connectNodes();
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

                    RoomContainer neighbourRoom = door.Value.RoomContainerB;
                    if (!neighbourRoom.CreatedNodes.ContainsKey(door.Value))
                        neighbourRoom.CreatedNodes.Add(door.Value, doorNode);
                }
            }
        }
    }

    private void connectNodes()
    {
        foreach (RoomContainer room in _dungeon.finishedRooms)
        {
            room.CreatedNodes.TryGetValue(room, out Node roomCenterNode);
            foreach (KeyValuePair<RoomContainer, Door> door in room.CreatedDoors)
            {
                room.CreatedNodes.TryGetValue(door.Value, out Node doorNode);
                AddConnection(doorNode, roomCenterNode);
            }
        }
    }
}