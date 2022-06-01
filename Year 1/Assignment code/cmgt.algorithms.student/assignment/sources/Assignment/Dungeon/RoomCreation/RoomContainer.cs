using System;
using System.Collections.Generic;
using System.Drawing;
using DoorCreation;
using GXPEngine;
using RoomCreation.DoorCreation;

public struct RoomArea
{
    public int left;
    public int right;
    public int top;
    public int bot;
}

/**
 * This class represents (the data for) a Room, at this moment only a rectangle in the dungeon.
 */ 

namespace RoomCreation
{
    public class RoomContainer : DungeonComponent
    {
        public RoomCreationHandler RoomCreator;
        public DoorCreationHandler DoorCreator;
        public RoomDebugInfo debugInfo;
        public Rectangle OriginalSize;
        public RoomArea RoomArea;
        public Dictionary<RoomContainer, Door> CreatedDoors = new Dictionary<RoomContainer, Door>();
        public Dictionary<DungeonComponent, Node> CreatedNodes = new Dictionary<DungeonComponent, Node>();

        public Dictionary<RoomContainer, NeighbourRoomDirection> ConnectedRooms =
            new Dictionary<RoomContainer, NeighbourRoomDirection>();

        public int ID { get; set; }
        public float RandomSplitValue;

        public RoomContainer(Rectangle pOriginalSize)
        {
            OriginalSize = pOriginalSize;

            initializeSubsystems();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //									    void initializeSubsystems()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initialize the different handlers for this room. R
        /// esponsible for RoomCreationHandler, DoorCreationHandler, and RoomDebugInfo.
        /// </summary>
        private void initializeSubsystems()
        {
            RoomCreator = new RoomCreationHandler(this, OriginalSize, RandomSplitValue);
            RoomArea = RoomCreator.ThisRoomAreaProps;
            DoorCreator = new DoorCreationHandler(this, RoomArea);

            if (AlgorithmsAssignment.ENABLE_ROOM_VISUAL_DEBUG)
                debugInfo = new RoomDebugInfo
                {
                    ScreenPosition =
                    {
                        x = (RoomArea.left + 1) * AlgorithmsAssignment.SCALE,
                        y = (RoomArea.top + 7) * AlgorithmsAssignment.SCALE
                    }
                };
        }

        //------------------------------------------------------------------------------------------------------------------------
        //									    int CalculateArea()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Determine the m2 (total surface) of this room, by performing width * height.
        /// </summary>
        public int CalculateArea()
        {
            int width = RoomArea.right - RoomArea.left;
            int height = RoomArea.bot - RoomArea.top;

            return width * height;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //									    void UpdateProperties()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Passes on a request for RoomDebugInfo to update the visual debug graphic on screen.
        /// </summary>
        public void UpdateProperties()
        {
            debugInfo.UpdateDebugInformation(ID, RoomArea, CreatedDoors.Count, CreatedNodes.Count);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //										            string ToString()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns important info about this room in the console.
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return String.Format("Room ID: {0}\nLeft side:{1}, right side:{2}, \ntop side:{3}, bottom side:{4}", ID,
                RoomArea.left, RoomArea.right, RoomArea.top, RoomArea.bot);
        }
    }

    /// <summary>
    /// Necessary for generalising RoomContainer and Door, so we can place nodes more easily.
    /// </summary>
    public abstract class DungeonComponent : GameObject
    {
        
    }
}