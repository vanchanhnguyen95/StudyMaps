using System.IO;

namespace BAGeocoding.Entity
{
    public class CacheParamsData
    {
        public string ImportSegmentFileRegion { get; set; }
        public string ImportSegmentFileMap { get; set; }
        public string ImportSegmentFileName { get; set; }
        public string ImportSegmentFolderData { get; set; }
        
        public string ImportPlaceFolderData { get; set; }

        public string ImportRouteFileMap { get; set; }
        public string ImportRouteFileName { get; set; }

        public string CheckHouseNumber { get; set; }

        public string WriteFolder { get; set; }

        public CacheParamsData()
        {
            ImportSegmentFileRegion = string.Empty;
            ImportSegmentFileMap = string.Empty;
            ImportSegmentFileName = string.Empty;
            ImportSegmentFolderData = string.Empty;

            ImportPlaceFolderData = string.Empty;
            
            WriteFolder = string.Empty;
        }

        public void InitDefaul(string folderStr)
        {
            ImportSegmentFileRegion = FolderDetect(folderStr, string.Format(@"{0}\Data\Region", folderStr));
            ImportSegmentFileMap = FolderDetect(folderStr, string.Format(@"{0}\Data\Road", folderStr));
            ImportSegmentFileName = FolderDetect(folderStr, string.Format(@"{0}\Data\Road", folderStr));
            ImportSegmentFolderData = FolderDetect(folderStr, string.Format(@"{0}\Data\Road", folderStr));

            ImportPlaceFolderData = FolderDetect(folderStr, string.Format(@"{0}\Data\Place\Ha Noi\Ha Dong", folderStr));
            
            ImportRouteFileMap = FolderDetect(folderStr, string.Format(@"{0}\Data\Route", folderStr));
            ImportRouteFileName = FolderDetect(folderStr, string.Format(@"{0}\Data\Route", folderStr));

            CheckHouseNumber = FolderDetect(folderStr, string.Format(@"{0}\Data\Road", folderStr));

            WriteFolder = FolderDetect(folderStr, string.Format(@"{0}\Result", folderStr));
        }

        private string FolderDetect(string folderMain, string folderSub)
        {
            try
            {
                if (Directory.Exists(folderSub) == true)
                    return folderSub;
                else
                    return folderMain;
            }
            catch { return folderMain; }
        }
    }
}
