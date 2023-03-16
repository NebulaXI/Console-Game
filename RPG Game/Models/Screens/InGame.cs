using RPG_Game.Models.Players;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Models.Screens
{
    public class InGame:Screen
    {

        public override Player OpenScreen(Player player)
        {
            int rowsInMatrix = 10; int colsInMatrix = 10;
            int playerRow = 1; int playerCol =1;
            ICollection<Monster> monsters = new List<Monster>();
            bool isInGame = true;
            while (isInGame)
            {
                Monster newMonster = CreateNewMonster();
                monsters.Add(newMonster);
                char[,] field = CreateMatrix(rowsInMatrix,colsInMatrix,playerRow, playerCol, player, monsters);
                DrawMatrix(field, player);
                char attackOrMove = CheckChoiceInput(); 
                if (attackOrMove == '1')
                {
                    List<Monster> possibleMonstersToAttack = GetMonstersInRange(playerRow,playerCol,field,player,monsters);
                    if (possibleMonstersToAttack.Count==0)
                    {
                        //Player looses move if there isn't targets
                        Console.WriteLine("No available targets in your range");
                    }
                    else
                    {
                        int i = 0;
                        foreach (var monster in possibleMonstersToAttack)
                        {
                            Console.WriteLine($"{i}) target with remaining blood {monster.Health}");
                            i++;
                        }
                        Console.WriteLine($"Which one to attack:");
                        int choosenMonsterToAttack = CheckInputMosterChoice(possibleMonstersToAttack);
                        foreach (var monster in possibleMonstersToAttack)
                        {
                            if (choosenMonsterToAttack==i)
                            {
                                int monsterHealth = player.Attack(player, monster);
                                if (monsterHealth <= 0) { monsters.Remove(monster); }
                                else { monster.Health = monsterHealth; }
                            }
                            i++;
                        }

                    }
                }
                else
                {
                    int[] playerPosition = PlayerMove(rowsInMatrix,colsInMatrix,playerRow,playerCol,player);
                    playerRow = playerPosition[0];
                    playerCol = playerPosition[1];
                    if (CheckIfTheresMonster(field, playerRow, playerCol))
                    {
                        Monster currentMonster = GetCurrentMonster(monsters, playerRow, playerCol);
                        int monsterHealth=player.Attack(player, currentMonster);
                        if (monsterHealth <= 0) { monsters.Remove(currentMonster); }
                        else { currentMonster.Health = monsterHealth; }
                    }
                    
                }
                MonstersMove(monsters,playerRow,playerCol,field,player);
                if (player.Health <= 0) { isInGame = false; };
                Console.Clear();
            }
            
            return player;
        }
        
        static Monster CreateNewMonster()
        {
            Monster newMonster = new Monster();
            newMonster.Setup();
            return newMonster;
        }
        static char[,] CreateMatrix(int rows,int cols,int rowOfPlayer, int colOfPlayer, Player player,ICollection<Monster> monsters)
        {
            char[,] field = new char[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    foreach (var monster in monsters)
                    {
                        if (row == monster.Position[0] && col == monster.Position[1])
                        {
                            field[row, col] = monster.Symbol;
                            continue;
                        }
                        else if (row == rowOfPlayer && col == colOfPlayer)
                        {
                            field[row, col] = player.Symbol;
                            continue;
                        }
                        else
                        {
                            field[row, col] = '▒';
                        }
                        

                    }
                    
                }
            }
            return field;
        }
        static void DrawMatrix(char[,] field,Player player)
        {
            Console.WriteLine($"Health: {player.Health}\tMana: {player.Mana}\n\n\n");
            for (int row = 0; row < field.GetLength(0); row++)
            {
                for (int col = 0; col < field.GetLength(1); col++)
                {
                    Console.Write(field[row, col]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("Choose your action \n1)Attack \n2)Move");
        }

        static bool CheckIfMonsterIsNextToPlayer(int playerRow,int playerCol,int monsterRow,int monsterCol)
        {
            bool isThereAPlayer = false;
            //w
            if (monsterRow - 1 == playerRow && playerCol == monsterCol){isThereAPlayer = true;}
            //s
            else if (monsterRow + 1 == playerRow && playerCol == monsterCol){ isThereAPlayer= true; } 
            //a
            else if(monsterRow==playerRow&& playerCol==monsterCol-1) { isThereAPlayer = true; }
            //d
            else if (monsterRow == playerRow && playerCol == monsterCol + 1) { isThereAPlayer = true; }
            //e
            else if(monsterRow-1==playerRow&& playerCol==monsterCol+1) { isThereAPlayer = true; }
            //q
            else if(monsterRow - 1 == playerRow && playerCol == monsterCol - 1){ isThereAPlayer= true; }
            //x
            else if (monsterRow + 1 == playerRow && playerCol == monsterCol + 1) { isThereAPlayer = true; }
            //z
            else if (monsterRow + 1 == playerRow && playerCol == monsterCol - 1) { isThereAPlayer = true; }
            return isThereAPlayer;
        }
        static void MonstersMove(ICollection<Monster> monsters, int playerRow,int playerCol, char[,] field,Player player)
        {
            foreach (var monster in monsters)
            {
                List<int[]> monsterPossibleMoves = CheckAllPossibleMonsterMoves(monster, field);
                int[] optimalMonsterMove = GetOptimalMonsterMove(monsterPossibleMoves,playerRow,playerCol,field,monster);
                monster.Position[0] = optimalMonsterMove[0];
                monster.Position[1] = optimalMonsterMove[1];
                if(CheckIfMonsterIsNextToPlayer(playerRow,playerCol, monster.Position[0], monster.Position[1]))
                {
                    player.Health=monster.Attack(player, monster);
                }
            }
        }
        static List<int[]> CheckAllPossibleMonsterMoves(Monster monster, char[,] field)
        {
            List<int[]> possiblePossitions=new List<int[]>();
            int initialMonsterRow = monster.Position[0];
            int initialMonsterCol = monster.Position[1];
            int[] monsterPosition;
            //'w':
            monster.Position[0] = CheckIfIsUnderValue(field.GetLength(0), initialMonsterRow, monster.Range);
            monsterPosition = new int[] { monster.Position[0],initialMonsterCol };
            possiblePossitions.Add(monsterPosition);
            //'s':
            monster.Position[0] = CheckIfIsOverValue(field.GetLength(0), initialMonsterRow, monster.Range);
            monsterPosition = new int[] { monster.Position[0], initialMonsterCol };
            possiblePossitions.Add(monsterPosition);
            //'d':
            monster.Position[1] = CheckIfIsOverValue(field.GetLength(1), initialMonsterCol, monster.Range);
            monsterPosition = new int[] {initialMonsterRow, monster.Position[1] };
            possiblePossitions.Add(monsterPosition);
            //'a':
            monster.Position[1] = CheckIfIsUnderValue(field.GetLength(1), initialMonsterCol, monster.Range);
            monsterPosition = new int[] { initialMonsterRow, monster.Position[1] };
            possiblePossitions.Add(monsterPosition);
            //'e':
            monster.Position[0] = CheckIfIsUnderValue(field.GetLength(0), initialMonsterRow, monster.Range);
            monster.Position[1] = CheckIfIsOverValue(field.GetLength(1), initialMonsterCol, monster.Range);
            monsterPosition = new int[] { monster.Position[0], monster.Position[1] };
            possiblePossitions.Add(monsterPosition);
            //'x':
            monster.Position[0] = CheckIfIsOverValue(field.GetLength(0), initialMonsterRow, monster.Range);
            monster.Position[1] = CheckIfIsOverValue(field.GetLength(1), initialMonsterCol, monster.Range);
            monsterPosition = new int[] { monster.Position[0], monster.Position[1] };
            possiblePossitions.Add(monsterPosition);
            //'q':
            monster.Position[0] = CheckIfIsUnderValue(field.GetLength(0), initialMonsterRow, monster.Range);
            monster.Position[1] = CheckIfIsUnderValue(field.GetLength(1), initialMonsterCol, monster.Range);
            monsterPosition = new int[] { monster.Position[0], monster.Position[1] };
            possiblePossitions.Add(monsterPosition);
            //'z':
            monster.Position[0] = CheckIfIsOverValue(field.GetLength(0), initialMonsterRow, monster.Range);
            monster.Position[1] = CheckIfIsUnderValue(field.GetLength(1), initialMonsterCol, monster.Range);
            monsterPosition = new int[] { monster.Position[0], monster.Position[1] };
            possiblePossitions.Add(monsterPosition);

            return possiblePossitions;
        }

        static int CheckInputMosterChoice(List<Monster> monstersToAttack)
        {
            bool isInputIncorrect = true;
            int returnPick = 0;
            while (isInputIncorrect)
            {
                try
                {
                    ConsoleKeyInfo pick = Console.ReadKey();
                    if ((int)pick.KeyChar >=0 || (int)pick.KeyChar<monstersToAttack.Count)
                    {
                        returnPick = (int)pick.KeyChar;
                        isInputIncorrect = false;
                    }
                    else
                    {
                        throw new ArgumentException("\nInvalid input!Plese select one of the targets above.");
                    }
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae.Message);
                }
            }
            return returnPick;
        }

        static int[] GetOptimalMonsterMove(List<int[]> monsterPossibleMoves,int playerRow,int playerCol,char [,] field,Monster monster)
        {
            int optimalMonsterRow = 0;
            int optimalMonsterCol = 0;
            foreach (var move in monsterPossibleMoves)
            {
                int monsterRow = move[0];
                int monsterCol = move[1];
                int initialRowDifference = Math.Abs(playerRow - monsterRow);
                int initialColDifference = Math.Abs(playerCol - monsterCol);


                //Check up for monster
                if (playerRow < monsterRow)
                {
                    int possibleMonsterRow = CheckIfIsUnderValue(field.GetLength(0), monster.Position[0], monster.Range);
                    if (Math.Abs(playerRow-possibleMonsterRow)<initialRowDifference)
                    {
                        //Check 'q'
                        if (playerCol<monsterCol)
                        {
                            int possibleMonsterCol = CheckIfIsUnderValue(field.GetLength(1), monster.Position[1], monster.Range);
                            if (Math.Abs(playerCol - possibleMonsterCol) < initialColDifference)
                            {
                                initialColDifference= possibleMonsterCol;
                                optimalMonsterRow=possibleMonsterRow;
                                optimalMonsterCol = possibleMonsterCol;
                            }
                        }
                        //Check 'e'
                        else if (playerCol > monsterCol)
                        {
                            int possibleMonsterCol = CheckIfIsOverValue(field.GetLength(1), monster.Position[1], monster.Range);
                            if (Math.Abs(playerCol - possibleMonsterCol) < initialColDifference)
                            {
                                initialColDifference = possibleMonsterCol;
                                optimalMonsterRow=possibleMonsterRow;
                                optimalMonsterCol = possibleMonsterCol;
                            }
                        }
                        //Check 'w'
                        else
                        {
                            initialRowDifference = playerRow - possibleMonsterRow;
                            optimalMonsterRow = possibleMonsterRow;
                            optimalMonsterCol= playerCol;
                        }
                        
                    }
                }
                //Check down positions for monster
                else
                {
                    int possibleMonsterRow = CheckIfIsOverValue(field.GetLength(0), monster.Position[0], monster.Range);
                    if (Math.Abs(playerRow - possibleMonsterRow) < initialRowDifference)
                    {
                        //Check 'z'
                        if (playerCol < monsterCol)
                        {
                            int possibleMonsterCol = CheckIfIsUnderValue(field.GetLength(1), monster.Position[1], monster.Range);
                            if (Math.Abs(playerCol - possibleMonsterCol) < initialColDifference)
                            {
                                initialColDifference = possibleMonsterCol;
                                optimalMonsterRow = possibleMonsterRow;
                                optimalMonsterCol = possibleMonsterCol;
                            }
                        }
                        //Check 'x'
                        else if (playerCol > monsterCol)
                        {
                            int possibleMonsterCol = CheckIfIsOverValue(field.GetLength(1), monster.Position[1], monster.Range);
                            if (Math.Abs(playerCol - possibleMonsterCol) < initialColDifference)
                            {
                                initialColDifference = possibleMonsterCol;
                                optimalMonsterRow = possibleMonsterRow;
                                optimalMonsterCol = possibleMonsterCol;
                            }
                        }
                        //Check 'w'
                        else
                        {
                            initialRowDifference = playerRow - possibleMonsterRow;
                            optimalMonsterRow = possibleMonsterRow;
                            optimalMonsterCol = playerCol;
                        }

                    }
                }
                
            }
            int[] optimalPosition = new int[] { optimalMonsterRow, optimalMonsterCol };
            return optimalPosition;
        }

        static List<int[]> GetPlayerRange(int playerRow, int playerCol, char[,] field,Player player)
        {
            int initialPlayerRow = playerRow;
            int initialPlayerCol = playerCol;
            List<int[]> possiblePositionsForAttack= new List<int[]>();
            for (int i = 0; i < player.Range; i++)
            {
                int[] position;
                //w
                playerRow = CheckIfIsUnderValue(field.GetLength(0), initialPlayerRow, i++);
                position = new int[] {playerRow,playerCol};
                possiblePositionsForAttack.Add(position);
                //s
                playerRow = CheckIfIsOverValue(field.GetLength(0), initialPlayerRow, i++);
                position = new int[] { playerRow,playerCol };
                possiblePositionsForAttack.Add(position);
                //a
                playerCol=CheckIfIsUnderValue(field.GetLength(1),initialPlayerCol, i++);
                position = new int[] { playerRow, playerCol };
                possiblePositionsForAttack.Add(position);
                //d
                playerCol = CheckIfIsOverValue(field.GetLength(1),initialPlayerCol, i++);
                position = new int[] { playerRow, playerCol + (i++) };
                possiblePositionsForAttack.Add(position);
                //e
                playerRow = CheckIfIsUnderValue(field.GetLength(0), initialPlayerRow, i++);
                playerCol = CheckIfIsOverValue(field.GetLength(1), initialPlayerCol, i++);
                position = new int[] { playerRow, playerCol };
                possiblePositionsForAttack.Add(position);
                //x
                playerRow = CheckIfIsOverValue(field.GetLength(0), initialPlayerRow, i++);
                playerCol = CheckIfIsOverValue(field.GetLength(1), initialPlayerCol, i++);
                position = new int[] { playerRow, playerCol };
                possiblePositionsForAttack.Add(position);
                //q
                playerRow = CheckIfIsUnderValue(field.GetLength(0), initialPlayerRow, i++);
                playerCol = CheckIfIsUnderValue(field.GetLength(1), initialPlayerCol, i++);
                position = new int[] { playerRow, playerCol };
                possiblePositionsForAttack.Add(position);
                //z
                playerRow = CheckIfIsOverValue(field.GetLength(0), initialPlayerRow, i++);
                playerCol = CheckIfIsUnderValue(field.GetLength(1), initialPlayerCol, i++);
                position = new int[] { playerRow, playerCol };
                possiblePositionsForAttack.Add(position);

            }
            return possiblePositionsForAttack;
        }

        static int CheckIfIsUnderValue(int totalRowsOrCols,int currentPosition, int range)
        {
            if (currentPosition - range < 0)
            {
                currentPosition = UnderValue(totalRowsOrCols, currentPosition, range);
                return currentPosition;
            }
            else
            {
                return currentPosition -= range;
            }
        }

        static int CheckIfIsOverValue(int totalRowsOrCols,int currentPosistion,int range)
        {
            if (currentPosistion + range > totalRowsOrCols - 1)
            {
                currentPosistion = OverValue(totalRowsOrCols, currentPosistion, range);
                return currentPosistion;
            }
            else
            {
                return currentPosistion += range;
            }
        }
        static List<Monster> GetMonstersInRange(int playerRow,int playerCol, char[,] field,Player player, ICollection<Monster> monsters)
        {
            List<Monster> mostersToAttack = new List<Monster>();
            List<int[]> positionsForPosibleAttack = GetPlayerRange(playerRow, playerCol, field,player);
            foreach (var position in positionsForPosibleAttack)
            {
                int currentRow = position[0];
                int currentCol = position[1];
                if(CheckIfTheresMonster(field, currentRow, currentCol))
                {
                    var monster = GetCurrentMonster(monsters, currentRow, currentCol);
                    mostersToAttack.Add(monster);
                }
            }
            return mostersToAttack;
        }
        static char CheckChoiceInput()
        {
            bool isInputIncorrect = true;
            char returnPick = 'n';
            while (isInputIncorrect)
            {
                try
                {
                    ConsoleKeyInfo pick = Console.ReadKey();
                    if (pick.KeyChar == '1' || pick.KeyChar == '2')
                    {
                        returnPick = pick.KeyChar;
                        isInputIncorrect = false;
                    }
                    else
                    {
                        throw new ArgumentException("\nInvalid input!Plese select 1 for attack or 2 for move.");
                    }
                }
                catch (ArgumentException ae)
                { 
                    Console.WriteLine(ae.Message);
                }
            }
            return returnPick;
        }
        static Monster GetCurrentMonster(ICollection<Monster> monsters,int monsterRow,int monsterCol)
        {
            Monster monsterReturn=new Monster();
            foreach (var monster in monsters)
            {
                if (monsterRow == monster.Position[0] && monsterCol == monster.Position[1])
                {
                    monsterReturn= monster;
                }
            }
            return monsterReturn;
        }
        
        static int[] PlayerMove(int rows,int cols,int playerRow,int playerCol,Player player)
        {
            Console.WriteLine("\nOptions:\n\tW - Move up;\n\tS - Move down;\n\tD - Move right;\n\tA - Move left;\n\tE - Move diagonally up & right;\n\tX - Move diagonally down & right;\n\tQ - Move diagonally up & left;\n\tZ - Move diagonally down & left;");
            char direction = CheckMoveInput();
            int[] playerPosition = new int[] { playerRow,playerCol};
            switch (direction)
            {
                case 'w':
                    playerRow = CheckIfIsUnderValue(rows, playerRow, player.Range);
                    playerPosition[0]= playerRow;
                    break;
                case 's':
                    playerRow = CheckIfIsOverValue(rows, playerRow, player.Range);
                    playerPosition[0] = playerRow;
                    break;
                case 'd':
                    playerCol = CheckIfIsOverValue(cols,playerCol, player.Range);
                    playerPosition[1] = playerCol;
                    break;
                case 'a':
                    playerCol = CheckIfIsUnderValue(cols, playerCol, player.Range);
                    playerPosition[1] = playerCol;
                    break;
                case 'e':
                    playerRow = CheckIfIsUnderValue(rows,playerRow, player.Range);
                    playerCol = CheckIfIsOverValue(cols, playerCol, player.Range);
                    playerPosition[0] = playerRow;
                    playerPosition[1] = playerCol;
                    break;
                case 'x':
                    playerRow = CheckIfIsOverValue(rows, playerRow, player.Range);
                    playerCol = CheckIfIsOverValue(cols, playerCol, player.Range);
                    playerPosition[0] = playerRow;
                    playerPosition[1] = playerCol;
                    break;
                case 'q':
                    playerRow = CheckIfIsUnderValue(rows, playerRow, player.Range);
                    playerCol = CheckIfIsUnderValue(cols, playerCol, player.Range);
                    playerPosition[0] = playerRow;
                    playerPosition[1] = playerCol;
                    break;
                case 'z':
                    playerRow = CheckIfIsOverValue(rows, playerRow, player.Range);
                    playerCol = CheckIfIsUnderValue(cols, playerCol, player.Range);
                    playerPosition[0] = playerRow;
                    playerPosition[1] = playerCol;
                    break;

            }
            return playerPosition;
        }

        static int UnderValue(int totalRowsOrCols, int currentPosition, int range)=> totalRowsOrCols + (currentPosition - range);
        
        static int OverValue(int totalRowsOrCols, int currentPosition, int range)=> (currentPosition + range) - totalRowsOrCols;
        
        static char CheckMoveInput()
        {
            bool isInputIncorrect = true;
            bool isInputLetter = false;
            char returnPick = 'n';
            while (isInputIncorrect)
            {
                try
                {
                    ConsoleKeyInfo pick = Console.ReadKey();
                    char pickChar =Char.ToLower(pick.KeyChar);
                    switch (pickChar)
                    {
                        case 'w':
                        case 's':
                        case 'd':
                        case 'a':
                        case 'e':
                        case 'x':
                        case 'q':
                        case 'z':
                            returnPick = pickChar;
                            isInputIncorrect= false;
                            isInputLetter = true;
                            break;
                           
                    }
                    if(!isInputLetter)
                    {
                        throw new ArgumentException("\nInvalid input!Plese select 1 of the options above.");
                    }
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae.Message);
                }
            }
            return returnPick;
        }

        static bool CheckIfTheresMonster(char[,] field, int playerRow,int playerCol)
        {
            bool isTheresMonster = false;
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[playerRow,playerCol]!= '▒')
                    {
                        isTheresMonster= true;
                    }
                }
            }
            return isTheresMonster;
        }

        
    }
}
