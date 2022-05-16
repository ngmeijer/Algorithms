using GXPEngine;
using System.Collections.Generic;
using System.Drawing;
using DoorCreation;
using System;

namespace RoomCreation
{
    namespace DoorCreation
    {
        public class DoorCreationHandler
        {
            private const int MAX_DOOR_COUNT = 1;
            public const int OFFSET = 1;
            private RoomContainer parentRoom;

            public DoorCreationHandler(RoomContainer pParent)
            {
                parentRoom = pParent;
            }

            public Door[] InitiateDoorHandling(List<RoomContainer> finishedRooms)
            {
                Door[] newDoors = new Door[MAX_DOOR_COUNT];
                Point doorPosition = new Point();

                foreach (RoomContainer room in finishedRooms)
                {
                    Console.WriteLine($"Parent room ID: {parentRoom.ID}. Neighbour room ID: {room.ID}");
                }

                return newDoors;
            }
        }
    }
}