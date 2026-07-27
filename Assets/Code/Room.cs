using System;
using System.Collections.Generic;
using System.Globalization;
using Items;
using UnityEngine;

namespace RoomInfo
{
    public enum RoomDirection // there is 4 directions the player can take to move between rooms
    {
        North,
        East,
        South,
        West
    }
    
    public class Room
    {
        public string RoomName; // each room has a name assigned to it
        public string RoomDescription; // each room has a description assigned to it

        [SerializeField]
        private Dictionary<RoomDirection, Room>
            _neighbourRooms =
                new Dictionary<RoomDirection, Room>(); // each room has a dictionary of neighbouring rooms assigned to it so movement is easier

        [SerializeField] public List<ItemData> _roomItems = new List<ItemData>(); // each room has a list of items assigned to it

        private ItemData _item;

        public Room(string roomName, string roomDescription) // when a room is created, it needs a name and description
        {
            RoomName = roomName;
            RoomDescription = roomDescription;
            Debug.Log($"Room created: {RoomName}" +
                      $"Items in room: {_roomItems.Count}"); // logs the room name and how many items are in the room when created
        }

        public void
            AddRoom(RoomDirection direction, Room roomName) // Add the rooms in the house... I think all rooms go here?
        {
            Room garden = new Room(RoomName, RoomDescription);
            Room foyer = new Room(RoomName, RoomDescription);
            Room kitchen = new Room(RoomName, RoomDescription);
            Room stairs = new Room(RoomName, RoomDescription);
            Room livingroom = new Room(RoomName, RoomDescription);
            Room hallway = new Room(RoomName, RoomDescription);
            Room study = new Room(RoomName, RoomDescription);
            Room guestroom = new Room(RoomName, RoomDescription);
            Room masterbedroom = new Room(RoomName, RoomDescription);
            Room bathroom = new Room(RoomName, RoomDescription);

            if (_neighbourRooms == null)
            {
                Debug.Log("There is no room that way.");
                return;
            }

            if (_neighbourRooms.ContainsKey(direction))
            {
                Debug.Log("There's already a room in that direction!");
                return;
            }

            _neighbourRooms.Add(direction, roomName);
        }

        public Room
            GetNeighbour(
                RoomDirection direction) // if there is actually a room neighbouring the room the player is on, return
        {
            if (_neighbourRooms.ContainsKey(direction))
            {
                return _neighbourRooms[direction];
            }

            return null;
        }

        private void
            ConnectRooms(Room roomA, RoomDirection direction, Room roomB) // tells the logic that the rooms have an opposite direction and they are connected
        {
            RoomDirection opposite = OppositeDirection(direction);
            roomA.AddRoom(direction, roomB);
            roomB.AddRoom(opposite, roomA);
        }

        public RoomDirection
            OppositeDirection(
                RoomDirection direction) // inverts room direction and gives the opposite direction room value
        {
            if (direction == RoomDirection.North)
            {
                return RoomDirection.South;
            }

            if (direction == RoomDirection.East)
            {
                return RoomDirection.West;
            }

            if (direction == RoomDirection.South)
            {
                return RoomDirection.North;
            }

            if (direction == RoomDirection.West)
            {
                return RoomDirection.East;
            }
            else
            {
                return direction;
            }
        }

        public void AddItem(ItemData item) // each room has specific items assigned to it, this adds those items to the room
        {
            Debug.Log("Trying to add item to" + RoomName + ", Items in room " + _roomItems.Count); // logs the item that was added to the room

            if (!_roomItems.Contains(item)) // to prevent duplicates - if the room doesn't have an item...
            {
                _roomItems.Add(item); //... then add the item to the room's item list
            }

        }

        public void RemoveItem(ItemData item)
        {
            _roomItems.Remove(item);
        }

        public ItemData TakeItem(string target)
        {
            target = target.ToLower();

            if (_roomItems.Count == 0)
            {
                return null;
            }

            foreach (ItemData item in _roomItems)
            {
                if (item.itemName == target) // if the item the player is trying to examine is in the room...
                {
                    return item;
                }
            }

            return null;
        }

        public string LookAround() // allows the player to look around the room and this prompts the room description again
        {
            string message = ""; // starts a message string to log information to the player
            message += "I LOOK around the " + RoomName + ".\n"; // logs the room name the player is in and the room description
            return message;
        }
        public string ExamineItem(string target)
        {
            string message = ""; // starts a message string to log information to the player
            target = target.ToLower();

            if (_roomItems.Count == 0)
            {
                message += "I don't see anything worth examining."; //... log that there are no items
                return message;
            }

            foreach (ItemData item in _roomItems)
            {
                if (item.itemName == target) // if the item the player is trying to examine is in the room...
                {
                    message += "I take a closer look at the " + target + ".\n" + item.itemDescription; // logs the item description to the player
                    return message;
                }
            }
            
            return "I don't see anything like that here.";
        }
    }
}