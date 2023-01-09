using System.Collections.Generic;

namespace BAGeocoding.Entity.Utility
{
    public class UTLConditionImportPlace
    {
        public bool State { get; set; }
        public string Folder { get; set; }
    }

    public class UTLConditionImportRoute
    {
        public int Distance { get; set; }
        public string FolderInput { get; set; }
        public string FileMap { get; set; }
        public string FileName { get; set; }
        public string FileData { get; set; }
        public string FolderOutput { get; set; }
        public List<double> CoeffList { get; set; }
        
        public int SegmentID { get; set; }

        public UTLConditionImportRoute()
        {
            CoeffList = new List<double>();
        }

        public UTLConditionImportRoute(UTLConditionImportRoute other)
        {
            Distance = other.Distance;
            FolderInput = other.FolderInput;
            FileName = other.FileName;
            FileMap = other.FileMap;
            FileData = other.FileData;
            FolderOutput = other.FolderOutput;
            CoeffList = new List<double>();
            for (int i = 0; i < other.CoeffList.Count; i++)
                CoeffList.Add(other.CoeffList[i]);
        }
    }
}
