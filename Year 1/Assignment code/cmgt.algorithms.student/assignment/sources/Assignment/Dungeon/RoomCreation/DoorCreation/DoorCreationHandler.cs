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
                Dictionary<RoomContainer, NeighbourRoomDirection> neighbourRooms =
                    roomFinder.findNeighbourRooms(parentRoom, pFinishedRooms);
                Dictionary<RoomContainer, DoorArea> newDoorPositions = findDoorSpacesForNeighbours(neighbourRooms);
                Door[] newDoors = createDoorsForAllNeighbours(newDoorPositions);
                return newDoors;
            }

            private DoorArea findSingleDoorPosition(RoomContainer pOtherRoom)
            {
                parentRoom.ConnectedRooms.TryGetValue(pOtherRoom, out NeighbourRoomDirection neighbourDirection);
                int sharedWall = getSharedWall(neighbourDirection);

                DoorArea newArea = new DoorArea()
                {
                    point1 = new Point(),
                    point2 = new Point(),
                    roomA = parentRoom,
                    roomB = pOtherRoom,
                    direction = neighbourDirection,
                    sharedWall = sharedWall
                };

                setDoorSpaceLimits(neighbourDirection, pOtherRoom.RoomArea, newArea);
                if (checkIfDoorHasEnoughSpace(newArea) == false) return null;

                return newArea;
            }

            private bool checkIfDoorHasEnoughSpace(DoorArea pNewArea)
            {
                Vector2 differenceVector =
                    new Vector2(pNewArea.point2.X - pNewArea.point1.X, pNewArea.point2.Y - pNewArea.point1.Y);

                if (differenceVector.Length() <
                    (AlgorithmsAssignment.MIN_DOOR_SPACE + AlgorithmsAssignment.DOOR_OFFSET * 2) + 1) return false;

                return true;
            }

            private Dictionary<RoomContainer, DoorArea> findDoorSpacesForNeighbours(
                Dictionary<RoomContainer, NeighbourRoomDirection> pNeighbourRooms)
            {
                Dictionary<RoomContainer, DoorArea> newDoorPositions = new Dictionary<RoomContainer, DoorArea>();
                foreach (var neighbourRoom in pNeighbourRooms)
                {
                    if (neighbourRoom.Key.CreatedDoors.ContainsKey(this.parentRoom)) continue;
                    DoorArea newArea = findSingleDoorPosition(neighbourRoom.Key);
                    if (newArea == null) continue;
                    newDoorPositions.Add(neighbourRoom.Key, newArea);
                }

                return newDoorPositions;
            }

            private Door[] createDoorsForAllNeighbours(Dictionary<RoomContainer, DoorArea> pNewDoorPositions)
            {
                List<Door> createdDoors = new List<Door>();

                foreach (var overlap in pNewDoorPositions)
                {
                    NeighbourRoomDirection direction = overlap.Value.direction;
                    int randomX = overlap.Value.sharedWall;
                    int randomY = overlap.Value.sharedWall;

                    //Not sure if this switch case looks better than an if ( || )?
                    switch (direction)
                    {
                        case NeighbourRoomDirection.Left:
                        case NeighbourRoomDirection.Right:
                            randomY = Utils.Random(overlap.Value.point1.Y + AlgorithmsAssignment.DOOR_OFFSET,
                                overlap.Value.point2.Y - AlgorithmsAssignment.DOOR_OFFSET);
                            break;
                        case NeighbourRoomDirection.Top:
                        case NeighbourRoomDirection.Bottom:
                            randomX = Utils.Random(overlap.Value.point1.X + AlgorithmsAssignment.DOOR_OFFSET,
                                overlap.Value.point2.X - AlgorithmsAssignment.DOOR_OFFSET);
                            break;
                    }

                    Door newDoor = new Door(new Point(randomX, randomY), overlap.Value.roomA, overlap.Value.roomB);

                    overlap.Value.roomA.CreatedDoors.Add(overlap.Value.roomB, newDoor);
                    overlap.Value.roomB.CreatedDoors.Add(overlap.Value.roomA, newDoor);

                    createdDoors.Add(newDoor);
                }

                return createdDoors.ToArray();
            }

            private void setDoorSpaceLimits(NeighbourRoomDirection pDirection, RoomArea pNeighbourRoomArea,
                DoorArea pDoorArea)
            {
                switch (pDirection)
                {
                    case NeighbourRoomDirection.Top:
                    case NeighbourRoomDirection.Bottom:
                        if (parentRoom.RoomArea.leftSide >= pNeighbourRoomArea.leftSide)
                            pDoorArea.point1.X = parentRoom.RoomArea.leftSide;
                        else pDoorArea.point1.X = pNeighbourRoomArea.leftSide;

                        if (parentRoom.RoomArea.rightSide >= pNeighbourRoomArea.rightSide)
                            pDoorArea.point2.X = pNeighbourRoomArea.rightSide;
                        else pDoorArea.point2.X = parentRoom.RoomArea.rightSide;

                        pDoorArea.point1.Y = pDoorArea.sharedWall;
                        pDoorArea.point2.Y = pDoorArea.sharedWall;
                        break;
                    case NeighbourRoomDirection.Left:
                    case NeighbourRoomDirection.Right:
                        if (parentRoom.RoomArea.topSide >= pNeighbourRoomArea.topSide)
                            pDoorArea.point1.Y = parentRoom.RoomArea.topSide;
                        else pDoorArea.point1.Y = pNeighbourRoomArea.topSide;

                        if (parentRoom.RoomArea.bottomSide <= pNeighbourRoomArea.bottomSide)
                            pDoorArea.point2.Y = parentRoom.RoomArea.bottomSide;
                        else pDoorArea.point2.Y = pNeighbourRoomArea.bottomSide;

                        pDoorArea.point1.X = pDoorArea.sharedWall;
                        pDoorArea.point2.X = pDoorArea.sharedWall;
                        break;
                }
            }

            private int getSharedWall(NeighbourRoomDirection pDirection)
            {
                switch (pDirection)
                {
                    case NeighbourRoomDirection.Top:
                        return parentRoom.RoomArea.topSide;
                    case NeighbourRoomDirection.Bottom:
                        return parentRoom.RoomArea.bottomSide;
                    case NeighbourRoomDirection.Left:
                        return parentRoom.RoomArea.leftSide;
                    case NeighbourRoomDirection.Right:
                        return parentRoom.RoomArea.rightSide;
                }

                return -1;
            }
        }
    }
}