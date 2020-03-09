#pragma once
#include <switch.h>
#include <string>
#include <functional>
#include <cstring>

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
const char *getAppName(u64 application_id);
} // namespace Utils
