
#include <string>
#include "aes.h"

// 此类是从 dllExport.dll 导出的 
class   AesEncryptor {
public:
      AesEncryptor(unsigned char* key);
	~AesEncryptor(void);

	std::string EncryptString(std::string strInfor);
	std::string DecryptString(std::string strMessage);
	static std::string Encrypt(std::string md5);

	void EncryptTxtFile(const char* inputFileName, const char* outputFileName);
	void DecryptTxtFile(const char* inputFileName, const char* outputFileName);

private:
	void Byte2Hex(const unsigned char* src, int len, char* dest);
	void Hex2Byte(const char* src, int len, unsigned char* dest);
	int  Char2Int(char c);
private:
	AES* m_pEncryptor;
};


