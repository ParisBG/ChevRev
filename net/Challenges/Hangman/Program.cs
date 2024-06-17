/*
Hangman - Play a game of hangman with the computer. 
A "random" word is chosen, and the number of letters is displayed. 
The player has to guess letters, which are revealed in the word if correct. 
Incorrect guessed can be counted, or used as a countdown against the player. 

Challenge: Use formatting in the console display to "update" the display without moving down the terminal output. 
Keep the hidden word, previously guessed letters, and the players input in fixed locations on screen.
*/


using System;
using System.Net.NetworkInformation;
using System.IO;
using System.Security.Cryptography.X509Certificates;

public class HangMan {
    public static List<string> allGuesses = new List<string>();

    public static void Main(string[] args){
        List<string> words;
        string error = "NONE";
        bool userVictory = false;
        string hiddenWord;
        int difficulty;

        //Removes definitions, white spaces, and duplicates from .txt file
        words = PreProcessTxtFile("-");

        //Prompt user to select their difficulty
        difficulty = DifficultySelector();

        //Divvy the words into buckets (lists) of 9 words a piece based on word length (easy <= 9 words, med <= 12 words, and hard <= 17 words), return game word
        (string gameWord, string level, int countdown) = GenerateGameWord(words,difficulty);

        Console.WriteLine($"gameWord.Length before encryption: {gameWord.Count()}");

        //Hides the game word
        hiddenWord = EncryptWord(gameWord, false);
    
        while(countdown > 0 && !userVictory){
            Console.Clear(); 

            //Displays the ghetto Hangman figure, the level, number of letters, and the hidden word in the console
            DisplayHangman(gameWord,level);

            //Allows user to guess word until countdown == 0
            (countdown, error, hiddenWord, userVictory) = RunGame(gameWord,countdown, error, hiddenWord);
                
        }

        Console.WriteLine("\nCONGRATULATIONS, YOU HAVE ACHIEVED VICTORY!\n");
    }

    public static List<string> PreProcessTxtFile(string deliminator){
        //https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/read-write-text-file
        string line;
        string word;
        List<string> words = new List<string>();
        string rawTxtPath = "rawVocab.txt";
        
        try {
            StreamReader sr = new StreamReader(rawTxtPath);

            //ERROR: Exception: Object reference not set to an instance of an object.
            do {
                line = sr.ReadLine();
                word = line.Split(deliminator)[0];

                //Prevent duplicates and empty spaces from going in list
                if (word.Count() != 0 && !words.Contains(word)){ words.Add(word); }

            } while (line != null);

            sr.Close();
            Console.ReadLine();

        } catch(Exception e){
            Console.WriteLine("Exception: " + e.Message);

        } finally {
            Console.WriteLine("Executing finally block.");
        }

        return words;
    }

    public static void Testing(List<string> words){        
        //Sort by count words list by word length
        //words.Sort((a,b) => a.Length - b.Length);

        //int min, max = words[0].Count();
        int min = words[0].Count();
        int max = words[0].Count();

        foreach (string word in words){
            if (word.Count() < min){
                min = word.Count();
            }

            if (word.Count() > max){
                max = word.Count();
            }
        }

        Console.WriteLine($"\n\nmin = {min}");
        Console.WriteLine($"max = {max}");
    }

    public static (string,string, int) GenerateGameWord(List<string> words, int difficulty){
        int limit = 9;
        int wordLengthMax;
        string level;
        double multiplier;
        string gameWord;
        int countdown;


        //Create a list of 9 appropriately sized words with a word length based on difficulty selected
        switch(difficulty){
        case 1:
            wordLengthMax = 9;
            level = "Easy";
            multiplier = 2;
            break;

        case 2:
            wordLengthMax = 12;
            level = "Medium";
            multiplier = 1.5;
            break;

        case 3:
            wordLengthMax = 17;
            level = "Hard";
            multiplier = 1.2;
            break;

        default:
            wordLengthMax = 11;
            level = "Error Level";
            multiplier = 1.7;
            break;
        }

        //Shaves down our original, 130+ word list into 9 filled with only words of the appropriate length, then randomly selects one of those words
        gameWord = BucketizeWords(words, wordLengthMax,limit).Trim();

        //Calculate # of tries (countdown) based on selected difficulty     
        countdown = (int)(gameWord.Count() * multiplier);

        return (gameWord, level, countdown);

    }

    public static string BucketizeWords(List<string> words, int wordLengthMax, int limit){
        List<string> gameOptions = new List<string>();
        string lowerCaseGameWord;
        string finalGameWord = "";

        //Bucketize words by difficulty/Count
        for (int i = 0; i < words.Count; i++){
            if (words[i].Count() <= wordLengthMax && gameOptions.Count < limit){
                gameOptions.Add(words[i]);
            }
        }

        lowerCaseGameWord = gameOptions[new Random().Next(limit)];

        for (int x = 0; x < lowerCaseGameWord.Length; x++){
            if (x == 0){
                finalGameWord += Char.ToString(lowerCaseGameWord[x]).ToUpper();
            } else{
                finalGameWord += lowerCaseGameWord[x];
            }
        }

        return finalGameWord;
    }
   
