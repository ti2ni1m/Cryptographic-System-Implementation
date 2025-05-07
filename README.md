# Cryptographic-System-Implementation

## SDES

### Description:

The program implements a simplified version of the DES or Data Encryption Standard algorithm. SDES operates on a plain-text which in 8-bit binary, using a 10-bit binary key to either encrypt or decrypt. The solution should generate two 8-bit subkeys which are known as K1 and K2 from a 10-bit input key using the permutations and left shifts, using an 8-bit plaintext, applying the initial permutation, following the Fiestel structure, using the 2 subkeys encrypts the plaintext into a ciphertext, and like Encryption, to decrypt the ciphertext, the process reverses it by using the same subkeys in reverse order. The main components include the key generation, permutation, substitution, and XOR operations.

### Test Plan:

To make sure that the implementation of SDES correctly encrypts the plaintext, decrypts the ciphertext back to original form, and output the subkeys, there will be plenty of tests taking place to show if they work. These tests are the key generation 1010000010 gives out two subkeys 10100100 and 01000011 respectively, both an encryption and decryption process where the key will be the same and the plaintext 10101010 should give out the ciphertext 01100000 and the ciphertext should decrypt it back to the exact plaintext given, and to give an invalid key length and plaintext length to see if a message prints out an error.


### Test Results:

From the expected results, it did output the actual results that was expected to be. The key generation did output the 2 subkeys, and the encryption of the plaintext did give the ciphertext and decryption returned to the original plaintext as expected. As well as, giving the error message if the input is invalid.

### Simple Manual for Testing:

To run the program, you can click on the down button next to play and click "Run associated project with this file". If not working, add the Practical-Assignment file into the WorkSpace, then open a New Terminal by clicking Terminal section next to Run and select Practical Assignment. When the two files are given, command ‘cd SDES’ for SDES file or ‘cd RSA’ for RSA. Next, for both, command ‘dotnet run’ to run the program.

After running the program, input an 8-bit text, and 10-bit key, it will out put the 2 subkeys, encrypted ciphertext if plaintext wants to be encrypted, and decrypted plaintext if ciphertext wants to be decrypted. The invalid output should print out a message.

---

## RSA

### Description:

The program uses RSA algorithm to performs encryption and decryption. It has two modes which are Automatic and Manual. In Automatic, two prime numbers are randomly generated, calculates n=p*q and Euler’s Totient function, randomly selects a valid public key exponent and ensures it is coprime with phi, calculates the private key exponent using modular inverse, randomly generates a message, encrypts it using public key and then decrypts it using private key. In Manual, two prime numbers are inputted, displays a list of valid public key and allows user to choose one, and then encrypts and decrypts the message.


### Test Plan:

To make sure that the implementation of RSA correctly performs key generation, message encryption, and decryption in both automatic and manual modes. These tests are to automatically get 2 prime numbers and the message and their encrypted and decrypted messages, manual inputs with valid prime numbers p=11, q=17, e=59, m=31 and manual inputs with numbers p=12, q=21 and invalid public key e=5 to see if there is a message.


### Test Result:

From the plan, the automatic mode did give 2 random prime numbers, a random public and private keys, and encrypted and decrypted messages. With the second test where p=11, q=17, e=59, m=31, the test was successful in printing the correct encrypted and decrypted messages which are 27 and 31 respectively. The third test was also successful in printing out the error message where the public key isn’t invertible.



### Simple Manual for Testing:

To run the program, you can click on the down button next to play and click "Run associated project with this file". If not working, add the Practical-Assignment file into the WorkSpace, then open a New Terminal by clicking Terminal section next to Run and select Practical Assignment. When the two files are given, command ‘cd SDES’ for SDES file or ‘cd RSA’ for RSA. Next, for both, command ‘dotnet run’ to run the program.

After running the program, choose either the Automatic or Manual mode. In automatic, the prime numbers will randomly generate, same with the public and private keys, and the message is encrypted and decrypted. In manual, enter the valid prime numbers, public key and message to get the encrypted ciphertext and decrypted plaintext.
