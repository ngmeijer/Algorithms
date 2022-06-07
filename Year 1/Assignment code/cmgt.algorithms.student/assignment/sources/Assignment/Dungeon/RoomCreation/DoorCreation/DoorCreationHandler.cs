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
        public class DoorArea
        {
            public Point point1;
            public Point point2;
            public RoomContainer roomA;
            public RoomContainer roomB;
            public NeighbourRoomDirection direction;
            public int sharedWall;
        }

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
            //										  Door[] InitiateDoorHandling()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Find all neighbour rooms. Find valid "regions" (to allow for procedural door placement)
            /// to place a door, for every neighbour room. Initialize door instances.
            /// </summary>
            /// * @param List pFinishedRooms: a random value, ranging from (with current settings) 0.35f to 0.65f.
            /// <returns>Door[]</returns>
            public Door[] InitiateDoorHandling(List<RoomContainer> pFinishedRooms)
            {
                Dictionary<RoomContainer, NeighbourRoomDirection> neighbourRooms =
                    roomFinder.findNeighbourRooms(parentRoom, pFinishedRooms);
                Dictionary<RoomContainer, DoorArea> newDoorPositions = findDoorSpacesForNeighbours(neighbourRooms);
                Door[] newDoors = createDoorsForAllNeighbours(newDoorPositions);
                return newDoors;
            }

            //------------------------------------------------------------------------------------------------------------------------
            //								 DoorArea findSingleDoorPosition()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Find a valid space/region for a door to be placed in, for the given neighbour room.
            /// First, request direction of the neighbour room.
            /// Then, get the shared wall.
            /// Finds the outer borders (overlap between rooms), between which a door can be placed.
            /// Finally, check if there is enough space with the current settings. If not, null is returned and a door will not be placed.
            /// </summary>
            /// * @param List pFinishedRooms: a random value, ranging from (with current settings) 0.35f to 0.65f.
            /// <returns>DoorArea</returns>
            private DoorArea findSingleDoorPosition(RoomContainer pOtherRoom)
            {
                parentRoom.ConnectedRooms.TryGetValue(pOtherRoom, out NeighbourRoomDirection neighbourDirection);
                int sharedWall = getSharedWall(neighbourDirection);
                Console.WriteLine($"\n||||||||||||Shared wall value of room {parentRoom.ID} & neighbour room {pOtherRoom.ID} is: {sharedWall}. DIrection: {neighbourDirection}");

                DoorArea newArea = new DoorArea()
                {
                    point1 = new Point(),
                    point2 = new Point(),
                    roomA = parentRoom,
                    roomB = pOtherRoom,
                    direction = neighbourDirection,
                    sharedWall = sharedWall
                };

                setDoorSpaceLimits(neighbourDirection, pOtherRoom, newArea);
                if (checkIfDoorHasEnoughSpace(newArea) == false)
                {
                    //Possible suggestion for a bottleneck fix: it double checks whether a door can be placed; once with one room as the "main room" and the other room as the "neighbour room", and once the other way around.
                    Console.WriteLine($"\nRoom {parentRoom.ID} did not have a valid door position connecting with room {pOtherRoom.ID}");
                    return null;
                }
                return newArea;
            }

            //------------------------------------------------------------------------------------------------------------------------
            //								 bool checkIfDoorHasEnoughSpace()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Checks if there's still at least 1 cell of space, by comparing the length of the total overlap length between the rooms,
            /// with the minimum amount of door space + twice (for the left/right or top/bottom side) the door offset. 
            /// </summary>
            /// * @param pNewarea: given available space.
            /// <returns>bool</returns>
            private bool checkIfDoorHasEnoughSpace(DoorArea pNewArea)
            {
                Vector2 differenceVector =
                    new Vector2(pNewArea.point2.X - pNewArea.point1.X, pNewArea.point2.Y - pNewArea.point1.Y);

                return differenceVector.Length() >
                       (AlgorithmsAssignment.MIN_DOOR_SPACE + AlgorithmsAssignment.DOOR_OFFSET * 2);
            }

            //------------------------------------------------------------------------------------------------------------------------
            //						Dictionary<RoomContainer, DoorArea> findDoorSpacesForNeighbours()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Loops over the given neighbour rooms, and find the first iteration of
            /// available door space for every door connecting to the neighbour room.
            /// </summary>
            /// * @param pNeighbourRooms: rooms that have a common border with this room.
            /// <returns>Dictionary<RoomContainer><DoorArea></returns>
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

            //------------------------------------------------------------------------------------------------------------------------
            //						        Door[] createDoorsForAllNeighbours()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Loops over all the overlap data containers. Finds a procedural exact coordinate/position for the door.
            /// Adds door to both connected rooms. Required for node generation, later on.
            /// </summary>
            /// * @param pNewDoorPositions: the final available door spawnpoints.
            /// <returns>Dictionary<RoomContainer><DoorArea></returns>
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
                    Console.WriteLine($"Room {parentRoom.ID} now has a door connecting with room {overlap.Value.roomB.ID} at position {newDoor.location}");
                }

                return createdDoors.ToArray();
            }

            //------------------------------------------------------------------------------------------------------------------------
            //	        					        void setDoorSpaceLimits()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Loops over all the overlap data containers. Finds a procedural exact coordinate/position for the door.
            /// Adds door to both connected rooms. Required for node generation, later on.
            /// </summary>
            /// * @param pDirection: the direction (Top, Bottom, Left, Right) of the neighbour room.
            /// * @param pNeighbourRoomArea: the area properties of the neighbour room.
            /// * @param pDoorArea: the data object for the door overlap. Some values (e.g. RoomA and B) already assigned. 
            private void setDoorSpaceLimits(NeighbourRoomDirection pDirection, RoomContainer pNeighbourRoom,
                DoorArea pDoorArea)
            {
                switch (pDirection)
                {
                    case NeighbourRoomDirection.Top:
                    case NeighbourRoomDirection.Bottom:
                        if (parentRoom.RoomArea.left >= pNeighbourRoom.RoomArea.left)
                            pDoorArea.point1.X = parentRoom.RoomArea.left;
                        else pDoorArea.point1.X = pNeighbourRoom.RoomArea.left;

                        if (parentRoom.RoomArea.right >= pNeighbourRoom.RoomArea.right)
                            pDoorArea.point2.X = pNeighbourRoom.RoomArea.right;
                        else pDoorArea.point2.X = parentRoom.RoomArea.right;

                        pDoorArea.point1.Y = pDoorArea.sharedWall;
                        pDoorArea.point2.Y = pDoorArea.sharedWall;
                        break;
                    case NeighbourRoomDirection.Left:
                    case NeighbourRoomDirection.Right:
                        if (parentRoom.RoomArea.top >= pNeighbourRoom.RoomArea.top)
                            pDoorArea.point1.Y = parentRoom.RoomArea.top;
                        else pDoorArea.point1.Y = pNeighbourRoom.RoomArea.top;

                        if (parentRoom.RoomArea.bot <= pNeighbourRoom.RoomArea.bot)
                            pDoorArea.point2.Y = parentRoom.RoomArea.bot;
                        else pDoorArea.point2.Y = pNeighbourRoom.RoomArea.bot;

                        pDoorArea.point1.X = pDoorArea.sharedWall;
                        pDoorArea.point2.X = pDoorArea.sharedWall;
                        break;
                }

                Console.WriteLine($"Room {parentRoom.ID} & neighbour room {pNeighbourRoom.ID} initial door space: point 1 ={pDoorArea.point1} | point 2 = {pDoorArea.point2} ");
            }

            //------------------------------------------------------------------------------------------------------------------------
            //	        					        void getSharedWall()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Helper method to quickly get the "shared wall" between rooms, based on the neighbour direction. Returns -1 if no valid value has been entered.
            /// </summary>
            /// * @param pDirection: the direction (Top, Bottom, Left, Right) of the neighbour room.
            /// <returns>int</returns>
            private int getSharedWall(NeighbourRoomDirection pDirection)
            {
                switch (pDirection)
                {
                    case NeighbourRoomDirection.Top:
                        return parentRoom.RoomArea.top;
                    case NeighbourRoomDirection.Bottom:
                        return parentRoom.RoomArea.bot;
                    case NeighbourRoomDirection.Left:
                        return parentRoom.RoomArea.left;
                    case NeighbourRoomDirection.Right:
                        return parentRoom.RoomArea.right;
                }

                return -1;
            }
        }
    }
}