    public static int DifficultySelector(){
        string userInput;

        Console.WriteLine("\n*********** Choose Your Difficulty: ***********\n" );
        Console.WriteLine("[1] - Easy");
        Console.WriteLine("[2] - Medium");
        Console.WriteLine("[3] - Hard\n");
        
        do {
            Console.Write("Please select your difficulty by entering either 1, 2, or 3: ");
            userInput = Console.ReadLine();

        } while(!IsValidIntInput(userInput,1,3));

        return Int32.Parse(userInput);
    }

    public static bool IsValidIntInput(string userInput, int min, int max){
        int value;

        try{
            value = Int32.Parse(userInput);
        } catch(Exception){
            Console.WriteLine("\nInput type should be only an integer value,  Please check your entry and try Again...\n");
            return false;
        }

        if (value < min || value > max){
            Console.WriteLine("Input should be only an integer value between " + min + "-" + max + " (Inclusive)! Please Try Again...\n");
            return false;
        }

        return true;
    }

    public static void DisplayHangman(string gameWord, string level){
        //https://inventwithpython.com/bigbookpython/project34.html
        string hangman = 
        
         "       +---+     \n" +
         "       |   |     \n" +
         "       |   . .   \n" +
         "       |    ~    \n"+
         "       |         \n"+
         "       |         \n"+
        "    =====         \n";
        
        Console.WriteLine("*******************************");
        Console.WriteLine($"LEVEL: {level}");
        Console.WriteLine($"LETTERS: {gameWord.Count()}");
        //Console.WriteLine($"ANSWER: \"{gameWord}\" (DISPLAYED FOR TESTING PURPOSES ONLY...)\n");
        Console.WriteLine(hangman);
    }

    public static (int,string,string,bool) RunGame(string gameWord, int countdown, string error, string hiddenWord){
        string guesses = "";
        bool userVictory = false;
        string input;

        foreach (string guess in allGuesses){
            guesses += guess + " ";
        }

        Console.WriteLine($"ERROR: {error}\n"); 
        Console.WriteLine($"COUNTDOWN: {countdown}\t  \tWORD: {hiddenWord}\t   \tGUESSED: {guesses}");
        Console.Write($"Enter your guess: ");
        input = Console.ReadLine();

        //Perform validation to ensure user input is a single character or an attempt at the entire word
        error = IsValidGuess(input, gameWord);

        if (error == "NONE"){
            allGuesses.Add(input);
            (hiddenWord,userVictory,countdown) = IsGuessCorrect(input, gameWord, hiddenWord, countdown);
        }

        return (countdown, error, hiddenWord, userVictory);
    }

    public static string IsValidGuess(string userInput, string gameWord){
        string error = "NONE";

        if (userInput.Any(char.IsDigit)){
            error = "Guesses should only contain letters, not numbers.";
            
        } else if (userInput.Length != 1 && userInput.Length != gameWord.Count()){
            error = "Guesses must either only be a single letter or the entire word. Please try again...";

        } else if (allGuesses.Contains(userInput)){
            error = "Guesses must be unique! Please avoid re-guessing previous guesses and try again...";
        } else {
            error = "NONE";
        }

        return error;
    }

    public static (string,bool, int) IsGuessCorrect(string userInput, string gameWord, string hiddenWord, int countdown){
        //Ensure that case sensitivity will not affect game results
        gameWord = gameWord.ToLower();
        userInput = userInput.ToLower();

        if (userInput == gameWord){
            hiddenWord = gameWord;

        } else if (gameWord.Contains(userInput)){
            hiddenWord = RevealHiddenWord(userInput, gameWord, hiddenWord);
            if (hiddenWord != gameWord){
                countdown--;
            }

        } else {
            //If users guess was valid but completely incorrect...
            countdown--;
        }

        return (hiddenWord, (hiddenWord == gameWord), countdown);
    }

    public static string EncryptWord(string gameWord, bool partiallyRevelead){
        string hiddenWord = "";
        string secret;

        //Replace gameWord with stars if user has not yet guessed anything
        if (!partiallyRevelead){
            for (int i = 0; i < gameWord.Count(); i++){
                //Ensures end of word does have any extra whitespace
                secret = (i == gameWord.Count()-1) ? "*" : "* ";
                hiddenWord += secret;
            }
        } else {
            //Format the hidden word to reveal users correct guesses spaced neatly
            for (int i = 0; i < gameWord.Count(); i++){
                //Format the hidden word to reveal users correct guesses spaced neatly
                secret = (i == gameWord.Count()-1) ? Char.ToString(gameWord[i]) : Char.ToString(gameWord[i]) + " ";
                hiddenWord += secret;
            }
        }

        return hiddenWord;
    }

    public static string RevealHiddenWord(string userInput, string gameWord, string hiddenWord){
        List<int> occurrences = new List<int>();
        string[] hiddenNoSpace = hiddenWord.Split(' ');

        //Find all occurrences of the correct user guess in gameWord string and populate a dict with char as key and index as val
        for (int i = 0; i < gameWord.Count(); i++){
            if (userInput == Char.ToString(gameWord[i])){
                occurrences.Add(i);
            }
        }

        for (int i = 0; i < occurrences.Count(); i++){
            //Swap out hidden word "*" with actual correct letter every time it appears in word
            hiddenNoSpace[occurrences[i]] = userInput;
        }

        //Reformat hidden word so that each character is neatly spaced unless the game is over, if so then just return the game word
        return (String.Concat(hiddenNoSpace) == gameWord) ? gameWord: EncryptWord(String.Concat(hiddenNoSpace),true);
    }

}

