using System.Collections.Generic;

using BAGeocoding.Entity.Enum;

namespace BAGeocoding.Entity.ConvertData
{
    public class CVRCondition
    {
        public EnumCVRText2MifKind KindID { get; set; }
        public EnumBAGObjectType TypeID { get; set; }
        public bool MultiFile { get; set; }
        public string InputStr { get; set; }
        public string OutputStr { get; set; }
        public List<CVRFile> FileList { get; set; }
    }

    public class CVRFile
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string NameFull { get { return string.Format("{0}\\{1}", Path, Name); } }

        public CVRFile() { }

        public CVRFile(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}