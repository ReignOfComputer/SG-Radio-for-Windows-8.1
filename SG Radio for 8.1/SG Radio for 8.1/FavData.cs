namespace SG_Radio_for_8._1
{
    public class FavData
        {
            public string FavImage { get; set; }
            public string FavTitle { get; set; }

            public FavData() { }

            public FavData(string itemImageSet, string itemNameSet)
            {
                FavImage = itemImageSet;
                FavTitle = itemNameSet;
            }
        }
}
