//PA3 Example
//THIS IS A CHANGE
//Initialize variables

using Microsoft.Win32.SafeHandles;

int [] credits = {0,0};

//Main
int userInput = GetMenuChoice(credits);
while (userInput != 4 && userInput!=5){
    Route(userInput, credits);
    userInput = GetMenuChoice(credits);
}
if (userInput ==4){
    int hours = credits[0] + credits[1];
    Console.WriteLine($"Thanks for playing! You earned a total of {hours} hours.");
}
else{
    EndingCredits();
}

//Menu methods
static int GetMenuChoice(int [] credits){
    if (ContinuePlaying(credits)){
        DisplayMenu();
        string userChoice = Console.ReadLine();
        if (IsValidChoice(userChoice)) {
            return int.Parse(userChoice);
        }
        else return 0;
    }
    return 5;
}

static bool ContinuePlaying(int [] credits){
    int total = credits[0]+credits[1];
    if (total == 6){
        return false;
    }
    else{
        return true;
    }
}

static void DisplayMenu(){
    Console.Clear();
    Console.WriteLine("Enter 1, 2, 3, or 4");
    Console.WriteLine("1. Password Cracker\n2. Spin the Wheel\n3. View Credits\n4. Exit");
}

static void Route(int userInput, int[] credits){
    if (userInput == 1){
        PasswordCracker(credits);
    }
    else if (userInput == 2){
        SpinWheel(credits);
    }
    else if(userInput == 3){
        ViewCredits(credits);
    }
    else{
        SayInvalid();
        PauseAction();
    }
}

static bool IsValidChoice(string userChoice) {
    if (userChoice == "1" || userChoice == "2" || userChoice == "3" || userChoice == "4" || userChoice == "5") {
        return true;
    }
    return false;
}

static void SayInvalid(){
    Console.WriteLine("Invalid input.");
}

static void PauseAction(){
    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}

//Password Cracker
static void PasswordCracker(int[] credits){
    if (!PasswordCreditLimit(credits)){
        PasswordInstructions();
        int userInput = GetUserChoice();
        while (!PasswordCreditLimit(credits)&& userInput != 0){ //check if user has received three points from password cracker
            Password(credits);
            if(!PasswordCreditLimit(credits)){
                PasswordInstructions();
                userInput = GetUserChoice();
            }
            else{
                Console.WriteLine("You have received 3 credit hours from Password Cracker!");
                PauseAction();
            }
        }
    }
    else{
        Console.WriteLine("You have received 3 credit hours from Password Cracker!");
        PauseAction();
    }
}

static void PasswordInstructions(){
    Console.Clear();
    Console.WriteLine("In this training, the computer will generate a random password");
    Console.WriteLine("You will have 10 tries to guess the password.");
    Console.WriteLine("If correct: \tyou will gain 1 credit hour!\nIf incorrect: \tyou will lose 1 credit hour!");
    Console.WriteLine("\nPress 1 to play, 0 to exit.");
}

static int GetUserChoice(){
    int value;
    bool goodVal = int.TryParse(Console.ReadLine(), out value); //returns true if user input matches int data type
    while(goodVal == false){
        SayInvalid();
        Console.WriteLine("Press 1 to play, 0 to exit.");
        goodVal = int.TryParse(Console.ReadLine(), out value);
    }
    return value;
}

static bool PasswordCreditLimit(int[] credits){  //returns if user has met game credit hour limit
    if (credits[0] >= 3){
        credits[0] = 3;
        
        return true;
    }
    else {
        return false;
    }
}

static void Password(int[] credits){
    int passwordCredits = credits[0];
    string word = GetRandomWord();
    char[] displayWord = SetDisplayWord(word);
    int missed = 0;
    string[] guesses = new string [10];
    int guessCount = 0;
    string guessedWord = "";

    while (KeepGoing(displayWord, missed))
    {
        ShowBoard(displayWord, missed, guesses, guessCount);
        Console.WriteLine();
        guessedWord = GetPasswordGuess(word);
        CheckChoice(displayWord, word, ref missed, guesses, ref guessCount, guessedWord);
        if(!KeepGoing(displayWord, missed)&&missed!=11){
            PasswordWin(ref guessedWord, word); //make sure user is entering in the actual password, not just letters
        }
    }

    if (missed == 11)
    {
        Console.WriteLine("Sorry you lost");
        passwordCredits--;
    }
    else {
        Console.Write("Password: ");
        for (int i = 0; i < displayWord.Length; i++)
        {
            Console.Write(displayWord[i]);
        }
        Console.WriteLine("\nYou Won!");
        passwordCredits++;
    }
    PauseAction();
    credits[0] = passwordCredits;
}

static string GetPasswordGuess(string word){
    string userInput = Console.ReadLine();
    while(userInput.Length != word.Length){
        System.Console.WriteLine($"You must guess a word that is {word.Length} letters long!");
        userInput = Console.ReadLine();
    }
    return userInput.ToUpper();
}

static void PasswordWin(ref string guessedWord, string word){
    while(guessedWord.ToUpper() != word){
        Console.WriteLine("Please enter the password spelled correctly (Enter ! for help):");
        guessedWord = Console.ReadLine();
        if (guessedWord == "!"){
            Console.WriteLine($"The password is spelled {word}!");
            guessedWord = word;
        }
    }
}

