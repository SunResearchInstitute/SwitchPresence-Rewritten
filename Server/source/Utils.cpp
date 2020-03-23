#include "Utils.h"
#include <cstring>

namespace Utils
{
const char *getAppName(u64 programId)
{
    static NsApplicationControlData appControlData;
    size_t appControlDataSize;
    NacpLanguageEntry *languageEntry;

    memset(&appControlData, 0, sizeof(NsApplicationControlData));

    if (R_SUCCEEDED(nsGetApplicationControlData(NsApplicationControlSource_Storage, programId, &appControlData, sizeof(NsApplicationControlData), &appControlDataSize)))
    {
        if (R_SUCCEEDED(nacpGetLanguageEntry(&appControlData.nacp, &languageEntry)))
        {
            if (languageEntry != nullptr)
                return languageEntry->name;
        }
    }
    return "A Game";
}
} // namespace Utils
