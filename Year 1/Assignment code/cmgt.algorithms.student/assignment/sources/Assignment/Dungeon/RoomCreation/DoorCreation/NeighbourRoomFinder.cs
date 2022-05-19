using System;
using System.Collections.Generic;
using System.Drawing;

namespace RoomCreation
{
    namespace DoorCreation
    {
        public struct DoorArea
        {
            public Point point1;
            public Point point2;
            public RoomContainer roomA;
            public RoomContainer roomB;
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
            /// 
            /// </summary>
            /// <returns>List<RoomContainer></returns>
            public List<RoomContainer> findNeighbourRooms(RoomContainer pParentRoom, List<RoomContainer> pFinishedRooms,
                out List<DoorArea> pDoorPositions)
            {
                Console.WriteLine($"\n\n");

                List<RoomContainer> neighbourRooms = new List<RoomContainer>();
                pDoorPositions = new List<DoorArea>();

                foreach (RoomContainer otherRoom in pFinishedRooms)
                {
                    if (neighbourRooms.Contains(otherRoom)) continue;
                    if (otherRoom.ID == pParentRoom.ID) continue;
                    RoomArea other = otherRoom.RoomArea;

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

                    if (otherRoomLeftOfMain || otherRoomRightOfMain)
                    {
                        // Console.WriteLine(
                        //     $"\nMain room: {pParentRoom.ID}. Neighbour room: {otherRoom.ID}. " +
                        //     $"\nTop aligned: {otherRoomAboveMain}. Bot aligned: {otherRoomUnderMain}" +
                        //     $"\nLeft aligned: {otherRoomLeftOfMain}. Right aligned: {otherRoomRightOfMain}");

                        DoorArea newArea = new DoorArea()
                        {
                            point1 = new Point(),
                            point2 = new Point(),
                            roomA = pParentRoom,
                            roomB = otherRoom
                        };

                        if (pParentRoom.RoomArea.topSide >= otherRoom.RoomArea.topSide)
                            newArea.point1.Y = pParentRoom.RoomArea.topSide;
                        else newArea.point1.Y = otherRoom.RoomArea.topSide;

                        if (pParentRoom.RoomArea.bottomSide <= otherRoom.RoomArea.bottomSide)
                            newArea.point2.Y = pParentRoom.RoomArea.bottomSide;
                        else newArea.point2.Y = otherRoom.RoomArea.bottomSide;

                        if (otherRoomLeftOfMain)
                        {
                            newArea.point1.X = otherRoom.RoomArea.leftSide;
                            newArea.point2.X = otherRoom.RoomArea.leftSide;
                        }

                        if (otherRoomRightOfMain)
                        {
                            newArea.point1.X = pParentRoom.RoomArea.leftSide;
                            newArea.point2.X = pParentRoom.RoomArea.leftSide;
                        }

                        pDoorPositions.Add(newArea);
                    }

                    if (otherRoomAboveMain || otherRoomUnderMain)
                    {
                        // Console.WriteLine(
                        //     $"\nMain room: {pParentRoom.ID}. Neighbour room: {otherRoom.ID}. " +
                        //     $"\nTop aligned: {otherRoomAboveMain}. Bot aligned: {otherRoomUnderMain}" +
                        //     $"\nLeft aligned: {otherRoomLeftOfMain}. Right aligned: {otherRoomRightOfMain}");
                        DoorArea newArea = new DoorArea()
                        {
                            point1 = new Point(),
                            point2 = new Point(),
                            roomA = pParentRoom,
                            roomB = otherRoom
                        };

                        if (pParentRoom.RoomArea.leftSide >= otherRoom.RoomArea.leftSide)
                            newArea.point1.X = pParentRoom.RoomArea.leftSide;
                        else newArea.point1.X = otherRoom.RoomArea.leftSide;

                        if (pParentRoom.RoomArea.rightSide >= otherRoom.RoomArea.rightSide)
                            newArea.point2.X = pParentRoom.RoomArea.rightSide;
                        else newArea.point2.X = otherRoom.RoomArea.rightSide;

                        //Y is the same for both points.
                        if (otherRoomAboveMain)
                        {
                            newArea.point1.Y = otherRoom.RoomArea.topSide;
                            newArea.point2.Y = otherRoom.RoomArea.topSide;
                        }

                        if (otherRoomUnderMain)
                        {
                            newArea.point1.Y = pParentRoom.RoomArea.bottomSide;
                            newArea.point2.Y = pParentRoom.RoomArea.bottomSide;
                        }

                        pDoorPositions.Add(newArea);

                        // Console.WriteLine(
                        //     $"\nRoom ID: {pParentRoom.ID}. \nPoint 1: {newArea.point1}. Point 2: {newArea.point2}");
                    }

                    if ((otherRoomLeftOfMain || otherRoomRightOfMain) && (otherRoomAboveMain || otherRoomUnderMain))
                    {
                        neighbourRooms.Add(otherRoom);
                    }
                }

                return neighbourRooms;
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
                => pOtherSide >= (pMainSide0 + DoorCreationHandler.OFFSET) &&
                   pOtherSide <= (pMainSide1 - DoorCreationHandler.OFFSET);
        }
    }
}