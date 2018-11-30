using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace SG_Radio_for_8._1.Data
{
    public class SGRADIODataItem
    {
        public SGRADIODataItem(String uniqueId, String title, String imagePath, String content)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.ImagePath = imagePath;
            this.Content = content;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string ImagePath { get; private set; }
        public string Content { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    public class SGRADIODataGroup
    {
        public SGRADIODataGroup(String uniqueId, String title, String imagePath)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.ImagePath = imagePath;
            this.Items = new ObservableCollection<SGRADIODataItem>();
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string ImagePath { get; private set; }
        public ObservableCollection<SGRADIODataItem> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    public sealed class SGRADIODataSource
    {
        private static SGRADIODataSource _SGRADIODataSource = new SGRADIODataSource();

        private ObservableCollection<SGRADIODataGroup> _groups = new ObservableCollection<SGRADIODataGroup>();
        public ObservableCollection<SGRADIODataGroup> Groups
        {
            get { return this._groups; }
        }

        public static async Task<IEnumerable<SGRADIODataGroup>> GetGroupsAsync()
        {
            await _SGRADIODataSource.GetSGRADIODataAsync();

            return _SGRADIODataSource.Groups;
        }

        public static async Task<SGRADIODataGroup> GetGroupAsync(string uniqueId)
        {
            await _SGRADIODataSource.GetSGRADIODataAsync();
            var matches = _SGRADIODataSource.Groups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<SGRADIODataItem> GetItemAsync(string uniqueId)
        {
            await _SGRADIODataSource.GetSGRADIODataAsync();
            var matches = _SGRADIODataSource.Groups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private async Task GetSGRADIODataAsync()
        {
            if (this._groups.Count != 0)
                return;

            Uri dataUri = new Uri("ms-appx:///DataModel/SGRADIO.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Groups"].GetArray();

            foreach (JsonValue groupValue in jsonArray)
            {
                JsonObject groupObject = groupValue.GetObject();
                SGRADIODataGroup group = new SGRADIODataGroup(groupObject["UniqueId"].GetString(),
                                                            groupObject["Title"].GetString(),
                                                            groupObject["ImagePath"].GetString());

                foreach (JsonValue itemValue in groupObject["Items"].GetArray())
                {
                    JsonObject itemObject = itemValue.GetObject();
                    group.Items.Add(new SGRADIODataItem(itemObject["UniqueId"].GetString(),
                                                       itemObject["Title"].GetString(),
                                                       itemObject["ImagePath"].GetString(),
                                                       itemObject["Content"].GetString()));
                }
                this.Groups.Add(group);
            }
        }
    }
}
        /*
        {
          "UniqueId": "5011",
          "Title": "DesiNetworks",
          "ImagePath": "ms-appx:///Images/Others/Others_12.png",
          "Content": "http://192.99.8.192:3224/;stream.nsv"
        },
        */