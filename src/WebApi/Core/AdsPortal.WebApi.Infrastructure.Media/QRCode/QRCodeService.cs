namespace AdsPortal.Infrastructure.Media.QRCode
{
    using System;
    using System.Globalization;
    using AdsPortal.WebApi.Application.Interfaces.Media;
    using QRCoder;
    using static QRCoder.PayloadGenerator;

    public class QRCodeService : IQRCodeService
    {
        private QRCodeGenerator QRGenerator { get; }

        public QRCodeService()
        {
            QRGenerator = new QRCodeGenerator();
        }

        public byte[] CreateTextCode(string text, int pixelsPerModule = 8)
        {
            QRCodeData qrCodeData = QRGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

            return QRCodeToBitmap(pixelsPerModule, qrCodeData);
        }

        public byte[] CreateBinaryCode(byte[] data, int pixelsPerModule = 8)
        {
            QRCodeData qrCodeData = QRGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);

            return QRCodeToBitmap(pixelsPerModule, qrCodeData);
        }

        public byte[] CreateWebCode(Uri uri, int pixelsPerModule = 8)
        {
            Url generator = new Url(uri.AbsoluteUri);

            string payload = generator.ToString();
            QRCodeData qrCodeData = QRGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);

            return QRCodeToBitmap(pixelsPerModule, qrCodeData);
        }

        public byte[] CreateCalendarCode(string subject,
                                         string description,
                                         double latitude,
                                         double longitude,
                                         DateTime start,
                                         TimeSpan duration,
                                         bool allDayEvent = false,
                                         int pixelsPerModule = 8)
        {
            return CreateCalendarCode(subject,
                                      description,
                                      latitude,
                                      longitude,
                                      start,
                                      start.Add(duration),
                                      allDayEvent,
                                      pixelsPerModule);
        }

        public byte[] CreateCalendarCode(string subject,
                                         string description,
                                         double latitude,
                                         double longitude,
                                         DateTime start,
                                         DateTime end,
                                         bool allDayEvent = false,
                                         int pixelsPerModule = 8)
        {
            CalendarEvent generator = new CalendarEvent(subject,
                                                        description,
                                                        string.Format(CultureInfo.InvariantCulture, "{0},{1}", latitude, longitude),
                                                        start,
                                                        end,
                                                        allDayEvent);
            string payload = generator.ToString();
            QRCodeData qrCodeData = QRGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);

            return QRCodeToBitmap(pixelsPerModule, qrCodeData);
        }

        private static byte[] QRCodeToBitmap(int pixelsPerModule, QRCodeData qrCodeData)
        {
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeAsBitmapByteArr = qrCode.GetGraphic(pixelsPerModule);

            return qrCodeAsBitmapByteArr;
        }
    }
}
