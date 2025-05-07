using System;
using System.Numerics;

class RSA
{
    static void Main()
    {
        //Loop to repeatedly show menu options until user chooses to exit
        while (true)
        {
            Console.WriteLine("\nRSA Algorithm");
            Console.WriteLine("1. Automatic: ");
            Console.WriteLine("2. Manual: ");
            Console.WriteLine("3. Exit");
            Console.Write("Choose: ");
            int choice = int.Parse(Console.ReadLine());

            //Exit the loop if option 3 is selected
            if (choice == 3) break;

            if (choice == 1)
            {
                Auto();
            }
            else if (choice == 2)
            {
                Manual();
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }
    }

    //Function for automatic RSA key and message generation
    static void Auto()
    {
        Random rand = new Random();
        int p = GenRandPrime(rand, 100); //Generates random prime p
        int q = GenRandPrime(rand, 100); //Generates random prime q
        Console.WriteLine($"Randomly generated primes: p = {p}, q = {q}");

        int n = p * q; //Calulates the n = p * q
        int phi = (p - 1) * (q - 1); //Calculates Euler's Totient function

        int e = GenRandE(phi, rand); //Generates a random public key exponent
        Console.WriteLine($"Randomly generated public key exponent e = {e}");

        int d = ModInv(e, phi); //Calculates the private key exponent which is the modular inverse of e
        Console.WriteLine($"Calculated private key exponent d = {d}");

        // Encryption
        int m = rand.Next(1, n); // Generating a random integer message
        Console.WriteLine($"Randomly generated message m = {m}");

        int c = SqMl(m, e, n); //Encrypts the message using public key
        Console.WriteLine($"Encrypted ciphertext c = {c}");

        // Decryption
        int deM = SqMl(c, d, n); //Decrypts the chipher text using private key
        Console.WriteLine($"Decrypted message = {deM}");
    }

    //Function for manual RSA key and message input
    static void Manual()
    {
        //User inputs prime numbers p and q
        Console.Write("Enter prime number p: ");
        int p = int.Parse(Console.ReadLine());

        Console.Write("Enter prime number q: ");
        int q = int.Parse(Console.ReadLine());

        int n = p * q; // Calculates n = p * q 
        int phi = (p - 1) * (q - 1); //Euler's Totient function is calculated

        //Display a list of valid public key exponents
        Console.WriteLine("Choose a vaild key from the list below");
        List<int> validKeys = new List<int>();
        for (int i = 2; i < phi; i++)
        {
            if (GCD(i, phi) == 1)
            {
                validKeys.Add(i);
            }
        }

        // Print the list of valid keys
        Console.WriteLine("Choose a valid public key exponent (e) from the list below:");
        foreach (int key in validKeys)
        {
            Console.Write(key + " ");
        }
        Console.WriteLine();
        Console.WriteLine("Enter public key exponent e: ");
        int e = int.Parse(Console.ReadLine());

        int d = ModInv(e, phi); //Calculates the private key exponent
        Console.WriteLine($"Private key exponent d = {d}");

        //Encryption
        Console.Write("Enter a message to encrypt: ");
        int m = int.Parse(Console.ReadLine());

        int c = SqMl(m, e, n); //Encrypts the message
        Console.WriteLine($"Encrypted ciphertext = {c}");

        // Decryption
        int deM = SqMl(c, d, n); //Decrypts the cipher text
        Console.WriteLine($"Decrypted message = {deM}");

    }

    //Function to generate random prime numbers within a specified limit
    static int GenRandPrime(Random rand, int limit)
    {
        int prime;
        do
        {
            prime = rand.Next(2, limit); //Generate random numbers between 2 and limit
        } while (!Prime(prime)); //Repeat until a prime number is found
        return prime;
    }

    //Function to check if a number is prime
    static bool Prime(int num)
    {
        if (num <= 1) return false; //Prime numbers are greater than 1
        for (int i = 2; i * i <= num; i++)
        {
            if (num % i == 0) return false;
        }
        return true;
    }

    //Function to generate a valid public key exponent e
    static int GenRandE(int phi, Random rand)
    {
        int e;
        do
        {
            e = rand.Next(2, phi); //Generate random e between 2 and phi
        } while (GCD(e, phi) != 1); //Repeat until GCD(e, phi) = 1 (valid e)
        return e;
    }

    //Function to calculate the GCD of two numbers using the Euclidean algorithm
    static int GCD(int v, int w)
    {
        while (w != 0) //Continue until w = 0
        {
            int temp = w;
            w = v % w; //Replace w with v mod w
            v = temp; //Replace v with the old w value
        }
        return v; //Return the GCD (v when w = 0)
    }

    //Function to calculate modular inverse of e mod phi using Extended Euclidean algorithm
    static int ModInv(int e, int phi)
    {
        int t = 0, newT = 1; //Initialize coefficients for Extended Euclidean algorithm
        int r = phi, newR = e; //Set r = phi and newR = e

        while (newR != 0) //Continue until newR = 0
        {
            int quot = r / newR; //Calculate quotient
            (t, newT) = (newT, t - quot * newT); //Update coefficients
            (r, newR) = (newR, r - quot * newR); //Update remainders
        }

        if (r > 1) throw new ArgumentException("e is not invertible"); //If only e and phi are not coprime
        if (t < 0) t += phi;  // Ensure t (d) is positive

        return t; //Return the modular inverse
    }

    //Function to perform modular exponentiation (Square and Multiply algorithm)
    static int SqMl(int baseNo, int exp, int mod)
    {
        BigInteger result = 1; //Start result as 1
        BigInteger basebig = baseNo; //Convert base to BigInteger for large number handling

        while (exp > 0) //Loop until exp becomes 0
        {
            if ((exp & 1) == 1) //If the least significant bit of exp is 1
            {
                result = (result * basebig) % mod; //Multiply result by base and mod
            }
            basebig = (basebig * basebig) % mod; //Square the base and mod
            exp >>= 1; //Right shift exponent 
        }
        return (int)result; //Return the final result mod n
    }
}