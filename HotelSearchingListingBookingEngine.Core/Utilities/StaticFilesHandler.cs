using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace HotelSearchingListingBookingEngine.Core.Utilities
{
    public class StaticFilesHandler<T>
    {
        public T ParseFileData(string fileName)
        {
            byte[] fileData;
            try
            {
                fileData = File.ReadAllBytes(fileName);
            }
            catch (IOException ioException)
            {
                Logger.StoreLog(ioException.ToString());
                throw new Exception();
            }
            catch (Exception exception)
            {
                Logger.StoreLog(exception.ToString());
                throw new Exception();
            }
            try
            {
                string data = ASCIIEncoding.ASCII.GetString(fileData);
                data = data.TrimStart('?');
                return (JsonConvert.DeserializeObject<T>(data));
            }
            catch (Exception baseException)
            {
                Logger.StoreLog(baseException.ToString());
                throw new Exception();
            }
        }
    }
}
