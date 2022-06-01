using GXPEngine;

public class RoomDebugInfo : GameObject
{
    //"Worldspace" coordinates
    public Vec2 ScreenPosition;
    private EasyDraw debugText;
    public int ID { get; private set; }
    public int DoorCount { get; private set; }
    public int NodeCount { get; private set; }
    public RoomArea RoomArea { get; private set; }

    public delegate void OnRoomPropertiesGenerated(int pID, RoomArea pRoomArea);

    public event OnRoomPropertiesGenerated onGenerated;

    public RoomDebugInfo()
    {
        handleDebugTextInitalization();
    }

    //------------------------------------------------------------------------------------------------------------------------
    //									    void handleDebugTextInitalization()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Initialize the EasyDraw object, later to be updated with the newest data.
    /// </summary>
    private void handleDebugTextInitalization()
    {
        debugText = new EasyDraw(game.width, game.height);
        AddChild(debugText);
        debugText.SetColor(0, 0, 0);
        debugText.SetScaleXY(0.1f, 0.1f);
    }
    
    //------------------------------------------------------------------------------------------------------------------------
    //										   void UpdateDebugInformation()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Updates the properties, and the EasyDraw object for easier/visual debugging.
    /// Helped a lot with finding and solving bugs.
    /// </summary>
    /// * @param pID: identification number of the room.
    /// * param pRoomArea: room border data.
    /// * param pDoorCount: total amount of doors in this room.
    /// * param pNodeCount: total amount of nodes (which the agent is able to traverse).
    /// <returns>bool</returns>
    public void UpdateDebugInformation(int pID, RoomArea pRoomArea, int pDoorCount, int pNodeCount)
    {
        updateRoomID(pID);
        updateRoomArea(pRoomArea);
        updateDoorCount(pDoorCount);
        updateNodeCount(pNodeCount);

        debugText.Text($"ID: {ID}." +
                       $"\nLeft: {RoomArea.left}." +
                       $"\nRight: {RoomArea.right}." +
                       $"\nTop: {RoomArea.top}." +
                       $"\nBottom:{RoomArea.bot}" +
                       $"\nDoor count: {DoorCount}" +
                       $"\nNode count: {NodeCount}", ScreenPosition.x, ScreenPosition.y + 135);

        onGenerated?.Invoke(pID, RoomArea);
    }

    //------------------------------------------------------------------------------------------------------------------------
    //								                void updateRoomArea()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Receive new RoomArea data.
    /// </summary>
    private void updateRoomArea(RoomArea pArea) => RoomArea = pArea;

    //------------------------------------------------------------------------------------------------------------------------
    //								                void UpdateRoomID()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Receive new ID.
    /// </summary>
    private void updateRoomID(int pID) => ID = pID;
   
    //------------------------------------------------------------------------------------------------------------------------
    //								                void updateDoorCount()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Receive the number of the total amount of doors.
    /// </summary>
    private void updateDoorCount(int pDoorCount) => DoorCount = pDoorCount;
    
    //------------------------------------------------------------------------------------------------------------------------
    //								                void updateNodeCount()
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Receive the total amount of nodes (placed on doors and in the room).
    /// </summary>
    private void updateNodeCount(int pNodeCount) => NodeCount = pNodeCount;
}