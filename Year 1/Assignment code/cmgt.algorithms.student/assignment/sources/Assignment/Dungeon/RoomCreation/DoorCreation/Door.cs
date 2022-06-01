using System.Drawing;
using GXPEngine;
using RoomCreation;

/**
 * This class represents (the data for) a Door, at this moment only a position in the dungeon.
 * Changes to this class might be required based on your specific implementation of the algorithm.
 */
namespace DoorCreation
{
    public class Door : DungeonComponent
    {
        public readonly Point location;

        //Keeping tracks of the Rooms that this door connects to,
        //might make your life easier during some of the assignments
        public RoomContainer RoomContainerA = null;
        public RoomContainer RoomContainerB = null;

        //You can also keep track of additional information such as whether the door connects horizontally/vertically
        //Again, whether you need flags like this depends on how you implement the algorithm, maybe you need other flags
        public bool horizontal = false;
        private EasyDraw debugText;

        public Door(Point pLocation, RoomContainer pRoomA, RoomContainer pRoomB)
        {
            location = pLocation;
            RoomContainerA = pRoomA;
            RoomContainerB = pRoomB;
            if (AlgorithmsAssignment.ENABLE_DOOR_VISUAL_DEBUG) handleDebugTextInitalization();
        }


        //------------------------------------------------------------------------------------------------------------------------
        //			                          void handleDebugTextInitalization()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initialize an EasyDraw object, displaying the properties of this door. What rooms does it connect? What is its position?
        /// </summary>
        private void handleDebugTextInitalization()
        {
            debugText = new EasyDraw(game.width, game.height);
            AddChild(debugText);
            debugText.SetColor(0, 0, 255);
            debugText.SetScaleXY(0.1f, 0.1f);

            debugText.TextSize(10);
            debugText.Text($"{location}." +
                           $"\nMain:{RoomContainerA.ID}." +
                           $"\nOther:{RoomContainerB.ID}", location.X * AlgorithmsAssignment.SCALE,
                location.Y * AlgorithmsAssignment.SCALE);
        }
    }
}