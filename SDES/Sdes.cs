using System;

class SDES
{
    //Permutation tables for the different steps in the S-DES algorithm
    static int[] P10 = { 3, 5, 2, 7, 4, 10, 1, 9, 8, 6 };
    static int[] P8 = { 6, 3, 7, 4, 8, 5, 10, 9 };
    static int[] P4 = { 2, 3, 4, 1 };
    static int[] IP = { 2, 6, 3, 1, 4, 8, 5, 7 }; //Initial Permutation
    static int[] IPminus1 = { 4, 1, 3, 5, 7, 2, 8, 6 }; //Inverse Initial Permutation
    static int[] EP = { 4, 1, 2, 3, 2, 3, 4, 1 }; //Expanded Permutation

    //S-boxes used in the Fk function
    static int[,] S0 = {
        {1,0,3,2},
        {3,2,1,0},
        {0,2,1,3},
        {3,1,3,2}
    };

    static int[,] S1 = {
        {0,1,2,3},
        {2,0,1,3},
        {3,0,1,0},
        {2,1,0,3}
    };

    static void Main()
    {
        //Loop to allow the user to keep using the system until they choose to stop by exiting
        while (true)
        {
            Console.WriteLine("S-DES System");
            Console.WriteLine("1. Encrypt?");
            Console.WriteLine("2. Decrypt?");
            Console.WriteLine("3. Exit?");
            Console.Write("Whats your options?: ");
            int choice = int.Parse(Console.ReadLine());

            //Exits the loop
            if (choice == 3) break;

            //Get 8-bit input where it could be either plaintext or ciphertext, and 10-bit key
            Console.Write("Enter an 8-bit binary input: ");
            string input = Console.ReadLine();
            //Check if plaintext or ciphertext length is valid
            if (input.Length != 8)
            {
                Console.WriteLine("The plaintext must be an 8-bit binary number");
                continue; //Allows users to input again
            }
            Console.Write("Enter a 10-bit binary key: ");
            string key = Console.ReadLine();
            //Check if key length is valid
            if (key.Length != 10)
            { 
                Console.WriteLine("The key must be a 10-bit binary number");
                continue; //Allows users to input again
            }

            //Encrypts or decrypts the input
            string result = (choice == 1) ? Encrypt(input, key) : Decrypt(input, key);
            Console.WriteLine("The output is: " + result);
        }
    }

    //Encrypts the 8-bit plaintext using the 10-bit key
    static string Encrypt(string plaintext, string key)
    {
        //Generate K1 and K2 subkeys
        string[] keys = GenerateKey(key);
        Console.WriteLine("Encryption using Key 1 (K1): " + keys[0]);
        Console.WriteLine("Encryption using Key 2 (K2): " + keys[1]);
        //Initial permutation (IP) on the plaintext
        string initial = Permute(plaintext, IP);
        //Apply Fk (Fiestal function) with K1, swap the left and right halves, and apply Fk with K2
        string fk1 = Fk(initial, keys[0]);
        string swapped = Swap(fk1);
        string fk2 = Fk(swapped, keys[1]);
        //Apply inverse initial permutation (IP^-1) to get the ciphertext
        string cipher = Permute(fk2, IPminus1);
        return cipher;
    }

    //Decrypts the 8-bit ciphertext using the 10-bit key
    static string Decrypt(string ciphertext, string key)
    {
        //Generate K1 and K2 subkeys
        string[] keys = GenerateKey(key);
        Console.WriteLine("Decryption using Key 2 (K2): " + keys[1]);
        Console.WriteLine("Decryption using Key 1 (K1): " + keys[0]);
        //Initial permutation (IP) on the ciphertext
        string initial = Permute(ciphertext, IP);
        //Apply Fk (Fiestal function) with K2(reverse order for decryption), swap, and apply Fk with K1
        string fk1 = Fk(initial, keys[1]);
        string swapped = Swap(fk1);
        string fk2 = Fk(swapped, keys[0]);
        //Apply inverse initial permutation (IP^-1) to get the plaintext
        string plain = Permute(fk2, IPminus1);
        return plain;
    }

    //Generates the two subkeys (K1 and K2) from the 10-bit key
    static string[] GenerateKey(string key)
    {
    //Apply P10 permutation to the key
    string pkey = Permute(key, P10);

    //Perform left shift and apply P8 to get K1
    string leftone = LeftShift(pkey.Substring(0, 5), 1) + LeftShift(pkey.Substring(5), 1);
    string k1 = Permute(leftone, P8);

    //Perform another left shift and apply P8 to get K2
    string lefttwo = LeftShift(leftone.Substring(0, 5), 2) + LeftShift(leftone.Substring(5), 2);
    string k2 = Permute(lefttwo, P8);

    Console.WriteLine("Generated Key 1: " + k1);
    Console.WriteLine("Generated Key 2: " + k2);

    return new string[] { k1, k2 };
    }


    //Fk function: applies the S-DES Fk operation using a subkey
    static string Fk(string input, string key)
    {
        //Split input into left and right halves
        string left = input.Substring(0, 4);
        string right = input.Substring(4);
        //Expand and permute the right half, then XOR with the subkey
        string epRight = Permute(right, EP);
        string xorResult = Xor(epRight, key);
        //Apply S-boxes and P4 permutation to the XOR result
        string sboxout = SBox(xorResult.Substring(0, 4), S0) + SBox(xorResult.Substring(4), S1);
        string psboxout = Permute(sboxout, P4);
        //XOR the result with the left half, then return the result concatenated with the unchanged right half
        string result = Xor(left, psboxout) + right;
        return result;
    }

    //Helper function to permute bits based on a given permutation table
    static string Permute(string input, int[] permutationTable)
    {
    string result = "";
    foreach (int index in permutationTable)
    {
        result += input[index - 1];  // -1 because permutation tables are 1-based.
    }
    return result;
    }


    //Left-shifts a binary string by a specified number of bits
    static string LeftShift(string input, int shifts)
    {
    return input.Substring(shifts) + input.Substring(0, shifts);
    }

    //XOR function to perform bitwise XOR on two binary strings
    static string Xor(string x, string y)
    {
        char[] result = new char[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            result[i] = (x[i] == y[i]) ? '0' : '1'; //If bits are the same, result is 0, else 1
        }
        return new string(result);
    }

    //S-box lookup function to get a 2-bit output from a 4-bit input using a given S-box
    static string SBox(string input, int[,] sbox)
    {
        //The row is determined by the first and last bits, and the column by the middle two bits
        int row = Convert.ToInt32(input[0].ToString() + input[3], 2);
        int column = Convert.ToInt32(input[1].ToString() + input[2], 2);
        return Convert.ToString(sbox[row, column], 2).PadLeft(2, '0');
    }

    //Swaps the left and right 4-bit halves of an 8-bit string
    static string Swap(string input)
    {
        return input.Substring(4) + input.Substring(0, 4);
    }
}
