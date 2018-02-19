using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Radio_for_8._1
{
    public class Constants
    {
        public static string getName(string id)
        {
            string itemName;
            switch (id)
            {
                case "1000":
                    {
                        itemName = "Gold 90.5FM";
                        break;
                    }
                case "1001":
                    {
                        itemName = "HOT FM91.3";
                        break;
                    }
                case "1002":
                    {
                        itemName = "Kiss 92FM";
                        break;
                    }
                case "1003":
                    {
                        itemName = "Symphony 92.4FM";
                        break;
                    }
                case "1004":
                    {
                        itemName = "938LIVE";
                        break;
                    }
                case "1005":
                    {
                        itemName = "Class 95FM";
                        break;
                    }
                case "1006":
                    {
                        itemName = "987FM";
                        break;
                    }
                case "1007":
                    {
                        itemName = "Lush 99.5FM";
                        break;
                    }
                case "1008":
                    {
                        itemName = "973FM";
                        break;
                    }
                case "1009":
                    {
                        itemName = "The Live Radio";
                        break;
                    }
                case "1010":
                    {
                        itemName = "BBC World Service";
                        break;
                    }
                case "1011":
                    {
                        itemName = "Power 98 FM";
                        break;
                    }
                case "2000":
                    {
                        itemName = "Ria 89.7FM";
                        break;
                    }
                case "2001":
                    {
                        itemName = "Y.E.S. 93.3FM";
                        break;
                    }
                case "2002":
                    {
                        itemName = "Warna 94.2FM";
                        break;
                    }
                case "2003":
                    {
                        itemName = "Capital 95.8";
                        break;
                    }
                case "2004":
                    {
                        itemName = "XFM 96.3";
                        break;
                    }
                case "2005":
                    {
                        itemName = "Oli 96.8FM";
                        break;
                    }
                case "2006":
                    {
                        itemName = "Love 97.2FM";
                        break;
                    }
                case "2007":
                    {
                        itemName = "UFM 1003";
                        break;
                    }
                case "2008":
                    {
                        itemName = "88.3 Jia FM";
                        break;
                    }
                default:
                    itemName = "Unknown";
                    break;
            }
            return itemName;
        }

        public static string getLyrics(string id)
        {
            string lyricsAPI;
            switch (id)
            {
                case "1000":
                    {
                        lyricsAPI = "http://www.mediacorpradio.sg/radioliveplayer/lyrics.asp?id=905";
                        break;
                    }
                case "1001":
                    {
                        lyricsAPI = "http://meta.radioactive.sg/index.php?format=json&m=913fm";
                        break;
                    }
                case "1002":
                    {
                        lyricsAPI = "http://meta.radioactive.sg/index.php?format=json&m=sph-kiss92";
                        break;
                    }
                case "1003":
                    {
                        lyricsAPI = "http://www.mediacorpradio.sg/radioliveplayer/lyrics.asp?id=924";
                        break;
                    }
                case "1004":
                    {
                        lyricsAPI = "http://www.mediacorpradio.sg/radioliveplayer/lyrics.asp?id=938";
                        break;
                    }
                case "1005":
                    {
                        lyricsAPI = "http://www.mediacorpradio.sg/radioliveplayer/lyrics.asp?id=950";
                        break;
                    }
                case "1006":
                    {
                        lyricsAPI = "http://www.mediacorpradio.sg/radioliveplayer/lyrics.asp?id=987";
                        break;
                    }
                case "1007":
                    {
                        lyricsAPI = "http://www.mediacorpradio.sg/radioliveplayer/lyrics.asp?id=995";
                        break;
                    }
                case "2000":
                    {
                        lyricsAPI = "http://www.mediacorpradio.sg/radioliveplayer/lyrics.asp?id=897";
                        break;
                    }
                case "2001":
                    {
                        lyricsAPI = "http://www.mediacorpradio.sg/radioliveplayer/lyrics.asp?id=933";
                        break;
                    }
                case "2002":
                    {
                        lyricsAPI = "http://www.mediacorpradio.sg/radioliveplayer/lyrics.asp?id=942";
                        break;
                    }
                case "2003":
                    {
                        lyricsAPI = "http://www.mediacorpradio.sg/radioliveplayer/lyrics.asp?id=958";
                        break;
                    }
                case "2004":
                    {
                        lyricsAPI = "http://www.mediacorpradio.sg/radioliveplayer/lyrics.asp?id=963";
                        break;
                    }
                case "2005":
                    {
                        lyricsAPI = "http://www.mediacorpradio.sg/radioliveplayer/lyrics.asp?id=968";
                        break;
                    }
                case "2006":
                    {
                        lyricsAPI = "http://www.mediacorpradio.sg/radioliveplayer/lyrics.asp?id=972";
                        break;
                    }
                case "2007":
                    {
                        lyricsAPI = "http://meta.radioactive.sg/index.php?format=json&m=1003fm";
                        break;
                    }
                default:
                    lyricsAPI = "Unknown";
                    break;
            }
            return lyricsAPI;
        }

        public static string getStream(string id)
        {
            string itemStream;
            switch (id)
            {
                case "1000":
                    {
                        itemStream = "http://mediacorp.rastream.com/905fm";
                        break;
                    }
                case "1001":
                    {
                        itemStream = "http://sph.rastream.com/913fm";
                        break;
                    }
                case "1002":
                    {
                        itemStream = "http://sph.rastream.com/sph-kiss92";
                        break;
                    }
                case "1003":
                    {
                        itemStream = "http://mediacorp.rastream.com/924fm";
                        break;
                    }
                case "1004":
                    {
                        itemStream = "http://mediacorp.rastream.com/938fm";
                        break;
                    }
                case "1005":
                    {
                        itemStream = "http://mediacorp.rastream.com/950fm";
                        break;
                    }
                case "1006":
                    {
                        itemStream = "http://mediacorp.rastream.com/987fm";
                        break;
                    }
                case "1007":
                    {
                        itemStream = "http://mediacorp.rastream.com/995fm";
                        break;
                    }
                case "1008":
                    {
                        itemStream = "http://209.105.250.73:8650/stream";
                        break;
                    }
                case "1009":
                    {
                        itemStream = "http://209.105.250.73:8650/stream"; // Placeholder
                        // itemStream = "http://s3.viastreaming.net:8530/"; The Live Radio is DOWN
                        break;
                    }
                case "1010":
                    {
                        itemStream = "http://livewmstream-ws.bbc.co.uk.edgestreams.net/reflector:38972";
                        break;
                    }
                case "1011":
                    {
                        itemStream = "http://streaming.clickhere2.com/power98.asx";
                        break;
                    }
                case "2000":
                    {
                        itemStream = "http://mediacorp.rastream.com/897fm";
                        break;
                    }
                case "2001":
                    {
                        itemStream = "http://mediacorp.rastream.com/933fm";
                        break;
                    }
                case "2002":
                    {
                        itemStream = "http://mediacorp.rastream.com/942fm";
                        break;
                    }
                case "2003":
                    {
                        itemStream = "http://mediacorp.rastream.com/958fm";
                        break;
                    }
                case "2004":
                    {
                        itemStream = "http://mediacorp.rastream.com/963fm";
                        break;
                    }
                case "2005":
                    {
                        itemStream = "http://mediacorp.rastream.com/968fm";
                        break;
                    }
                case "2006":
                    {
                        itemStream = "http://mediacorp.rastream.com/972fm";
                        break;
                    }
                case "2007":
                    {
                        itemStream = "http://sph.rastream.com/1003fm";
                        break;
                    }
                case "2008":
                    {
                        itemStream = "http://streaming.clickhere2.com/883jia.asx";
                        break;
                    }
                default:
                    {
                        itemStream = "Unknown";
                        break;
                    }
            }
            return itemStream;
        }

        public static string getTitle(string id)
        {
            string mediacorpAPI;
            switch (id)
            {
                case "1000":
                    {
                        mediacorpAPI = "http://www.mediacorpradio.sg/radioliveplayer/albumInfo.asp?id=905";
                        break;
                    }
                case "1003":
                    {
                        mediacorpAPI = "http://www.mediacorpradio.sg/radioliveplayer/albumInfo.asp?id=924";
                        break;
                    }
                case "1004":
                    {
                        mediacorpAPI = "http://www.mediacorpradio.sg/radioliveplayer/albumInfo.asp?id=938";
                        break;
                    }
                case "1005":
                    {
                        mediacorpAPI = "http://www.mediacorpradio.sg/radioliveplayer/albumInfo.asp?id=950";
                        break;
                    }
                case "1006":
                    {
                        mediacorpAPI = "http://www.mediacorpradio.sg/radioliveplayer/albumInfo.asp?id=987";
                        break;
                    }
                case "1007":
                    {
                        mediacorpAPI = "http://www.mediacorpradio.sg/radioliveplayer/albumInfo.asp?id=995";
                        break;
                    }
                case "2000":
                    {
                        mediacorpAPI = "http://www.mediacorpradio.sg/radioliveplayer/albumInfo.asp?id=897";
                        break;
                    }
                case "2001":
                    {
                        mediacorpAPI = "http://www.mediacorpradio.sg/radioliveplayer/albumInfo.asp?id=933";
                        break;
                    }
                case "2002":
                    {
                        mediacorpAPI = "http://www.mediacorpradio.sg/radioliveplayer/albumInfo.asp?id=942";
                        break;
                    }
                case "2003":
                    {
                        mediacorpAPI = "http://www.mediacorpradio.sg/radioliveplayer/albumInfo.asp?id=958";
                        break;
                    }
                case "2004":
                    {
                        mediacorpAPI = "http://www.mediacorpradio.sg/radioliveplayer/albumInfo.asp?id=963";
                        break;
                    }
                case "2005":
                    {
                        mediacorpAPI = "http://www.mediacorpradio.sg/radioliveplayer/albumInfo.asp?id=968";
                        break;
                    }
                case "2006":
                    {
                        mediacorpAPI = "http://www.mediacorpradio.sg/radioliveplayer/albumInfo.asp?id=972";
                        break;
                    }
                default:
                    mediacorpAPI = "Unknown";
                    break;
            }
            return mediacorpAPI;
        }

        public static string getImage(string id)
        {
            string itemImage;
            switch (id)
            {
                case "1000":
                    {
                        itemImage = "ms-appx:///Images/English/portal_1_600_C.png";
                        break;
                    }
                case "1001":
                    {
                        itemImage = "ms-appx:///Images/English/portal_2_600_C.png";
                        break;
                    }
                case "1002":
                    {
                        itemImage = "ms-appx:///Images/English/portal_3_600_C.png";
                        break;
                    }
                case "1003":
                    {
                        itemImage = "ms-appx:///Images/English/portal_4_600_C.png";
                        break;
                    }
                case "1004":
                    {
                        itemImage = "ms-appx:///Images/English/portal_5_600_C.png";
                        break;
                    }
                case "1005":
                    {
                        itemImage = "ms-appx:///Images/English/portal_6_600_C.png";
                        break;
                    }
                case "1006":
                    {
                        itemImage = "ms-appx:///Images/English/portal_7_600_C.png";
                        break;
                    }
                case "1007":
                    {
                        itemImage = "ms-appx:///Images/English/portal_8_600_C.png";
                        break;
                    }
                case "1008":
                    {
                        itemImage = "ms-appx:///Images/English/portal_9_600_C.png";
                        break;
                    }
                case "1009":
                    {
                        itemImage = "ms-appx:///Images/English/portal_10_600_C.png";
                        break;
                    }
                case "1010":
                    {
                        itemImage = "ms-appx:///Images/English/portal_11_600_C.png";
                        break;
                    }
                case "1011":
                    {
                        itemImage = "ms-appx:///Images/English/portal_12_600_C.png";
                        break;
                    }
                case "2000":
                    {
                        itemImage = "ms-appx:///Images/Others/news_1_600_C.png";
                        break;
                    }
                case "2001":
                    {
                        itemImage = "ms-appx:///Images/Others/news_2_600_C.png";
                        break;
                    }
                case "2002":
                    {
                        itemImage = "ms-appx:///Images/Others/news_3_600_C.png";
                        break;
                    }
                case "2003":
                    {
                        itemImage = "ms-appx:///Images/Others/news_4_600_C.png";
                        break;
                    }
                case "2004":
                    {
                        itemImage = "ms-appx:///Images/Others/news_5_600_C.png";
                        break;
                    }
                case "2005":
                    {
                        itemImage = "ms-appx:///Images/Others/news_6_600_C.png";
                        break;
                    }
                case "2006":
                    {
                        itemImage = "ms-appx:///Images/Others/news_7_600_C.png";
                        break;
                    }
                case "2007":
                    {
                        itemImage = "ms-appx:///Images/Others/news_8_600_C.png";
                        break;
                    }
                case "2008":
                    {
                        itemImage = "ms-appx:///Images/Others/news_9_600_C.png";
                        break;
                    }
                default:
                    itemImage = "Unknown";
                    break;
            }
            return itemImage;
        }

        public static string getStnWebsite(string id)
        {
            string gotoURL;
            switch (id)
            {
                case "1000":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/gold90/";
                    break;
                case "1001":
                    gotoURL = "http://www.radio913.com/";
                    break;
                case "1002":
                    gotoURL = "http://kiss92.sg/";
                    break;
                case "1003":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/symphony924/";
                    break;
                case "1004":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/938live/";
                    break;
                case "1005":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/class95/";
                    break;
                case "1006":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/987fm/";
                    break;
                case "1007":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/lush995/";
                    break;
                case "1008":
                    gotoURL = "http://973fm.webs.com/";
                    break;
                case "1009":
                    gotoURL = "http://theliveradio.sg/";
                    break;
                case "1010":
                    gotoURL = "http://www.bbc.co.uk/worldserviceradio";
                    break;
                case "1011":
                    gotoURL = "http://www.power98.com.sg/";
                    break;
                case "2000":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/ria897/default.aspx";
                    break;
                case "2001":
                    gotoURL = "http://entertainment.xin.msn.com/zh/radio/yes933";
                    break;
                case "2002":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/warna942/default.aspx";
                    break;
                case "2003":
                    gotoURL = "http://entertainment.xin.msn.com/zh/radio/capital958/";
                    break;
                case "2004":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/xfm963/default.aspx";
                    break;
                case "2005":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/oli968/default.aspx";
                    break;
                case "2006":
                    gotoURL = "http://entertainment.xin.msn.com/zh/radio/love972/";
                    break;
                case "2007":
                    gotoURL = "http://www.ufm1003.sg/";
                    break;
                case "2008":
                    gotoURL = "http://www.883jia.com.sg/";
                    break;
                default:
                    gotoURL = "Unknown";
                    break;
            }
            return gotoURL;
        }

        public static string getStnProgramming(string id)
        {
            string gotoURL;
            switch (id)
            {
                case "1000":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/gold905/programmes.aspx";
                    break;
                case "1001":
                    gotoURL = "http://www.radio913.com/shows";
                    break;
                case "1002":
                    gotoURL = "http://kiss92.sg/shows.html";
                    break;
                case "1003":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/symphony924/programmes.aspx";
                    break;
                case "1004":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/938live/programmes.aspx";
                    break;
                case "1005":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/class95/programmes.aspx";
                    break;
                case "1006":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/987fm/programmes.aspx";
                    break;
                case "1007":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/lush995/on-air.aspx";
                    break;
                case "1008":
                    gotoURL = "http://973fm.webs.com/programmes.html";
                    break;
                case "1009":
                    gotoURL = "http://theliveradio.sg/index.php?id=3";
                    break;
                case "1010":
                    gotoURL = "http://www.bbc.co.uk/worldserviceradio/programmes";
                    break;
                case "1011":
                    gotoURL = "http://www.power98.com.sg/shows";
                    break;
                case "2000":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/ria897/programmes.aspx";
                    break;
                case "2001":
                    gotoURL = "http://entertainment.xin.msn.com/zh/radio/yes933/programmes.aspx";
                    break;
                case "2002":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/warna942/programmes.aspx";
                    break;
                case "2003":
                    gotoURL = "http://entertainment.xin.msn.com/zh/radio/capital958/programmes.aspx";
                    break;
                case "2004":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/xfm963/programmes.aspx";
                    break;
                case "2005":
                    gotoURL = "http://entertainment.xin.msn.com/en/radio/oli968/programmes.aspx";
                    break;
                case "2006":
                    gotoURL = "http://entertainment.xin.msn.com/zh/radio/love972/programmes.aspx";
                    break;
                case "2007":
                    gotoURL = "http://www.ufm1003.sg/shows";
                    break;
                case "2008":
                    gotoURL = "http://www.883jia.com.sg/shows/";
                    break;
                default:
                    gotoURL = "Unknown";
                    break;
            }
            return gotoURL;
        }

        public static string getIdFromName(string name)
        {
            string itemId;
            switch (name)
            {
                case "Gold 90.5FM":
                    {
                        itemId = "1000";
                        break;
                    }
                case "HOT FM91.3":
                    {
                        itemId = "1001";
                        break;
                    }
                case "Kiss 92FM":
                    {
                        itemId = "1002";
                        break;
                    }
                case "Symphony 92.4FM":
                    {
                        itemId = "1003";
                        break;
                    }
                case "938LIVE":
                    {
                        itemId = "1004";
                        break;
                    }
                case "Class 95FM":
                    {
                        itemId = "1005";
                        break;
                    }
                case "987FM":
                    {
                        itemId = "1006";
                        break;
                    }
                case "Lush 99.5FM":
                    {
                        itemId = "1007";
                        break;
                    }
                case "973FM":
                    {
                        itemId = "1008";
                        break;
                    }
                /*case "The Live Radio":
                    {
                        itemId = "1009";
                        break;
                    }*/
                case "Power 98 FM":
                    {
                        itemId = "1011";
                        break;
                    }
                case "Ria 89.7FM":
                    {
                        itemId = "2000";
                        break;
                    }
                case "Y.E.S. 93.3FM":
                    {
                        itemId = "2001";
                        break;
                    }
                case "Warna 94.2FM":
                    {
                        itemId = "2002";
                        break;
                    }
                case "Capital 95.8":
                    {
                        itemId = "2003";
                        break;
                    }
                case "XFM 96.3":
                    {
                        itemId = "2004";
                        break;
                    }
                case "Oli 96.8FM":
                    {
                        itemId = "2005";
                        break;
                    }
                case "Love 97.2FM":
                    {
                        itemId = "2006";
                        break;
                    }
                case "UFM 1003":
                    {
                        itemId = "2007";
                        break;
                    }
                case "88.3 Jia FM":
                    {
                        itemId = "2008";
                        break;
                    }
                case "90.5":
                    {
                        itemId = "1000";
                        break;
                    }
                case "91.3":
                    {
                        itemId = "1001";
                        break;
                    }
                case "92.0":
                    {
                        itemId = "1002";
                        break;
                    }
                case "92.4":
                    {
                        itemId = "1003";
                        break;
                    }
                case "93.8":
                    {
                        itemId = "1004";
                        break;
                    }
                case "95.0":
                    {
                        itemId = "1005";
                        break;
                    }
                case "98.7":
                    {
                        itemId = "1006";
                        break;
                    }
                case "99.5":
                    {
                        itemId = "1007";
                        break;
                    }
                case "97.3":
                    {
                        itemId = "1008";
                        break;
                    }
                case "89.7":
                    {
                        itemId = "2000";
                        break;
                    }
                case "93.3":
                    {
                        itemId = "2001";
                        break;
                    }
                case "94.2":
                    {
                        itemId = "2002";
                        break;
                    }
                case "95.8":
                    {
                        itemId = "2003";
                        break;
                    }
                case "96.3":
                    {
                        itemId = "2004";
                        break;
                    }
                case "96.8":
                    {
                        itemId = "2005";
                        break;
                    }
                case "97.2":
                    {
                        itemId = "2006";
                        break;
                    }
                case "100.3":
                    {
                        itemId = "2007";
                        break;
                    }
                case "BBC World Service":
                    {
                        itemId = "1010";
                        break;
                    }
                default:
                    itemId = "Unknown";
                    break;
            }
            return itemId;
        }
    }
}
