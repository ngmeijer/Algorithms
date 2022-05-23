using System;
using GXPEngine;
using System.Collections.Generic;
using System.Drawing;
using DoorCreation;
using GXPEngine.Core;

namespace RoomCreation
{
    namespace DoorCreation
    {
        public class DoorCreationHandler
        {
            public const int MINIMUM_DOOR_SPACE = 10;
            public const int DOOR_OFFSET = 1;

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
                Dictionary<RoomContainer, DoorArea> newDoorPositions = new Dictionary<RoomContainer, DoorArea>();
                Dictionary<RoomContainer, NeighbourRoomDirection> neighbourRooms =
                    roomFinder.findNeighbourRooms(parentRoom, pFinishedRooms);

                foreach (var neighbourRoom in neighbourRooms)
                {
                    if (neighbourRoom.Key.CreatedDoors.ContainsKey(this.parentRoom)) continue;
                    DoorArea newArea = findDoorPosition(neighbourRoom.Key);
                    if (newArea == null) continue;
                    newDoorPositions.Add(neighbourRoom.Key, newArea);
                }

                foreach (var overlap in newDoorPositions)
                {
                    Console.WriteLine(overlap);
                    NeighbourRoomDirection direction = overlap.Value.direction;
                    int randomX = overlap.Value.commonBorder;
                    int randomY = overlap.Value.commonBorder;
                    if (direction == NeighbourRoomDirection.Left || direction == NeighbourRoomDirection.Right)
                    {
                        randomY = Utils.Random(overlap.Value.point1.Y + DOOR_OFFSET,
                            overlap.Value.point2.Y - DOOR_OFFSET);
                    }

                    if (direction == NeighbourRoomDirection.Top || direction == NeighbourRoomDirection.Bottom)
                    {
                        randomX = Utils.Random(overlap.Value.point1.X + DOOR_OFFSET,
                            overlap.Value.point2.X - DOOR_OFFSET);
                    }

                    Door newDoor = new Door(new Point(randomX, randomY), overlap.Value.roomA, overlap.Value.roomB);

                    overlap.Value.roomA.CreatedDoors.Add(overlap.Value.roomB, newDoor);
                    overlap.Value.roomB.CreatedDoors.Add(overlap.Value.roomA, newDoor);

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

                //This is when we start checking WHERE doors can be placed. Refactor to DoorCreationHandler.
                if (neighbourDirection == NeighbourRoomDirection.Left ||
                    neighbourDirection == NeighbourRoomDirection.Right)
                {
                    if (parentRoom.RoomArea.topSide >= pOtherRoom.RoomArea.topSide)
                        newArea.point1.Y = parentRoom.RoomArea.topSide;
                    else newArea.point1.Y = pOtherRoom.RoomArea.topSide;

                    if (parentRoom.RoomArea.bottomSide <= pOtherRoom.RoomArea.bottomSide)
                        newArea.point2.Y = parentRoom.RoomArea.bottomSide;
                    else newArea.point2.Y = pOtherRoom.RoomArea.bottomSide;

                    if (neighbourDirection == NeighbourRoomDirection.Left)
                    {
                        newArea.direction = NeighbourRoomDirection.Left;
                        newArea.point1.X = parentRoom.RoomArea.leftSide;
                        newArea.point2.X = parentRoom.RoomArea.leftSide;
                        newArea.commonBorder = parentRoom.RoomArea.leftSide;
                    }

                    if (neighbourDirection == NeighbourRoomDirection.Right)
                    {
                        newArea.direction = NeighbourRoomDirection.Right;
                        newArea.point1.X = parentRoom.RoomArea.rightSide;
                        newArea.point2.X = parentRoom.RoomArea.rightSide;
                        newArea.commonBorder = parentRoom.RoomArea.rightSide;
                    }
                }

                if (neighbourDirection == NeighbourRoomDirection.Top ||
                    neighbourDirection == NeighbourRoomDirection.Bottom)
                {
                    if (parentRoom.RoomArea.leftSide >= pOtherRoom.RoomArea.leftSide)
                        newArea.point1.X = parentRoom.RoomArea.leftSide;
                    else newArea.point1.X = pOtherRoom.RoomArea.leftSide;

                    if (parentRoom.RoomArea.rightSide >= pOtherRoom.RoomArea.rightSide)
                        newArea.point2.X = pOtherRoom.RoomArea.rightSide;
                    else newArea.point2.X = parentRoom.RoomArea.rightSide;

                    //Y is the same for both points.
                    if (neighbourDirection == NeighbourRoomDirection.Top)
                    {
                        newArea.direction = NeighbourRoomDirection.Top;
                        newArea.point1.Y = parentRoom.RoomArea.topSide;
                        newArea.point2.Y = parentRoom.RoomArea.topSide;
                        newArea.commonBorder = parentRoom.RoomArea.topSide;
                    }

                    if (neighbourDirection == NeighbourRoomDirection.Bottom)
                    {
                        newArea.direction = NeighbourRoomDirection.Bottom;
                        newArea.point1.Y = parentRoom.RoomArea.bottomSide;
                        newArea.point2.Y = parentRoom.RoomArea.bottomSide;
                        newArea.commonBorder = parentRoom.RoomArea.bottomSide;
                    }
                }

                Vector2 differenceVector =
                    new Vector2(newArea.point2.X - newArea.point1.X, newArea.point2.Y - newArea.point1.Y);

                if (differenceVector.Length() < (MINIMUM_DOOR_SPACE + DOOR_OFFSET * 2) + 1) newArea = null;

                return newArea;
            }
        }
    }
}