namespace SG_Radio_for_8._1
{
    public class Constants
    {
        public static string getName(string id)
        {
            string itemName;
            switch (id)
            {
                case "4000":
                    {
                        itemName = "BBC World Service";
                        break;
                    }
                case "4001":
                    {
                        itemName = "Money FM 89.3";
                        break;
                    }
                case "4002":
                    {
                        itemName = "GOLD 90.5FM";
                        break;
                    }
                case "4003":
                    {
                        itemName = "One FM 91.3";
                        break;
                    }
                case "4004":
                    {
                        itemName = "Kiss 92FM";
                        break;
                    }
                case "4005":
                    {
                        itemName = "Symphony 92.4FM";
                        break;
                    }
                case "4006":
                    {
                        itemName = "938Now";
                        break;
                    }
                case "4007":
                    {
                        itemName = "Class 95FM";
                        break;
                    }
                case "4008":
                    {
                        itemName = "Power 98FM";
                        break;
                    }
                case "4009":
                    {
                        itemName = "987FM";
                        break;
                    }
                case "4010":
                    {
                        itemName = "973FM";
                        break;
                    }
                case "4011":
                    {
                        itemName = "Asia Expat Radio";
                        break;
                    }
                case "4012":
                    {
                        itemName = "Bible Witness Web Radio";
                        break;
                    }
                case "4013":
                    {
                        itemName = "Hitz.fm";
                        break;
                    }
                case "4014":
                    {
                        itemName = "Orion Station";
                        break;
                    }
                case "5000":
                    {
                        itemName = "88.3Jia FM";
                        break;
                    }
                case "5001":
                    {
                        itemName = "Y.E.S. 93.3FM";
                        break;
                    }
                case "5002":
                    {
                        itemName = "Capital 95.8FM";
                        break;
                    }
                case "5003":
                    {
                        itemName = "96.3 Hao FM";
                        break;
                    }
                case "5004":
                    {
                        itemName = "Love 97.2FM";
                        break;
                    }
                case "5005":
                    {
                        itemName = "UFM 1003";
                        break;
                    }
                case "5006":
                    {
                        itemName = "Ria 89.7FM";
                        break;
                    }
                case "5007":
                    {
                        itemName = "Warna 94.2FM";
                        break;
                    }
                case "5008":
                    {
                        itemName = "Oli 96.8FM";
                        break;
                    }
                case "5009":
                    {
                        itemName = "Radio Melody";
                        break;
                    }
                case "5010":
                    {
                        itemName = "Naga FM";
                        break;
                    }
                case "5011":
                    {
                        itemName = "DesiNetworks";
                        break;
                    }
                case "5012":
                    {
                        itemName = "Desi Dance";
                        break;
                    }
                default:
                    itemName = "Unknown";
                    break;
            }
            return itemName;
        }