static void CheckChoice(char[] displayWord, string word, ref int missed,
                                string[] guesses, ref int guessCount, string guessedWord){

    if(InGuessList(guessedWord, guesses, guessCount)){
        Console.WriteLine($"{guessedWord} has already been guessed");
    }
    else{
        bool isCorrectGuess = false;
        char[] wordArray = word.ToCharArray();
        for(int i = 0; i<wordArray.Length;i++){
            if(guessedWord[i]==wordArray[i]){
                displayWord[i] = guessedWord[i];
                isCorrectGuess = true;
            }
        }
        if(!isCorrectGuess){
            missed++;
            Console.WriteLine("Word had no matching letters.");
            PauseAction();
        }

        guesses[guessCount] = guessedWord;
        guessCount++;
    }

    Console.Clear();
    
}



static bool InGuessList(string guessedWord, string[] guesses, int guessCount){
    for (int i=0; i < guessCount; i++){
        if (guessedWord == guesses[i]){
            return true;
        }
    }
    return false;
}

static bool KeepGoing(char[] displayWord, int missed){
    bool index = false;
    char check = '_';
    for(int i = 0; i < displayWord.Length; i++){
        if (check == displayWord[i]){
            index = true;
        }
    }
   
    if (missed < 11 && index == true){
        return true;
    }
    else return false;
}

static void ShowBoard(char[] displayWord, int missed, string[] guesses, int guessCount){
    Console.Clear();
    Console.WriteLine("Word to guess: ");
    for (int i = 0; i < displayWord.Length; i++)
    {
        Console.Write(displayWord[i]);
    }

    DisplayGuesses(guesses, guessCount);
    Console.WriteLine("Current times guessed: " + missed);

}

static void DisplayGuesses(string[] guesses, int guessCount){
    Console.WriteLine("\nWords Guessed: ");
    for(int i = 0; i<guessCount; i++){
        System.Console.WriteLine(guesses[i]+ ", ");
    }
    System.Console.WriteLine();
}

static char[] SetDisplayWord(string word){
    char[] letters = new char[word.Length];
    for (int i = 0; i < letters.Length; i++){
        letters[i]= '_';
    }
    /*SetDisplayWord to return a character array of 
    * underscores to match the word returned in step 3
    */

    return letters;
}

static string GetRandomWord(){
    // Selects a random word from the array below and returns it.
    
    string[] randomWords = new string[] { "SPY", "HACKER", "HERBERT", "PENGUIN", "SECRET"};
    Random rand = new Random();
    int index = rand.Next(randomWords.Length);

    return randomWords[index];
}

//Spin the Wheel
static void SpinWheel(int[] credits){
    if(!WheelCreditLimit(credits)){
        WheelInstructions();
        int userInput = GetUserChoice();
        while (!WheelCreditLimit(credits)&& userInput != 0){ //check if user has received three points from password cracker
            Wheel(credits);
            WheelInstructions();
            userInput = GetUserChoice();
        }
    }

}

static void WheelInstructions(){
    Console.Clear();
    Console.WriteLine("In this training, you will spin the wheel\nThere are several options you could land on:");
    Console.WriteLine("\tEarn 1 credit hour\n\tRandomly give or take 1 hour");
    Console.WriteLine("\tLose 1 credit hour\n\tLose all credits");
    Console.WriteLine("\tNothing happens\nEnter 1 to play, enter 0 to exit");
}

static bool WheelCreditLimit(int[] credits){
    if (credits[1] >= 3){
        credits[1] = 3;
        Console.WriteLine("You have received 3 credit hours from Spin the Wheel!");
        PauseAction();
        return true;
    }
    else {
        return false;
    }
}

static void Wheel(int[] credits){
    int wheelCredits = credits[1];
    Random rnd = new Random();
    int number = rnd.Next(5);
    switch(number){
        case 0:
            wheelCredits++;
            Console.WriteLine("You won one credit hour!");
            break;
        case 1:
            int random = rnd.Next(2);
            if (random == 0){
                wheelCredits+=2;
                Console.WriteLine("You won two credit hours!");
            }
            else{
                wheelCredits -=2;
                Console.WriteLine("You lost two credit hours!");
            }
            break;
        case 2:
            wheelCredits --;
            Console.WriteLine("You lose one credit hour!");
            break;
        case 3:
            if (wheelCredits <= 0){
                wheelCredits -= 2;
            }
            else{
                wheelCredits = 0;
            }
            Console.WriteLine("You lost all of your credits!");
            break;
        default: 
            Console.WriteLine("You landed on nothing!");
            break;

    }
    credits[1] = wheelCredits;
    PauseAction();
}

//View Ciara's Credits
static void ViewCredits(int[] credits){
    int total = credits[0] + credits[1];
    Console.WriteLine($"Total Credit Hours: {total}");
    Console.WriteLine($"Password Cracker Hours: {credits[0]}");
    Console.WriteLine($"Spin the Wheel Hours: {credits[1]}");
    PauseAction();
}

//Ending Credits: The user has received 3 credit hours from both games
static void EndingCredits(){
    Console.WriteLine("CONGRATS!! You have completed spy training!");
}
