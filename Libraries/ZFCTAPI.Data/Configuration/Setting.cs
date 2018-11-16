using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.Configuration
{
    /// <summary>
    /// Represents a setting
    /// </summary>
    [Table("Setting")]
    public partial class Setting : BaseEntity
    {
        public Setting()
        {
        }

        public Setting(string name, string value, int storeId = 0)
        {
            this.Name = name;
            this.Value = value;
            this.StoreId = storeId;
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the store for which this setting is valid. 0 is set when the setting is for all stores
        /// </summary>
        public int StoreId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}