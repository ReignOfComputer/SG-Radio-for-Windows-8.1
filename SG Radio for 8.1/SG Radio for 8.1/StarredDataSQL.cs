namespace SG_Radio_for_8._1
{
    public class StarredDataSQL
    {
        [SQLite.AutoIncrement, SQLite.PrimaryKey]
        public int ID { get; set; }
        public string StarredImage { get; set; }
        public string StarredTitle { get; set; }
        public string StarredTime { get; set; }
    }
}
