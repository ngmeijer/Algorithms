using System;
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
            public const int OFFSET = 0;

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
                List<DoorArea> newDoorPositions = new List<DoorArea>();
                Dictionary<RoomContainer, NeighbourRoomDirection> neighbourRooms =
                    roomFinder.findNeighbourRooms(parentRoom, pFinishedRooms);

                Console.WriteLine(neighbourRooms.Count);

                foreach (var neighbourRoom in neighbourRooms)
                {
                    DoorArea newArea = findDoorPosition(neighbourRoom.Key);
                    newDoorPositions.Add(newArea);
                }

                foreach (var overlap in newDoorPositions)
                {
                    int averageX = (overlap.point1.X + overlap.point2.X) / 2;
                    int averageY = (overlap.point1.Y + overlap.point2.Y) / 2;

                    Door newDoor = new Door(new Point(averageX, averageY), overlap.roomA, overlap.roomB);

                    newDoors.Add(newDoor);
                }

                //To array, because in advance I do not know how many doors there will be (and if there would be a limit, if the limit would be reached.
                //So for that reason, create a temp List, once all doors are added, convert to array
                return newDoors.ToArray();
            }

            private DoorArea findDoorPosition(RoomContainer pOtherRoom)
            {
                parentRoom.ConnectedRooms.TryGetValue(pOtherRoom, out NeighbourRoomDirection neighbourDirection);

                DoorArea newArea = new DoorArea()
                {
                    point1 = new Point(),
                    point2 = new Point(),
                    roomA = parentRoom,
                    roomB = pOtherRoom
                };
                
                Console.WriteLine(neighbourDirection);

                //This is when we start checking WHERE doors can be placed. Refactor to DoorCreationHandler.
                if (neighbourDirection == NeighbourRoomDirection.Left ||
                    neighbourDirection == NeighbourRoomDirection.Right)
                {
                    Console.WriteLine(
                        $"\nMain room: {parentRoom.ID}. Neighbour room: {pOtherRoom.ID}. " +
                        $"\nDirection: {neighbourDirection}");

                    if (parentRoom.RoomArea.topSide >= pOtherRoom.RoomArea.topSide)
                        newArea.point1.Y = parentRoom.RoomArea.topSide;
                    else newArea.point1.Y = pOtherRoom.RoomArea.topSide;

                    if (parentRoom.RoomArea.bottomSide <= pOtherRoom.RoomArea.bottomSide)
                        newArea.point2.Y = parentRoom.RoomArea.bottomSide;
                    else newArea.point2.Y = pOtherRoom.RoomArea.bottomSide;

                    if (neighbourDirection == NeighbourRoomDirection.Left)
                    {
                        newArea.point1.X = parentRoom.RoomArea.leftSide;
                        newArea.point2.X = parentRoom.RoomArea.leftSide;
                    }

                    if (neighbourDirection == NeighbourRoomDirection.Right)
                    {
                        newArea.point1.X = parentRoom.RoomArea.rightSide;
                        newArea.point2.X = parentRoom.RoomArea.rightSide;
                    }
                }

                if (neighbourDirection == NeighbourRoomDirection.Top ||
                    neighbourDirection == NeighbourRoomDirection.Bottom)
                {
                    Console.WriteLine(
                        $"\nMain room: {parentRoom.ID}. Neighbour room: {pOtherRoom.ID}. " +
                        $"\nDirection: {neighbourDirection}");

                    if (parentRoom.RoomArea.leftSide >= pOtherRoom.RoomArea.leftSide)
                        newArea.point1.X = parentRoom.RoomArea.leftSide;
                    else newArea.point1.X = pOtherRoom.RoomArea.leftSide;

                    if (parentRoom.RoomArea.rightSide >= pOtherRoom.RoomArea.rightSide)
                        newArea.point2.X = parentRoom.RoomArea.rightSide;
                    else newArea.point2.X = pOtherRoom.RoomArea.rightSide;

                    //Y is the same for both points.
                    if (neighbourDirection == NeighbourRoomDirection.Top)
                    {
                        newArea.point1.Y = parentRoom.RoomArea.topSide;
                        newArea.point2.Y = parentRoom.RoomArea.topSide;
                    }

                    if (neighbourDirection == NeighbourRoomDirection.Bottom)
                    {
                        newArea.point1.Y = parentRoom.RoomArea.bottomSide;
                        newArea.point2.Y = parentRoom.RoomArea.bottomSide;
                    }
                }

                return newArea;
            }
        }
    }
}