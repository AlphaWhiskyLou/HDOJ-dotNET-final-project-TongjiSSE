#include "pch.h"
#include "aes.h"
#include "aesencryptor.h"
#include "string.h"

#include <fstream>
using namespace std;

AesEncryptor::AesEncryptor(unsigned char* key)
{
	m_pEncryptor = new AES(key);
}


AesEncryptor::~AesEncryptor(void)
{
	delete m_pEncryptor;
}

void AesEncryptor::Byte2Hex(const unsigned char* src, int len, char* dest) {
	for (int i = 0; i < len; ++i) {
		sprintf_s(dest + i * 2, 3, "%02X", src[i]);
		//_snprintf_s(dest + i * 2, 3, "%02X", src[i]);
	}
}

void AesEncryptor::Hex2Byte(const char* src, int len, unsigned char* dest) {
	int length = len / 2;
	for (int i = 0; i < length; ++i) {
		dest[i] = Char2Int(src[i * 2]) * 16 + Char2Int(src[i * 2 + 1]);
	}
}

int AesEncryptor::Char2Int(char c) {
	if ('0' <= c && c <= '9') {
		return (c - '0');
	}
	else if ('a' <= c && c <= 'f') {
		return (c - 'a' + 10);
	}
	else if ('A' <= c && c <= 'F') {
		return (c - 'A' + 10);
	}
	return -1;
}
string AesEncryptor::Encrypt(std::string md5) {
	unsigned char key[16] = { 0x44, 0x7e, 0x88, 0x86 };
	AesEncryptor l_AES(key);
	std::string AESstr = l_AES.EncryptString(md5);
	return AESstr;
}
string AesEncryptor::EncryptString(string strInfor)
{
	int nLength = strInfor.length();
	int spaceLength = 16 - (nLength % 16);
	unsigned char* pBuffer = new unsigned char[nLength + spaceLength + 1];
	memset(pBuffer, '\0', nLength + spaceLength + 1);
	//        memcpy_s(pBuffer, nLength + spaceLength, strInfor.c_str(), nLength);
	memcpy(pBuffer, strInfor.c_str(), nLength);
	m_pEncryptor->Cipher(pBuffer, nLength + spaceLength);


	char* pOut = new char[2 * (nLength + spaceLength) + 1];
	memset(pOut, '\0', 2 * (nLength + spaceLength) + 1);
	Byte2Hex(pBuffer, nLength + spaceLength, pOut);

	string retValue(pOut);
	delete[] pBuffer;
	pBuffer = NULL;
	delete[] pOut;
	pOut = NULL;
	return retValue;
}

string AesEncryptor::DecryptString(string strMessage) {
	int nLength = strMessage.length() / 2;
	unsigned char* pBuffer = new unsigned char[nLength + 1];
	memset(pBuffer, '\0', nLength + 1);
	Hex2Byte(strMessage.c_str(), strMessage.length(), pBuffer);

	m_pEncryptor->InvCipher(pBuffer, nLength);
	string retValue((char*)pBuffer);
	delete[] pBuffer;
	return retValue;
}

void AesEncryptor::EncryptTxtFile(const char* inputFileName, const char* outputFileName) {
	ifstream ifs;

	// Open file:
	ifs.open(inputFileName);
	if (!ifs) {
		//UNILOGW("AesEncryptor::EncryptTxtFile() - Open input file failed!");
		return;
	}

	// Read config data:
	string strInfor;
	string strLine;
	while (!ifs.eof()) {
		char temp[1024];
		memset(temp, '\0', 1024);
		ifs.read(temp, 1000);
		strInfor += temp;
	}
	ifs.close();

	// Encrypt
	strLine = EncryptString(strInfor);

	// Writefile 
	ofstream ofs;
	ofs.open(outputFileName);
	if (!ofs) {
		//UNILOGW("AesEncryptor::EncryptTxtFile() - Open output file failed!");
		return;
	}
	ofs << strLine;
	ofs.close();
}

void AesEncryptor::DecryptTxtFile(const char* inputFile, const char* outputFile) {
	ifstream ifs;

	// Open file:
	ifs.open(inputFile);
	if (!ifs) {
		//UNILOGW("AesEncryptor::DecryptTxtFile() - Open input file failed!");
		return;
	}

	// Read config data:
	string strInfor;
	string strLine;
	while (!ifs.eof()) {
		char temp[1024];
		memset(temp, '\0', 1024);
		ifs.read(temp, 1000);
		strInfor += temp;
	}
	ifs.close();

	// Encrypt
	strLine = DecryptString(strInfor);

	// Writefile 
	ofstream ofs;
	ofs.open(outputFile);
	if (!ofs) {
		//UNILOGW("AesEncryptor::DecryptTxtFile() - Open output file failed!");
		return;
	}
	ofs << strLine;
	ofs.close();
}

