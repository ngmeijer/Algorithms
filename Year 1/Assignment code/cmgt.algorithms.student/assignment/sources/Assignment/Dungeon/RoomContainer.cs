using System;
using System.Collections.Generic;
using System.Drawing;
using GXPEngine;

public struct RoomArea
{
    public int leftSide;
    public int rightSide;
    public int topSide;
    public int bottomSide;
}

public enum DoorMaster
{
    THIS_ROOM,
    NEIGHBOUR_ROOM,
    UNDEFINED,
};

/**
 * This class represents (the data for) a Room, at this moment only a rectangle in the dungeon.
 */
public class RoomContainer : GameObject
{
    public RoomCreationHandler RoomCreator;
    public DoorCreationHandler DoorCreator;
    public RoomDebugInfo debugInfo;
    public Rectangle OriginalSize;
    public RoomArea RoomArea;

    public int ID;
    public float RandomSplitValue;

    private delegate Door OnDoorPlacing(RoomArea pOtherArea);
    private OnDoorPlacing onDoorPlace;

    public RoomContainer(Rectangle pOriginalSize)
    {
        AlgorithmsAssignment.OnGenerateDestroyPrevious += handleDestroy;
        OriginalSize = pOriginalSize;

        debugInfo = new RoomDebugInfo(ID, RoomArea);
        AddChild(debugInfo);
        RoomCreator = new RoomCreationHandler(this, RoomArea, OriginalSize, RandomSplitValue);
        DoorCreator = new DoorCreationHandler(this, RoomArea);

        debugInfo.UpdateRoomArea(RoomCreator.ThisRoomAreaProps);
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
    private void handleDestroy()
    {
        AlgorithmsAssignment.OnGenerateDestroyPrevious -= handleDestroy;
        Destroy();
    }
}