#pragma once
#include <switch.h>

#define R_ASSERT(res_expr)          \
    ({                              \
        const auto rc = (res_expr); \
        if (R_FAILED(rc))           \
        {                           \
            fatalThrow(rc);         \
        }                           \
    })

namespace Utils
{
extern SetLanguage systemLanguage;

// Used when initializing. gets a set session and retrieve the system language to the utils variable.
Result getSystemLanguage();

// This is nacpGetLanguageEntry from libnx, except we don't want to call setInitialize and setGetSystemLanguage every time we call the function, so we edit them out.
Result getLanguageEntry(NacpStruct* nacp, NacpLanguageEntry** langentry);

const char *getAppName(u64 application_id);
} // namespace Utils