        public static string getStream(string id)
        {
            string itemStream;
            switch (id)
            {
                case "4000":
                    {
                        itemStream = "http://bbcwssc.ic.llnwd.net/stream/bbcwssc_mp1_ws-eieuk";
                        break;
                    }
                case "4001":
                    {
                        itemStream = "http://20673.live.streamtheworld.com/MONEY_893_SC";
                        break;
                    }
                case "4002":
                    {
                        itemStream = "http://mediacorp.rastream.com/905fm";
                        break;
                    }
                case "4003":
                    {
                        itemStream = "http://20673.live.streamtheworld.com/ONE_FM_913_SC";
                        break;
                    }
                case "4004":
                    {
                        itemStream = "http://20673.live.streamtheworld.com/KISS_92_SC";
                        break;
                    }
                case "4005":
                    {
                        itemStream = "http://mediacorp.rastream.com/924fm";
                        break;
                    }
                case "4006":
                    {
                        itemStream = "http://mediacorp.rastream.com/938fm";
                        break;
                    }
                case "4007":
                    {
                        itemStream = "http://mediacorp.rastream.com/950fm";
                        break;
                    }
                case "4008":
                    {
                        itemStream = "http://18583.live.streamtheworld.com/POWER98.mp3";
                        break;
                    }
                case "4009":
                    {
                        itemStream = "http://mediacorp.rastream.com/987fm";
                        break;
                    }
                case "4010":
                    {
                        itemStream = "http://icecasthost.heropiggy95.xyz:8650/stream";
                        break;
                    }
                case "4011":
                    {
                        itemStream = "http://axr.rastream.com/axr-singapore";
                        break;
                    }
                case "4012":
                    {
                        itemStream = "http://biblewitness.com:8000/;stream.nsv";
                        break;
                    }
                case "4013":
                    {
                        itemStream = "http://38.107.243.226:8920/;stream.nsv";
                        break;
                    }
                case "4014":
                    {
                        itemStream = "http://188.165.192.5:8528/stream/";
                        break;
                    }
                case "5000":
                    {
                        itemStream = "http://19053.live.streamtheworld.com/883JIA_S";
                        break;
                    }
                case "5001":
                    {
                        itemStream = "http://mediacorp.rastream.com/933fm";
                        break;
                    }
                case "5002":
                    {
                        itemStream = "http://mediacorp.rastream.com/958fm";
                        break;
                    }
                case "5003":
                    {
                        itemStream = "http://20673.live.streamtheworld.com/HAO_963_SC";
                        break;
                    }
                case "5004":
                    {
                        itemStream = "http://mediacorp.rastream.com/972f";
                        break;
                    }
                case "5005":
                    {
                        itemStream = "http://20673.live.streamtheworld.com/UFM_1003_SC";
                        break;
                    }
                case "5006":
                    {
                        itemStream = "http://mediacorp.rastream.com/897fm";
                        break;
                    }
                case "5007":
                    {
                        itemStream = "http://mediacorp.rastream.com/942fm";
                        break;
                    }
                case "5008":
                    {
                        itemStream = "http://mediacorp.rastream.com/968fm";
                        break;
                    }
                case "5009":
                    {
                        itemStream = "http://37.59.28.208:8113/stream";
                        break;
                    }
                case "5010":
                    {
                        itemStream = "http://songs.nagafm.com:8006/stream/1/";
                        break;
                    }
                case "5011":
                    {
                        itemStream = "http://192.99.8.192:3224/;stream.nsv";
                        break;
                    }
                case "5012":
                    {
                        itemStream = "http://usa6.fastcast4u.com:5029/stream";
                        break;
                    }
                default:
                    itemStream = "Unknown";
                    break;
            }
            return itemStream;
        }

