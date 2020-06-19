using System;
using System.Collections.Generic;

namespace BattleShip {
  class Game { //The game class will be used throuout the game. it will contain the boards and the ships
    public int score = 100;
    public string name;
    public string[,] userBoard; //Users board
    public string[,] ViewComputerBoard; //This is where the user can see where they have fired and if it was a miss or a hit.
    public string[,] computerBoard; //Computers board
    public List<KeyValuePair<int, int>> computerShotRecord = new List<KeyValuePair<int, int>>();
    public bool winStatus;

    public Dictionary<string, int> ships = new Dictionary<string, int> { //Dictionary of the ships name, and their size.
        //{"Carrier", 5},                     //Commented out because makes the game too long and boring
        //{"Battleship", 4},
        //{"Cruiser", 3 },
        //{ "Submarine", 3 },
        { "Destroyer", 2 }
      };

    public Dictionary<string, int> userShipsLeft = new Dictionary<string, int> { //Dictionary of the ships name, and their size.
        //{"Carrier", 5},                       //Commented out because makes the game too long and boring
        //{"Battleship", 4},
        //{"Cruiser", 3 },
        //{ "Submarine", 3 },
        { "Destroyer", 2 }
      };
    public Dictionary<string, int> computerShipsLeft = new Dictionary<string, int> { //Dictionary of the ships name, and their size.
        //{"Carrier", 5},                       //Commented out because makes the game too long and boring
        //{"Battleship", 4},
        //{"Cruiser", 3 },
        //{ "Submarine", 3 },
        { "Destroyer", 2 }
      };

    public void Setup() { //setup
      computerShotRecord.Add(new KeyValuePair<int, int>(10, 10));
    }
    public string[,] InitBoard() { //Initialises boards. returns an empty board for the user and computer to add ships to
      string[,] board = new string[10, 10];

      for (int x = 0; x < board.GetLength(0); ++x) {  //ITERATE OVER BOTH DIMENSIONS
        for (int y = 0; y < board.GetLength(1); ++y) {
          board[x, y] = $"water [{x}, {y}]"; //Set each value of the board to water and its coordinates. helps with planning shots.
        }
      }
      return board;
    }
    public void ReadWriteFiles() {
      string path = @"C:\highscore.txt";
      if (System.IO.File.Exists(path)) {
        int highScore = Int32.Parse(System.IO.File.ReadAllText(path));
        if (score > highScore) {
          Console.WriteLine($"Nice {name}, you beat the high score!!");
          Console.ReadKey();
          Console.WriteLine($"Your score: {score}\n the old high score was: {highScore}");
          }
        } else {
          System.IO.File.WriteAllText(path, score.ToString());
        }
    }
  }

