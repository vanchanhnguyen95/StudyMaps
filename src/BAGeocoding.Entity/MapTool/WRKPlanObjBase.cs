using System;
using System.Collections.Generic;
using System.Data;

using BAGeocoding.Entity.Enum.MapTool;
using BAGeocoding.Entity.MapObj;

using BAGeocoding.Utility;

namespace BAGeocoding.Entity.MapTool
{
    public class WRKPlanObjBase : SQLDataUlt
    {
        public int PlanID { get; set; }
        public string Name { get; set; }
        public string NoteOld { get; set; }
        public string NoteNew { get; set; }

        public int GridEdit { get; set; }
        public string GridView { get; set; }

        public short StateOpts { get; set; }
        public int ActionID { get; set; }
        public string ActionStr { get; set; }

        public byte ApprovedState { get; set; }
        public EnumMTLApprovedState EnumApprovedState { get { return (EnumMTLApprovedState)ApprovedState; } set { ApprovedState = (byte)value; } }

        public int EditorID { get; set; }
        public int EditTime { get; set; }
        public DateTime EditTimeGMT { get { return DataUtl.GetTimeUnix(EditTime); } set { EditTime = DataUtl.GetUnixTime(value); } }
        public long MigrateID { get; set; }

        public WRKPlanObjBase()
        {
            Name = string.Empty;
            NoteOld = string.Empty;
            NoteNew = string.Empty;
            GridView = string.Empty;
        }

        public WRKPlanObjBase(WRKPlanObjBase other)
        {
            PlanID = other.PlanID;
            Name = other.Name;
            NoteOld = other.NoteOld;
            NoteNew = other.NoteNew;
            GridEdit = other.GridEdit;
            GridView = other.GridView;
            StateOpts = other.StateOpts;
            ActionID = other.ActionID;
            ActionStr = other.ActionStr;
            ApprovedState = other.ApprovedState;
            EditorID = other.EditorID;
            EditTime = other.EditTime;
            MigrateID = other.MigrateID;
        }

        public bool FromDataNote(DataRow dr)
        {
            try
            {
                NoteOld = base.GetDataValue<string>(dr, "NoteStr");
                EditorID = base.GetDataValue<int>(dr, "EditorID");
                EditTime = base.GetDataValue<int>(dr, "EditTime");

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanPoint.FromDataNote, ex: {0}", ex.ToString()));
                return false;
            }
        }
        
        public bool FromDataRow(DataRow dr)
        {
            try
            {
                PlanID = base.GetDataValue<int>(dr, "PlanID");
                Name = base.GetDataValue<string>(dr, "Name");
                
                GridEdit = base.GetDataValue<int>(dr, "GridEdit");
                GridView = base.GetDataValue<string>(dr, "GridView");

                StateOpts = base.GetDataValue<short>(dr, "StateOpts");
                ActionID = base.GetDataValue<int>(dr, "ActionID");
                ApprovedState = base.GetDataValue<byte>(dr, "ApprovedState");

                EditorID = base.GetDataValue<int>(dr, "EditorID");
                EditTime = base.GetDataValue<int>(dr, "EditTime");
                MigrateID = base.GetDataValue<long>(dr, "MigrateID");

                StateOptsAdjust();
                ActionBuild();

                return true;
            }
            catch (Exception ex)
            {
                LogFile.WriteError(string.Format("WRKPlanPoint.FromDataRow, ex: {0}", ex.ToString()));
                return false;
            }
        }

        #region ==================== Trạng thái của đối tượng ====================
        public bool StateOptsGet(EnumMTLStateOption opts)
        {
            return ((StateOpts & (int)Math.Pow(2, (int)opts)) > 0);
        }

        public void StateOptsSet(EnumMTLStateOption opts, bool state)
        {
            // Bít đã được bật
            if (((StateOpts >> (int)opts) & 1) > 0)
            {
                if (state == false)
                    StateOpts = (short)(StateOpts - (int)Math.Pow(2, (int)opts));
            }
            // Bít chưa bật
            else
            {
                if (state == true)
                    StateOpts = (short)(StateOpts + (int)Math.Pow(2, (int)opts));
            }
        }

        public void StateOptsAdjust()
        {
            if (IsCreate() == true)
                StateOptsSet(EnumMTLStateOption.IsCreatNew, true);

            else if (IsDelete() == true)
                StateOptsSet(EnumMTLStateOption.IsVisible, false);

            else if (IsUpdate() == true)
                StateOptsSet(EnumMTLStateOption.IsProcess, true);
        }
        #endregion

        #region ==================== Mã thao tác đối tượng ====================
        public bool ActionGet(EnumMTLObjectAction opts)
        {
            return ((ActionID & (int)Math.Pow(2, (int)opts)) > 0);
        }

        public void ActionSet(EnumMTLObjectAction opts, bool state)
        {
            // Bít đã được bật
            if (((ActionID >> (int)opts) & 1) > 0)
            {
                if (state == false)
                    ActionID = (short)(ActionID - (int)Math.Pow(2, (int)opts));
            }
            // Bít chưa bật
            else
            {
                if (state == true)
                    ActionID = (short)(ActionID + (int)Math.Pow(2, (int)opts));
            }
        }

        public void ActionBuild()
        {
            ActionStr = string.Empty;
            List<EnumItemAttribute> atributeList = StringUlt.GetListEnumAttribute(EnumMTLObjectAction.Generate);
            for (int i = 0; i < atributeList.Count; i++)
            {
                if (ActionGet((EnumMTLObjectAction)atributeList[i].Value) == false)
                    continue;
                else if (ActionStr.Length > 0)
                    ActionStr += ", ";
                ActionStr += atributeList[i].Name;
            }
        }
        #endregion

        public bool IsCreate()
        {
            if (ActionGet(EnumMTLObjectAction.NewByAction) == true)
                return true;
            else if (ActionGet(EnumMTLObjectAction.NewByCut) == true)
                return true;
            else if (ActionGet(EnumMTLObjectAction.NewByJoin) == true)
                return true;
            else
                return false;
        }

        public bool IsUpdate()
        {
            if (ActionGet(EnumMTLObjectAction.EditObject) == true)
                return true;
            else
                return false;
        }

        public bool IsDelete()
        {
            if (ActionGet(EnumMTLObjectAction.DeleteByAction) == true)
                return true;
            else if (ActionGet(EnumMTLObjectAction.DeleteByCut) == true)
                return true;
            else if (ActionGet(EnumMTLObjectAction.DeleteByJoin) == true)
                return true;
            else
                return false;
        }

        public void UpdateNote(WRKPlanObjBase other)
        {
            if (other.NoteOld == null)
                return;
            else if (other.NoteOld.Length == 0)
                return;
            else if (NoteOld == null)
                NoteOld = string.Empty;
            else if (NoteOld.Length > 0)
                NoteOld += " - ";
            NoteOld += other.NoteOld;
        }
    }
}