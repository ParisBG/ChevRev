public class Program{
    /* TO DO:
    -Fix try/catch block - not accepting 0 nor 1
    -Add Try/Catch to Register() - name input & 
    -Format output better
    */
    
    // Main method: Get user's name, gets users flip guess, randomly flips coin, compares guess with flip result, outputs results, loops until user exits.
    public static void Main(string[] args){
        string name = Register();
        bool exit = false;
        int count = 0;
        string userGuessValue;
        string result;

        while(!exit){
            userGuessValue = (UserGuess() == 0) ? "Heads" : "Tails";
            exit = CoinFlip(name, (count+1),userGuessValue);
            count++;
        }

        Console.WriteLine("Thanks for playing! \n");
    }

    // CoinFlip method: regurgitates user's guess & flip result, converts flip result to a string, compares guess with result, gives user an exit option
    public static bool CoinFlip(string name, int iteration, string userGuess){
        Console.WriteLine("Flipping in progress...\n");

        Random rand = new Random();
        string flipResult; //True == Tails else Heads
        string? again;
        int value = rand.Next();
        int remainder = value % 2;
    
        Console.WriteLine(name + "' Guess " + iteration + " Results = " + userGuess);

        flipResult = (remainder == 0) ? "Heads" : "Tails";
        Console.WriteLine(name + "' Flip " + iteration + " Results = " + flipResult + "!");

        if (userGuess == flipResult){
            Console.WriteLine("Congratulations! you guessed correctly. \n");
        } else {
            Console.WriteLine("Ooooh! unfortuantely you guessed incorrectly. \n");
        }

        Console.WriteLine("Would you like to flip again (y/n)? ");
        again =  Console.ReadLine();

 

        return (again == "n" || again == "N" || again == "no" || again == "NO" || again == "No");
    }

    // Register method: Asks user to input their name    
    public static string Register(){
        Console.WriteLine("Please your name: ");
        return Console.ReadLine();
    }

    // UserGuess method: Asks user to input an int as a prediction, uses try/catch statements to handle various incorrect user inputs
    public static int UserGuess(){
        int prediction;
        Console.WriteLine("What is your coin flip prediction?");
        Console.WriteLine("Enter: \n 0 = Heads \n OR \n 1 = Tails \n");
        string rawInput = Console.ReadLine();

        try {
             prediction = Int32.Parse(rawInput);
        } catch (FormatException){
            Console.WriteLine("Input should only be either 1 or 0. Try again.");
            UserGuess();

        } catch (ArgumentNullException){
            Console.WriteLine("Your input can not be empty, please enter a number.");
            UserGuess();

        } catch (OverflowException){
            Console.WriteLine("Your input must be a 32 bit integer, please enter a smaller number.");
            UserGuess();

        } catch (Exception){
            Console.WriteLine("Unexpected input, please enter an integer and try again.");
            UserGuess();

        } finally {

            if (prediction != 0 || prediction != 1){
                UserGuess();
            }
        }

         return prediction;

       

    }
}