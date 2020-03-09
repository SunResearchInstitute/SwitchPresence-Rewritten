#include "Utils.h"
#include <cstring>

// needed for getLanguageEntry because libnx hid it internally
constexpr static u32 g_nacpLanguageTable[15] = {
    [SetLanguage_JA]    = 2,
    [SetLanguage_ENUS]  = 0,
    [SetLanguage_FR]    = 3,
    [SetLanguage_DE]    = 4,
    [SetLanguage_IT]    = 7,
    [SetLanguage_ES]    = 6,
    [SetLanguage_ZHCN]  = 14,
    [SetLanguage_KO]    = 12,
    [SetLanguage_NL]    = 8,
    [SetLanguage_PT]    = 10,
    [SetLanguage_RU]    = 11,
    [SetLanguage_ZHTW]  = 13,
    [SetLanguage_ENGB]  = 1,
    [SetLanguage_FRCA]  = 9,
    [SetLanguage_ES419] = 5,
};

namespace Utils
{

SetLanguage systemLanguage{};

Result getSystemLanguage()
{
    Result rc = 0;
    u64 LanguageCode;
    SetLanguage Language;

    rc = setInitialize();
    if (R_SUCCEEDED(rc))
    {
        rc = setGetSystemLanguage(&LanguageCode);
        if (R_SUCCEEDED(rc))
        {
            rc = setMakeLanguage(LanguageCode, &Language);
            if (R_SUCCEEDED(rc))
            {
                if (Language < 0)
                    rc = MAKERESULT(Module_Libnx, LibnxError_BadInput);
                if (Language >= 15)
                    Language = SetLanguage_ENUS;
            }
        }
    }

    if (R_SUCCEEDED(rc))
        systemLanguage = Language;

    setExit();
    return rc;
}

Result getLanguageEntry(NacpStruct* nacp, NacpLanguageEntry** langentry)
{
    NacpLanguageEntry *entry{};
    u32 i{};

    if (nacp == nullptr || langentry == nullptr)
        return MAKERESULT(Module_Libnx, LibnxError_BadInput);

    *langentry = nullptr;

    entry = &nacp->lang[g_nacpLanguageTable[systemLanguage]];

    if (entry->name[0]==0 && entry->author[0]==0) {
        for(i=0; i<16; i++) {
            entry = &nacp->lang[i];
            if (entry->name[0] || entry->author[0]) break;
        }
    }

    if (entry->name[0]==0 && entry->author[0]==0)
        return MAKERESULT(Module_Libnx, LibnxError_BadInput);

    *langentry = entry;

    return 0;
}

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
