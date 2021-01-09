namespace AdsPortal.WebApi.Application.Interfaces.Media
{
    using System;

    public interface IQRCodeService
    {
        byte[] CreateTextCode(string text, int pixelsPerModule = 16);
        byte[] CreateBinaryCode(byte[] data, int pixelsPerModule = 16);

        byte[] CreateWebCode(Uri uri, int pixelsPerModule = 16);

        byte[] CreateCalendarCode(string subject,
                                  string description,
                                  double latitude,
                                  double longitude,
                                  DateTime start,
                                  DateTime end,
                                  bool allDayEvent = false,
                                  int pixelsPerModule = 16);

        byte[] CreateCalendarCode(string subject,
                                  string description,
                                  double latitude,
                                  double longitude,
                                  DateTime start,
                                  TimeSpan duration,
                                  bool allDayEvent = false,
                                  int pixelsPerModule = 16);
    }
}
