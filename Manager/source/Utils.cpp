#include <switch.h>
#include "Utils.h"
#include <cstring>
#include <sstream>
#include <filesystem>
#include <sys/stat.h>
#include <gd.h>
#include "menu.h"

using namespace std;

namespace Utils
{
void printItems(const vector<string> &items, string menuTitle)
{
    printf(CONSOLE_MAGENTA "\x1b[0;%dH%s\n", (40 - ((int)menuTitle.size() / 2)), menuTitle.c_str());
    for (int i = 0; i < (int)items.size(); i++)
    {
        const char *prefix = " ";
        if (selection == i)
            prefix = ">";
        printf(CONSOLE_WHITE "%s%s", prefix, items[i].c_str());

        printf("\n");
    }
}

bool isPresenceActive()
{
    u64 pid = 0;
    pmdmntGetTitlePid(&pid, PresenceTID);
    if (pid > 0)
        return true;
    else
        return false;
}

Result DumpIcons()
{
    NsApplicationRecord *appRecords = new NsApplicationRecord[512];
    size_t actualAppRecordCnt = 0;
    Result rc;
    rc = nsListApplicationRecord(appRecords, sizeof(NsApplicationRecord) * 512, 0, &actualAppRecordCnt);
    if (R_FAILED(rc))
        return rc;

    mkdir("sdmc:/Icons", 0777);
    for (u32 i = 0; i < actualAppRecordCnt; i++)
    {
        stringstream ss;
        ss << "sdmc:/Icons/" << 0 << hex << appRecords[i].titleID << ".jpg";
        if (!filesystem::exists(ss.str()))
        {
            NsApplicationControlData *appControlData = new NsApplicationControlData();
            int urc = Utils::getAppControlData(appRecords[i].titleID, appControlData);
            if (R_FAILED(urc))
                return urc;
            gdImagePtr in = gdImageCreateFromJpegPtr(sizeof(appControlData->icon), appControlData->icon);
            if (!in)
            {
                return (MAKERESULT(417, 0));
            }
            gdImageSetInterpolationMethod(in, GD_BILINEAR_FIXED);
            gdImagePtr out = gdImageScale(in, 512, 512);
            if (!out)
            {
                return (MAKERESULT(417, 1));
            }
            FILE *file = fopen(ss.str().c_str(), "wb");
            gdImageJpeg(out, file, 100);
            fclose(file);

            gdImageDestroy(in);
            gdImageDestroy(out);

            delete appControlData;
        }
    }
    delete[] appRecords;
    return 0;
}

Result getAppControlData(u64 tid, NsApplicationControlData *appControlData)
{
    size_t appControlDataSize = 0;

    memset(appControlData, 0x00, sizeof(NsApplicationControlData));

    Result rc;
    rc = nsGetApplicationControlData(1, tid, appControlData, sizeof(NsApplicationControlData), &appControlDataSize);
    if (R_FAILED(rc))
    {
        delete appControlData;
        return rc;
    }

    return 0;
}

void startErrorScreen(Result rc)
{
    char str[35];
    sprintf(str, "Error: 0x%x", rc);
    printf(CONSOLE_RED "\x1b[21;%d%s", center(80, (int)strlen(str)), str);
    printf(CONSOLE_RED "\x1b[22;%dPress `+` to exit!", center(80, 17));
    consoleUpdate(nullptr);
    while (appletMainLoop())
    {
        hidScanInput();
        u64 kDown = hidKeysDown(CONTROLLER_P1_AUTO);
        if (kDown & KEY_PLUS)
        {
            consoleExit(nullptr);
            scene = -69;
        }
    }
}
} // namespace Utils
