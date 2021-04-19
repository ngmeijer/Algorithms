using System;
using System.Drawing;
using GXPEngine;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

enum AXIS
{
    HORIZONTAL,
    VERTICAL,
};

class SufficientDungeon : Dungeon
{
    private RNG numGenerator;

    private List<Room> roomsToSplit;

    private int maxRooms = 6;
    private int maxDoors = 1;

    private Size startingSize;

    public SufficientDungeon(Size pSize) : base(pSize)
    {
        roomsToSplit = new List<Room>();
        startingSize = pSize;
    }

    protected override void generate(int pMinimumRoomSize)
    {
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

        roomsToSplit.Clear();
        finishedRooms.Clear();

        Room startingRoom = new Room(new Rectangle(0, 0, startingSize.Width, startingSize.Height));
        roomsToSplit.Add(startingRoom);

        int mainIteration = 0;
        int subIteration = 0;

        while (roomsToSplit.Count > 0 && finishedRooms.Count < maxRooms)
        {
            for (int mainRoomsIndex = 0; mainRoomsIndex < roomsToSplit.Count; mainRoomsIndex++)
            {
                Room[] newRooms = roomsToSplit[mainRoomsIndex].Split();

                roomsToSplit.Remove(startingRoom);

                for (int subRoomsIndex = 0; subRoomsIndex < newRooms.Length; subRoomsIndex++)
                {
                    if (newRooms[subRoomsIndex].ShouldSplit(newRooms[subRoomsIndex].area, pMinimumRoomSize))
                    {
                        Console.WriteLine($"Should split room with width {newRooms[subRoomsIndex].area.Width}");
                        Room[] newSubRooms = newRooms[subRoomsIndex].Split();
                        finishedRooms.Add(newSubRooms[0]);
                        finishedRooms.Add(newSubRooms[1]);
                    }
                }

                finishedRooms.Add(newRooms[0]);
                finishedRooms.Add(newRooms[1]);

                subIteration++;
                Console.WriteLine($"Sub iteration: {subIteration}");
            }

            mainIteration++;
            Console.WriteLine($"Main iteration: {mainIteration}");
        }

        #endregion
    }
}