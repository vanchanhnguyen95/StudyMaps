// See https://aka.ms/new-console-template for more information
using BAGeocoding.Bll.CheckData;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
Console.WriteLine("Test GDAL!");
// Test Bll.CheckData
string fileMap = @"TestNinhThuan.shp";
//string fileName = @"TestHL.shp";
string fileName = @"TestNinhThuan.shp";
var lst = CheckDataManager.CheckSegment(fileMap, fileName, "Chanh");

Console.ReadLine();