using System;
using System.Drawing;
using GXPEngine;
using System.Collections.Generic;
using System.Linq;

class SufficientDungeon : Dungeon
{
    #region

    private int roomWidth1 = 41;
    private int roomHeight1 = 20;

    private int xPosition1 = 0;
    private int yPosition1 = 0;

    #endregion

    private List<Room> roomsToSplit;

    private int maxRooms = 9;
    private int maxDoors = 1;

    public SufficientDungeon(Size pSize) : base(pSize)
    {
        roomsToSplit = new List<Room>();
    }

    protected override void generate(int pMinimumRoomSize)
    {
        roomsToSplit.Add(new Room(new Rectangle(xPosition1, yPosition1, roomWidth1, roomHeight1)));

        //roomWidth1 ended up being the actual width of the area the rooms can be spawned in.
        Console.WriteLine("game width:" + roomWidth1);

        while (roomsToSplit.Count > 0 && rooms.Count < maxRooms)
        {
            Room currentRoom = roomsToSplit.First();

            int randomDoorPosY = Utils.Random(currentRoom.area.Y + 1, currentRoom.area.Height - 1);

            for (int currentDoors = 0; currentDoors < maxDoors; currentDoors++)
            {
                //Console.WriteLine("current amount of doors: " + currentDoors);
                //Console.WriteLine("max amount of doors: " + maxDoors);

                doors.Add(new Door(new Point(currentRoom.area.Width, randomDoorPosY)));
                doors.Add(new Door(new Point(currentRoom.area.Width - 1, randomDoorPosY)));
            }


            if (currentRoom.area.Width <= pMinimumRoomSize || currentRoom.area.Height <= pMinimumRoomSize)
            {
                roomsToSplit.Remove(currentRoom);
                rooms.Add(currentRoom);
            }
            else
            {
                roomsToSplit.Add(new Room(new Rectangle(currentRoom.area.X,
                                                        currentRoom.area.Y,
                                                        currentRoom.area.Width / 2,
                                                        currentRoom.area.Height)));

                roomsToSplit.Add(new Room(new Rectangle(currentRoom.area.X + currentRoom.area.Width / 2,
                                                        currentRoom.area.Y,
                                                        currentRoom.area.Width / 2,
                                                        currentRoom.area.Height / 2)));

                roomsToSplit.Remove(currentRoom);
            }

            #region Starting code

            //Room currentRoom = roomsToSplit.First();

            ////roomsToSplit.Add(currentRoom);

            ////if (roomWidth1 > pMinimumRoomSize)
            ////{
            ////    roomWidth1 /= 2;
            ////}

            ////if (roomHeight1 > pMinimumRoomSize)
            ////{
            ////    roomHeight1 /= 2;
            ////}

            //roomsToSplit.Remove(currentRoom);
            //rooms.Add(currentRoom);

            #endregion
        }

        Console.WriteLine("Rooms size decreased: " + rooms.Count);
        //Console.WriteLine("Rooms yet to decrease: " + roomsToSplit.Count);

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

        #region 4th iteration

        //Changing roomWidth1 to a static number removed the randomness..
        //roomsToSplit.Add(new Room(new Rectangle(xPosition1, yPosition1, roomWidth1, roomHeight1)));

        //while (roomsToSplit.Count > 0 && rooms.Count < maxRooms)
        //{
        //    Room currentRoom = roomsToSplit.First();

        //    if (currentRoom.area.Width <= pMinimumRoomSize || currentRoom.area.Height <= pMinimumRoomSize)
        //    {
        //        roomsToSplit.Remove(currentRoom);
        //        rooms.Add(currentRoom);
        //    }
        //    else
        //    {
        //        roomsToSplit.Add(new Room(new Rectangle(currentRoom.area.X,
        //                                                currentRoom.area.Y,
        //                                                currentRoom.area.Width / 2,
        //                                                currentRoom.area.Height)));

        //        roomsToSplit.Add(new Room(new Rectangle(currentRoom.area.X + currentRoom.area.Width / 2,
        //                                                currentRoom.area.Y,
        //                                                currentRoom.area.Width / 2,
        //                                                currentRoom.area.Height)));
        //        Console.WriteLine("current room width:" + currentRoom.area.Width);
        //        roomsToSplit.Remove(currentRoom);
        //    }

        //    //doors.Add(new Door(new Point(Utils.Random(currentRoom.area.X + 1, currentRoom.area.Width - 1), currentRoom.area.Height)));
        //}

        #endregion

        #endregion
    }
}
