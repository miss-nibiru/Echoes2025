using System;
using System.Collections.Generic;
using UnityEngine;
using Commodore;
using RoomInfo;
using Items;
using Unity.VisualScripting;
using static Items.AllItems;

public class TestCommodore : CommodoreBehavior
{
    private bool _gameStarted = false;
    private Room _currentRoom;
    private ItemManager _items;
    private bool frontDoorLocked = true;
    private bool studyDoorLocked = true;
    private bool bedroomDoorLocked = true;
    private bool drainOpened = false;
    private bool patchDug = false;
    private List<Room> _rooms = new List<Room>();
    private Dictionary<string, ItemData> _playerInventory = new Dictionary<string, ItemData>();

    // declare all rooms as fields
    private Room hallway;
    private Room study;
    private Room guestroom;
    private Room masterbedroom;
    private Room bathroom;
    private Room garden;
    private Room foyer;
    private Room kitchen;
    private Room stairs;
    private Room livingroom;

    // constructor: create rooms and wire the map
    public TestCommodore()
    {
        hallway = new Room("HALLWAY.", "This HALLWAY seems to lead to several rooms. There is a rustic looking door with a diamond symbol to the WEST, a plain white door to the NORTH, and deep down to the EAST I see the BEDROOM door with heart motifs all over. I could go back SOUTH to the FOYER.");
        study = new Room("STUDY.", "This room is filled with old books and a sturdy desk. Dust covers most surfaces. The only exit is back SOUTH to the HALLWAY. Maybe I should take a closer look?");
        guestroom = new Room("GUEST ROOM.", "What a dirty old GUESTROOM. Seems to have been more of a storage room. There is a bed against the wall and a small window letting in some light. The exit is back SOUTH to the hallway. Should I Look around?");
        masterbedroom = new Room("BEDROOM", "This is a large bedroom with a grand bed and antique furniture. The room has an eerie feel to it. Behind me there's the door leading SOUTH back to the HALLWAY and another door to the NORTH that seems to lead to a BATHROOM... Something feels off here...");
        bathroom = new Room("BATHROOM.", "A small, dimly lit BATHROOM with cracked tiles and a rusty bathtub. The DRAIN looks a bit loose. The only exit is back SOUTH to the BEDROOM. Should I Look around?");
        garden = new Room("GARDEN.", "The overgrown GARDEN is filled with tangled plants. One PATCH of soil looks disturbed, unlike the rest of the garden. No one has been here in a while. The air is thick with the scent of damp earth. To the NORTH is the locked FRONT DOOR. It has an interesting spade-like symbol on it.");
        foyer = new Room("FOYER.", "The old house is dark - the smell of musk is in the air. The front door is behind me to the SOUTH. I see the KITCHEN is to the EAST, the STAIRS lead up to the second floor NORTH, and an old-looking LIVING ROOM is to the WEST.");
        kitchen = new Room("KITCHEN.", "The KITCHEN is cluttered with dirty dishes and old appliances. A chair is laying in the middle of the floor. I can go WEST back to the FOYER. There's something interesting here...");
        stairs = new Room("STAIRS.", "The wooden STAIRS creak underfoot as I step on them. The banister is worn. I can go back down SOUTH to the FOYER or up NORTH to the dark HALLWAY.");
        livingroom = new Room("LIVING ROOM.", "The LIVING ROOM is filled with faded furniture and a cold fireplace. Dusty curtains hang over the windows. The only exit is back EAST to the FOYER. Should I Look around?");

        // All rooms need a neighbour assigned to them so movement is possible

        garden.AddRoom(RoomDirection.North, foyer);
        foyer.AddRoom(RoomDirection.South, garden);

        kitchen.AddRoom(RoomDirection.West, foyer);
        foyer.AddRoom(RoomDirection.East, kitchen);

        stairs.AddRoom(RoomDirection.South, foyer);
        foyer.AddRoom(RoomDirection.North, stairs);

        foyer.AddRoom(RoomDirection.West, livingroom);
        livingroom.AddRoom(RoomDirection.East, foyer);

        stairs.AddRoom(RoomDirection.North, hallway);
        hallway.AddRoom(RoomDirection.South, stairs);

        hallway.AddRoom(RoomDirection.West, study);
        study.AddRoom(RoomDirection.South, hallway);

        hallway.AddRoom(RoomDirection.North, guestroom);
        guestroom.AddRoom(RoomDirection.South, hallway);

        hallway.AddRoom(RoomDirection.East, masterbedroom);
        masterbedroom.AddRoom(RoomDirection.South, hallway);

        masterbedroom.AddRoom(RoomDirection.North, bathroom);
        bathroom.AddRoom(RoomDirection.South, masterbedroom);

        // Add rooms to list and log
        _rooms.Add(garden);
        _rooms.Add(foyer);
        _rooms.Add(stairs);
        _rooms.Add(kitchen);
        _rooms.Add(livingroom);
        _rooms.Add(hallway);
        _rooms.Add(study);
        _rooms.Add(guestroom);
        _rooms.Add(masterbedroom);
        _rooms.Add(bathroom);

        foreach (Room room in _rooms)
        {
            Debug.Log($"The room name is {room.RoomName} and its description is {room.RoomDescription}");
            NeighbourRoom(RoomDirection.North, room);
            NeighbourRoom(RoomDirection.South, room);
            NeighbourRoom(RoomDirection.East, room);
            NeighbourRoom(RoomDirection.West, room);
            Debug.Log("**************");
        }

        void NeighbourRoom(RoomDirection direction, Room currentRoom)
        {
            Room neighbourRoom = currentRoom.GetNeighbour(direction);
            if (neighbourRoom != null)
            {
                Debug.Log($"The neighbour room is {neighbourRoom.RoomName}");
                Debug.Log("STEP 1 END -- ROOMS CREATED AND MAP WIRED");
            }
        }

        _items = new ItemManager(); // STEP 2 --- ITEM MANAGER CREATED
        {
            Debug.Log("STEP 2 - Item Manager Created");
        }
        _items.InitializeItems(); // STEP 3 - INITIALIZE ITEMS TO ADD TO ROOMS
        {
            Debug.Log("STEP 3 - Items Initialized. Total items: " + _items._allItems.Count);
        }

        AddRoomItems(); // STEP 4 - ADD ITEMS TO ROOMS
        {
            Debug.Log("STEP 4 - Items added to world: " + _items._allItems.Count);
        }

        _currentRoom = garden; // PLAYER STARTING ROOM
    }

