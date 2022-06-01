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
        //									    void defineRoomArea()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initiate a RoomArea copy, with the properties of the parent room.
        /// -1 for the right & bottom side, otherwise there would be walls with a thickness of 2 cells.
        /// </summary>
        /// <returns>void</returns>
        private void defineRoomArea()
        {
            roomArea = new RoomArea
            {
                left = originalSize.X,
                right = originalSize.X + originalSize.Width - 1,
                top = originalSize.Y,
                bot = originalSize.Y + originalSize.Height - 1
            };
        }

        //------------------------------------------------------------------------------------------------------------------------
        //								  RoomContainer[] defineRooms(AXIS pSplitAxis)
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the room instances and graphics. Based on the preferred splitAxis,
        /// determine the horizontal/vertical measurements of the children rooms.
        /// </summary>
        /// <returns>RoomContainer[]</returns>
        private RoomContainer[] defineRooms(AXIS pSplitAxis)
        {
            RoomContainer[] newRooms = new RoomContainer[2];
            Rectangle[] roomSizes = initializeSizeContainers();

            switch (pSplitAxis)
            {
                case AXIS.HORIZONTAL:
                    roomSizes[0].Width = (int) (originalSize.Width * randomSplitValue);

                    roomSizes[1].Width = originalSize.Width - roomSizes[0].Width + 1;
                    roomSizes[1].X = roomArea.left + roomSizes[0].Width - 1;
                    break;
                case AXIS.VERTICAL:
                    roomSizes[0].Height = (int) (originalSize.Height * randomSplitValue);

                    roomSizes[1].Height = originalSize.Height - roomSizes[0].Height + 1;
                    roomSizes[1].Y = originalSize.Y + roomSizes[0].Height - 1;
                    break;
            }

            for (int i = 0; i < newRooms.Length; i++)
            {
                newRooms[i] = new RoomContainer(roomSizes[i])
                {
                    x = parent.x * randomSplitValue,
                    y = parent.y * randomSplitValue
                };
            }

            return newRooms;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //								Rectangle[] initializeSizeContainers()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initialize two room graphics EasyDraw instances.
        /// </summary> 
        /// <returns>Rectangle[]</returns>
        private Rectangle[] initializeSizeContainers()
        {
            Rectangle[] roomGraphics = new Rectangle[2];
            roomGraphics[0] =
                new Rectangle(roomArea.left, roomArea.top, originalSize.Width, originalSize.Height);
            roomGraphics[1] =
                new Rectangle(roomArea.left, roomArea.top, originalSize.Width, originalSize.Height);

            return roomGraphics;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //										         bool ShouldSplit()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks if either the width or height of the original "parent" room still exceeds the minimum size,
        /// when multiplied with the given random value.
        /// </summary>
        /// * @param float pRandomSplitValue: a random value, ranging from (with current settings) 0.35f to 0.65f.
        /// <returns>bool</returns>
        public bool ShouldSplit(float pRandomSplitValue) =>
            originalSize.Width * pRandomSplitValue > AlgorithmsAssignment.MIN_ROOM_SIZE ||
            originalSize.Height * pRandomSplitValue > AlgorithmsAssignment.MIN_ROOM_SIZE;

        //------------------------------------------------------------------------------------------------------------------------
        //								    Room[] Split(float pRandomMultiplication)
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Performs the splitting action.
        /// </summary>
        /// * @param float pRandomSplitValue: a random value, ranging from (with current settings) 0.35f to 0.65f.
        /// <returns>RoomContainer[]</returns>
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
        /// Compares the width with the height, returns the largest one as an enum value.
        /// </summary>
        /// <returns>AXIS</returns>
        private AXIS checkLargerAxis() => originalSize.Width > originalSize.Height ? AXIS.HORIZONTAL : AXIS.VERTICAL;
    }
}