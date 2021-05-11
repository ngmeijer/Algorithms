using GXPEngine;

public class RoomDebugInfo : GameObject
{
    //"Worldspace" coordinates
    public Vec2 ScreenPosition;
    private EasyDraw debugText;
    public int ID { get; private set; }
    public int DoorCount { get; private set; }
    public RoomArea RoomArea { get; private set; }

    public delegate void OnRoomPropertiesGenerated(int pID, RoomArea pRoomArea);
    public event OnRoomPropertiesGenerated onGenerated;

    public RoomDebugInfo(int pID, RoomArea pRoomArea)
    {
        ID = pID;
        RoomArea = pRoomArea;

        handleDebugTextInitalization();
    }

    //------------------------------------------------------------------------------------------------------------------------
    //									    void handleDebugTextInitalization()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private void handleDebugTextInitalization()
    {
        debugText = new EasyDraw(game.width, game.height);
        AddChild(debugText);
        debugText.SetColor(0, 255, 0);
        debugText.SetScaleXY(0.1f, 0.1f);
    }

    public void UpdateDebugInformation(int pID, RoomArea pRoomArea)
    {
        updateRoomID(pID);
        UpdateRoomArea(pRoomArea);
    }

    //------------------------------------------------------------------------------------------------------------------------
    //								     void updateRoomArea(RoomArea pArea)
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    public void UpdateRoomArea(RoomArea pArea)
    {
        RoomArea = pArea;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //								                void UpdateRoomID(int pID)
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private void updateRoomID(int pID)
    {
        ID = pID;
        debugText.Text($"ID: {pID}." +
                    $"\nLeft: {RoomArea.leftSide}." +
                    $"\nRight: {RoomArea.rightSide}." +
                    $"\nTop: {RoomArea.topSide}." +
                    $"\nBottom:{RoomArea.bottomSide}" +
                    $"\nDoor count: {DoorCount}", ScreenPosition.x, ScreenPosition.y + 135);

        onGenerated?.Invoke(pID, RoomArea);
    }

    public void UpdateDoorCount(int pDoorCount)
    {
        DoorCount = pDoorCount;
    }
}