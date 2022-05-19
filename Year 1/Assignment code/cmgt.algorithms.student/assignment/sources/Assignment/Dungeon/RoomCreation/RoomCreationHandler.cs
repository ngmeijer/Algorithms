using System;
using System.Drawing;
using GXPEngine;

namespace RoomCreation
{
    public class RoomCreationHandler
    {
        private Rectangle originalSize;
        private RoomArea roomArea;

        public RoomArea ThisRoomAreaProps
        {
            get => roomArea;
            private set => roomArea = value;
        }

        private float randomSplitValue;
        private RoomContainer parent;

        public RoomCreationHandler(RoomContainer pParent, RoomArea pRoomArea, Rectangle pOriginalSize,
            float pSplitValue)
        {
            parent = pParent;
            roomArea = pRoomArea;
            originalSize = pOriginalSize;
            randomSplitValue = pSplitValue;

            defineRoomArea();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //										       void defineRoomArea()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns>String</returns>
        private void defineRoomArea()
        {
            roomArea.leftSide = originalSize.X;
            roomArea.rightSide = originalSize.X + originalSize.Width - 1;
            roomArea.topSide = originalSize.Y;
            roomArea.bottomSide = originalSize.Y + originalSize.Height - 1;

            parent.debugInfo.ScreenPosition.x = (roomArea.leftSide + 1) * AlgorithmsAssignment.SCALE;
            parent.debugInfo.ScreenPosition.y = (roomArea.topSide + 4) * (AlgorithmsAssignment.SCALE);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //								         Room[] defineRooms(AXIS pSplitAxis)
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns>String</returns>
        private RoomContainer[] defineRooms(AXIS pSplitAxis)
        {
            RoomContainer[] newRooms = new RoomContainer[2];
            Rectangle[] roomSizes = defineSizes();

            switch (pSplitAxis)
            {
                case AXIS.HORIZONTAL:
                    roomSizes[0].Width = (int) (originalSize.Width * randomSplitValue);

                    roomSizes[1].Width = originalSize.Width - roomSizes[0].Width + 1;
                    roomSizes[1].X = roomArea.leftSide + roomSizes[0].Width - 1;
                    break;
                case AXIS.VERTICAL:
                    roomSizes[0].Height = (int) (originalSize.Height * randomSplitValue);

                    roomSizes[1].Height = originalSize.Height - roomSizes[0].Height + 1;
                    roomSizes[1].Y = originalSize.Y + roomSizes[0].Height - 1;
                    break;
            }

            newRooms[0] = new RoomContainer(roomSizes[0]);
            newRooms[1] = new RoomContainer(roomSizes[1]);

            newRooms[0].x = parent.x * randomSplitValue;
            newRooms[0].y = parent.y * randomSplitValue;

            newRooms[1].x = parent.x * randomSplitValue;
            newRooms[1].y = parent.y * randomSplitValue;

            return newRooms;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //										    Rectangle[] defineSizes()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns>String</returns>
        private Rectangle[] defineSizes()
        {
            Rectangle[] roomSizes = new Rectangle[2];
            roomSizes[0] = new Rectangle(roomArea.leftSide, roomArea.topSide, originalSize.Width, originalSize.Height);
            roomSizes[1] = new Rectangle(roomArea.leftSide, roomArea.topSide, originalSize.Width, originalSize.Height);

            return roomSizes;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //										         bool ShouldSplit()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns>String</returns>
        public bool ShouldSplit(float pRandomSplitValue)
        {
            int minSize = AlgorithmsAssignment.MIN_ROOM_SIZE;
            
            if (originalSize.Width * pRandomSplitValue > minSize && originalSize.Height * pRandomSplitValue > minSize)
                return true;

            return false;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //								    Room[] Split(float pRandomMultiplication)
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns>String</returns>
        public RoomContainer[] Split(float pRandomMultiplication)
        {
            randomSplitValue = pRandomMultiplication;
            AXIS splitAxis = checkLargerAxis();
            RoomContainer[] newRooms = defineRooms(splitAxis);

            return newRooms;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //										      AXIS checkLargerAxis()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns>String</returns>
        private AXIS checkLargerAxis()
        {
            AXIS axis = AXIS.VERTICAL;

            if (originalSize.Width > originalSize.Height)
                axis = AXIS.HORIZONTAL;

            return axis;
        }
    }
}