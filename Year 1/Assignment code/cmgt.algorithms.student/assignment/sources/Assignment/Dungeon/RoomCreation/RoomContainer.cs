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
    public class RoomContainer : GameObject
    {
        public RoomCreationHandler RoomCreator;
        public DoorCreationHandler DoorCreator;
        public RoomDebugInfo debugInfo;
        public Rectangle OriginalSize;
        public RoomArea RoomArea;

        public int ID { get; private set; }
        public float RandomSplitValue;

        private delegate Door OnDoorPlacing(RoomArea pOtherArea);

        private OnDoorPlacing onDoorPlace;

        public RoomContainer(Rectangle pOriginalSize)
        {
            OriginalSize = pOriginalSize;

            initializeSubsystems();

            debugInfo.onGenerated += updateRoomProperties;
            debugInfo.UpdateRoomArea(RoomArea);
        }

        private void initializeSubsystems()
        {
            debugInfo = new RoomDebugInfo(ID, RoomArea);
            AddChild(debugInfo);
            RoomCreator = new RoomCreationHandler(this, RoomArea, OriginalSize, RandomSplitValue);
            RoomArea = RoomCreator.ThisRoomAreaProps;
            DoorCreator = new DoorCreationHandler(this);
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
        public void HandleDestroy()
        {
            RemoveChild(debugInfo);
            debugInfo.Destroy();
            RoomCreator = null;
            debugInfo = null;
            DoorCreator = null;
            Destroy();
        }

        private void updateRoomProperties(int pID, RoomArea pRoomArea)
        {
            ID = pID;
            RoomArea = pRoomArea;
        }
    }
}