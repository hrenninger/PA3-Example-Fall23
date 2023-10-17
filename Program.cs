//PA3 Example
//THIS IS A CHANGE
//Initialize variables

int [] credits = {0,0};

//Main
int userInput = GetMenuChoice(credits);
while (userInput != 4 && userInput!=5){
    Route(userInput, credits);
    userInput = GetMenuChoice(credits);
}
if (userInput ==4){
    Console.WriteLine("Thanks for playing!");
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
    PasswordInstructions();
    int userInput = GetUserChoice();
    while (!PasswordCreditLimit(credits)&& userInput == 1){ //check if user has received three points from password cracker
        Password(credits);
        PasswordInstructions();
        userInput = GetUserChoice();
    }
    if (PasswordCreditLimit(credits)){
        Console.WriteLine("You have received 3 credits from Password Cracker!");
        PauseAction();
    }
}

static void PasswordInstructions(){
    Console.Clear();
    Console.WriteLine("In this training, the computer will generate a random password");
    Console.WriteLine("\tPossible passwords: Spy, Hacker, Herbert, Penguin, Secret");
    Console.WriteLine("You will have 10 tries to guess the password.");
    Console.WriteLine("If correct: \n\tyou will gain 1 credit hour!\nIf incorrect:");
    Console.WriteLine("\tyou will lose 1 credit hour! \nPress 1 to play, 0 to exit.");
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
    Console.Clear();
    string word = GetRandomWord();
    char[] displayWord = SetDisplayWord(word);
    int missed = 0;
    string guessed = "No Letters Guessed Yet";
    string guessedWord = "";

    while (KeepGoing(displayWord, missed))
    {
        ShowBoard(displayWord, missed, guessed);
        Console.WriteLine();
        guessedWord = Console.ReadLine().ToUpper();
        CheckChoice(displayWord, word, ref missed, ref guessed, guessedWord);
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
                                ref string guessed, string guessedWord){
    int index = 0;
    char[] wordArray = word.ToCharArray();
    for(int i = 0; i < wordArray.Length; i++){
        for(int j =0; j< guessedWord.Length; j++){
            if (guessedWord[j] == wordArray[i]){
                index = 1;
                displayWord[i] = guessedWord[j];
            }
            else{
                if (guessed == "No Letters Guessed Yet" ){
                    guessed = guessedWord[j].ToString();
                }
                else if (NotInGuessed(guessed, guessedWord[j])){ //makes sure guessed does not have duplicates
                    guessed += " " + guessedWord[j].ToString();
                            
                }
            }
        }
    }
    missed++;
    
    if (index == 0){
        missed++;
        Console.WriteLine("Word had no matching letters.");
        PauseAction();
    }

    Console.Clear();
    
}

static bool NotInGuessed(string guessed, char incorrectGuess){
    char[] guess = guessed.ToCharArray();
    for (int i=0; i < guessed.Length; i++){
        if (guess[i] == incorrectGuess){
            return false;
        }
    }
    return true;
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

static void ShowBoard(char[] displayWord, int missed, string guessed){
    Console.WriteLine("Word to guess: ");
    for (int i = 0; i < displayWord.Length; i++)
    {
        Console.Write(displayWord[i]);
    }

    Console.WriteLine();
    Console.WriteLine("Letters guessed: " + guessed);

    Console.WriteLine("Current times guessed: " + missed);

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
    WheelInstructions();
    int userInput = GetUserChoice();
    while (!WheelCreditLimit(credits)&& userInput == 1){ //check if user has received three points from password cracker
        Wheel(credits);
        WheelInstructions();
        userInput = GetUserChoice();
    }
    if(WheelCreditLimit(credits)){
        Console.WriteLine("You have received 3 credits from Spin the Wheel!");
        PauseAction();
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
            wheelCredits =0;
            credits[0] = 0;
            Console.WriteLine("You lost all of your credits!");
            break;
        default: 
            Console.WriteLine("You landed on nothing!");
            break;
    }
    if(wheelCredits<0){
        wheelCredits = 0;
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