  class Program {
    static void Main() { //MAIN
      Game game = new Game(); //Instantiates game class.
      game.Setup();
      game.ViewComputerBoard = game.InitBoard();
      Console.WriteLine("Welcome to battleship!");
      Console.WriteLine("Enter your name: ");
      game.name = Console.ReadLine(); //gets input from user. Name is used to personalise messages in the game. 
      Console.WriteLine($"Ok {game.name}, its time to set up your board!");
      Console.ReadKey();
      ShipSetup(game); //Calls the function that sets up the board

      bool CheckForWin() {
        int hitCount = 0;
        int maxHits = 0;

        //foreach(int shipLength in game.userShipsLeft) {
        //  if
        //}

        foreach (KeyValuePair<string, int> ship in game.ships) {maxHits += ship.Value;}

        for (int x = 0; x < game.computerBoard.GetLength(0); ++x) {//ITERATE OVER BOTH DIMENSIONS to check if all the ships are hit
          for (int y = 0; y < game.computerBoard.GetLength(1); ++y) {
            foreach (KeyValuePair<string, int> ship in game.ships) {
              if (game.computerBoard[x, y] != ship.Key) {
                if (game.computerBoard[x, y] == $"water [{x}, {y}]") {
                } else {
                  hitCount += 1;
                  Console.WriteLine(hitCount);
                }
              } else {
                return false;
                
              }
            }
          }
        }
        if (hitCount == maxHits) {
          return true;
        } else {
          return false;
        }
      }

      static void CheckIfHit(int coord1, int coord2, Game game, bool isUser) { //checks if the shot hit the board
        string shotLanding = game.computerBoard[coord1, coord2];
        if (isUser) {
          if (shotLanding == $"water [{coord1}, {coord2}]") { //If the shot landed in water
            Console.ReadKey();
            Console.WriteLine($"Miss! You'll get it next time{game.name}");
            game.ViewComputerBoard[coord1, coord2] = "   miss   ";
          } else {
              foreach (KeyValuePair<string, int> ship in game.ships) { //for each ship 
                if (ship.Key == shotLanding) { //if the ship is == to the ship name
                Console.ReadKey();
                Console.WriteLine($"NICE HIT {game.name.ToUpper()}: " + ship.Key); //Hit message
                game.computerBoard[coord1, coord2] = "hit " + ship.Key; //changes value to hit and the ship name
                  game.ViewComputerBoard[coord1, coord2] = $"hit {ship.Key}";
                foreach (KeyValuePair<string, int> shipLength in game.computerShipsLeft) { //
                    if (ship.Key == shipLength.Key) {
                      game.computerShipsLeft[shipLength.Key] = shipLength.Value - 1; //takes 1 from the ship length left if its a hit
                      if (game.computerShipsLeft[shipLength.Key] == 0) {
                      Console.ReadKey();
                      Console.WriteLine($"{shipLength.Key} SUNK, NICE JOB {game.name}!");
                      Console.ReadKey();
                      }
                      Console.WriteLine($"You have {game.computerShipsLeft[shipLength.Key]} more squares of this ship left to hit.");
                      break;
                    }
                  }
                }
              }
            }
        } else {
          if (shotLanding == $"water [{coord1}, {coord2}]") { //If the shot landed in water
            Console.WriteLine($"Computer Missed at [{coord1}, {coord2}]");
            Console.ReadKey();
            } else {
            foreach (KeyValuePair<string, int> ship in game.ships) { //for each ship 
              if (ship.Key == shotLanding) { //if the ship is == to the ship name
                Console.WriteLine($"Uh oh {game.name}! computer hit: " + ship.Key); //Hit message
                Console.ReadKey();
                game.userBoard[coord1, coord2] = "hit " + ship.Key; //changes value to hit and the ship name
                foreach (KeyValuePair<string, int> shipLength in game.userShipsLeft) { //
                  if (ship.Key == shipLength.Key) {
                    game.userShipsLeft[shipLength.Key] = shipLength.Value - 1; //takes 1 from the ship length left if its a hit
                    if (game.userShipsLeft[shipLength.Key] == 0) {
                      Console.WriteLine($"Computer SUNK {shipLength.Key}, Get your head in the game {game.name}!!");
                      Console.ReadKey();
                      }
                    break;
                  }
                }
              }
            }
          }
        }
      }

      static void ComputerFire(Game game) {
        var random = new Random(); //Instantiates random class to generate random numbers
        KeyValuePair<int, int> coords = new KeyValuePair<int, int>(random.Next(0, 9), random.Next(0, 9)); //coords to shoot at
        foreach (KeyValuePair<int, int> usedCoord in game.computerShotRecord) { //something weird going wrong here System.InvalidOperationException: "Collection was modified; enumeration operation may not execute."
         // at System.ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion()\n at System.Collections.Generic.List`1.Enumerator.MoveNextRare()\n at BattleShip.Program.< Main > g__ComputerFire | 0_2(Game game) in / Users / bradystroud / OneDrive / Semester 3 / Programming / C# Projects/BattleShip/BattleShip/Program.cs:line 149\n   at BattleShip.Program.Main() in /Users/bradystroud/OneDrive/Semester 3/Programming/C# Projects/BattleShip/BattleShip/Program.cs:183
          if (coords.Key == usedCoord.Key && coords.Value == usedCoord.Value) {

            break;// ??????????? dont 

          } 
        }
        game.computerShotRecord.Add(new KeyValuePair<int, int>(coords.Key, coords.Value));
        CheckIfHit(coords.Key, coords.Value, game, false);
      }

      static void Fire(Game game) {
        printBoard(game.ViewComputerBoard, "this board below shows the places you have shot");
        Console.WriteLine("Enter coordinates (0-9): ");
        int coord1;
        int coord2;
        try {
          coord1 = Int32.Parse(Console.ReadLine());
          coord2 = Int32.Parse(Console.ReadLine());
          CheckIfHit(coord1, coord2, game, true);
        } catch (Exception) {
          Console.WriteLine("input error. only enter numbers < 9");
          Fire(game);
        }
      }
      while (!game.winStatus) { //This is the main while loop. most of the actual game happens in here. The loop continues until the winStatus is true, which means the user or the computer has run out of ships
        game.score -= 1; //takes one from the score. The less turns it takes to win means more points
        Console.WriteLine("Game is continuing");
        Fire(game); //Fire function
        game.winStatus = CheckForWin();
        ComputerFire(game);
        game.winStatus = CheckForWin();
      }
      Console.WriteLine("OK the game is over, would you like to play again?(y/n)"); 
      String playAgain = Console.ReadLine();
      if (playAgain == "y") {
        game.ReadWriteFiles();
        Main();
      } else {
        game.ReadWriteFiles();
        Console.WriteLine("Thanks for playing!");
        Environment.Exit(0); //Exit terminal

        }
      //Fire(game);

    }
    

