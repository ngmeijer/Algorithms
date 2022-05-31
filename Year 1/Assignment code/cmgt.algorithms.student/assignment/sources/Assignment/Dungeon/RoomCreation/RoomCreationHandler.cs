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

        public RoomCreationHandler(RoomContainer pParent, Rectangle pOriginalSize,
            float pSplitValue)
        {
            parent = pParent;
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
            roomArea = new RoomArea
            {
                leftSide = originalSize.X,
                rightSide = originalSize.X + originalSize.Width - 1,
                topSide = originalSize.Y,
                bottomSide = originalSize.Y + originalSize.Height - 1
            };

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
            Rectangle[] roomSizes = initializeSizeContainers();

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

            for (int i = 0; i < newRooms.Length; i++)
            {
                newRooms[i] = new RoomContainer(roomSizes[i]);
                newRooms[i].x = parent.x * randomSplitValue;
                newRooms[i].y = parent.y * randomSplitValue;
            }

            return newRooms;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //										    Rectangle[] defineSizes()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary> 
        /// <returns>String</returns>
        private Rectangle[] initializeSizeContainers()
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
            
            if (originalSize.Width * pRandomSplitValue > minSize || originalSize.Height * pRandomSplitValue > minSize)
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