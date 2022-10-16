//#include "pch.h"
//#include "aes.h"
//#include "md5.h"
//#include "aesencryptor.h"
//
//std::string  _stdcall Encrypt(std::string md5) {
//    return	"123";
//}
//std::string _stdcall GetMD5(std::string data) {
//    return "456";
// }
#include "pch.h" // use stdafx.h in Visual Studio 2017 and earlier
#include <utility>
#include <limits.h>
#include "MyDll.h"
#include "aesencryptor.h"
#include "md5.h"
#include <fstream> 
#include <iostream>
using namespace std;
// DLL internal state variables:
static unsigned long long previous_;  // Previous value, if any
static unsigned long long current_;   // Current sequence value
static unsigned index_;               // Current seq. position

// Initialize a Fibonacci relation sequence
// such that F(0) = a, F(1) = b.
// This function must be called before any other function.
void fibonacci_init(
    const unsigned long long a,
    const unsigned long long b)
{
    index_ = 0;
    current_ = a;
    previous_ = b; // see special case when initialized
}

// Produce the next value in the sequence.
// Returns true on success, false on overflow.
bool fibonacci_next()
{
    // check to see if we'd overflow result or position
    if ((ULLONG_MAX - previous_ < current_) ||
        (UINT_MAX == index_))
    {
        return false;
    }

    // Special case when index == 0, just return b value
    if (index_ > 0)
    {
        // otherwise, calculate next sequence value
        previous_ += current_;
    }
    std::swap(current_, previous_);
    ++index_;
    return true;
}

// Get the current value in the sequence.
unsigned long long fibonacci_current()
{
    return current_;
}

// Get the current index position in the sequence.
unsigned fibonacci_index()
{
    return index_;
}

unsigned char key[16] = { 0x40, 0x1e, 0x78, 0x26 };
AesEncryptor l_AES(key);
char returnsmall[1024];
char returnlarge[1024*30];
std::ofstream outfile;

int Encrypt(char* md5, char* result ) {
    std::string temp = std::string(md5);
	std::string AESstr = l_AES.EncryptString(temp);
    //const int len = AESstr.length();
    char* c =const_cast<char*>( AESstr.c_str());
    //strcpy_s(c, len+1,AESstr.c_str());
    int length = strlen(c);

    memset(returnsmall, 0, length);
    memcpy(returnsmall, c, length);
    memcpy(result, returnsmall, length);
    return  length;
}

int   GetMD5(char* msg, char* result)
{
    std::string temp = std::string(msg);
    std::string 	str = MD5(temp).toString();
    char* c= const_cast<char*>(str.c_str());
    int Length = strlen(c);
    memset(returnsmall, 0, Length);
    memcpy(returnsmall, c, Length);
    memcpy(result, returnsmall, Length);
    return  Length;
 }
int Decrypt(char* aes, char* md5) {
    std::string temp = std::string(aes);
    std::string str = l_AES.DecryptString(temp);
    char* c = const_cast<char*>(str.c_str());
    int Length = strlen(c);
    memset(returnsmall, 0, Length);
    memcpy(returnsmall, c, Length);
    memcpy(md5, returnsmall, Length);
    return  Length;
}
int StrToByte(char* strmsg, char* bytemsg) {
    
    int Length = strlen(strmsg);
    memset(returnsmall, 0, Length);
    memcpy(returnsmall, strmsg, Length);
    memcpy(bytemsg, returnsmall, Length);
    return  Length;
}