        public static string getTitle(string id)
        {
            string mediacorpAPI;
            switch (id)
            {
                case "4002":
                    {
                        mediacorpAPI = "http://liveradio.toggle.sg/api/playouthistory?stationId=905fm";
                        break;
                    }
                case "4005":
                    {
                        mediacorpAPI = "http://liveradio.toggle.sg/api/playouthistory?stationId=924fm";
                        break;
                    }
                case "4006":
                    {
                        mediacorpAPI = "http://liveradio.toggle.sg/api/playouthistory?stationId=938fm";
                        break;
                    }
                case "4007":
                    {
                        mediacorpAPI = "http://liveradio.toggle.sg/api/playouthistory?stationId=950fm";
                        break;
                    }
                case "4009":
                    {
                        mediacorpAPI = "http://liveradio.toggle.sg/api/playouthistory?stationId=987fm";
                        break;
                    }
                case "5001":
                    {
                        mediacorpAPI = "http://liveradio.toggle.sg/api/playouthistory?stationId=933fm";
                        break;
                    }
                case "5002":
                    {
                        mediacorpAPI = "http://liveradio.toggle.sg/api/playouthistory?stationId=958fm";
                        break;
                    }
                case "5004":
                    {
                        mediacorpAPI = "http://liveradio.toggle.sg/api/playouthistory?stationId=972fm";
                        break;
                    }
                case "5006":
                    {
                        mediacorpAPI = "http://liveradio.toggle.sg/api/playouthistory?stationId=897fm";
                        break;
                    }
                case "5007":
                    {
                        mediacorpAPI = "http://liveradio.toggle.sg/api/playouthistory?stationId=942fm";
                        break;
                    }
                case "5008":
                    {
                        mediacorpAPI = "http://liveradio.toggle.sg/api/playouthistory?stationId=968fm";
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
                case "4000":
                    {
                        itemImage = "ms-appx:///Images/English/English_01.png";
                        break;
                    }
                case "4001":
                    {
                        itemImage = "ms-appx:///Images/English/English_02.png";
                        break;
                    }
                case "4002":
                    {
                        itemImage = "ms-appx:///Images/English/English_03.png";
                        break;
                    }
                case "4003":
                    {
                        itemImage = "ms-appx:///Images/English/English_04.png";
                        break;
                    }
                case "4004":
                    {
                        itemImage = "ms-appx:///Images/English/English_05.png";
                        break;
                    }
                case "4005":
                    {
                        itemImage = "ms-appx:///Images/English/English_06.png";
                        break;
                    }
                case "4006":
                    {
                        itemImage = "ms-appx:///Images/English/English_07.png";
                        break;
                    }
                case "4007":
                    {
                        itemImage = "ms-appx:///Images/English/English_08.png";
                        break;
                    }
                case "4008":
                    {
                        itemImage = "ms-appx:///Images/English/English_09.png";
                        break;
                    }
                case "4009":
                    {
                        itemImage = "ms-appx:///Images/English/English_10.png";
                        break;
                    }
                case "4010":
                    {
                        itemImage = "ms-appx:///Images/English/English_11.png";
                        break;
                    }
                case "4011":
                    {
                        itemImage = "ms-appx:///Images/English/English_12.png";
                        break;
                    }
                case "4012":
                    {
                        itemImage = "ms-appx:///Images/English/English_13.png";
                        break;
                    }
                case "4013":
                    {
                        itemImage = "ms-appx:///Images/English/English_14.png";
                        break;
                    }
                case "4014":
                    {
                        itemImage = "ms-appx:///Images/English/English_15.png";
                        break;
                    }
                case "5000":
                    {
                        itemImage = "ms-appx:///Images/Others/Others_01.png";
                        break;
                    }
                case "5001":
                    {
                        itemImage = "ms-appx:///Images/Others/Others_02.png";
                        break;
                    }
                case "5002":
                    {
                        itemImage = "ms-appx:///Images/Others/Others_03.png";
                        break;
                    }
                case "5003":
                    {
                        itemImage = "ms-appx:///Images/Others/Others_04.png";
                        break;
                    }
                case "5004":
                    {
                        itemImage = "ms-appx:///Images/Others/Others_05.png";
                        break;
                    }
                case "5005":
                    {
                        itemImage = "ms-appx:///Images/Others/Others_06.png";
                        break;
                    }
                case "5006":
                    {
                        itemImage = "ms-appx:///Images/Others/Others_07.png";
                        break;
                    }
                case "5007":
                    {
                        itemImage = "ms-appx:///Images/Others/Others_08.png";
                        break;
                    }
                case "5008":
                    {
                        itemImage = "ms-appx:///Images/Others/Others_09.png";
                        break;
                    }
                case "5009":
                    {
                        itemImage = "ms-appx:///Images/Others/Others_10.png";
                        break;
                    }
                case "5010":
                    {
                        itemImage = "ms-appx:///Images/Others/Others_11.png";
                        break;
                    }
                case "5011":
                    {
                        itemImage = "ms-appx:///Images/Others/Others_12.png";
                        break;
                    }
                case "5012":
                    {
                        itemImage = "ms-appx:///Images/Others/Others_13.png";
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
                case "4000":
                    gotoURL = "https://www.bbc.co.uk/worldserviceradio";
                    break;
                case "4001":
                    gotoURL = "http://www.moneyfm893.sg/";
                    break;
                case "4002":
                    gotoURL = "https://radio.toggle.sg/en/gold-905-fm";
                    break;
                case "4003":
                    gotoURL = "https://onefm.sg/";
                    break;
                case "4004":
                    gotoURL = "https://kiss92.sg/";
                    break;
                case "4005":
                    gotoURL = "https://radio.toggle.sg/en/symphony-924-fm";
                    break;
                case "4006":
                    gotoURL = "https://radio.toggle.sg/en/938now";
                    break;
                case "4007":
                    gotoURL = "https://radio.toggle.sg/en/class-95-fm";
                    break;
                case "4008":
                    gotoURL = "https://www.power98.com.sg/";
                    break;
                case "4009":
                    gotoURL = "https://radio.toggle.sg/en/987";
                    break;
                case "4010":
                    gotoURL = "http://973fm.weebly.com/";
                    break;
                case "4011":
                    gotoURL = "http://www.axr.online/";
                    break;
                case "4012":
                    gotoURL = "http://www.biblewitness.com/webradio/";
                    break;
                case "4013":
                    gotoURL = "https://tunein.com/radio/HitzFM-Singapore-s178364/";
                    break;
                case "4014":
                    gotoURL = "http://www.orionstation.net/";
                    break;
                case "5000":
                    gotoURL = "https://www.883jia.com.sg/";
                    break;
                case "5001":
                    gotoURL = "https://radio.toggle.sg/en/yes-933-fm";
                    break;
                case "5002":
                    gotoURL = "https://radio.toggle.sg/en/capital-958-fm";
                    break;
                case "5003":
                    gotoURL = "http://www.fm963.sg/";
                    break;
                case "5004":
                    gotoURL = "https://radio.toggle.sg/en/love-972-fm";
                    break;
                case "5005":
                    gotoURL = "https://ufm1003.sg/";
                    break;
                case "5006":
                    gotoURL = "https://radio.toggle.sg/en/ria-897-fm";
                    break;
                case "5007":
                    gotoURL = "https://radio.toggle.sg/en/warna-942-fm";
                    break;
                case "5008":
                    gotoURL = "https://radio.toggle.sg/en/oli-968-fm";
                    break;
                case "5009":
                    gotoURL = "http://www.rmelody.net/";
                    break;
                case "5010":
                    gotoURL = "http://www.nagafm.com/";
                    break;
                case "5011":
                    gotoURL = "https://www.facebook.com/desinetworks/";
                    break;
                case "5012":
                    gotoURL = "https://www.facebook.com/desinetworks/";
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
                case "4000":
                    gotoURL = "https://www.bbc.co.uk/schedules/p00fzl9p#on-now";
                    break;
                case "4001":
                    gotoURL = "http://www.moneyfm893.sg/programme-schedule/";
                    break;
                case "4002":
                    gotoURL = "https://radio.toggle.sg/en/gold-905-fm/shows";
                    break;
                case "4003":
                    gotoURL = "https://onefm.sg/shows.html";
                    break;
                case "4004":
                    gotoURL = "https://kiss92.sg/shows.html";
                    break;
                case "4005":
                    gotoURL = "https://radio.toggle.sg/en/symphony-924-fm/programmes";
                    break;
                case "4006":
                    gotoURL = "https://radio.toggle.sg/en/938now/programmes";
                    break;
                case "4007":
                    gotoURL = "https://radio.toggle.sg/en/class-95-fm/programmes";
                    break;
                case "4008":
                    gotoURL = "https://www.power98.com.sg/shows/";
                    break;
                case "4009":
                    gotoURL = "https://radio.toggle.sg/en/987/shows";
                    break;
                case "4010":
                    gotoURL = "http://973fm.weebly.com/programmes.html";
                    break;
                case "4011":
                    gotoURL = "http://www.axr.online/pages/schedule/";
                    break;
                case "4012":
                    gotoURL = "http://www.biblewitness.com/webradio/programming/monday-friday.html";
                    break;
                case "4013":
                    gotoURL = "https://tunein.com/radio/HitzFM-Singapore-s178364/";
                    break;
                case "4014":
                    gotoURL = "http://www.orionstation.net/";
                    break;
                case "5000":
                    gotoURL = "https://www.883jia.com.sg/programme-lineup/";
                    break;
                case "5001":
                    gotoURL = "https://radio.toggle.sg/en/yes-933-fm/shows";
                    break;
                case "5002":
                    gotoURL = "https://radio.toggle.sg/en/capital-958-fm/shows";
                    break;
                case "5003":
                    gotoURL = "http://www.fm963.sg/shows/";
                    break;
                case "5004":
                    gotoURL = "https://radio.toggle.sg/en/love-972-fm/programmes";
                    break;
                case "5005":
                    gotoURL = "https://ufm1003.sg/shows.html";
                    break;
                case "5006":
                    gotoURL = "https://radio.toggle.sg/en/ria-897-fm/programmes";
                    break;
                case "5007":
                    gotoURL = "https://radio.toggle.sg/en/warna-942-fm/rancangan";
                    break;
                case "5008":
                    gotoURL = "https://radio.toggle.sg/en/oli-968-fm/programmes";
                    break;
                case "5009":
                    gotoURL = "http://www.rmelody.net/";
                    break;
                case "5010":
                    gotoURL = "http://www.nagafm.com/program/";
                    break;
                case "5011":
                    gotoURL = "https://www.facebook.com/desinetworks/";
                    break;
                case "5012":
                    gotoURL = "https://www.facebook.com/desinetworks/";
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
                case "BBC World Service":
                    {
                        itemId = "4000";
                        break;
                    }
                case "Money FM 89.3":
                    {
                        itemId = "4001";
                        break;
                    }
                case "GOLD 90.5FM":
                    {
                        itemId = "4002";
                        break;
                    }
                case "One FM 91.3":
                    {
                        itemId = "4003";
                        break;
                    }
                case "Kiss 92FM":
                    {
                        itemId = "4004";
                        break;
                    }
                case "Symphony 92.4FM":
                    {
                        itemId = "4005";
                        break;
                    }
                case "938Now":
                    {
                        itemId = "4006";
                        break;
                    }
                case "Class 95FM":
                    {
                        itemId = "4007";
                        break;
                    }
                case "Power 98FM":
                    {
                        itemId = "4008";
                        break;
                    }
                case "987FM":
                    {
                        itemId = "4009";
                        break;
                    }
                case "973FM":
                    {
                        itemId = "4010";
                        break;
                    }
                case "Asia Expat Radio":
                    {
                        itemId = "4011";
                        break;
                    }
                case "Bible Witness Web Radio":
                    {
                        itemId = "4012";
                        break;
                    }
                case "Hitz.fm":
                    {
                        itemId = "4013";
                        break;
                    }
                case "Orion Station":
                    {
                        itemId = "4014";
                        break;
                    }
                case "88.3Jia FM":
                    {
                        itemId = "5000";
                        break;
                    }
                case "Y.E.S. 93.3FM":
                    {
                        itemId = "5001";
                        break;
                    }
                case "Capital 95.8FM":
                    {
                        itemId = "5002";
                        break;
                    }
                case "96.3 Hao FM":
                    {
                        itemId = "5003";
                        break;
                    }
                case "Love 97.2FM":
                    {
                        itemId = "5004";
                        break;
                    }
                case "UFM 1003":
                    {
                        itemId = "5005";
                        break;
                    }
                case "Ria 89.7FM":
                    {
                        itemId = "5006";
                        break;
                    }
                case "Warna 94.2FM":
                    {
                        itemId = "5007";
                        break;
                    }
                case "Oli 96.8FM":
                    {
                        itemId = "5008";
                        break;
                    }
                case "Radio Melody":
                    {
                        itemId = "5009";
                        break;
                    }
                case "Naga FM":
                    {
                        itemId = "5010";
                        break;
                    }
                case "DesiNetworks":
                    {
                        itemId = "5011";
                        break;
                    }
                case "Desi Dance":
                    {
                        itemId = "5012";
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
