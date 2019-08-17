#include "Utils.h"

using namespace std;

namespace Utils
{
NsApplicationControlData *getAppControlData(u64 tid)
{
    NsApplicationControlData *appControlData = new NsApplicationControlData();
    size_t appControlDataSize = 0;

    Result rc;
    rc = nsGetApplicationControlData(1, tid, appControlData, sizeof(NsApplicationControlData), &appControlDataSize);
    if (R_FAILED(rc))
    {
        delete appControlData;
        fatalSimple(rc);
    }

    return appControlData;
}

string getAppName(u64 tid)
{
    NsApplicationControlData *appControlData = getAppControlData(tid);
    NacpLanguageEntry *languageEntry = nullptr;
    Result rc;
    rc = nacpGetLanguageEntry(&appControlData->nacp, &languageEntry);
    delete appControlData;
    if (R_FAILED(rc))
        fatalSimple(rc);

    if (languageEntry == nullptr)
        return string("A Game");
    else
        return string(languageEntry->name);
}
} // namespace Utils
