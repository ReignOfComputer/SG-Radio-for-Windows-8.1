namespace SG_Radio_for_8._1
{
    public class FavDataSQL
    {
        [SQLite.AutoIncrement, SQLite.PrimaryKey]
        public int ID { get; set; }
        public string FavID { get; set; }
    }
}
