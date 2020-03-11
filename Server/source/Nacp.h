#pragma once
#include <switch.h>

// Used when initializing. gets a set session and retrieve the system language to an internal variable.
Result getSystemLanguage();

// This is nacpGetLanguageEntry from libnx, except we don't want to call setInitialize and setGetSystemLanguage every time we call the function, so we edit them out.
Result getLanguageEntry(NacpStruct *nacp, NacpLanguageEntry **langentry);