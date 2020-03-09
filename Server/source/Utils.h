#pragma once
#include <switch.h>
#include <string>
#include <functional>

#define R_ASSERT(res_expr) \
    ({ \
        const auto rc = (res_expr); \
        if (R_FAILED(rc)) {  \
            fatalThrow(rc); \
        } \
    })

namespace Utils
{
std::string getAppName(u64 tid);
} // namespace Utils
