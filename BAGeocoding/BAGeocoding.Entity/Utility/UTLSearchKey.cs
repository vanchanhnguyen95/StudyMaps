using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.Utility
{
    public class UTLSearchKey : SQLDataUlt
    {
        public string KeyStr { get; set; }
        public Hashtable ObjectID { get; set; }

        public bool FromDataRow(DataRow drKey, DataTable dtSegment, bool isExt, bool isFull = false)
        {
            try
            {
                KeyStr = base.GetDataValue<string>(drKey, "KeyStr");
                ObjectID = new Hashtable();
                DataRow[] drList = dtSegment.Select(string.Format("KeyID = {0}", base.GetDataValue<int>(drKey, "KeyID")));
                for (int i = 0; i < drList.Length; i++)
                {
                    BAGKeyRate keyRate = new BAGKeyRate();
                    if (keyRate.FromDataRow(drList[i], isFull) == false)
                        return false;
                    else if (isExt == true)
                    {
                        if (ObjectID.ContainsKey(keyRate.ObjectID) == false)
                            ObjectID.Add(keyRate.ObjectID, keyRate);
                    }
                    else
                    {
                        if (ObjectID.ContainsKey(Convert.ToInt16(keyRate.ObjectID)) == false)
                            ObjectID.Add(Convert.ToInt16(keyRate.ObjectID), keyRate);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteData("UTLSearchKey.FromDataRow, ex: " + ex.ToString());
                return false;
            }
        }

        public byte[] ToBinary(bool ex = false, bool fl = false)
        {
            List<byte> resultList = new List<byte>();
            byte[] bffKey = Constants.TCVN3CodePage.GetBytes(KeyStr);
            resultList.Add((byte)bffKey.Length);
            resultList.AddRange(bffKey);

            resultList.AddRange(BitConverter.GetBytes(ObjectID.Count));
            foreach (object key in ObjectID.Keys)
            {
                BAGKeyRate keyRate = (BAGKeyRate)ObjectID[key];
                if (ex == true)
                    resultList.AddRange(BitConverter.GetBytes(Convert.ToInt32(key)));
                else
                    resultList.AddRange(BitConverter.GetBytes(Convert.ToInt16(key)));
                resultList.AddRange(keyRate.ToBinary(fl));
            }

            return resultList.ToArray();
        }
    }
}
