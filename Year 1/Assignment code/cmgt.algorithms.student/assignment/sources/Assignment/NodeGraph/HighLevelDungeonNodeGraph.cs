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
        Console.WriteLine("\n\n\n\n\n\n");
        createNodes();

        //Seperating the creation and connecting process, because then I can be sure I'm not trying to connect to nodes that don't exist yet.
        connectNodes();

        foreach(KeyValuePair<DungeonComponent, Node> nodeValue in nodes)
        {
            Console.WriteLine($"Node {nodeValue.Value.id} has connections:");
            foreach (Node node in nodeValue.Value.connections)
            {
                Console.WriteLine(node.id);
            }
        }
    }

    private void createNodes()
    {
        foreach (RoomContainer room in _dungeon.finishedRooms)
        {
            Node roomCenterNode = new Node(getRoomCenter(room));
            Console.WriteLine($"Added ROOM node at position {roomCenterNode.location}");

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
                    Console.WriteLine($"Added DOOR node at position {doorNode.location}");
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