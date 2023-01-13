using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapObj
{
    public class BAGSearchKey
    {
        public bool IsValid { get; set; }
        public bool IsSpecial { get; set; }
        public short Building { get; set; }
        public string Road { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Original { get; set; }

        public BAGSearchKey() { }

        public BAGSearchKey(int type, string org, string key)
        {
            IsValid = true;
            short numberResult = 0;
            string[] tmp = key.Split(Constants.DEFAULT_SPLIT_KEYS);
            if (type == 2)
            {
                switch (tmp.Length)
                {
                    case 1:
                        #region ==================== 1 param ====================
                        Building = 0;
                        Road = tmp[0].Trim();
                        District = "";
                        Province = "ha noi";
                        #endregion
                        break;
                    case 2:
                        #region ==================== 2 params ====================
                        Building = 0;
                        Road = tmp[0].Trim();
                        District = "";
                        Province = tmp[1].Trim();
                        #endregion
                        break;
                    case 3:
                        #region ==================== 3 params ====================
                        if (DataUtl.GetNumber(tmp[0], 1, ref numberResult))
                        {
                            Road = tmp[1].Trim();
                            District = "";
                            Province = tmp[2].Trim();
                        }
                        else
                        {
                            Road = tmp[0].Trim();
                            District = tmp[1].Trim();
                            Province = tmp[2].Trim();
                        }
                        Building = numberResult;
                        #endregion
                        break;
                    case 4:
                        #region ==================== 4 params ====================
                        DataUtl.GetNumber(tmp[0], ref numberResult);
                        Building = numberResult;
                        Road = tmp[1].Trim();
                        District = tmp[2].Trim();
                        Province = tmp[3].Trim();
                        #endregion
                        break;
                    default:
                        IsValid = false;
                        break;
                }
            }

            Original = org.ToLower();
            IsSpecial = (Original.IndexOf(Road) < 0);
        }
    }
}
