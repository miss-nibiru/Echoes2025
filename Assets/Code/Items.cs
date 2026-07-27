using System.Collections.Generic;
using UnityEngine;

namespace Items
{ 
    public enum ItemType { Usable, CorrectClue, IncorrectClue }
    public enum AllItems
    {
        Housekey, // usable item — found in the garden; unlocks and enters the house into the foyer.
        Studykey, // usable item — found in the kitchen; unlocks the study door upstairs.
        Masterbedroomkey, // usable item — found in the study; unlocks the main bedroom upstairs.
        Screwdriver, // usable item — found in the study; used to open the bathtub drain panel revealing a hidden pendant clue.
        Bloodyknife, // correct clue — found in the kitchen; confirms a violent act took place, consistent with santiago’s attack on francisco.
        Oldrope, // correct clue — found in the kitchen; used to tie someone to the chair.
        Torndiarypage, // correct clue — found in the study; written by roberta, naming santiago and francisco, revealing their relationship and that she suspects francisco is very lonely.
        Weddingphotograph, // correct clue — found in the main bedroom; inscription reads “roberta & santiago, 2012.” francisco is faintly visible in the background, staring angrily.
        Pendant, // correct clue — found in the bathroom drain, revealed with screwdriver; shows the wedding photo with santiago’s face scratched off, engraved “must go.”
        Brokenpendant, // incorrect clue — found in the garden; unrelated sentimental trinket that distracts the player into emotional speculation.
        Prescriptionbottle, // incorrect clue — found in the main bedroom; sleeping pills belonging to santiago, misleads toward a suicide theory.
        Rustyhammer, // incorrect clue — found in the living room; looks like a weapon but is unrelated to the murder.
        Cigarettebutt, // incorrect clue — found on the kitchen table; faint red lipstick mark, false implication of another visitor.
        Shovel, // usable item — used on the garden patch.
        //Gardenpatch, // correct clue — reveals francisco’s body; identifiable by a gold tooth and clothing remains.
        body, // correct clue - this is francisco's body
    }
    public class ItemData
    {
        public ItemData(AllItems itemId, ItemType itemType, string itemRoom, string itemName, string itemDescription)
        {
            this.itemId = itemId;
            this.itemType = itemType;
            this.itemName = itemName;
            this.itemRoom = itemRoom;
            this.itemDescription = itemDescription;
        }

        public AllItems itemId { get; }
        public ItemType itemType { get; }
        public string itemRoom { get; }
        public string itemName { get; }
        public string itemDescription { get; }
        
    }

    public class ItemManager
    {
        public bool CorrectClue = true;
        public bool UsableItem = true;
        public int NumberCorrectClues = 6;
        public int NumberWrongClues = 4;
        public int NumberUsableItems = 5;
        public int CurrentCorrectClues = 0;
        public int CurrentWrongClues = 0;

        // stores all item definitions
                 public Dictionary<AllItems, ItemData> _allItems = new Dictionary<AllItems, ItemData>();

        public void InitializeItems()
        { 
            Debug.Log("Initializing Items");
            _allItems.Clear();
            {
                _allItems.Add(AllItems.Housekey, new ItemData (AllItems.Housekey, ItemType.Usable, "garden", "spade key", "This is a very interesting key. The key's teeth are shaped like spades. Might come in handy."));
                _allItems.Add(AllItems.Studykey, new ItemData (AllItems.Studykey, ItemType.Usable, "kitchen", "diamond key", "A small silver key. The key's teeth are shaped like diamonds. I wonder what it unlocks..."));
                _allItems.Add(AllItems.Masterbedroomkey, new ItemData (AllItems.Masterbedroomkey, ItemType.Usable, "study", "heart key", "A golden key with heart-shaped teeth. Seems important."));
                _allItems.Add(AllItems.Screwdriver, new ItemData (AllItems.Screwdriver, ItemType.Usable, "study", "screwdriver", "I could use this to unscrew something."));
                _allItems.Add(AllItems.Bloodyknife, new ItemData (AllItems.Bloodyknife, ItemType.CorrectClue, "kitchen", "knife", "This looks old and rusted... the dark marks on it are interesting. I wonder if it was used to attack someone."));
                _allItems.Add(AllItems.Oldrope, new ItemData (AllItems.Oldrope, ItemType.CorrectClue, "kitchen", "rope", "A thick old rope in the middle of the kitchen floor? Weird place to leave it lying around. Could have been used to tie someone up?"));
                _allItems.Add(AllItems.Torndiarypage, new ItemData (AllItems.Torndiarypage, ItemType.CorrectClue, "study", "diary page", "It reads 'Ever since the wedding, the tension between Santiago and Francisco has grown unbearable. I hope the brothers can find peace soon. Francisco seems so lonely lately...'"));
                _allItems.Add(AllItems.Weddingphotograph, new ItemData (AllItems.Weddingphotograph, ItemType.CorrectClue, "Master Bedroom", "photo", "Beautiful couple. In the back of the photo is written: 'Roberta & Santiago, 2012'. There is a faintly visible male figure in the background. Seems to be staring angrily at the couple. There's something shiny in his mouth."));
                _allItems.Add(AllItems.Pendant, new ItemData (AllItems.Pendant, ItemType.CorrectClue, "bathroom", "pendant", "Why would this pendant be hidden in the bathtub drain? The pendant shows a wedding photo, but the groom's face is scratched off. On the back is engraved 'MUST GO'."));
                _allItems.Add(AllItems.Brokenpendant, new ItemData (AllItems.Brokenpendant, ItemType.IncorrectClue, "livingroom", "necklace", "An old broken pendant. It looks like it used to be important to someone, but it's broken now."));
                _allItems.Add(AllItems.Prescriptionbottle, new ItemData (AllItems.Prescriptionbottle, ItemType.IncorrectClue, "Master Bedroom", "pill bottle", "A prescription bottle? Seem to be sleeping pills. The dosage is high... Almost dangerously so..."));
                _allItems.Add(AllItems.Rustyhammer, new ItemData (AllItems.Rustyhammer, ItemType.IncorrectClue, "guestroom", "hammer", "Why would a hammer be in the guestroom? Did someone placed it here on purpose?"));
                _allItems.Add(AllItems.Cigarettebutt, new ItemData (AllItems.Cigarettebutt, ItemType.IncorrectClue, "kitchen", "cigarette", "Someone was into this stuff. Seems to have faint red lipstick on. I wonder if the wife had anything to do with it"));
                _allItems.Add(AllItems.Shovel, new ItemData (AllItems.Shovel, ItemType.Usable, "guestroom", "shovel", "What a weird spot to store a shovel... It seems to have been used but it is hard to tell"));
                //_allItems.Add(AllItems.Gardenpatch, new ItemData (AllItems.Gardenpatch, ItemType.CorrectClue, "garden", "patch", "This part looks disturbed unlike the rest of the garden. "));
                _allItems.Add(AllItems.body, new ItemData (AllItems.body, ItemType.CorrectClue, "garden", "body", "Ugh - there is a body here. Decayed for sure... a golden tooth?"));
                
                Debug.Log("Items Loaded: " + _allItems.Count);
            }
        }
    }
}