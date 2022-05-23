﻿using System;
using System.Collections.Generic;
using System.Drawing;
using DoorCreation;
using GXPEngine;
using RoomCreation.DoorCreation;

public struct RoomArea
{
    public int leftSide;
    public int rightSide;
    public int topSide;
    public int bottomSide;
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

        public Dictionary<RoomContainer, NeighbourRoomDirection> ConnectedRooms =
            new Dictionary<RoomContainer, NeighbourRoomDirection>();

        public int ID { get; set; }
        public float RandomSplitValue;

        public RoomContainer(Rectangle pOriginalSize)
        {
            OriginalSize = pOriginalSize;

            initializeSubsystems();
        }

        private void initializeSubsystems()
        {
            debugInfo = new RoomDebugInfo();
            RoomCreator = new RoomCreationHandler(this, OriginalSize, RandomSplitValue);
            RoomArea = RoomCreator.ThisRoomAreaProps;
            DoorCreator = new DoorCreationHandler(this, RoomArea);

            debugInfo.ScreenPosition.x = (RoomArea.leftSide + 1) * AlgorithmsAssignment.SCALE;
            debugInfo.ScreenPosition.y = (RoomArea.topSide + 4) * (AlgorithmsAssignment.SCALE);
        }

        public int CalculateArea()
        {
            int width = RoomArea.rightSide - RoomArea.leftSide;
            int height = RoomArea.bottomSide - RoomArea.topSide;

            return width * height;
        }

        public void UpdateProperties()
        {
            debugInfo.UpdateDebugInformation(ID, RoomArea, CreatedDoors.Count);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //										            string ToString()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return String.Format("Room ID: {0}\nLeft side:{1}, right side:{2}, \ntop side:{3}, bottom side:{4}", ID,
                RoomArea.leftSide, RoomArea.rightSide, RoomArea.topSide, RoomArea.bottomSide);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //										        void handleDestroy()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns>String</returns>
        public override void HandleDestroy()
        {
            RemoveChild(debugInfo);
            debugInfo.Destroy();
            RoomCreator = null;
            debugInfo = null;
            DoorCreator = null;
            Destroy();
        }
    }

    public abstract class DungeonComponent : GameObject
    {
        public abstract void HandleDestroy();
    }
}