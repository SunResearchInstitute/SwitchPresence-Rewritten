#include "Utils.h"
#include <cstring>
#include "Nacp.h"

namespace Utils
{
const char *getAppName(u64 application_id)
{
    static NsApplicationControlData appControlData;
    size_t appControlDataSize = 0;
    NacpLanguageEntry *languageEntry = nullptr;

    memset(&appControlData, 0x00, sizeof(NsApplicationControlData));

    if (R_SUCCEEDED(nsGetApplicationControlData(NsApplicationControlSource_Storage, application_id, &appControlData, sizeof(NsApplicationControlData), &appControlDataSize)))
    {
        if (R_SUCCEEDED(getLanguageEntry(&appControlData.nacp, &languageEntry)))
        {
            if (languageEntry != nullptr)
                return languageEntry->name;
        }
    }
    return "A Game";
}
} // namespace Utils
