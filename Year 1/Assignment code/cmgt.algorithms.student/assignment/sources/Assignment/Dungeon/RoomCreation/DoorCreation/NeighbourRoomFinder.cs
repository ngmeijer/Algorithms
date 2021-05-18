using System.Collections.Generic;

namespace RoomCreation
{
    namespace DoorCreation
    {
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
            public List<RoomContainer> findNeighbourRooms(RoomContainer pParentRoom, List<RoomContainer> pFinishedRooms)
            {
                List<RoomContainer> neighbourRooms = new List<RoomContainer>();

                foreach (RoomContainer otherRoom in pFinishedRooms)
                {
                    if (neighbourRooms.Contains(otherRoom)) continue;
                    if (otherRoom.ID == pParentRoom.ID) continue;
                    RoomArea other = otherRoom.RoomArea;

                    bool leftSideAligned = checkHorizontalNeighbourAlignment(other.leftSide + 1);
                    bool rightSideAligned = checkHorizontalNeighbourAlignment(other.rightSide - 1);

                    bool topSideAligned = checkVerticalNeighbourAlignment(other.topSide + 1);
                    bool bottomSideAligned = checkVerticalNeighbourAlignment(other.bottomSide - 1);

                    if ((leftSideAligned || rightSideAligned) && (topSideAligned || bottomSideAligned))
                        neighbourRooms.Add(otherRoom);
                }

                return neighbourRooms;
            }

            //------------------------------------------------------------------------------------------------------------------------
            //			                           bool checkNeighbourRoomBoundaryConditions()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// 
            /// </summary>
            /// <returns>Bool</returns>
            private bool checkVerticalNeighbourAlignment(int pOtherSide)
            {
                if (checkIfOnExactBorder(pOtherSide, roomArea.topSide, roomArea.bottomSide) ||
                    checkIfInsideAreaWithOffset(pOtherSide, roomArea.topSide, roomArea.bottomSide))
                    return true;
                return false;
            }

            //------------------------------------------------------------------------------------------------------------------------
            //			                           bool checkNeighbourRoomBoundaryConditions()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// 
            /// </summary>
            /// <returns>Bool</returns>
            private bool checkHorizontalNeighbourAlignment(int pOtherSide)
            {
                if (checkIfOnExactBorder(pOtherSide, roomArea.leftSide, roomArea.rightSide) ||
                    checkIfInsideAreaWithOffset(pOtherSide, roomArea.leftSide, roomArea.rightSide))
                    return true;
                return false;
            }

            //------------------------------------------------------------------------------------------------------------------------
            //			                                           bool checkIfOnExactBorder()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// 
            /// </summary>
            /// <returns>Bool</returns>
            private bool checkIfOnExactBorder(int pOtherSide, int pMainSide0, int pMainSide1)
                => pOtherSide == pMainSide0 || pOtherSide == pMainSide1;

            //------------------------------------------------------------------------------------------------------------------------
            //			                                      bool checkIfInsideAreaWithOffset()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// 
            /// </summary>
            /// <returns>Bool</returns>
            private bool checkIfInsideAreaWithOffset(int pOtherSide, int pMainSide0, int pMainSide1)
                => pOtherSide > (pMainSide0 + DoorCreationHandler.OFFSET) &&
                   pOtherSide < (pMainSide1 - DoorCreationHandler.OFFSET);
        }
    }
}