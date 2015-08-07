using HvN.BigHero.DAL.Utility;

namespace HvN.BigHero.DAL.Model
{
    public class ColumnViewModel
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool NullAble { get; set; }
        public ColumnType DataType { get; set; }
        public object Value { get; set; }
        public string CssClass
        {
            get
            {
                return NullAble ? string.Empty : "mandatory";
            }
        }

        public string DatePickerClass
        {
            get
            {
                return DataType == ColumnType.DateTime ? "datetimepicker" : string.Empty;
            }
        }
    }
}
