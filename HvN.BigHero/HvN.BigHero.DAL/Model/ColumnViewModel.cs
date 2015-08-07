namespace HvN.BigHero.DAL.Model
{
    public class ColumnViewModel
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool NullAble { get; set; }

        public string CssClass
        {
            get { return NullAble ? "" : "mandatory"; }
        }
    }
}
