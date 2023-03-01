using RPG_Game.Models.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Models.Screens
{
    public class CharacterSelect:Screen
    {


        public override Player OpenScreen(Player player)
        {
           char selectedCharacter = SelectCharacter();
           bool isGoingToBuff = SelectToBuff();
            if (isGoingToBuff)
            {
               int remainingPoints = 3;
               Dictionary<string, int> pointsDivision = PointsDivision(remainingPoints);
               int boostStrenghtPoints = pointsDivision["boostStrenghtPoints"];
               int boostAgilityPoints = pointsDivision["boostAgilityPoints"];
               int boostIntelligencePoints = pointsDivision["boostIntelligencePoints"];

              
                player = player.CreateCustomPlayer(selectedCharacter, boostStrenghtPoints, boostAgilityPoints, boostIntelligencePoints,player);
            }
            else
            {
                player = player.CreateDefaultPlayer(selectedCharacter,player);
            }
            
            player.Setup();
            Console.Clear();
            return player;
            
        }
       
        Dictionary<string,int> PointsDivision(int remainingPoints)
        {
            Dictionary<string, int> returnDic = new Dictionary<string, int>
            {
                { "boostStrenghtPoints", 0 },
                { "boostAgilityPoints", 0 },
                { "boostIntelligencePoints", 0 }
            };
            while (remainingPoints != 0)
            {
                Console.WriteLine($"\nRemaining points:{remainingPoints}");
                Console.WriteLine("Add to Strenght:");
                try
                {
                    int addedPoints = CheckPointsInput(remainingPoints);
                    returnDic["boostStrenghtPoints"] = addedPoints;
                }
                catch (ArgumentException ae) { Console.WriteLine(ae); }
                remainingPoints -= returnDic["boostStrenghtPoints"];

                if (remainingPoints > 0)
                {
                    Console.WriteLine($"Remaining points:{remainingPoints}");
                    Console.WriteLine("Add to Agility:");
                    try
                    {
                        int addedPoints = CheckPointsInput(remainingPoints);
                        returnDic["boostAgilityPoints"] = addedPoints;
                    }
                    catch (ArgumentException ae) { Console.WriteLine(ae); }
                    remainingPoints -= returnDic["boostAgilityPoints"];
                }

                if (remainingPoints > 0)
                {
                    Console.WriteLine($"Remaining points:{remainingPoints}");
                    Console.WriteLine("Add to Intelligence:");
                    try
                    {
                        int addedPoints = CheckPointsInput(remainingPoints);
                        returnDic["boostIntelligencePoints"] = addedPoints;
                    }
                    catch (ArgumentException ae) { Console.WriteLine(ae); }
                    remainingPoints -= returnDic["boostIntelligencePoints"];
                }

            }
            return returnDic;
        }
        static bool SelectToBuff()
        {
            Console.WriteLine("\nWould you like to buff up your stats before starting?\t\t\t(Limit: 3 points total)\r\n\tResponse (Y\\N): \r\n");
            bool isGoingToBuff = false;
            bool isInputIncorrect = true;
            while (isInputIncorrect)
            {
                try
                {
                    isGoingToBuff = CheckChoiceToBuffInputIsCorrect();
                    isInputIncorrect = false;
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae.Message);
                }
            }
            return isGoingToBuff;
        }
        static bool CheckChoiceToBuffInputIsCorrect()
        {
            ConsoleKeyInfo choiceToBuff = Console.ReadKey();
            bool IsGoingToBuff = false;
            
                if (choiceToBuff.KeyChar == 'y' || choiceToBuff.KeyChar == 'Y')
                {
                    IsGoingToBuff= true;
                }
                else if (choiceToBuff.KeyChar == 'n' || choiceToBuff.KeyChar == 'N')
                {
                    IsGoingToBuff= false;
                }
                else
                {
                    throw new ArgumentException("\nPlease choose y or Y if you want to buff your hero,otherwise choose n or N.");
                }
            
            return IsGoingToBuff;
        }
        static char SelectCharacter()
        {
            Console.WriteLine("Choose character type:\n\nOptions:\n\n1) Warrior\n\n2) Archer\n\n3) Mage\n\nYour pick:");
            char pick = 'n';
            bool isInputIncorrect = true;
            while (isInputIncorrect)
            {
                
                try
                {
                    pick = CheckCharacterSelectInput();
                    isInputIncorrect = false;
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae.Message);
                }
            }
            return pick;
        }
        static char CheckCharacterSelectInput()
        {
            ConsoleKeyInfo pick = Console.ReadKey();
            char returnChar = 'n';
            if (pick.KeyChar == '1' || pick.KeyChar == '2' || pick.KeyChar == '3')
            {
                switch (pick.KeyChar)
                {
                    case '1':
                     returnChar = '1';
                     break;
                    case '2':
                     returnChar = '2';
                     break;
                    case '3':
                     returnChar = '3';
                     break;
                }

            }
            else
            {
               throw new ArgumentException("\nInvalid choice!Please select one of the options above.");
            }
             
            return returnChar;
        }

        int CheckPointsInput(int remainingPoints)
        {
            var input = Console.ReadLine();
            int temp;
            int addedPoints;
            if (int.TryParse(input, out temp))
            {
                if (remainingPoints < temp || temp < 0)
                {
                    throw new ArgumentException($"The input must be a positive digit,less than {remainingPoints} or 0.");
                }
                else
                {
                    addedPoints =temp;
                }
            }
            else{
                throw new ArgumentException("The input must be a digit.");
            }
            
            return addedPoints;
        }

        

        
    }
}
