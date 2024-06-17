/*
    Hot or Cold - The classic "hot or cold" game, but with numbers. 
    The computer generates a random number, and the player has to make guesses until they correctly guess the random number. 
    The computer can only tell the player if they guessed too high, too low, or correctly. 
    Challenge: Allow the player to select the possible number range (ie. 3 - 9, 0 - 20, 30 - 100) to choose their own difficulty.
*/

using System;
using System.Collections.Generic;  
using System.Diagnostics;
using System.Net;

public class HotCold {

    public static void Main(string[] args){
        string name; 

        Console.WriteLine("Please your name: ");
        name = Console.ReadLine();

        while(Run(name));
       
    }

    //Get user name and allow the player to select the possible number range (ie. 0 - 20, 3 - 9, 30 - 100) to choose their own difficulty.
    public static (int,int,string) DifficultySelector(string name){
        int difficulty = -1;
        (int, int, string) cupcake = (3,9,"Cupcake"); 
        (int, int, string) meh = (0,20,"Meh"); 
        (int, int, string) impossible = (30,100,"Impossible"); 

        Dictionary<int,(int,int,string)> levels = new Dictionary<int,(int,int,string)>();
        levels.Add(1, cupcake);
        levels.Add(2, meh);
        levels.Add(3, impossible);

        Console.WriteLine("\nWelcome " + name + "! Now, Choose Your Difficulty:\n" );
        Console.WriteLine("***********ALL NUMBER RANGES ARE INCLUSIVE***********" );
        Console.WriteLine("[1] - Cupcake (Range 3 - 9)");
        Console.WriteLine("[2] - Meh (Range 0 - 20)");
        Console.WriteLine("[3] - Impossible (Range 30 - 100)\n");

        Console.WriteLine("Please select your difficulty level (Enter 1, 2, or 3): ");

        try{
            difficulty = Int32.Parse(Console.ReadLine());
        } catch(Exception){
            Console.WriteLine("Input should be an integer! Please enter either 1, 2, or 3. Try again...\n");
            DifficultySelector(name);
        }

        if (difficulty < 1 || difficulty > 3){
            Console.WriteLine("Input should only be either 1, 2, or 3. Try again...\n");
            DifficultySelector(name);
        }

        return levels[difficulty];
    }

    // UserGuess method: Asks user to input an int as a prediction, uses try/catch statements to handle various incorrect user inputs
    public static bool UserGuess(int min, int max, string difficulty, int computerGuess){
        string userInput;
        string selectNewLevel;
        string again;
        int userPrediction;

        Console.WriteLine("\nLevel " + difficulty + ": Enter any number from " + min + "-" + max + " (Inclusive):");
        userInput = Console.ReadLine();

        if (!IsValidIntInput(userInput, min, max)){
            UserGuess(min, max, difficulty, computerGuess);
        }

        userPrediction = Int32.Parse(userInput);

        //Compares user vs computer guess and outputs all results
        if (userPrediction > computerGuess){
            Console.WriteLine("\nTOO HIGH!");
        } else if (userPrediction < computerGuess){
            Console.WriteLine("\nTOO LOW!");
        } else if (userPrediction == computerGuess){
            Console.WriteLine("\nAI Guess = " + computerGuess);
            Console.WriteLine("CONGRATULATIONS! YOU GOT IT!");
            Console.WriteLine("\nPlay Another Level(y/n)?"); 
            selectNewLevel = Console.ReadLine();
            return (selectNewLevel == "y" || selectNewLevel == "Y" || selectNewLevel == "Yes" || selectNewLevel == "YES");
        }

        Console.WriteLine("Try again(y/n)"); 
        again = Console.ReadLine();

        if (int.TryParse(again, out int value)){
            Console.WriteLine("\nPlease enter only either \"y\" or \"n\""); 
            Console.WriteLine("Try again(y/n)"); 
            again = Console.ReadLine();
        }

        if (again == "y"){   
           UserGuess(min, max, difficulty, computerGuess);
        }
        return false;
    }

    public static bool Run(string name){
        int computerGuess;

        //Gets user input and translates to a difficulty range
        var (start, end, difficulty) = DifficultySelector(name);

        //Converts user difficulty range into a random computer guess (inclusive)
        computerGuess = new Random().Next(start,end);

        //Asks user for guess as input, compute the results, allow user to quit/play again/select new level
        return UserGuess(start, end, difficulty, computerGuess);
    }

    public static bool IsValidIntInput(string userInput, int min, int max){
        int value;

          try{
            value = Int32.Parse(userInput);
        } catch(Exception){
            Console.WriteLine("\nInput type should be only an integer value,  Please check your entry and try Again...\n");
            return false;
        }

        if (value < min|| value > max){
            Console.WriteLine("Input should be only an integer value between " + min + "-" + max + " (Inclusive)! Please Try Again...\n");
            return false;
        } else{
            return true;
        }    
    }

}