    static void ShipSetup(Game game) {

    
      game.userBoard = game.InitBoard();
      
      ComputerShipSetup(game);

      //                          Coordinates                                   board to add ships to       ship name, length       horizontal or vertical         
      bool AddShips(int startCoord1, int startCoord2, int endCoord1, int endCoord2, string[,] board, KeyValuePair<string, int> ship, bool direction) { //Function that adds the ships to the board. returns bool if the ship was added sucessfully
        
        int difference; //used to determine if the coordinates entered are the right distance apart

        Console.WriteLine(startCoord1 + ", " + startCoord2);
        if (startCoord2 < endCoord2 || startCoord1 < endCoord1) { //IF ITS GOING DOWN OR RIGHT
          if(direction) { //if its horizontal or vertical
            difference = (endCoord2 - startCoord2) + 1;
          } else {
            difference = (endCoord1 - startCoord1) + 1;
          }
          
          if (difference != ship.Value) { //If coordinates entered are the wrong distance apart
            Console.WriteLine("Please enter values that fit in the distance: " + ship.Value + ", not: " + difference);
            return false;
          } else {
            for (int i = 0; i <= ship.Value - 1; i++) { //ITERATE OVER SHIPS POSITION ONCE TO CHECK ITS CLEAR FROM OTHER SHIPS
              if (direction) {
                if (board[startCoord1, startCoord2 + i] == "water [" + startCoord1 + ", " + (startCoord2 + i) + "]") {
                } else {
                  Console.WriteLine("ERROR: SHIP IS ALREADY IN THIS POSITION"); //Ship is already using this position
                  return false;
                }

              } else {
                if (board[(startCoord1 + i), startCoord2] == $"water [{startCoord1 + i}, {startCoord2}]") {
                }
                if (board[startCoord1 + i, startCoord2] == $"water [{startCoord1 + i}, {startCoord2}]") {
                } else {
                  Console.WriteLine("ERROR: SHIP IS ALREADY IN THIS POSITION");
                  return false;
                }
              }

            } //iF HERE, ALL MUST BE CLEAR SHIPS CAN BE SAFELY ADDED
            for (int i = 0; i <= ship.Value - 1; i++) { //i = distance from original position
              if(direction) { //if its horizontal or vertical
                if (board[startCoord1, startCoord2 + i] == "water [" + startCoord1 + ", " + (startCoord2 + i) + "]") { //Shouldnt need this line as all should be clear
                  board[startCoord1, startCoord2 + i] = ship.Key;
                  Console.WriteLine("Ship added: "+ board[startCoord1, startCoord2 + i]);
                  
                } else { //This shouldnt ever occur
                  //Console.WriteLine("ERROR: SHIP IS ALREADY IN THIS POSITION");
                  return false;
                }
               
              } else {
                if (board[startCoord1 + i, startCoord2] == $"water [{startCoord1 + i}, {startCoord2}]") {
                  board[startCoord1 + i, startCoord2] = ship.Key;
                  //Console.WriteLine(board[startCoord1 + i, startCoord2]);
                  
                } else {
                  return false;
                }
              }
            }
            return true;
          }
        } else {
          Console.WriteLine("Error");
          return false;
        }
      }

      foreach (KeyValuePair<string, int> ship in game.ships) {//For each ship, get the coordinates and add the ships.
        EnterShipPosition(ship, game); 
      }

      void EnterShipPosition(KeyValuePair<string, int> ship, Game game) {

        Console.WriteLine("Enter " + ship.Key + " start (length: " + ship.Value + "): ");
        int startCoord1 = Int32.Parse(Console.ReadLine()); //Get values from user
        int startCoord2 = Int32.Parse(Console.ReadLine());

        Console.WriteLine("Enter " + ship.Key + " end (length: " + ship.Value + "): ");
        int endCoord1 = Int32.Parse(Console.ReadLine());
        int endCoord2 = Int32.Parse(Console.ReadLine());

        if (startCoord1 == endCoord1) { //if its horizontal or vertical
          AddShips(startCoord1, startCoord2, endCoord1, endCoord2, game.userBoard, ship, true); //TODO: RUNS DID ADD SHIP BUT DONT THINK IT 
        } else if (startCoord2 == endCoord2) {
          AddShips(startCoord1, startCoord2, endCoord1, endCoord2, game.userBoard, ship, false);
        } else {
          Console.WriteLine("Input error.");
          Console.WriteLine("");

        }
        
        printBoard(game.userBoard, "            USER BOARD: ");

      }

      void ComputerShipSetup(Game game) {
        bool didAddShip; //bool val. if true, the ship was added sucessfully.
        game.computerBoard = game.InitBoard(); //initialises the computers board


        foreach (KeyValuePair<string, int> ship in game.ships) { //iterates over each ship in the ships dictionary

          List<int> coords = GetCoords(ship); //Gets coordinates
          if (coords[0] == coords[2]) {
            didAddShip = AddShips(coords[0], coords[1], coords[2], coords[3], game.computerBoard, ship, true); //inserts ships into board if is horizontals
          } else {
            didAddShip = AddShips(coords[0], coords[1], coords[2], coords[3], game.computerBoard, ship, false); //inserts ships into board if is vertical
          }
          if (!didAddShip) { // if the ships were not added 

            //TODO: get new coordinates and try agains
            ComputerShipSetup(game);
          }
          
        }
      }


      static List<int> GetCoords(KeyValuePair<string, int> ship) { //This is the func that picks the computers ship postions. 
        var random = new Random(); //Instantiates random class to generate random numbers
        List<int> coords = new List<int> { 1, 2, 3, 4 };
        while (coords[0] != coords[2] && coords[1] != coords[3]) {
          coords = new List<int> { random.Next(10 - ship.Value), random.Next(9), random.Next(10 - ship.Value), random.Next(9)
        }; //random coordinates for the ship
        if (coords[0] == coords[2]) { //if its horizontal or vertical
          while ((coords[3] - coords[1]) + 1 != ship.Value) {
            coords = new List<int> { coords[0], random.Next(9), coords[2], random.Next(9) };
          }
        } else if (coords[1] == coords[3]) { //horizontal
          while ((coords[2] - coords[0]) + 1 != ship.Value) {
            coords = new List<int> { random.Next(9), coords[1], random.Next(9), coords[3] };
          }
        }

        }
        return coords;
      }
      printBoard(game.userBoard, "BOARD BEFORE GAME STARTS"); //These lines just print the boards before the game starts
      Console.ReadKey();
    }
   

   

    static void printBoard(string[,] board, String message) { //This func is used to print the boards in a more human readable format
      Console.WriteLine(message); //Users board or Computers Board
      var RowColcount = 10;
      for (int row = 0; row < RowColcount; row++) { //Iterate over both dimensions
        Console.WriteLine("[ " +
          "" +
          "");
        for (int col = 0; col < RowColcount; col++) {
          Console.Write("[ {0} ]", board[col, row]);
        }
        Console.WriteLine(" ]");
      }
    }
  }
}
