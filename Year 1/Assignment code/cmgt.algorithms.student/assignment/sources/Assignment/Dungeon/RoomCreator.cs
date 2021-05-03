using System.Drawing;
using GXPEngine;

public class RoomCreator
{
    private Rectangle originalSize;
    private RoomArea roomArea;
    private float randomSplitValue;
    private Transformable parent;

    public RoomCreator(Transformable pParent, RoomArea pRoomArea, Rectangle pOriginalSize, float pSplitValue)
    {
        parent = pParent;
        roomArea = pRoomArea;
        originalSize = pOriginalSize;
        randomSplitValue = pSplitValue;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //								         Room[] defineRooms(AXIS pSplitAxis)
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    private Room[] defineRooms(AXIS pSplitAxis)
    {
        Room[] newRooms = new Room[2];
        Rectangle[] roomSizes = defineSizes();

        switch (pSplitAxis)
        {
            case AXIS.HORIZONTAL:
                roomSizes[0].Width = (int)(originalSize.Width * randomSplitValue);

                roomSizes[1].Width = originalSize.Width - roomSizes[0].Width + 1;
                roomSizes[1].X = roomArea.leftSide + roomSizes[0].Width - 1;
                break;
            case AXIS.VERTICAL:
                roomSizes[0].Height = (int)(originalSize.Height * randomSplitValue);

                roomSizes[1].Height = originalSize.Height - roomSizes[0].Height + 1;
                roomSizes[1].Y = originalSize.Y + roomSizes[0].Height - 1;
                break;
        }

        newRooms[0] = new Room(roomSizes[0]);
        newRooms[1] = new Room(roomSizes[1]);

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
    //								    Room[] Split(float pRandomMultiplication)
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <returns>String</returns>
    public Room[] Split(float pRandomMultiplication)
    {
        randomSplitValue = pRandomMultiplication;
        AXIS splitAxis = checkLargerAxis();
        Room[] newRooms = defineRooms(splitAxis);

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