    public string DirectionDescription(RoomDirection direction) // Sean set this up - converts directions enums at the top to string that can be outputted to the console?
    {
        switch (direction)
        {
            case RoomDirection.North: return "North";
            case RoomDirection.South: return "South";
            case RoomDirection.East: return "East";
            case RoomDirection.West: return "West";
            default: return "NONE";
        }
        //STEP 1 END -- ROOMS CREATED AND MAP WIRED
    }

    private void AddRoomItems() // assigns items to rooms
    {

        //all items in the kitchen
        kitchen.AddItem(_items._allItems[Bloodyknife]);
        kitchen.AddItem(_items._allItems[Cigarettebutt]);
        kitchen.AddItem(_items._allItems[Oldrope]);
        kitchen.AddItem(_items._allItems[Studykey]);

        //all items in the garden
        garden.AddItem(_items._allItems[Housekey]);
        //garden.AddItem(_items._allItems[Gardenpatch]);

        //all items in the living room
        livingroom.AddItem(_items._allItems[Brokenpendant]);

        //all items in the study
        study.AddItem(_items._allItems[Torndiarypage]);
        study.AddItem(_items._allItems[Screwdriver]);
        study.AddItem(_items._allItems[Masterbedroomkey]);

        //all items in the master bedroom
        masterbedroom.AddItem(_items._allItems[Weddingphotograph]);
        masterbedroom.AddItem(_items._allItems[Prescriptionbottle]);

        //all items in the guest room
        guestroom.AddItem(_items._allItems[Rustyhammer]);
        guestroom.AddItem(_items._allItems[Shovel]);

        Debug.Log("Items assigned to rooms. Total items in kitchen " + kitchen._roomItems.Count);
        Debug.Log("Items assigned to rooms. Total items in garden " + garden._roomItems.Count);
        Debug.Log("Items assigned to rooms. Total items in living room " + livingroom._roomItems.Count);
        Debug.Log("Items assigned to rooms. Total items in study " + study._roomItems.Count);
        Debug.Log("Items assigned to rooms. Total items in master bedroom " + masterbedroom._roomItems.Count);
        Debug.Log("Items assigned to rooms. Total items in guest room " + guestroom._roomItems.Count);
        Debug.Log("Items assigned to rooms. Total items in bathroom " + bathroom._roomItems.Count);

        //STEP 4 -- ASSIGN ITEMS TO ROOMS
    }

