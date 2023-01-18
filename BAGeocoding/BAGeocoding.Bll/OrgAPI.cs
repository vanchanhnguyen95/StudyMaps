using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

//using OSGeo.OGR;
//using OSGeo.OSR;
using MaxRev.Gdal.Core;
using OSGeo.OGR;
using OSGeo.OSR;

using BAGeocoding.Utility;

namespace BAGeocoding.Bll
{
    public class OrgAPI
    {
        private DataSource ds;
        private Layer layer;
        private bool b_write = false;
        private const float PROJEC = ((float)7 * (float)6370997);

        public bool BWrite
        {
            get { return b_write; }
        }

        public DataSource DS
        {
            get { return ds; }
        }

        public OrgAPI(string fileName, int wr)
        {
            b_write = wr > 0 ? true : false;
            try
            {
                //ogr.RegisterAll();Chanh
                GdalBase.ConfigureAll();
            }
            catch (Exception ex)
            {
                LogFile.WriteError(ex.ToString());
            }
            //ds = ogr.Open(fileName, wr);Chanh
            ds = Ogr.Open(fileName, wr);
        }

        public OrgAPI(string s_folder, string s_layer, int wkb, int wr)
        {
            b_write = wr > 0 ? true : false;
            string tmp_Dir = System.IO.Directory.GetCurrentDirectory();
            try
            {
                //ogr.RegisterAll();Chanh
                GdalBase.ConfigureAll();
            }
            catch (Exception ex)
            {
                LogFile.WriteError(ex.ToString());
            }
            if (Directory.Exists(s_folder) == false)
                Directory.CreateDirectory(s_folder);
            Directory.SetCurrentDirectory(s_folder);
            CreateLayer(s_folder, s_layer, wkb);
            Directory.SetCurrentDirectory(tmp_Dir);
        }

        public Layer GetLayer()
        {
            if (ds != null)
                return ds.GetLayerByIndex(0);
            else
                return layer;
        }

        public string GetLayerName()
        {
            Layer layer = ds.GetLayerByIndex(0);
            return layer.GetName();
        }

        //public int GetFeatureCount() //Chanh
        public long GetFeatureCount()
        {
            Layer layer = ds.GetLayerByIndex(0);
            return layer.GetFeatureCount(0);
        }

        public Feature GetFeatureById(int id)
        {
            Layer layer = ds.GetLayerByIndex(0);
            return layer.GetFeature(id);
        }

        public FieldDefn[] GetFields()
        {
            Layer layer = ds.GetLayerByIndex(0);
            FeatureDefn fd = layer.GetLayerDefn();
            int n_f = fd.GetFieldCount();
            FieldDefn[] a_f = new FieldDefn[n_f];
            for (int i = 0; i < n_f; i++)
            {
                a_f[i] = fd.GetFieldDefn(i);
            }
            return a_f;
        }

        public string GetValueByNameAndID(string fieldName, int id)
        {
            Layer layer = ds.GetLayerByIndex(0);
            Feature feature = layer.GetFeature(id);
            return feature.GetFieldAsString(fieldName);
        }

        public void CreateLayer(string s_folder, string s_layer, int wkb)
        {
            //Driver driver = ogr.GetDriverByName("ESRI Shapefile");//Chanh
            Driver driver = Ogr.GetDriverByName("ESRI Shapefile");
            if (driver != null)
            {
                ds = driver.CreateDataSource(s_folder, new string[] { });
                //layer = ds.CreateLayer(s_layer, null, wkb, new string[] { });//Chanh
                layer = ds.CreateLayer(s_layer, null, (wkbGeometryType)wkb, new string[] { });
            }
        }

        public void CreateField(string name, int type, int leng)
        {
            //FieldDefn fdefn = new FieldDefn(name, type);//Chanh
            FieldDefn fdefn = new FieldDefn(name,(FieldType)type);
            fdefn.SetWidth(leng);
            int exec = layer.CreateField(fdefn, 1);
        }

        public void CreateFeature(Feature feature)
        {
            int exec = layer.CreateFeature(feature);
        }

        public void DeleteFeature(int fid)
        {
            int exec = layer.DeleteFeature(fid);
        }
    }
}