using Demo_NTier_XmlJsonData.DataAccessLayer;
using Demo_NTier_XmlJsonData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_NTier_XmlJsonData.BusinessLayer;

/// <summary>
/// Demo app for XML and Json serialization
/// </summary>
namespace Demo_NTier_XmlJsonData.PresentationLayer
{
    public class ConsoleView
    {
        private static FlintstoneCharacterBusiness _fcBusiness = new FlintstoneCharacterBusiness();
        public ConsoleView()
        {
            DisplayWelcomeScreen();
            DisplayMainMenu();
            DisplayClosingScreen();
        }

        private static void DisplayShoppingLists()
        {
            List<FlintstoneCharacter> characters = _fcBusiness.AllFlintstoneCharacters();

            DisplayScreenHeader("Shopping Lists by Character");

            Console.WriteLine();
            Console.WriteLine("\tCharacters");
            Console.WriteLine("\t------------------");
            foreach (var character in characters)
            {
                Console.WriteLine("\t" + character.FullName);

                if (character.GroceryList != null && character.GroceryList.Any())
                {
                    Console.WriteLine("\t\tShopping List");
                    Console.WriteLine("\t\t-------------");
                    foreach (var item in character.GroceryList)
                    {
                        Console.WriteLine("\t\t" + item.Name.PadRight(15) + item.Quantity.ToString().PadLeft(4));
                    }
                }
            }

            DisplayMainMenuPrompt();
        }

