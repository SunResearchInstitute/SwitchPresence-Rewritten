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
    printf(CONSOLE_RED "Error: 0x%x\n", rc);
    printf(CONSOLE_RED "Press `+` to exit!");
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
