#include "Utils.h"

using namespace std;

namespace Utils
{
void printItems(const vector<string> &items, string menuTitle, int selection)
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

PresenceState getPresenceState()
{
    PresenceState state;
    u64 pid = 0;

    if (R_SUCCEEDED(pmdmntGetProcessId(&pid, TID)))
    {
        if (pid > 0)
            //note that this returns instantly
            //because the file might not exist but still be running
            return PresenceState::Enabled;
        else
            state = PresenceState::Error;
    }
    else
        state = PresenceState::Disabled;

    if (!filesystem::exists(PROGRAMDIR))
        state = PresenceState::NotFound;

    return state;
}

Result DumpIcons()
{
    NsApplicationRecord *appRecords = new NsApplicationRecord[512];
    s32 actualAppRecordCnt = 0;
    Result rc;
    rc = nsListApplicationRecord(appRecords, sizeof(NsApplicationRecord) * 512, 0, &actualAppRecordCnt);
    if (R_FAILED(rc))
    {
        delete[] appRecords;
        return rc;
    }

    mkdir("sdmc:/Icons", 0777);
    for (s32 i = 0; i < actualAppRecordCnt; i++)
    {
        stringstream ss;
        ss << "sdmc:/Icons/" << 0 << hex << appRecords[i].application_id << ".jpg";
        if (!filesystem::exists(ss.str()))
        {
            NsApplicationControlData *appControlData = new NsApplicationControlData();
            Result rc = Utils::getAppControlData(appRecords[i].application_id, appControlData);
            if (R_FAILED(rc))
            {
                delete appControlData;
                //2016-0050: GetApplicationControlData: unable to find control for the input title ID
                if (rc == MAKERESULT(16, 50))
                    continue;
                else
                    return rc;
            }

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
    rc = nsGetApplicationControlData(NsApplicationControlSource_Storage, tid, appControlData, sizeof(NsApplicationControlData), &appControlDataSize);
    if (R_FAILED(rc))
        return rc;

    return 0;
}

u64 GetControllerInputs()
{
    hidScanInput();
    u64 kDown = 0;
    for (u8 controller = 0; controller < 10; controller++)
        kDown |= hidKeysDown(static_cast<HidControllerID>(controller));

    return kDown;
}
} // namespace Utils