    // Match the base class signature and make it public
    protected override string ProcessCommand(string command) // all the needed commands are here
    { 
        Debug.Log("[Before Command] CurrentRoom = " + (_currentRoom != null ? _currentRoom.RoomName : "NULL"));
        if (string.IsNullOrWhiteSpace(command))
            return "What should I do now?";
        
        command = command.Trim();
        string trimmed = command.Trim();
        
        if (command.StartsWith("START GAME", StringComparison.OrdinalIgnoreCase)) // starts the game
        {
            ResetGame();
            _gameStarted = true;
            string intro = "The house has been abandoned for years.\nThe neighbours still talk about the night the Riveras vanished. Screams, then silence. No bodies. No answers.A married couple disappears after a domestic disturbance.\nThe police did one sweep and walked away. Everyone did. As if the walls themselves wanted the story buried.\nTonight, I’ve been granted one final chance to reconstruct what happened here. If answers are anywhere, they’re inside this house. And the house is still listening.\n";
            return intro + _currentRoom.RoomDescription + "I should check my DICTIONARY.";
        }
        
        if (command.StartsWith("END GAME", StringComparison.OrdinalIgnoreCase))
        {
            _gameStarted = false;
            return "I have decided to quit my investigation. If I want to start another investigation, I should type 'START GAME'.";
        }
        
        if (!_gameStarted)
        {
                return "The investigation hasn't started yet. I should type 'START GAME' to begin.";
        }
        
        if (command.StartsWith("GO", StringComparison.OrdinalIgnoreCase))
        {
            string directionText = command.Length > 2 ? command.Substring(2).Trim() : string.Empty;
            if (string.IsNullOrEmpty(directionText))
                return "I should choose a direction. I can go North, South, East, West";

            if (Enum.TryParse<RoomDirection>(directionText, true, out RoomDirection dir))
            {
                Debug.Log("[GO] starting from: " + _currentRoom.RoomName);

                Room neighbourRoom = _currentRoom.GetNeighbour(dir);
                if (neighbourRoom == null)
                {
                    return "I can't go that way.";
                }
                
                // Just 3 locked doors in the house:
                if (_currentRoom == garden && dir == RoomDirection.North)
                {
                    if (frontDoorLocked)
                    {
                        return "The FRONT DOOR is locked. I need to unlock it first.";
                    }
                    else
                    {
                        _currentRoom = foyer;
                        return "I move NORTH to the foyer. " + _currentRoom.RoomDescription;
                    }
                }
                
                if (_currentRoom == hallway && dir == RoomDirection.West)
                {
                    if (studyDoorLocked)
                    {
                        return "The STUDY DOOR is locked. I need to unlock it first.";
                    }
                    else
                    {
                        _currentRoom = study;
                        return "You move WEST to the STUDY. " + _currentRoom.RoomDescription;
                    }
                }
                
                if (_currentRoom == hallway && dir == RoomDirection.East)
                {
                    if (bedroomDoorLocked)
                    {
                        return "The BEDROOM is locked. I need to unlock it first.";
                    }
                    else
                    {
                        _currentRoom = masterbedroom;
                        return "I move EAST to the BEDROOM. " + _currentRoom.RoomDescription;
                    }
                }
                
                _currentRoom = neighbourRoom;
                return "I move " + DirectionDescription(dir) + " to the " + _currentRoom.RoomName + " " + _currentRoom.RoomDescription;
            }

            return "That is not a valid direction I can go to.";
        }

        if (command.StartsWith("EXAMINE", StringComparison.OrdinalIgnoreCase))
        {
            string target = command.Length > 8 ? command.Substring(8).Trim() : string.Empty;

            if (string.IsNullOrEmpty(target))
            {
                return "What should I EXAMINE?";
            }
            
            if (target.Equals ("room", StringComparison.OrdinalIgnoreCase) || target.Equals (_currentRoom.RoomName.TrimEnd('.'), StringComparison.OrdinalIgnoreCase))
            {
                return "I EXAMINE the " + _currentRoom.RoomName + ": " + _currentRoom.RoomDescription;
            }
            
            return _currentRoom.ExamineItem(target);
        }

        if (command.StartsWith("LOOK", StringComparison.OrdinalIgnoreCase))
        {
            string message;
            message = _currentRoom.LookAround();
            if (_currentRoom._roomItems.Count != 0)
            {
                message += "There's a something interesting in this room... I see:\n";

                foreach (ItemData item in _currentRoom._roomItems)
                {
                    message += item.itemName + "\n";
                }
            }
            
            if (_currentRoom._roomItems.Count == 0)
            {
                message += "There's nothing interesting in this room";
            }

            return message;
        }

        if (command.StartsWith("TAKE", StringComparison.OrdinalIgnoreCase))
        {
            string target = command.Length > 5 ? command.Substring(5).Trim() : string.Empty;
            if (string.IsNullOrEmpty(target))
            {
                return "What should I take?";
            }

            ItemData foundItem = _currentRoom.TakeItem(target);
            if (foundItem != null)
            {
                if (foundItem.itemType == ItemType.Usable)
                {
                    //if item, they do take it
                    _playerInventory.Add(foundItem.itemName, foundItem);
                    _currentRoom.RemoveItem(foundItem);
                    return "I now have a(n) " + foundItem.itemName + " in my inventory.";
                }
                else
                {
                    //if clue, do they record it?
                    return "I have to be very careful when recording my clues... Would this be one? Should I Record this?";
                }
            }
            else
            {
                return "There is no " + target + " here to take.";
            }
        }

        if (command.StartsWith("INVENTORY", StringComparison.OrdinalIgnoreCase))
        {
            string message = "";
            if (_playerInventory.Count != 0)
            {
                message += "ITEMS:\n";

                foreach (ItemData item in _playerInventory.Values)
                {
                    message += item.itemName + "\n";
                }
            }
            else
            {
                message = "I have nothing in my inventory.";
            }

            return message;
        }

        if (command.StartsWith("RECORD", StringComparison.OrdinalIgnoreCase))
        {
            string message = "";
            string target = command.Length > 7 ? command.Substring(7).Trim() : string.Empty;
            target = target.ToLower();
            int currentClues = _items.CurrentCorrectClues + _items.CurrentWrongClues;

            if (string.IsNullOrEmpty(target))
            {
                message = "What do you want to RECORD?";
            }

            ItemData foundItem = _currentRoom.TakeItem(target);
            if (foundItem != null)
            {
                if (foundItem.itemType == ItemType.CorrectClue)
                {
                    _items.CurrentCorrectClues += 1; // add to teh correct clues
                    _currentRoom.RemoveItem(foundItem); //remove hte clue from the room, so it cant be used again
                    currentClues++;
                    message = "I have recorded " + foundItem.itemName + " \nI Now have " + currentClues + " clues out of 6.";
                }
                else if (foundItem.itemType == ItemType.IncorrectClue)
                {
                    _items.CurrentWrongClues += 1;
                    _currentRoom.RemoveItem(foundItem);
                    currentClues++;
                    message = "I have recorded " + foundItem.itemName + " \nI Now have " + currentClues + " clues out of 6.";
                }
                else
                {
                    // if the item is a usable item
                    message = "This is not a clue I can RECORD.";
                }
            }
            else
            {
                message = "There is no " + target + " here to record.";
            }

            //if they have 6 clues, do end game stuff
            if (currentClues >= 6)
            {
                message += "I have recorded enough clues to make an accusation. I should type 'REPORT' to see the results of my investigation.";
            }

            return message;
        }

        if (command.StartsWith("USE", StringComparison.OrdinalIgnoreCase)) // OMG WHAT A COMPLICATED THING, WHY DID I THINK THIS WAS A GOOD IDEA!!!
        {
            string fullCommand = command.Length > 4 ? command.Substring(4).Trim() : string.Empty;
            fullCommand = fullCommand.ToLower();
            string itemName = fullCommand;
            string targetName = string.Empty;
            int onIndex = fullCommand.IndexOf(" on ", StringComparison.OrdinalIgnoreCase);

            if (onIndex >= 0)
            {
                itemName = fullCommand.Substring(0, onIndex);
                targetName = fullCommand.Substring(onIndex + 4);
            }

            if (string.IsNullOrEmpty(fullCommand))
            {
                return "What should I USE?";
            }

            if (_playerInventory.Count == 0)
            {
                return "I have no items to USE.";
            }

            bool foundItem = false; // flag to check if item is in inventory
            //bool doorLocked = true; // assume the door is locked initially

            foreach (ItemData item in _playerInventory.Values)
            {
                if (item.itemName == itemName)
                {
                    foundItem = true;
                }
            }
            
            if (!foundItem)
            {
                return "I don't have that item in my inventory.";
            }

            if (itemName == "spade key") // usable item -- used on the front door.
            {
                
                if (_currentRoom != garden)
                {
                    return "You can't use the SPADE KEY here.";
                }
                
                if (targetName != "front door")
                {
                    return "try using the SPADE KEY on the FRONT DOOR.";
                }
                
                if (targetName == "front door" && !frontDoorLocked)
                {
                    return "The FRONT DOOR is already unlocked.";
                }
                
                if (targetName == "front door")
                {
                    frontDoorLocked = false;
                    return "You use the SPADE KEY to unlock the FRONT DOOR.";
                }
                
            }
            
            else if (itemName == "diamond key") // usable item -- used on the study door.
            {
                if (_currentRoom != hallway)
                {
                    return "You can't use the DIAMOND KEY here.";
                }
                
                if (targetName != "study door")
                {
                    return "try using the DIAMOND KEY on a locked door.";
                }
                
                if (targetName == "study door" && !studyDoorLocked)
                {
                    return "The STUDY DOOR is already unlocked.";
                }
                
                if (targetName == "study door")
                {
                    studyDoorLocked = false;
                    return "You use the DIAMOND KEY to unlock the STUDY DOOR.";
                }
            }
            
            else if (itemName == "heart key") // usable item -- used on the master bedroom door.
            {
                if (_currentRoom != hallway)
                {
                    return "You can't use the HEART KEY here.";
                }
                
                if (targetName != "bedroom door")
                {
                    return "try using the HEART KEY on the BEDROOM DOOR.";
                }
                
                if (targetName == "bedroom door" && !bedroomDoorLocked)
                {
                    return "The BEDROOM DOOR is already unlocked.";
                }
                
                if (targetName == "bedroom door")
                {
                    bedroomDoorLocked = false;
                    return "You use the HEART KEY to unlock the BEDROOM DOOR.";
                }
            }
            else if (itemName == "shovel") // usable item -- used on the garden patch.
            {
                if (_currentRoom != garden)
                {
                    return "You can't use the SHOVEL here.";
                }
                
                if (targetName != "patch")
                {
                    return "try using the SHOVEL on the PATCH.";
                }
                
                if (targetName == "patch" && patchDug)
                {
                    return "There's nothing else to find here.";
                }
                
                if (targetName == "patch")
                {
                    patchDug = true;
                    var bodyItem = _items._allItems[AllItems.body];
                    garden.AddItem(bodyItem); //create the item now just to avoid having to code more stuff hehehe
                    return "There is a " + bodyItem.itemName + "? " + bodyItem.itemDescription + " Should I RECORD this?";
                }
            }
            else if (itemName == "screwdriver") // usable item -- used in the drain in the bathroom.
            {
                if (_currentRoom != bathroom)
                {
                    return "I can't use the SCREWDRIVER here.";
                }
                
                if (targetName != "drain")
                {
                    return "I should try using the SCREWDRIVER on the DRAIN.";
                }
                
                if (targetName == "drain" && drainOpened)
                {
                    return "There's nothing else to find here.";
                }
                
                if (targetName == "drain")
                {
                    drainOpened = true;
                    var pendantItem = _items._allItems[AllItems.Pendant];
                    bathroom.AddItem(pendantItem); //create the item now just to avoid having to code more stuff hehehe
                    return "The screws come off. A " + pendantItem.itemName + "? " + pendantItem.itemDescription + " Could this be a clue? Should I RECORD it?";
                }
            }
            
            if (!string.IsNullOrEmpty(targetName))
            {
                return $"I can't use the {itemName} on the {targetName}.";
            }
            else
            {
                return $"I can't use the {itemName} here.";
            }
            
        }

        if (command.StartsWith("SECRET", StringComparison.OrdinalIgnoreCase)) // JUST FOR DEBUGGIN, NOTHING TO DO WITH THE GAME UNLESS YOU ARE A CHEATER
        {
            var parts = command.Split(' ');

            if (parts.Length >= 3)
            {
                if (parts[1].Equals("CORRECT", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(parts[2], out int amount))
                    {
                        _items.CurrentCorrectClues = amount;
                        return "Secret override: correct clues now " + amount;
                    }
                }

                if (parts[1].Equals("WRONG", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(parts[2], out int amount))
                    {
                        _items.CurrentWrongClues = amount;
                        return "Secret override: wrong clues now " + amount;
                    }
                }
            }

            return "Secret syntax:\nSECRET CORRECT <number>\nSECRET WRONG <number>";
        } 

        if (command == "DICTIONARY") // THE GAME HAS A DICTIONARY TO HELP WITH PROGRESSION AND SOFT BLOCK
        {
            return "GO - If I want to move NORTH/SOUTH/EAST/WEST\n" +
                   "LOOK - I want to look at the room\n" +
                   "EXAMINE - I need to examine objects to know what they do\n" +
                   "TAKE - To pick up an object\n" +
                   "INVENTORY - To show my inventory\n" +
                   "USE - To use an object on a target\n" +
                   "RECORD - To Record one out of 6 clues\n" +
                   "DICTIONARY - To show me this list";
        }
        
        if (command.StartsWith("REPORT", StringComparison.OrdinalIgnoreCase))
        {
            int correct = _items.CurrentCorrectClues;
            int wrong = _items.CurrentWrongClues;
            int total = correct + wrong;
            string endMessage = "";
            
            if (total < 6)
            {
                return "I haven't recorded enough clues to make an accusation yet. I need to RECORD 6 clues before I can REPORT my findings.";
            }
            
            if (correct <= 3)
            {
                return "I got " + _items.CurrentCorrectClues + " correct clues out of 6.\nI walk through every room again, hoping the walls will whisper something I missed—but nothing fits.\nI found fragments, hints of fear and conflict, but not enough to form a story. The Riveras slipped away and with only this much, the truth slips with them.\nTonight, the house keeps its silence.And so does the case.";
            }
            
            else if (correct <= 5)
            {
                return "I got " + _items.CurrentCorrectClues + " correct clues out of 6.\nPieces of the night begin to align: a struggle, a betrayal. I can feel the pattern beneath the dust—filled building, tempers breaking, someone trying to escape.\nBut too much is missing, and with gaps this wide, truth becomes guesswork.\nThe Riveras’ story is here. I just didn’t dig deep enough to uncover all of it.";
            }
            
            else if (correct == 6)
            {
                return "I got " + _items.CurrentCorrectClues + " correct clues out of 6.\nNow the whole picture emerges. Francisco blind in obsession, bound Roberta in panic, and Santiago’s rage ignited the moment he saw her tied and terrified.\nThe fight spilled through the house—into the bathroom, and only one brother walked out.\nBefore sunrise, the Riveras hid the body in the garden and vanished from this place forever.\nAt last, the house stops whispering. The truth is clear.";
            }

            _gameStarted = false;
            return endMessage + "\nThe investigation has ended. If I want to start another investigation, I should type 'START GAME'.";
        }

        return "Unknown command.";
            }

    private void ResetGame()
    {
        _currentRoom = garden;
        frontDoorLocked = true;
        studyDoorLocked = true;
        bedroomDoorLocked = true;
        drainOpened = false;
        patchDug = false;
        _items.CurrentCorrectClues = 0;
        _items.CurrentWrongClues = 0;
        _playerInventory.Clear();
    }
    
}
