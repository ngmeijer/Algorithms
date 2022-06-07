using System;
using System.Collections.Generic;
using System.Drawing;

namespace RoomCreation
{
    namespace DoorCreation
    {
        public enum NeighbourRoomDirection
        {
            Top,
            Bottom,
            Left,
            Right,
        }

        public class NeighbourRoomFinder
        {
            private RoomArea roomArea;

            public NeighbourRoomFinder(RoomArea pRoomArea)
            {
                roomArea = pRoomArea;
            }

            //------------------------------------------------------------------------------------------------------------------------
            //			                                List<Room> findNeighbourRooms()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Loops over all created rooms, and checks if they are valid neighbours (do they share a border (horizontal/vertical)?
            /// And are they in range on the other axis? Add room reference to the neighbour room's collection.
            /// </summary>
            /// * @param pParentRoom: a reference to the parent room.
            /// * @param pFinishedRooms: all rooms that have been created.
            /// <returns>Dictionary<RoomContainer,NeighbourRoomDirection></returns>
            public Dictionary<RoomContainer, NeighbourRoomDirection> findNeighbourRooms(RoomContainer pParentRoom,
                List<RoomContainer> pFinishedRooms)
            {
                foreach (RoomContainer otherRoom in pFinishedRooms)
                {
                    if (otherRoom.ConnectedRooms.ContainsKey(pParentRoom)) continue;
                    if (pParentRoom.ConnectedRooms.ContainsKey(otherRoom)) continue;
                    if (otherRoom.ID == pParentRoom.ID) continue;
                    RoomArea other = otherRoom.RoomArea;

                    bool otherRoomLeftOfMain = checkIfOnExactBorder(other.right, roomArea.left) &&
                                               (checkIfInsideAreaWithOffset(other.top, roomArea.top, roomArea.bot) ||
                                                checkIfInsideAreaWithOffset(other.bot, roomArea.top, roomArea.bot));
                    bool otherRoomRightOfMain = checkIfOnExactBorder(other.left, roomArea.right) &&
                                                (checkIfInsideAreaWithOffset(other.top, roomArea.top, roomArea.bot) ||
                                                 checkIfInsideAreaWithOffset(other.bot, roomArea.top, roomArea.bot));
                    bool otherRoomAboveMain = checkIfOnExactBorder(other.bot, roomArea.top) &&
                                              (checkIfInsideAreaWithOffset(other.left, roomArea.left, roomArea.right) ||
                                               checkIfInsideAreaWithOffset(other.right, roomArea.left, roomArea.right));
                    bool otherRoomUnderMain = checkIfOnExactBorder(other.top, roomArea.bot) &&
                                              (checkIfInsideAreaWithOffset(other.left, roomArea.left, roomArea.right) ||
                                               checkIfInsideAreaWithOffset(other.right, roomArea.left, roomArea.right));

                    if (otherRoomLeftOfMain)
                    {
                        Console.WriteLine($"|| Room {pParentRoom.ID} has {otherRoom.ID} as neighbour. ||");
                        pParentRoom.ConnectedRooms.Add(otherRoom, NeighbourRoomDirection.Left);
                        otherRoom.ConnectedRooms.Add(pParentRoom, NeighbourRoomDirection.Right);
                    }
                    else if (otherRoomRightOfMain)
                    {
                        Console.WriteLine($"|| Room { pParentRoom.ID} has {otherRoom.ID} as neighbour. ||");
                        pParentRoom.ConnectedRooms.Add(otherRoom, NeighbourRoomDirection.Right);
                        otherRoom.ConnectedRooms.Add(pParentRoom, NeighbourRoomDirection.Left);
                    }
                    else if (otherRoomAboveMain)
                    {
                        Console.WriteLine($"|| Room { pParentRoom.ID} has {otherRoom.ID} as neighbour. ||");
                        pParentRoom.ConnectedRooms.Add(otherRoom, NeighbourRoomDirection.Top);
                        otherRoom.ConnectedRooms.Add(pParentRoom, NeighbourRoomDirection.Bottom);
                    }
                    else if (otherRoomUnderMain)
                    {
                        Console.WriteLine($"|| Room { pParentRoom.ID} has {otherRoom.ID} as neighbour. ||");
                        pParentRoom.ConnectedRooms.Add(otherRoom, NeighbourRoomDirection.Bottom);
                        otherRoom.ConnectedRooms.Add(pParentRoom, NeighbourRoomDirection.Top);
                    }
                }

                return pParentRoom.ConnectedRooms;
            }

            //------------------------------------------------------------------------------------------------------------------------
            //			                                           bool checkIfOnExactBorder()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Checks if the given sides are on the exact same cell (for one axis only)
            /// </summary>
            /// <returns>Bool</returns>
            private bool checkIfOnExactBorder(int pOtherSide, int pMainSide)
                => pOtherSide == pMainSide;

            //------------------------------------------------------------------------------------------------------------------------
            //			                                      bool checkIfInsideAreaWithOffset()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Checks if the given neighbour side is within the sides of the opposite axis of this room. Because for example,
            /// rooms might often share the same horizontal axis, but vertically, they are entirely somewhere else on the screen.
            /// </summary>
            /// <returns>Bool</returns>
            private bool checkIfInsideAreaWithOffset(int pOtherSide, int pMainSide0, int pMainSide1)
                => pOtherSide >= pMainSide0 &&
                   pOtherSide <= pMainSide1;
        }
    }
}