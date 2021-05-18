using System;
using GXPEngine;
using System.Collections.Generic;
using System.Drawing;
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
    public class SufficientBaseDungeon : BaseDungeon
    {
        private List<RoomContainer> roomsToSplit = new List<RoomContainer>();

        private Size startingSize;

        public SufficientBaseDungeon(Size pSize) : base(pSize) => startingSize = pSize;

        protected override void generate(int pMinimumRoomSize)
        {
            //1s0t year code

            #region Starting code

            //Room currentRoom = roomsToSplit.First();

            //roomsToSplit.Add(currentRoom);

            //if (roomWidth1 > pMinimumRoomSize)
            //{
            //    roomWidth1 /= 2;
            //}

            //if (roomHeight1 > pMinimumRoomSize)
            //{
            //    roomHeight1 /= 2;
            //}

            //roomsToSplit.Remove(currentRoom);
            //finishedRooms.Add(currentRoom);

            #endregion

            #region 1st iteration

            //Use for-loop to generate the rooms instead of instantiating by hand
            //Generate new random width & height (keeping pMinimumRoomSize in mind), x-position and y-position values for every room in the for-loop
            //First iteration
            //for (int roomCountX = 0; roomCountX < maxRoomCount; roomCountX++)
            //{

            //_randomWidth = Utils.Random(pMinimumRoomSize, _maxRoomSize);
            //_randomHeight = Utils.Random(pMinimumRoomSize, _maxRoomSize);

            //_randomPositionY = Utils.Random(-12, 5);
            //_randomOffset = Utils.Random(7, 14);

            //rooms.Add(new Room(new Rectangle(0, _randomPositionY + _randomOffset, _randomWidth, _randomHeight)));

            //rooms.Add(new Room(new Rectangle(xPosition, yPosition, randomWidth, randomHeight)));

            //randomWidth = Utils.Random(pMinimumRoomSize, maxRoomSize);
            //randomHeight = Utils.Random(pMinimumRoomSize, maxRoomSize);

            //xPosition += randomWidth - offsetX;
            ////yPosition += randomHeight - offsetY;
            ///

            #endregion

            #region 2nd iteration

            //rooms.add(new room(new rectangle(xposition1, 0, roomwidth1, roomheight1)));
            ////rooms.add(new room(new rectangle(xposition2, yposition2, roomwidth2, roomheight2)));
            //doors.add(new door(new point(xposition1, utils.random(yposition1, yposition1 + roomheight1 - 2))));

            //if (roomwidth1 > pminimumroomsize)
            //{
            //    roomwidth1 /= 2;
            //}

            ////xposition1 += 10;
            //yposition2 += 10;

            //}

            #endregion

            #region 3rd iteration

            //for (int roomcount = 0; roomcount < maxRoomCount; roomcount++)
            //{
            //    int randomchance = Utils.Random(0, 2);

            //    if (randomchance == 0)
            //    {
            //        rooms.Add(new Room(new Rectangle(xPosition1, yPosition1, roomWidth1, roomHeight1)));
            //    }

            //    if (randomchance == 1)
            //    {
            //        rooms.Add(new Room(new Rectangle(xPosition2, yPosition2, roomWidth2, roomHeight2)));


            //    }
            //}

            //while (roomWidth1 > pMinimumRoomSize)
            //{
            //    roomWidth1 /= 2;
            //}

            #endregion

            #region 4th iteration

            //Changing roomWidth1 to a static number removed the randomness..roomsToSplit.Add(new Room(new Rectangle(xPosition1, yPosition1, roomWidth1, roomHeight1)));

            //Room startingRoom = new Room(new Rectangle(0, 0, startingSize.Width, startingSize.Height));
            //roomsToSplit.Add(startingRoom);

            //while (roomsToSplit.Count > 0 && finishedRooms.Count < maxRooms)
            //{
            //    Room currentRoom = roomsToSplit.First();

            //    if (currentRoom.area.Width <= pMinimumRoomSize || currentRoom.area.Height <= pMinimumRoomSize)
            //    {
            //        finishedRooms.Add(currentRoom);
            //    }
            //    else
            //    {
            //        Room[] roomsAfterSplitting = currentRoom.Split();
            //        for (int i = 0; i < roomsAfterSplitting.Length; i++)
            //        {
            //            if (roomsAfterSplitting[i].area.Width <= pMinimumRoomSize)
            //                finishedRooms.Add(roomsAfterSplitting[i]);
            //            else
            //            {
            //                roomsToSplit.Add(roomsAfterSplitting[i]);
            //            }
            //        }
            //    }

            //    roomsToSplit.Remove(currentRoom);
            //}

            //doors.Add(new Door(new Point(Utils.Random(currentRoom.area.X + 1, currentRoom.area.Width - 1), currentRoom.area.Height)));

            #endregion

            //2nd year code

            #region 5th iteration

            //roomsToSplit.Clear();
            //finishedRooms.Clear();

            //int iterationIndex = 0;
            //numGenerator = new RNG();
            ////Create a first room with size of window.
            //Room startingRoom = new Room(new Rectangle(0, 0, startingSize.Width, startingSize.Height));
            //roomsToSplit.Add(startingRoom);

            ////Check if there are rooms left to split, AND if the current count of rooms has not yet reached the maximum count.
            //while (roomsToSplit.Count > 0 && finishedRooms.Count < maxRooms)
            //{
            //    for (int j = 0; j < roomsToSplit.Count; j++)
            //    {
            //        Room currentRoom = roomsToSplit[j];

            //        Console.WriteLine($"Iteration index: {iterationIndex}");
            //        Console.WriteLine($"\n\nCurrent room width: {currentRoom.area.Width}");
            //        //Check if the width is large enough to split horizontally
            //        if (currentRoom.area.Width > pMinimumRoomSize)
            //        {
            //            //Return 2 new rooms, which is the result of splitting the taken room.
            //            Room[] newRooms = currentRoom.Split();

            //            for (int i = 0; i < newRooms.Length; i++)
            //            {
            //                roomsToSplit.Add(newRooms[i]);
            //            }

            //            iterationIndex++;
            //        }
            //        else
            //        {
            //            roomsToSplit.Remove(currentRoom);
            //            finishedRooms.Add(currentRoom);
            //        }
            //    }
            //}

            #endregion

            #region 6th iteration

            //roomsToSplit.Clear();
            //finishedRooms.Clear();

            //Room startingRoom = new Room(new Rectangle(0, 0, startingSize.Width, startingSize.Height));
            //roomsToSplit.Add(startingRoom);

            ////Debugging
            //int mainIteration = 0;
            //int subIteration = 0;

            //while (roomsToSplit.Count > 0 && finishedRooms.Count < maxRooms)
            //{
            //    for (int mainRoomsIndex = 0; mainRoomsIndex < roomsToSplit.Count; mainRoomsIndex++)
            //    {
            //        Room[] newRooms = roomsToSplit[mainRoomsIndex].Split();

            //        roomsToSplit.Remove(startingRoom);

            //        for (int subRoomsIndex = 0; subRoomsIndex < newRooms.Length; subRoomsIndex++)
            //        {
            //            if (newRooms[subRoomsIndex].ShouldSplit(newRooms[subRoomsIndex].area, pMinimumRoomSize))
            //            {
            //                Console.WriteLine($"Should split room with width {newRooms[subRoomsIndex].area.Width}");
            //                Room[] newSubRooms = newRooms[subRoomsIndex].Split();

            //                finishedRooms.Add(newSubRooms[0]);
            //                finishedRooms.Add(newSubRooms[1]);
            //            }
            //        }

            //        finishedRooms.Add(newRooms[0]);
            //        finishedRooms.Add(newRooms[1]);

            //        subIteration++;
            //        Console.WriteLine($"Sub iteration: {subIteration}");
            //    }

            //    mainIteration++;
            //    Console.WriteLine($"Main iteration: {mainIteration}");
            //}

            #endregion

            #region 7th iteration

            //roomsToSplit.Clear();
            //finishedRooms.Clear();

            //Room startingRoom = new Room(new Rectangle(0, 0, startingSize.Width, startingSize.Height));
            //roomsToSplit.Add(startingRoom);
            ////int iterationIndex = 0;

            //float randomDivision = Utils.Random(0.3f, 0.8f);

            //while (roomsToSplit.Count > 0 && finishedRooms.Count < maxRooms)
            //{
            //    //iterationIndex++;
            //    //Console.WriteLine($"\n1A: ITERATION: {iterationIndex}");
            //    //Console.WriteLine($"\n1B: roomsToSplit count: {roomsToSplit.Count}");

            //    for (int roomIndex = 0; roomIndex < roomsToSplit.Count; roomIndex++)
            //    {
            //        Room currentRoom = roomsToSplit[roomIndex];
            //        bool shouldRoomSplit = roomsToSplit[roomIndex].ShouldSplit(currentRoom.area, pMinimumRoomSize);

            //        roomsToSplit.Remove(currentRoom);
            //        Console.WriteLine($"\n             3A: Should room with index [ {roomIndex} ] split?::: {shouldRoomSplit}");

            //        if (shouldRoomSplit)
            //        {
            //            Room[] newRooms = currentRoom.Split(randomDivision);

            //            Console.WriteLine($"\n                    4A: newRoom index [ 0 ] has area: {newRooms[0].area}.");
            //            Console.WriteLine($"                    4B: newRoom index [ 1 ] has area: {newRooms[1].area}.");

            //            Console.WriteLine(newRooms[0].ToString());
            //            roomsToSplit.Add(newRooms[0]);
            //            roomsToSplit.Add(newRooms[1]);

            //            Console.WriteLine($"                    4C: roomsToSplit count: {roomsToSplit.Count}");
            //            Console.WriteLine($"                    4D: finishedRooms count: {finishedRooms.Count}");
            //        }
            //        else
            //        {
            //            finishedRooms.Add(currentRoom);
            //        }
            //        Console.WriteLine("             3B: Rooms were not allowed to split.");

            //        Console.WriteLine($"             3C: roomsToSplit count: {roomsToSplit.Count}.");
            //        Console.WriteLine($"             3D: finishedRooms count: {finishedRooms.Count}.");
            //    }

            //    Console.WriteLine($"        2A: roomsToSplit count: {roomsToSplit.Count}");
            //    Console.WriteLine($"        2B: finishedRooms count: {finishedRooms.Count}");
            //}


            #endregion

            #region 8th iteration

            Console.Clear();
            roomsToSplit.Clear();
            finishedRooms.Clear();
            int roomID = 0;

            RoomContainer startingRoom =
                new RoomContainer(new Rectangle(0, 0, startingSize.Width, startingSize.Height));
            roomsToSplit.Add(startingRoom);

            while (roomsToSplit.Count > 0)
            {
                for (int roomIndex = 0; roomIndex < roomsToSplit.Count; roomIndex++)
                {
                    float randomMultiplication = Utils.Random(0.35f, 0.65f);

                    RoomContainer currentFocusedRoom = roomsToSplit[roomIndex];
                    RoomContainer[] newRooms = currentFocusedRoom.RoomCreator.Split(randomMultiplication);

                    for (int subRoomIndex = 0; subRoomIndex < newRooms.Length; subRoomIndex++)
                    {
                        if (newRooms[subRoomIndex].RoomCreator.ShouldSplit())
                            roomsToSplit.Add(newRooms[subRoomIndex]);
                        else if (!finishedRooms.Contains(newRooms[subRoomIndex]))
                        {
                            finishedRooms.Add(newRooms[subRoomIndex]);
                        }
                    }

                    roomsToSplit.Remove(currentFocusedRoom);
                }
            }

            foreach (RoomContainer room in finishedRooms)
            {
                AddChild(room);
                room.debugInfo.UpdateDebugInformation(roomID, room.RoomArea);
                roomID++;
            }

            foreach (RoomContainer room in finishedRooms)
            {
                Door[] newDoors = room.DoorCreator.InitiateDoorHandling(finishedRooms);

                for (int i = 0; i < newDoors.Length; i++)
                {
                    doors.Add(newDoors[i]);
                }
            }

            #endregion
        }
    }
}