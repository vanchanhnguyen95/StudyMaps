using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BAGeocoding.Utility
{
    /// <summary>
    /// Function Extender cho đối tượng
    /// </summary>
    public static class ObjextExtender
    {
        /// <summary>
        /// Kiểm tra xem một đối tượng có Null hay không
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null || obj.Equals(DBNull.Value);
        }

        /// <summary>
        /// Kiểm tra đối tượng có không null hay không
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return obj.IsNull() == false;
        }

        /// <summary>
        /// Cast object to T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T As<T>(this object obj)
        {
            return obj == null ? default(T) : (T)obj;
        }

        /// <summary>
        /// Kiểm tra đối tượng có phải là T hay không
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Is<T>(this object obj)
        {
            return obj is T;
        }

        /// <summary>
        /// Copy Object
        /// </summary>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static T GetAttribute<T>(this FieldInfo mif) where T : Attribute
        {
            // Lấy ra Attributes
            object[] attr = mif.GetCustomAttributes(typeof(T), true);

            // return kết quả
            return attr.Length > 0 ? attr[0].As<T>() : null;
        }

        /// <summary>
        /// Copy 2 object
        /// </summary>
        public static T CopyObject<T>(this object objSource) where T : new()
        {
            if (objSource.IsNull())
                return new T();
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, objSource);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        public static int SetBitState(int val)
        {            
            return (int)Math.Pow(2, (int)val);
        }
    }
}
