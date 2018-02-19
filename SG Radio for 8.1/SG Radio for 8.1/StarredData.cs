namespace SG_Radio_for_8._1
{
    public class StarredData
        {
            public string StarredImage { get; set; }
            public string StarredTitle { get; set; }
            public string StarredTime { get; set; }

            public StarredData() { }

            public StarredData(string itemImageSet, string itemNameSet, string itemStartSet)
            {
                StarredImage = itemImageSet;
                StarredTitle = itemNameSet;
                StarredTime = itemStartSet;
            }
        }
}
