#include "FileManager.h"

String fileName;
File file;

FileManager::FileManager(String _fileName)
{
    fileName = _fileName;
}

bool FileManager::init(){

    return SPIFFS.begin(true);
}

bool FileManager::writeInFile(String line)
{
    String errorMsg;

    if (file)
        file.close();

    file = SPIFFS.open(fileName, FILE_APPEND);

    if (file)
    {
        if(file.println(line)){
           Serial.println("- file written");
        }else{
           Serial.println("- frite failed");
        }
        file.close();

        return true;
    }
    else
    {
        errorMsg = "Failed to open the file: " + String(fileName.c_str());
         Serial.println(errorMsg);
    }

    return false;
}

bool FileManager::destroy()
{
    if (file)
        file.close();

    return SPIFFS.remove((char *)fileName.c_str());
}

String FileManager::readFile()
{
    char *delimiter = "*";
    char *line;
    String contentFile = String("");

    if (file)
        file.close();

    file = SPIFFS.open(fileName.c_str());

    if (file)
    {
        while (file.available())
        {   
            contentFile += char(file.read());
        }
    }
    else
    {
        String errorMsg = "\nFailed in read file: " + String(fileName + "\n");
    }

    return contentFile;
}

void FileManager::newFile()
{
    if (file)
        file.close();

    SPIFFS.remove((char *)fileName.c_str());
    file = SPIFFS.open(fileName.c_str(), FILE_WRITE);
    file.close();
}

int FileManager::getUsedSpace()
{
    return SPIFFS.usedBytes();
}
