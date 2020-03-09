#include "Utils.h"

namespace Utils
{
std::string getAppName(u64 tid)
{
    static NsApplicationControlData appControlData;
    size_t appControlDataSize = 0;
    NacpLanguageEntry *languageEntry = nullptr;

    R_ASSERT(nsGetApplicationControlData(NsApplicationControlSource_Storage, tid, &appControlData, sizeof(NsApplicationControlData), &appControlDataSize));

    R_ASSERT(nacpGetLanguageEntry(&appControlData.nacp, &languageEntry));

    if (languageEntry == nullptr)
        return std::string("A Game");
    else
        return std::string(languageEntry->name);
}
} // namespace Utils
