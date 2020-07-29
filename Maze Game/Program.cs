using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Game
{
    public enum Direction { North, South, East, West }
    class Program
    {
        static void Main(string[] args)
        {
            Settings settings = Settings.GetConfiguration();
            if (settings.Messages != null)
            {
                Console.WriteLine("There is something wrong with the Configuration File, here are the details:");
                foreach (var message in settings.Messages)
                {
                    Console.WriteLine($"{message}");
                }
                Console.Read();
                return;
            }

            Items items = Items.GetItems();
            if (items.Messages != null)
            {
                Console.WriteLine("There is something wrong with the Items File, here are the details:");
                foreach (var message in items.Messages)
                {
                    Console.WriteLine($"{message}");
                }
                Console.Read();
                return;
            }

            // List of actions to give to the user when ecountering Threat
            List<Action> actions = new List<Action>();
            for (int i = 0; i < (items.Threats.Count < 10 ? items.Threats.Count : 10); i++)
            {
                actions.Add(items.Threats[i].Action);
            }


            //create emtpy list of rooms
            List<Room> maze = new List<Room>();
            maze = SetMaze(settings, maze, items);

            #region Test Maze Structure and Items Region
            /*
            foreach (var room in maze)
            {
                Console.WriteLine("Room:");
                Console.WriteLine($"ID: {room.ID} - Passages: {room.NumberOfPassages} - Max: {room.MaxNumberOfPassages}");
                if (room.Treasure != null)
                {
                    Console.WriteLine("Treasure:");
                    Console.WriteLine($"Name: {room.Treasure.Name} - Gain Wealth: {room.Treasure.GainWealth}");
                }
                else if (room.Threat != null)
                {
                    Console.WriteLine("Threat:");
                    Console.WriteLine($"Name: {room.Threat.Name} - Wealth Opportunity: {room.Threat.WealthOpportunity}");
                    Console.WriteLine($"Action: {room.Threat.Action.Name}");
                }
                Console.WriteLine("  Passages:");
                
                foreach (var passage in room.Passages)
                {
                    if (passage.Destination != null)
                    {
                        Console.WriteLine($"    Room ID: {passage.Destination.ID} - {passage.Direction} - IsExit: {passage.IsExit}");
                    }
                    else
                    {
                        Console.WriteLine($"    Exit - {passage.Direction} - IsExit: {passage.IsExit}");
                    }
                    
                }
                
            }
            Console.ReadLine();
            //*/
            #endregion Test Maze Structure and Items Region

            // Set player's location to a random room
            Random random = new Random();
            int randomRoom = random.Next(maze.Count);
            settings.Player.Location = maze[randomRoom];

            // Create the home screen
            string userInput = HomeScreen(settings, maze);

            if (userInput == null)
            {
                return;
            }

            bool endGame = false;
            do
            {
                DisplayPlayerInformation(settings.Player);
                DisplayRoomInformation(settings.Player, actions);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(CenterText("You may go now to a different room"));
                userInput = Console.ReadLine().ToLower();
                endGame = UserNextAction(userInput, settings.Player, maze);
            }
            while (endGame == false);
            Console.ReadLine();
            // or maybe put finished game screen here        
        }

        private static bool UserNextAction(string userInput, Player player, List<Room> maze)
        {
            string regexDeposit = @"^deposit \d+$";
            string regexWithdraw = @"^withdraw \d+$";
            bool repeat = true;
            do
            {
                if (Regex.IsMatch(userInput, regexDeposit) || Regex.IsMatch(userInput, regexWithdraw))
                {
                    repeat = DepositWithdrawWealth(player, userInput);
                }
                else if (userInput == "exit")
                {
                    return true;
                }
                else if (userInput == "north" || userInput == "south" || userInput == "west" || userInput == "east")
                {
                    repeat = NextLocation(player, userInput, maze);
                    if (player.Location == null)
                    {
                        return true;
                    }
                    if (repeat == true)
                    {
                        userInput = Console.ReadLine().ToLower();
                        continue;
                    }                    
                }

                if (repeat)
                {
                    Console.WriteLine();
                    Console.WriteLine(CenterText("You can only go in the following directions:"));
                    Console.WriteLine(CenterText("\"North\", \"South\", \"West\" and \"East\""));
                    Console.WriteLine();
                    Console.WriteLine(CenterText("Alternatively, you can leave or take Wealth by entering:"));
                    Console.WriteLine(CenterText("\"Deposit 1\" or \"Withdraw 1\""));
                    userInput = Console.ReadLine().ToLower();
                }                
            } while (repeat);
            return false;
        }

        private static bool DepositWithdrawWealth(Player player, string userInput)
        {
            string userAction = Regex.Match(userInput, @"^\w+ ").Value.Replace(" ","");
            string userInputAmout = Regex.Replace(userInput, userAction, "");
            uint userAmount;
            UInt32.TryParse(userInputAmout, out userAmount);
            if (userAction == "deposit")
            {
                if (player.Wealth >= userAmount)
                {
                    player.Wealth -= (int)userAmount;
                    player.Location.WealthLeft += userAmount;
                    return false;
                }
                else
                {
                    Console.WriteLine(CenterText("You don't have enough Wealth to perform this action."));
                    return true;
                }
            }
            else if (userAction == "withdraw")
            {
                if (player.Location.WealthLeft >= userAmount)
                {
                    player.Location.WealthLeft -= userAmount;
                    player.Wealth += (int)userAmount;
                    return false;
                }
                else
                {
                    Console.WriteLine(CenterText("You haven't left that much Wealth in this room."));
                    return true;
                }
            }
            else
            {
                Console.WriteLine(CenterText("unexpected deposit/ withdrawal error"));
                return true;
            }

        }

        private static bool NextLocation(Player player, string userInput, List<Room> maze)
        {
            // C# didn't like this field being undeclared, so I set it to North.
            Direction chosenDirection = Direction.North;

            if (userInput == "north")
            {
                chosenDirection = Direction.North;
            }
            else if (userInput == "south")
            {
                chosenDirection = Direction.South;
            }
            else if (userInput == "west")
            {
                chosenDirection = Direction.West;
            }
            else if (userInput == "east")
            {
                chosenDirection = Direction.East;
            }

            if (player.Location.Passages.Where(p => p.Direction == chosenDirection).Count() == 1)
            {
                Passage passage = player.Location.Passages.Where(p => p.Direction == chosenDirection).First();
                if (passage.IsExit == true)
                {
                    // TODO: Game finished screen
                    Console.Clear();
                    Console.WriteLine(CenterText("Well done, you have found the exit."));
                    Console.WriteLine(CenterText($"You finished the game with {player.Wealth} Wealth"));
                    uint wealthLeft = 0;
                    foreach (var room in maze)
                    {
                        wealthLeft += room.WealthLeft;
                    }

                    if (wealthLeft == 0)
                    {
                        Console.WriteLine(CenterText("Amazingly, you've finished the game without leaving any Wealth behind, Good Job"));
                    }
                    else
                    {
                        Console.WriteLine(CenterText($"Altogether you have left {wealthLeft} Wealth across all rooms"));
                    }
                    player.Location = null;
                    return false;
                }
                else
                {
                    player.Location = passage.Destination;
                    return false;
                }
            }
            else
            {
                Console.WriteLine(CenterText("You may not go that way"));
                return true;
            }
        }

        private static void DisplayRoomInformation(Player player, List<Action> actions)
        {
            if (player.Location.FirstTime)
            {
                if (player.Location.Threat != null)
                {
                    string actionNames = "";
                    foreach (var action in actions)
                    {
                        if (action == actions.Last())
                        {
                            actionNames += $"{action.Name}";
                        }
                        else
                        {
                            actionNames += $"{action.Name}, ";
                        }
                    }
                    if (actions.Count == 2)
                    {
                        actionNames = $"{actions[0].Name} or {actions[1].Name}";
                    }
                    Console.WriteLine(CenterText($"Oh no, there is a {player.Location.Threat.Name} in this room"));
                    Console.WriteLine(CenterText($"You must perform a correct action against this Threat if you want to leave this room,"));
                    Console.WriteLine(CenterText($"but be careful, if you make too many mistakes you will pay for it... literally."));
                    Console.WriteLine(CenterText($"Here is a list of actions:"));
                    Console.WriteLine(CenterText(actionNames));
                    uint tries = 3;
                    string userInput = "";
                    while (userInput != player.Location.Threat.Action.Name.ToLower() && tries > 0)
                    {
                        userInput = Console.ReadLine().ToLower();
                        if (userInput != player.Location.Threat.Action.Name.ToLower())
                        {
                            tries--;
                            if (tries == 1)
                            {
                                Console.WriteLine($"You only have {tries} try left");
                            }
                            else
                            {
                                Console.WriteLine($"You only have {tries} tries left");
                            }

                        }
                    }
                    if (tries == 0)
                    {
                        int wealthToTake = player.Location.Threat.WealthOpportunity / 2;
                        player.Wealth -= wealthToTake;
                        Console.Clear();
                        DisplayPlayerInformation(player);
                        Console.WriteLine(CenterText($"Unfortunetly, you've made too many mistakes, and the {player.Location.Threat.Name}"));
                        Console.WriteLine(CenterText($"took {wealthToTake} Wealth from you!"));
                    }
                    else
                    {
                        player.Wealth += player.Location.Threat.WealthOpportunity;
                        Console.Clear();
                        DisplayPlayerInformation(player);
                        Console.WriteLine(CenterText($"Wow, you've managed to defeat that {player.Location.Threat.Name},"));
                        Console.WriteLine(CenterText($"you also found {player.Location.Threat.WealthOpportunity} Wealth, it probably belonged to the previous challengers."));
                    }
                    player.Location.FirstTime = false;
                }
                else if (player.Location.Treasure != null)
                {
                    player.Wealth += player.Location.Treasure.GainWealth;
                    Console.Clear();
                    DisplayPlayerInformation(player);
                    Console.WriteLine(CenterText($"Wow, you found {player.Location.Treasure.Name}"));
                    Console.WriteLine(CenterText($"From my calculations, this should be worth around {player.Location.Treasure.GainWealth} Wealth"));
                    player.Location.FirstTime = false;
                }

            }
            else
            {
                Console.WriteLine(CenterText("You've already been in this room."));
                if (player.Location.Threat != null)
                {
                    Console.WriteLine(CenterText($"You can also see the traces of your battle with the {player.Location.Threat.Name}."));
                }
                else if (player.Location.Treasure != null)
                {
                    Console.WriteLine(CenterText($"You fondly remember finding {player.Location.Treasure.Name} in this room."));
                }
            }
        }

        private static void DisplayPlayerInformation(Player player)
        {
            Console.Clear();
            Console.WriteLine($"Player: {player.Name}");
            Console.WriteLine($"Current Wealth: {player.Wealth}");
            Console.WriteLine($"Wealth You left in this room: {player.Location.WealthLeft}");
            Console.WriteLine();
            Console.WriteLine();
            //Console.WriteLine($"{player.Location.ID}");
        }

        private static string HomeScreen(Settings settings, List<Room> maze)
        {
            Console.Clear();
            Console.WriteLine(CenterText("Welcome to the Maze Game"));
            Console.WriteLine(CenterText("by Olde Worlde Phunne gaming studio"));

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(CenterText($"You set your Player's name to {settings.Player.Name}"));
            Console.WriteLine(CenterText($"This maze has {maze.Count} rooms"));
            Console.WriteLine(CenterText($"Your starting Wealth is 0"));
            Console.WriteLine();
            Console.WriteLine(CenterText("Enter \"Continue\" in order to start a new game."));
            Console.WriteLine();
            Console.WriteLine(CenterText("Remember you can also leave some of your Wealth in a room by entering, for example, \"Deposit 5\","));
            Console.WriteLine(CenterText("the amount of Wealth left will be displayed in the top left corner."));
            Console.WriteLine(CenterText("If you want to take the money that you left in the room, just type in \"Withdraw 5\""));
            Console.WriteLine();

            string userInput = Console.ReadLine();
            while (userInput.ToLower() != "continue")
            {
                if (userInput.ToLower() == "exit")
                {
                    return null;
                }
                Console.WriteLine("Please enter \"Continue\" in order to start the game, otherwise please enter \"Exit\" to exit the program.");
                userInput = Console.ReadLine();
            }
            Console.Beep();
            Console.Clear();
            return userInput;
        }

        private static string CenterText(string welcomeText)
        {
            return string.Format("{0," + ((Console.WindowWidth / 2) + (welcomeText.Length / 2)) + "}", welcomeText);
        }

        private static List<Room> SetMaze(Settings settings, List<Room> rooms, Items items)
        {
            rooms = SetRooms(settings, rooms, items);

            if (rooms == null)
            {
                return null;
            }

            rooms = SetPassages(settings, rooms);

            return rooms;
        }

        private static List<Room> SetRooms(Settings settings, List<Room> rooms, Items items)
        {
            Random random = new Random();
            //loop for creating rooms and adding them to the rooms list
            for (int i = 0; i < settings.NumberOfRooms; i++)
            {
                Items item = SetItemInTheRoom(random.Next(2), random, items);
                Treasure treasure = item.Treasures.FirstOrDefault();
                Threat threat = item.Threats.FirstOrDefault();
                rooms.Add(new Room
                {
                    ID = i,
                    WealthLeft = 0,
                    NumberOfPassages = 0,
                    MaxNumberOfPassages = (uint)random.Next(2, 5),
                    NotAvaiableDirections = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList(),
                    AvaiableDirections = new List<Direction>(),
                    Passages = new List<Passage>(),
                    Treasure = treasure,
                    Threat = threat,
                    FirstTime = true
                });
            }
            return rooms;
        }

        private static Items SetItemInTheRoom(int randomValue, Random random, Items items)
        {
            Items item = new Items();
            bool isTreasure = randomValue == 1 ? true : false;
            if (isTreasure)
            {
                var randomIndex = random.Next(items.Treasures.Count);
                item.Treasures = new List<Treasure>();
                item.Treasures.Add(items.Treasures[randomIndex]);
                item.Threats = new List<Threat>();
            }
            else
            {
                var randomIndex = random.Next(items.Threats.Count);
                item.Threats = new List<Threat>();
                item.Threats.Add(items.Threats[randomIndex]);
                item.Treasures = new List<Treasure>();
            }
            return item;
        }

        private static List<Room> SetPassages(Settings settings, List<Room> rooms)
        {
            bool exitExists = false;
            Random random = new Random();

            foreach (var room in rooms)
            {
                List<Room> roomsCopy = rooms.ToList();
                //removing the posiblity of passage loop
                roomsCopy.Remove(room);

                //removing the posibility of getting a room without avaiable direcitons
                while (room.NumberOfPassages < room.MaxNumberOfPassages && room.NotAvaiableDirections.Count > 0)
                {
                    roomsCopy = roomsCopy.FindAll(r => r.NotAvaiableDirections.Count > 0);
                    roomsCopy = roomsCopy.FindAll(r => r.NumberOfPassages < 1);
                    //when all of the rooms have at least 1 passage, do this
                    if (roomsCopy.Count == 0)
                    {
                        roomsCopy = rooms.ToList().FindAll(r => r.NotAvaiableDirections.Count > 0);
                        roomsCopy = roomsCopy.FindAll(r => r.NumberOfPassages < r.MaxNumberOfPassages);
                        roomsCopy.Remove(room);
                        // if all of the rooms reached their maxNumberOfPassages and there are still
                        // passages to be created, ignore that limit
                        if (roomsCopy.Count == 0)
                        {
                            roomsCopy = rooms.ToList().FindAll(r => r.NotAvaiableDirections.Count > 0);
                            roomsCopy.Remove(room);

                            if (roomsCopy.Count == 0)
                            {
                                break;
                            }
                        }
                    }
                    //get random number for roomsCopy List
                    int roomCopyIndex = random.Next(roomsCopy.Count);
                    Room roomCopy = roomsCopy[roomCopyIndex];
                    Room destinationRoom = rooms[roomCopy.ID];
                    //get random number for AvaiableDirecitons list
                    int roomDirectionsIndex = random.Next(room.NotAvaiableDirections.Count);
                    if (exitExists == false)
                    {
                        room.Passages.Add(new Passage
                        {
                            IsExit = true,
                            Direction = room.NotAvaiableDirections[roomDirectionsIndex]
                        });
                        room.NotAvaiableDirections.RemoveAt(roomDirectionsIndex);
                        room.NumberOfPassages += 1;
                        exitExists = true;
                    }
                    else
                    {
                        room.Passages.Add(new Passage
                        {
                            IsExit = false,
                            Destination = destinationRoom,
                            Direction = room.NotAvaiableDirections[roomDirectionsIndex]
                        });
                        room.AvaiableDirections.Add(room.NotAvaiableDirections[roomDirectionsIndex]);
                        room.NotAvaiableDirections.RemoveAt(roomDirectionsIndex);
                        room.NumberOfPassages += 1;

                        int destinationRoomDirectionsIndex = random.Next(destinationRoom.NotAvaiableDirections.Count);
                        destinationRoom.Passages.Add(new Passage
                        {
                            IsExit = false,
                            Destination = room,
                            Direction = destinationRoom.NotAvaiableDirections[destinationRoomDirectionsIndex]
                        });
                        destinationRoom.AvaiableDirections.Add(destinationRoom.NotAvaiableDirections[destinationRoomDirectionsIndex]);
                        destinationRoom.NotAvaiableDirections.RemoveAt(destinationRoomDirectionsIndex);
                        destinationRoom.NumberOfPassages += 1;
                    }
                }
            }

            return rooms;
        }
    }
}
