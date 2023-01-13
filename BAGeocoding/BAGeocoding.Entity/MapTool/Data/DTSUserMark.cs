using System;
using System.Data;

using BAGeocoding.Entity.MapTool.Base;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool.Data
{
    public class DTSUserMark : SQLDataUlt
    {
        public USRUser UserInfo { get; set; }
        public int POICount { get; set; }

        public int SEGNewCount { get; set; }
        public int SEGNewNumber { get; set; }

        public int SEGOldNumber { get; set; }
        public int SEGOldInfor { get; set; }
        public int SEGOldDelete { get; set; }

        public decimal DistanceMove { get; set; }

        public int MarkTotal { get { return POICount + SEGNewCount + SEGNewNumber + SEGOldNumber + SEGOldInfor + SEGOldDelete; } }
        

        public int ObjectCount { get; set; }
        public DTSUserMarkNumber NumberOld { get; set; }
        public DTSUserMarkNumber NumberNew { get; set; }

        public DTSUserMark()
        {
            UserInfo = new USRUser();

            NumberOld = new DTSUserMarkNumber();
            NumberNew = new DTSUserMarkNumber();
        }


        public bool FromDataUser(DataRow dr)
        {
            try
            {
                if (UserInfo.FromDataSimple(dr) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("DTSUserMark.FromDataUser, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataCount(DataRow dr)
        {
            try
            {
                UserInfo.UserID = base.GetDataValue<int>(dr, "EditorID");
                ObjectCount = base.GetDataValue<int>(dr, "ObjectCount");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("DTSUserMark.FromDataCount, ex: {0}", ex.ToString()));
                return false;
            }
        }
        
        public bool FromDataNumber(DataRow dr)
        {
            try
            {
                UserInfo.UserID = base.GetDataValue<int>(dr, "EditorID");
                if (NumberNew.FromDataRows(dr) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("DTSUserMark.FromDataCount, ex: {0}", ex.ToString()));
                return false;
            }
        }

        public bool FromDataNumber(DataRow dr, string ol, string nw)
        {
            try
            {
                UserInfo.UserID = base.GetDataValue<int>(dr, "EditorID");
                if (NumberOld.FromDataRows(dr, ol) == false)
                    return false;
                else if (NumberNew.FromDataRows(dr, nw) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("DTSUserMark.FromDataNumber, ex: {0}", ex.ToString()));
                return false;
            }
        }

        /// <summary>
        /// Điểm khi làm POI (= Số lượng * 2)
        /// </summary>
        public void POICountUpdate(DTSUserMark other)
        {
            POICount += (other.ObjectCount * 2);
        }

        /// <summary>
        /// Điểm khi vẽ mới đường (= Số lượng * 2)
        /// </summary>
        public void SEGNewCountUpdate(DTSUserMark other)
        {
            SEGNewCount += (other.ObjectCount * 2);
        }

        /// <summary>
        /// Điểm khi làm số nhà (= Số lượng * 2)
        /// </summary>
        public void SEGNewNumberUpdate(DTSUserMark other)
        {
            if (other.NumberNew.StartLeft > 0)
                SEGNewNumber += 2;
            if (other.NumberNew.EndLeft > 0)
                SEGNewNumber += 2;
            if (other.NumberNew.StartRight > 0)
                SEGNewNumber += 2;
            if (other.NumberNew.EndRight > 0)
                SEGNewNumber += 2;
        }

        /// <summary>
        /// Điểm khi cập nhật số nhà (= Số lượng * 2)
        /// </summary>
        public void SEGOldNumberUpdate(DTSUserMark other)
        {
            if (other.NumberNew.StartLeft != other.NumberOld.StartLeft)
                SEGOldNumber += 2;
            if (other.NumberNew.EndLeft != other.NumberOld.EndLeft)
                SEGOldNumber += 2;
            if (other.NumberNew.StartRight != other.NumberOld.StartRight)
                SEGOldNumber += 2;
            if (other.NumberNew.EndRight != other.NumberOld.EndRight)
                SEGOldNumber += 2;
        }

        /// <summary>
        /// Điểm khi cập nhật tên (= 3 + (Số lượng - 1))
        /// </summary>
        public void SEGOldNameUpdate(DTSUserMark other)
        {
            SEGOldInfor += (3 + (other.ObjectCount - 1));
        }

        /// <summary>
        /// Điểm khi cập nhật thông tin ( = Số lượng)
        /// </summary>
        public void SEGOldInforUpdate(DTSUserMark other)
        {
            SEGOldInfor += other.ObjectCount;
        }

        /// <summary>
        /// Điểm khi xóa đường
        /// </summary>
        public void SEGOldDeleteUpdate(DTSUserMark other)
        {
            SEGOldDelete += other.ObjectCount;
        }

        /// <summary>
        /// Quảng đường di chuyển
        /// </summary>
        public void DistanceUpdate(DTSUserMark other)
        {
            DistanceMove += Convert.ToDecimal(other.ObjectCount);
        }



        public bool IsData()
        {
            if (POICount > 0)
                return true;
            else if (SEGNewCount > 0)
                return true;
            else if (SEGNewNumber > 0)
                return true;
            else if (SEGOldNumber > 0)
                return true;
            else if (SEGOldInfor > 0)
                return true;
            else if (SEGOldDelete > 0)
                return true;
            else if (DistanceMove > 0)
                return true;
            else
                return false;
        }
    }

    public class DTSUserMarkNumber : SQLDataUlt
    {
        public short StartLeft { get; set; }
        public short EndLeft { get; set; }
        public short StartRight { get; set; }
        public short EndRight { get; set; }

        public bool FromDataRows(DataRow dr, string pr = "")
        {
            try
            {
                StartLeft = base.GetDataValue<short>(dr, pr + "StartLeft");
                EndLeft = base.GetDataValue<short>(dr, pr + "EndLeft");

                StartRight = base.GetDataValue<short>(dr, pr + "StartRight");
                EndRight = base.GetDataValue<short>(dr, pr + "EndRight");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("DTSUserMarkNumber.FromDataRows, ex: {0}", ex.ToString()));
                return false;
            }
        }
    }
}
