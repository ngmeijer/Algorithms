﻿using System;
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
            /// todo// clean this **** up
            ///
            /// Loops over all created rooms, and checks if they are valid neighbours (do they share a border (horizontal/vertical)?
            /// And are they in range on the other axis? Refer to notes.
            /// </summary>
            /// <returns>List<RoomContainer></returns>
            public Dictionary<RoomContainer, NeighbourRoomDirection> findNeighbourRooms(RoomContainer pParentRoom,
                List<RoomContainer> pFinishedRooms)
            {
                foreach (RoomContainer otherRoom in pFinishedRooms)
                {
                    if (otherRoom.ConnectedRooms.ContainsKey(pParentRoom)) continue;
                    if (pParentRoom.ConnectedRooms.ContainsKey(otherRoom)) continue;
                    if (otherRoom.ID == pParentRoom.ID) continue;
                    RoomArea other = otherRoom.RoomArea;

                    //Checking IF rooms are neighbours.
                    bool otherRoomLeftOfMain = checkIfOnExactBorder(other.rightSide, roomArea.leftSide) &&
                                               (checkIfInsideAreaWithOffset(other.topSide, roomArea.topSide,
                                                    roomArea.bottomSide) ||
                                                checkIfInsideAreaWithOffset(other.bottomSide, roomArea.topSide,
                                                    roomArea.bottomSide));

                    bool otherRoomRightOfMain = checkIfOnExactBorder(other.leftSide, roomArea.rightSide) &&
                                                (checkIfInsideAreaWithOffset(other.topSide, roomArea.topSide,
                                                     roomArea.bottomSide) ||
                                                 checkIfInsideAreaWithOffset(other.bottomSide, roomArea.topSide,
                                                     roomArea.bottomSide));

                    bool otherRoomAboveMain = checkIfOnExactBorder(other.bottomSide, roomArea.topSide) &&
                                              (checkIfInsideAreaWithOffset(other.leftSide, roomArea.leftSide,
                                                   roomArea.rightSide) ||
                                               checkIfInsideAreaWithOffset(other.rightSide, roomArea.leftSide,
                                                   roomArea.rightSide));

                    bool otherRoomUnderMain = checkIfOnExactBorder(other.topSide, roomArea.bottomSide) &&
                                              (checkIfInsideAreaWithOffset(other.leftSide, roomArea.leftSide,
                                                   roomArea.rightSide) ||
                                               checkIfInsideAreaWithOffset(other.rightSide, roomArea.leftSide,
                                                   roomArea.rightSide));

                    if (otherRoomLeftOfMain)
                    {
                        pParentRoom.ConnectedRooms.Add(otherRoom, NeighbourRoomDirection.Left);
                        otherRoom.ConnectedRooms.Add(pParentRoom, NeighbourRoomDirection.Right);
                    }
                    else if (otherRoomRightOfMain)
                    {
                        pParentRoom.ConnectedRooms.Add(otherRoom, NeighbourRoomDirection.Right);
                        otherRoom.ConnectedRooms.Add(pParentRoom, NeighbourRoomDirection.Left);
                    }
                    else if (otherRoomAboveMain)
                    {
                        pParentRoom.ConnectedRooms.Add(otherRoom, NeighbourRoomDirection.Top);
                        otherRoom.ConnectedRooms.Add(pParentRoom, NeighbourRoomDirection.Bottom);
                    }
                    else if (otherRoomUnderMain)
                    {
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
            /// 
            /// </summary>
            /// <returns>Bool</returns>
            private bool checkIfOnExactBorder(int pOtherSide, int pMainSide)
                => pOtherSide == pMainSide;

            //------------------------------------------------------------------------------------------------------------------------
            //			                                      bool checkIfInsideAreaWithOffset()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// 
            /// </summary>
            /// <returns>Bool</returns>
            private bool checkIfInsideAreaWithOffset(int pOtherSide, int pMainSide0, int pMainSide1)
                => pOtherSide >= pMainSide0 &&
                   pOtherSide <= pMainSide1;
        }
    }
}