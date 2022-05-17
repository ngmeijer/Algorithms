using GXPEngine;
using System.Collections.Generic;
using System.Drawing;
using DoorCreation;

namespace RoomCreation
{
    namespace DoorCreation
    {
        public class DoorCreationHandler
        {
            public enum DoorMaster
            {
                THIS_ROOM = 0,
                NEIGHBOUR_ROOM = 1,
                UNDEFINED = 2,
            };

            private const int MAX_DOOR_COUNT = 2;
            public const int OFFSET = 3;

            private int doorCount;
            private RoomArea roomArea;
            private RoomContainer parentRoom;
            private NeighbourRoomFinder roomFinder;

            public DoorCreationHandler(RoomContainer pParentRoom, RoomArea pRoomArea)
            {
                parentRoom = pParentRoom;
                roomArea = pRoomArea;

                roomFinder = new NeighbourRoomFinder(roomArea);
            }

            //------------------------------------------------------------------------------------------------------------------------
            //			                                   void InitiateDoorHandling()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// 
            /// </summary>
            public Door[] InitiateDoorHandling(List<RoomContainer> pFinishedRooms)
            {
                List<Door> newDoors = new List<Door>();
                List<RoomContainer> neighbourRooms = roomFinder.findNeighbourRooms(parentRoom, pFinishedRooms);

                for (int i = 0; i < neighbourRooms.Count; i++)
                {
                    
                }

                //To array, because in advance I do not know how many doors there will be (and if there would be a limit, if the limit would be reached.
                //So for that reason, create a temp List, once all doors are added, convert to array
                return newDoors.ToArray();
            }

            //------------------------------------------------------------------------------------------------------------------------
            //			                                     void incrementDoorCount()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// 
            /// </summary>
            private void incrementDoorCount() => doorCount++;

            //------------------------------------------------------------------------------------------------------------------------
            //			                             DoorMaster defineDoorResponsibility()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// 
            /// </summary>
            /// <returns>DoorMaster</returns>
            private DoorMaster defineDoorResponsibility(RoomContainer pOtherRoom)
            {
                int otherRoomDoorCount = pOtherRoom.DoorCreator.doorCount;

                //Both rooms have max amount of doors
                if (doorCount >= MAX_DOOR_COUNT && otherRoomDoorCount >= MAX_DOOR_COUNT)
                    return DoorMaster.UNDEFINED;

                //This room has not reached the max yet, the other one has.
                if (doorCount < MAX_DOOR_COUNT && otherRoomDoorCount >= MAX_DOOR_COUNT)
                    return DoorMaster.THIS_ROOM;

                //This room has reached the max, the other one has not.
                if (doorCount >= MAX_DOOR_COUNT && otherRoomDoorCount < MAX_DOOR_COUNT)
                    return DoorMaster.NEIGHBOUR_ROOM;

                if (doorCount == otherRoomDoorCount)
                    return (DoorMaster)Utils.Random(0, 2);

                if (doorCount < otherRoomDoorCount)
                    return DoorMaster.THIS_ROOM;

                if (doorCount > otherRoomDoorCount)
                    return DoorMaster.NEIGHBOUR_ROOM;

                return DoorMaster.UNDEFINED;
            }

            private void findDoorPosition(RoomContainer pOtherRoom)
            {
                
            }
        }
    }
}