using GXPEngine;

public class RoomDebugInfo : GameObject
{
    //"Worldspace" coordinates
    public Vec2 ScreenPosition;
    private EasyDraw idText;
    public int ID { get; private set; }
    private RoomArea roomArea;

    public delegate void OnRoomPropertiesGenerated(int pID, RoomArea pRoomArea);
    public event OnRoomPropertiesGenerated onGenerated;

    public RoomDebugInfo(int pID, RoomArea pRoomArea)
    {
        ID = pID;
        roomArea = pRoomArea;

        handleDebugTextInitalization();
    }

    public void UpdateRoomArea(RoomArea pArea)
    {
        roomArea = pArea;
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
        idText = new EasyDraw(game.width, game.height);
        AddChild(idText);
        idText.SetColor(0, 255, 0);
        idText.SetScaleXY(0.1f, 0.1f);
    }

    //------------------------------------------------------------------------------------------------------------------------
    //								                void UpdateRoomID(int pID)
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    public void UpdateRoomID(int pID)
    {
        ID = pID;
        idText.Text($"ID: {pID}." +
                    $"\nLeft: {roomArea.leftSide}." +
                    $"\nRight: {roomArea.rightSide}." +
                    $"\nTop: {roomArea.topSide}." +
                    $"\nBottom:{roomArea.bottomSide}", ScreenPosition.x, ScreenPosition.y + 105);

        onGenerated?.Invoke(pID, roomArea);
    }
}