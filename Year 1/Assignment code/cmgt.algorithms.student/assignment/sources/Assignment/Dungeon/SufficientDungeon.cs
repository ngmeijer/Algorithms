using System;
using GXPEngine;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DoorCreation;
using RoomCreation;

enum AXIS
{
    HORIZONTAL,
    VERTICAL,
    UNDEFINED,
};

namespace Dungeon
{
    public class SufficientDungeon : BaseDungeon
    {
        private List<RoomContainer> roomsToSplit = new List<RoomContainer>();

        private Size startingSize;

        public SufficientDungeon(Size pSize) : base(pSize) => startingSize = pSize;

        protected override void generate(int pMinimumRoomSize)
        {
            #region 8th iteration

            if (AlgorithmsAssignment.MIN_ROOM_SIZE < AlgorithmsAssignment.MIN_DOOR_SPACE)
                throw new ArgumentException(
                    $"The minimum room size ({AlgorithmsAssignment.MIN_ROOM_SIZE}) should not be larger than the minimum door space ({AlgorithmsAssignment.MIN_DOOR_SPACE})!!!");

            generateNewRooms();
            removeRooms();
            generateDoors();
            
            #endregion
        }

        
        //------------------------------------------------------------------------------------------------------------------------
        //									    void UpdateDebugInformation()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// If enabled, add the EasyDraw object as a child and update its content. Object displays room data, such as its ID, room borders and door count.
        /// </summary>
        public override void UpdateDebugInformation()
        {
            if (AlgorithmsAssignment.ENABLE_ROOM_VISUAL_DEBUG)
                foreach (var roomInfo in finishedRooms)
                {
                    AddChild(roomInfo.debugInfo);
                    roomInfo.UpdateProperties();
                }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //									    void generateNewRooms()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The recursive algorithm for generating the rooms, according to the BSP concept. Start with one big room,
        /// split that room. Add resulting "children rooms" to the list with rooms yet to be split, if they should be.
        /// Else, generate an ID and add it to the finished collection.
        /// </summary>
        private void generateNewRooms()
        {
            RoomContainer startingRoom =
                new RoomContainer(new Rectangle(0, 0, startingSize.Width, startingSize.Height));
            roomsToSplit.Add(startingRoom);

            int roomID = 0;

            while (roomsToSplit.Count > 0)
            {
                for (int roomIndex = 0; roomIndex < roomsToSplit.Count; roomIndex++)
                {
                    float randomMultiplication = Utils.Random(AlgorithmsAssignment.MIN_RANDOM_MULTIPLIER,
                        AlgorithmsAssignment.MAX_RANDOM_MULTIPLIER);

                    RoomContainer currentFocusedRoom = roomsToSplit[roomIndex];
                    RoomContainer[] newRooms = currentFocusedRoom.RoomCreator.Split(randomMultiplication);

                    foreach (RoomContainer room in newRooms)
                    {
                        if (room.RoomCreator.ShouldSplit(randomMultiplication))
                            roomsToSplit.Add(room);
                        else if (!finishedRooms.Contains(room))
                        {
                            room.ID = roomID;
                            finishedRooms.Add(room);
                            roomID++;
                        }
                    }

                    roomsToSplit.Remove(currentFocusedRoom);
                }
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //									    void removeRooms()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the total area (width * height) of every room, and checks if it's smaller than the current known smallest room,
        /// or larger than the current known largest room. If either is true, replace the old value.
        /// Can/should be improved by checking if there are multiple rooms with the same smallest/largest area.
        /// </summary>
        private void removeRooms()
        {
            //MaxValue, because setting an arbitrary number (10000?) could technically cause exception cases. (not realistically but still :))
            int currentSmallestArea = int.MaxValue;
            int currentLargestArea = 0;
            RoomContainer smallestRoom = null;
            RoomContainer largestRoom = null;
            foreach (var room in finishedRooms)
            {
                int roomArea = room.CalculateArea();
                if (roomArea < currentSmallestArea)
                {
                    currentSmallestArea = roomArea;
                    smallestRoom = room;
                }

                if (roomArea > currentLargestArea)
                {
                    currentLargestArea = roomArea;
                    largestRoom = room;
                }
            }

            finishedRooms.Remove(largestRoom);
            finishedRooms.Remove(smallestRoom);
        }
        
        //------------------------------------------------------------------------------------------------------------------------
        //									    void generateDoors()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// After all rooms have been generated, loop over the rooms and initiate door generation.
        /// </summary>
        private void generateDoors()
        {
            foreach (RoomContainer room in finishedRooms)
            {
                Door[] newDoors = room.DoorCreator.InitiateDoorHandling(finishedRooms);

                foreach (Door door in newDoors)
                {
                    doors.Add(door);
                    AddChild(door);
                }
            }
        }
    }
}