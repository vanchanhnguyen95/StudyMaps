using Nest;
using System.ComponentModel;

namespace Elastic02.Models.Test
{
    public class Province
    {
        [Number(Index = true)]
        public int ProvinceID { get; set; }
        [Text(Index = true, Fielddata = true)]
        public string Code { get; set; }
        [Text(Index = true, Fielddata = true)]
        public string VName { get; set; }
        [Text(Index = true, Fielddata = true)]
        public string EName { get; set; }
        [Text(Index = true, Fielddata = true)]
        public string Abbreviate { get; set; }

        public Province() { }
        public Province(ProvinceAnalysis orther) {
            ProvinceID = orther.ProvinceID;
            Code = orther.Code;
            VName = orther.VName;
            EName = orther.EName;
            Abbreviate = orther.Abbreviate;
        }
    }

    [ElasticsearchType(IdProperty = nameof(Id)), Description("provinceanalysis")]
    public class ProvinceAnalysis : Province
    {
        [Text(Index = true, Fielddata = true)]
        public string ENameLower { get; set; }
        public ProvinceAnalysis() { }
        public ProvinceAnalysis(Province orther) {
            ProvinceID = orther.ProvinceID;
            Code = orther.Code;
            VName = orther.VName;
            EName = orther.EName;
            ENameLower = orther.EName.ToLower();
            Abbreviate = orther.Abbreviate;
        }
    }
}
