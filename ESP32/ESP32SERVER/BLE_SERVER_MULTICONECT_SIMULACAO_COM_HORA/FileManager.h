#ifndef FileManager_h
#define FileManager_h

#include <SPIFFS.h>
#include <FS.h>

class FileManager{

    private :
          String fileName;
  
    public:
        FileManager(String);
        bool init();
        bool writeInFile(String);
        bool destroy();
        String readFile();
        void newFile();
        int getUsedSpace();
        
};

#endif
