using System;

namespace BAGeocoding.Utility
{
    /// <summary>
    /// Thông tin Enum
    /// </summary>    
    public class EnumItemAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public EnumItemAttribute(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public EnumItemAttribute(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Tên
        /// </summary>
        private string name = string.Empty;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Value
        /// </summary>
        private object value = 0;
        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// FieldName
        /// </summary>
        private string fieldName = string.Empty;
        public string FieldName
        {
            get { return this.fieldName; }
            set { this.fieldName = value; }
        }
        
        /// <summary>
        /// Index Image
        /// </summary>
        private int imageIndex = 9999;
        public int ImageIndex
        {
            get { return imageIndex; }
            set { imageIndex = value; }
        }
    }
}