        /// <summary>
        /// Display Main Menu
        /// </summary>
        private static void DisplayMainMenu()
        {
            bool quitApplication = false;
            char menuChoiceKey;

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get the user's menu choice
                //
                Console.WriteLine("\ta) List All Characters");
                Console.WriteLine("\tb) Character Detail");
                Console.WriteLine("\tc) Add Character");
                Console.WriteLine("\td) Delete Character");
                Console.WriteLine("\te) Update Character");
                Console.WriteLine("\tq) Quit");
                Console.Write("\n\tEnter Choice: ");
                menuChoiceKey = Console.ReadKey().KeyChar;

                //
                // process user's choice
                //
                switch (menuChoiceKey)
                {
                    case 'a':
                    case 'A':
                        DisplayAllCharacters();
                        break;
                        
                    case 'b':
                    case 'B':
                        DisplayCharacterDetail();
                        break;

                    case 'c':
                    case 'C':
                        DisplayAddCharacter();
                        break;

                    case 'd':
                    case 'D':
                        DisplayDeleteCharacter();
                        break;
                    case 'E':
                    case 'e':
                        DisplayUpdateCharacter();
                        break;
                    case 'Q':
                    case 'q':
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\t*************************************************");
                        Console.WriteLine("\t   Please indicate your choice with a letter.");
                        Console.WriteLine("\t**************************************************");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        static void DisplayAddCharacter()
        {
                bool checking = true;
            int id = 0;
            FlintstoneCharacter character = new FlintstoneCharacter();
            DisplayScreenHeader("Add New character");
              
            while (checking)
            {
                Console.Clear();
                Console.WriteLine("ID?");
                if (int.TryParse(Console.ReadLine(), out id))
                {
                    character.Id = id; 
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please enter a valid number for your ID!");
                    Console.ReadKey();
                }
            }

            Console.Write("First Name?");
            character.FirstName = Console.ReadLine();
            Console.Write("Last Name?");
            character.LastName = Console.ReadLine();
            
            int age = 0;
            while (checking)
            {
                Console.Clear();
                Console.WriteLine("Age?");
                if (int.TryParse(Console.ReadLine(),out age))
                {
                    character.Age = age; 
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please enter a valid number for your age!");
                    Console.ReadKey();
                }
            }


             
            FlintstoneCharacter.GenderType gender; 
            while (checking)
            { 
                Console.WriteLine("Gender? <Male> OR <Female>");
                if (Enum.TryParse(Console.ReadLine(), out gender))
                {
                    if (gender == FlintstoneCharacter.GenderType.Female || gender == FlintstoneCharacter.GenderType.Male)
                    {
                    character.Gender = gender;
                    break; 
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please enter a valid gender <Male> : <Female>!"); 
                }
            }

            int gross = 0;
            while (checking)
            {
                Console.Clear();
                Console.WriteLine("Annual Gross Salary?");
                if (int.TryParse(Console.ReadLine(), out gross))
                {
                    character.AverageAnnualGross = gross;
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please enter a valid number for your gross!");
                    Console.ReadKey();
                }
            }

            Console.Clear();
            Console.WriteLine("Gimme a description!");
            character.Description = Console.ReadLine();

            _fcBusiness.AddFlintstoneCharacter(character);

            if (_fcBusiness.FileIoStatus == FileIoMessage.Complete)
            {
                Console.WriteLine("added to the data");
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }

            DisplayMainMenuPrompt();
        }

        static void DisplayUpdateCharacter()
        {
            /*
             * 
             * 
             * 
             */
            DisplayScreenHeader("Who do you want to update? (By Id)");

            List<FlintstoneCharacter> characters = _fcBusiness.AllFlintstoneCharacters();
            int id = DisplayGetCharacterIdFromList(characters);
            FlintstoneCharacter character = _fcBusiness.FlintstoneCharacterById(id);
            string whatToUpdate = "";
            bool checking = true;
            while (checking)
            {

                Console.WriteLine("Enter what you would like to update:");
                Console.WriteLine();
                Console.WriteLine($"<Age> <AverageAnnualGross> <FirstName> <LastName> <Gender> <Description> <GroceryList> <HireDate>");
                whatToUpdate = Console.ReadLine().ToLower();
                if (whatToUpdate == "age" || whatToUpdate == "averageannualgross" || whatToUpdate == "firstname" || whatToUpdate == "lastname" || whatToUpdate == "gender" || whatToUpdate == "description" || whatToUpdate == "grocerylist" || whatToUpdate == "hiredate")
                {
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please enter one of the listed properties.");
                }

            }
            FlintstoneCharacterBusiness.whatToChange(whatToUpdate,character);
        }

        static void DisplayDeleteCharacter()
        {

            FlintstoneCharacter character = new FlintstoneCharacter();
            DisplayScreenHeader("Delete old character");

            Console.WriteLine("Enter the Id of the character that you want to delete");
            character.Id = int.Parse(Console.ReadLine());


            _fcBusiness.DeleteFlintstoneCharacter(character);

            Console.WriteLine("Whats done is done.");
            DisplayMainMenuPrompt();
        }

        /// <summary>
        /// display a single character's information
        /// </summary>
        static void DisplayCharacterDetail()
        {
            List<FlintstoneCharacter> characters = _fcBusiness.AllFlintstoneCharacters();

            int id = DisplayGetCharacterIdFromList(characters);

            FlintstoneCharacter character = _fcBusiness.FlintstoneCharacterById(id);

            if (_fcBusiness.FileIoStatus == FileIoMessage.Complete)
            {
                DisplayScreenHeader("Character Information");
                DisplayCharacterInfo(character);
            }
            else
            {
                // process file IO error message
            }

            DisplayMainMenuPrompt();
        }

        /// <summary>
        /// list all character's full names and ids and query the user for an id
        /// </summary>
        /// <param name="characters">character id</param>
        /// <returns></returns>
        static int DisplayGetCharacterIdFromList(List<FlintstoneCharacter> characters)
        {
            bool validResponse = false;
            int id = -1;
            List<int> validIds = characters.Select(c => c.Id).OrderBy(characterId => characterId).ToList();

            do
            {
                DisplayScreenHeader("Choose Character");

                Console.WriteLine(
                    "Name".PadRight(20) +
                    "Id".PadRight(4)
                    );
                Console.WriteLine(
                    "-------------".PadRight(20) +
                    "-----".PadRight(4)
                    );

                foreach (var character in characters)
                {
                    Console.WriteLine(
                        character.FullName.PadRight(20) +
                        character.Id.ToString().PadRight(4)
                        );
                }
                Console.WriteLine();
                Console.Write("Enter Id:");
                if (!int.TryParse(Console.ReadLine(), out id))
                {
                    Console.WriteLine("Please enter an integer value for the ID.");
                    DisplayContinuePrompt();
                }
                else if (!validIds.Contains(id))
                {
                    Console.WriteLine("Please enter a valid Id shown above.");
                    DisplayContinuePrompt();
                }
                else
                {
                    validResponse = true;
                }
            } while (!validResponse);

            return id;
        }

        /// <summary>
        /// display all character property values
        /// </summary>
        /// <param name="character">character</param>
        static void DisplayCharacterInfo(FlintstoneCharacter character)
        {
            Console.WriteLine($"Last Name: {character.LastName}");
            Console.WriteLine($"First Name: {character.FirstName}");
            Console.WriteLine($"Age: {character.Age}");
            Console.WriteLine($"Gender: {character.Gender}");
            Console.WriteLine($"Average Annual Gross: {character.AverageAnnualGross:c}");
            Console.WriteLine($"Description: \n{character.Description}");
        }


        /// <summary>
        /// display a table of all characters: first name, last name, and id
        /// </summary>
        static void DisplayAllCharacters()
        {
            List<FlintstoneCharacter> characters = _fcBusiness.AllFlintstoneCharacters();

            if (_fcBusiness.FileIoStatus == FileIoMessage.Complete)
            {
                DisplayScreenHeader("Flintstone Characters");

                Console.WriteLine(
                    "First Name".PadRight(15) +
                    "Last Name".PadRight(15) +
                    "Id".PadRight(4)
                    );
                Console.WriteLine(
                    "-------------".PadRight(15) +
                    "-------------".PadRight(15) +
                    "-----".PadRight(4)
                    );

                foreach (var character in characters)
                {
                    Console.WriteLine(
                        character.FirstName.PadRight(15) +
                        character.LastName.PadRight(15) +
                        character.Id.ToString().PadRight(4)
                        );
                }
            }
            else
            {
                // process file IO error message
            }

            DisplayMainMenuPrompt();
        }


        #region HELPER METHODS

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
        static void DisplayMainMenuPrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
            DisplayMainMenu();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        /// <summary>
        /// display welcome screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tDemonstration:");
            Console.WriteLine("\t\tN-Tier CRUD with XML or JSON Persistence");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using our app.");
            Console.WriteLine();

            Console.WriteLine("\tPress any key to exit.");
            Console.ReadKey();
        }

        #endregion
    }
}
