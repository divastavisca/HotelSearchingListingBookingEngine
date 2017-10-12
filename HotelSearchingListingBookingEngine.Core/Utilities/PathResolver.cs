using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;

namespace HotelSearchingListingBookingEngine.Core.Utilities
{
    public class PathResolver
    {
        public static string Resolve(string parentToParentProjectDirectory, string parentProjectDirectoryName,string filePathRelativeToProject)
        {
            try
            {
                string currentAppDomain = AppDomain.CurrentDomain.BaseDirectory;
                string[] sourceStrings;
                if (currentAppDomain.Contains("\\"))
                    sourceStrings = currentAppDomain.Split('\\');
                else sourceStrings = currentAppDomain.Split('/');
                sourceStrings[0] = sourceStrings[0] + "\\";
                List<string> finalDirectory = new List<string>();
                foreach (string directory in sourceStrings)
                {
                    finalDirectory.Add(directory);
                    if (directory == parentToParentProjectDirectory)
                        break;
                }
                finalDirectory.Add(parentProjectDirectoryName);
                if (filePathRelativeToProject.Contains("\\"))
                    sourceStrings = filePathRelativeToProject.Split("\\");
                else sourceStrings = filePathRelativeToProject.Split('/');
                finalDirectory.AddRange(sourceStrings);
                var arr = finalDirectory.ToArray();
                var val= Path.Combine(finalDirectory.ToArray());
                return val;
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new FilePathResolverError()
                {
                    Source = filePathRelativeToProject
                };
            }
        }
    }
}
