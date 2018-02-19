#if !WINRT_NOT_PRESENT
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
#endif

namespace NotificationsExtensions
{
    public interface INotificationContent
    {
        string GetContent();

#if !WINRT_NOT_PRESENT
        XmlDocument GetXml();
#endif
    }

    public interface INotificationContentText
    {
        string Text { get; set; }

        string Lang { get; set; }
    }

    public interface INotificationContentImage
    {
        string Src { get; set; }

        string Alt { get; set; }

        bool AddImageQuery { get; set; }
    }

    namespace TileContent
    {
        public interface ITileNotificationContent : INotificationContent
        {
            bool StrictValidation { get; set; }

            string Lang { get; set; }

            string BaseUri { get; set; }

            TileBranding Branding { get; set; }

            bool AddImageQuery { get; set; }

#if !WINRT_NOT_PRESENT
            TileNotification CreateNotification();
#endif
        }

        public interface ISquare150x150TileNotificationContent : ITileNotificationContent
        {
        }

        public interface IWide310x150TileNotificationContent : ITileNotificationContent
        {
            ISquare150x150TileNotificationContent Square150x150Content { get; set; }

            bool RequireSquare150x150Content { get; set; }
        }

        public interface ISquare310x310TileNotificationContent : ITileNotificationContent
        {
            IWide310x150TileNotificationContent Wide310x150Content { get; set; }

            bool RequireWide310x150Content { get; set; }
        }

        public interface ISquare99x99TileNotificationContent : ITileNotificationContent
        {
        }

        public interface ISquare210x210TileNotificationContent : ITileNotificationContent
        {
            ISquare99x99TileNotificationContent Square99x99Content { get; set; }

            bool RequireSquare99x99Content { get; set; }
        }

        public interface IWide432x210TileNotificationContent : ITileNotificationContent
        {
            ISquare210x210TileNotificationContent Square210x210Content { get; set; }

            bool RequireSquare210x210Content { get; set; }
        }

        public interface ITileSquare150x150Block : ISquare150x150TileNotificationContent
        {
            INotificationContentText TextBlock { get; }

            INotificationContentText TextSubBlock { get; }
        }

        public interface ITileSquare150x150Image : ISquare150x150TileNotificationContent
        {
            INotificationContentImage Image { get; }
        }

        public interface ITileSquare150x150PeekImageAndText01 : ISquare150x150TileNotificationContent
        {
            INotificationContentImage Image { get; }

            INotificationContentText TextHeading { get; }

            INotificationContentText TextBody1 { get; }

            INotificationContentText TextBody2 { get; }

            INotificationContentText TextBody3 { get; }
        }

        public interface ITileSquare150x150PeekImageAndText02 : ISquare150x150TileNotificationContent
        {
            INotificationContentImage Image { get; }

            INotificationContentText TextHeading { get; }

            INotificationContentText TextBodyWrap { get; }
        }

        public interface ITileSquare150x150PeekImageAndText03 : ISquare150x150TileNotificationContent
        {
            INotificationContentImage Image { get; }

            INotificationContentText TextBody1 { get; }

            INotificationContentText TextBody2 { get; }

            INotificationContentText TextBody3 { get; }

            INotificationContentText TextBody4 { get; }
        }

        public interface ITileSquare150x150PeekImageAndText04 : ISquare150x150TileNotificationContent
        {
            INotificationContentImage Image { get; }

            INotificationContentText TextBodyWrap { get; }
        }

        public interface ITileSquare150x150Text01 : ISquare150x150TileNotificationContent
        {
            INotificationContentText TextHeading { get; }

            INotificationContentText TextBody1 { get; }

            INotificationContentText TextBody2 { get; }

            INotificationContentText TextBody3 { get; }
        }

        public interface ITileSquare150x150Text02 : ISquare150x150TileNotificationContent
        {
            INotificationContentText TextHeading { get; }

            INotificationContentText TextBodyWrap { get; }
        }




        public interface ITileSquare150x150Text03 : ISquare150x150TileNotificationContent
        {



            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }




            INotificationContentText TextBody3 { get; }




            INotificationContentText TextBody4 { get; }
        }




        public interface ITileSquare150x150Text04 : ISquare150x150TileNotificationContent
        {



            INotificationContentText TextBodyWrap { get; }
        }




        public interface ITileWide310x150BlockAndText01 : IWide310x150TileNotificationContent
        {



            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }




            INotificationContentText TextBody3 { get; }




            INotificationContentText TextBody4 { get; }




            INotificationContentText TextBlock { get; }




            INotificationContentText TextSubBlock { get; }
        }




        public interface ITileWide310x150BlockAndText02 : IWide310x150TileNotificationContent
        {



            INotificationContentText TextBodyWrap { get; }




            INotificationContentText TextBlock { get; }




            INotificationContentText TextSubBlock { get; }
        }




        public interface ITileWide310x150Image : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }
        }




        public interface ITileWide310x150ImageAndText01 : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextCaptionWrap { get; }
        }




        public interface ITileWide310x150ImageAndText02 : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextCaption1 { get; }




            INotificationContentText TextCaption2 { get; }
        }





        public interface ITileWide310x150ImageCollection : IWide310x150TileNotificationContent
        {



            INotificationContentImage ImageMain { get; }




            INotificationContentImage ImageSmallColumn1Row1 { get; }




            INotificationContentImage ImageSmallColumn2Row1 { get; }




            INotificationContentImage ImageSmallColumn1Row2 { get; }




            INotificationContentImage ImageSmallColumn2Row2 { get; }
        }





        public interface ITileWide310x150PeekImage01 : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextHeading { get; }




            INotificationContentText TextBodyWrap { get; }
        }





        public interface ITileWide310x150PeekImage02 : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextHeading { get; }




            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }




            INotificationContentText TextBody3 { get; }




            INotificationContentText TextBody4 { get; }
        }





        public interface ITileWide310x150PeekImage03 : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextHeadingWrap { get; }
        }





        public interface ITileWide310x150PeekImage04 : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextBodyWrap { get; }
        }





        public interface ITileWide310x150PeekImage05 : IWide310x150TileNotificationContent
        {



            INotificationContentImage ImageMain { get; }




            INotificationContentImage ImageSecondary { get; }




            INotificationContentText TextHeading { get; }




            INotificationContentText TextBodyWrap { get; }
        }





        public interface ITileWide310x150PeekImage06 : IWide310x150TileNotificationContent
        {



            INotificationContentImage ImageMain { get; }




            INotificationContentImage ImageSecondary { get; }




            INotificationContentText TextHeadingWrap { get; }
        }





        public interface ITileWide310x150PeekImageAndText01 : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextBodyWrap { get; }
        }





        public interface ITileWide310x150PeekImageAndText02 : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }




            INotificationContentText TextBody3 { get; }




            INotificationContentText TextBody4 { get; }




            INotificationContentText TextBody5 { get; }
        }






        public interface ITileWide310x150PeekImageCollection01 : ITileWide310x150ImageCollection
        {



            INotificationContentText TextHeading { get; }




            INotificationContentText TextBodyWrap { get; }
        }






        public interface ITileWide310x150PeekImageCollection02 : ITileWide310x150ImageCollection
        {



            INotificationContentText TextHeading { get; }




            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }




            INotificationContentText TextBody3 { get; }




            INotificationContentText TextBody4 { get; }
        }






        public interface ITileWide310x150PeekImageCollection03 : ITileWide310x150ImageCollection
        {



            INotificationContentText TextHeadingWrap { get; }
        }






        public interface ITileWide310x150PeekImageCollection04 : ITileWide310x150ImageCollection
        {



            INotificationContentText TextBodyWrap { get; }
        }






        public interface ITileWide310x150PeekImageCollection05 : ITileWide310x150ImageCollection
        {



            INotificationContentImage ImageSecondary { get; }




            INotificationContentText TextHeading { get; }




            INotificationContentText TextBodyWrap { get; }
        }






        public interface ITileWide310x150PeekImageCollection06 : ITileWide310x150ImageCollection
        {



            INotificationContentImage ImageSecondary { get; }




            INotificationContentText TextHeadingWrap { get; }
        }




        public interface ITileWide310x150SmallImageAndText01 : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextHeadingWrap { get; }
        }




        public interface ITileWide310x150SmallImageAndText02 : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextHeading { get; }




            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }




            INotificationContentText TextBody3 { get; }




            INotificationContentText TextBody4 { get; }
        }




        public interface ITileWide310x150SmallImageAndText03 : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextBodyWrap { get; }
        }




        public interface ITileWide310x150SmallImageAndText04 : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextHeading { get; }




            INotificationContentText TextBodyWrap { get; }
        }




        public interface ITileWide310x150SmallImageAndText05 : IWide310x150TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextHeading { get; }




            INotificationContentText TextBodyWrap { get; }
        }




        public interface ITileWide310x150Text01 : IWide310x150TileNotificationContent
        {



            INotificationContentText TextHeading { get; }




            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }




            INotificationContentText TextBody3 { get; }




            INotificationContentText TextBody4 { get; }
        }





        public interface ITileWide310x150Text02 : IWide310x150TileNotificationContent
        {



            INotificationContentText TextHeading { get; }




            INotificationContentText TextColumn1Row1 { get; }




            INotificationContentText TextColumn2Row1 { get; }




            INotificationContentText TextColumn1Row2 { get; }




            INotificationContentText TextColumn2Row2 { get; }




            INotificationContentText TextColumn1Row3 { get; }




            INotificationContentText TextColumn2Row3 { get; }




            INotificationContentText TextColumn1Row4 { get; }




            INotificationContentText TextColumn2Row4 { get; }
        }




        public interface ITileWide310x150Text03 : IWide310x150TileNotificationContent
        {



            INotificationContentText TextHeadingWrap { get; }
        }




        public interface ITileWide310x150Text04 : IWide310x150TileNotificationContent
        {



            INotificationContentText TextBodyWrap { get; }
        }




        public interface ITileWide310x150Text05 : IWide310x150TileNotificationContent
        {



            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }




            INotificationContentText TextBody3 { get; }




            INotificationContentText TextBody4 { get; }




            INotificationContentText TextBody5 { get; }
        }





        public interface ITileWide310x150Text06 : IWide310x150TileNotificationContent
        {



            INotificationContentText TextColumn1Row1 { get; }




            INotificationContentText TextColumn2Row1 { get; }




            INotificationContentText TextColumn1Row2 { get; }




            INotificationContentText TextColumn2Row2 { get; }




            INotificationContentText TextColumn1Row3 { get; }




            INotificationContentText TextColumn2Row3 { get; }




            INotificationContentText TextColumn1Row4 { get; }




            INotificationContentText TextColumn2Row4 { get; }




            INotificationContentText TextColumn1Row5 { get; }




            INotificationContentText TextColumn2Row5 { get; }
        }





        public interface ITileWide310x150Text07 : IWide310x150TileNotificationContent
        {



            INotificationContentText TextHeading { get; }




            INotificationContentText TextShortColumn1Row1 { get; }




            INotificationContentText TextColumn2Row1 { get; }




            INotificationContentText TextShortColumn1Row2 { get; }




            INotificationContentText TextColumn2Row2 { get; }




            INotificationContentText TextShortColumn1Row3 { get; }




            INotificationContentText TextColumn2Row3 { get; }




            INotificationContentText TextShortColumn1Row4 { get; }




            INotificationContentText TextColumn2Row4 { get; }
        }





        public interface ITileWide310x150Text08 : IWide310x150TileNotificationContent
        {



            INotificationContentText TextShortColumn1Row1 { get; }




            INotificationContentText TextShortColumn1Row2 { get; }




            INotificationContentText TextShortColumn1Row3 { get; }




            INotificationContentText TextShortColumn1Row4 { get; }




            INotificationContentText TextShortColumn1Row5 { get; }




            INotificationContentText TextColumn2Row1 { get; }




            INotificationContentText TextColumn2Row2 { get; }




            INotificationContentText TextColumn2Row3 { get; }




            INotificationContentText TextColumn2Row4 { get; }




            INotificationContentText TextColumn2Row5 { get; }
        }




        public interface ITileWide310x150Text09 : IWide310x150TileNotificationContent
        {



            INotificationContentText TextHeading { get; }




            INotificationContentText TextBodyWrap { get; }
        }





        public interface ITileWide310x150Text10 : IWide310x150TileNotificationContent
        {



            INotificationContentText TextHeading { get; }




            INotificationContentText TextPrefixColumn1Row1 { get; }




            INotificationContentText TextColumn2Row1 { get; }




            INotificationContentText TextPrefixColumn1Row2 { get; }




            INotificationContentText TextColumn2Row2 { get; }




            INotificationContentText TextPrefixColumn1Row3 { get; }




            INotificationContentText TextColumn2Row3 { get; }




            INotificationContentText TextPrefixColumn1Row4 { get; }




            INotificationContentText TextColumn2Row4 { get; }
        }





        public interface ITileWide310x150Text11 : IWide310x150TileNotificationContent
        {



            INotificationContentText TextPrefixColumn1Row1 { get; }




            INotificationContentText TextColumn2Row1 { get; }




            INotificationContentText TextPrefixColumn1Row2 { get; }




            INotificationContentText TextColumn2Row2 { get; }




            INotificationContentText TextPrefixColumn1Row3 { get; }




            INotificationContentText TextColumn2Row3 { get; }




            INotificationContentText TextPrefixColumn1Row4 { get; }




            INotificationContentText TextColumn2Row4 { get; }




            INotificationContentText TextPrefixColumn1Row5 { get; }




            INotificationContentText TextColumn2Row5 { get; }
        }






        public interface ITileSquare310x310BlockAndText01 : ISquare310x310TileNotificationContent
        {



            INotificationContentText TextHeadingWrap { get; }




            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }




            INotificationContentText TextBody3 { get; }




            INotificationContentText TextBody4 { get; }




            INotificationContentText TextBody5 { get; }




            INotificationContentText TextBody6 { get; }




            INotificationContentText TextBlock { get; }




            INotificationContentText TextSubBlock { get; }
        }




        public interface ITileSquare310x310BlockAndText02 : ISquare310x310TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextBlock { get; }




            INotificationContentText TextHeading1 { get; }




            INotificationContentText TextHeading2 { get; }




            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }




            INotificationContentText TextBody3 { get; }




            INotificationContentText TextBody4 { get; }
        }




        public interface ITileSquare310x310Image : ISquare310x310TileNotificationContent
        {



            INotificationContentImage Image { get; }
        }




        public interface ITileSquare310x310ImageAndText01 : ISquare310x310TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextCaptionWrap { get; }
        }




        public interface ITileSquare310x310ImageAndText02 : ISquare310x310TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextCaption1 { get; }




            INotificationContentText TextCaption2 { get; }
        }




        public interface ITileSquare310x310ImageAndTextOverlay01 : ISquare310x310TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextHeadingWrap { get; }
        }




        public interface ITileSquare310x310ImageAndTextOverlay02 : ISquare310x310TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextHeadingWrap { get; }




            INotificationContentText TextBodyWrap { get; }
        }




        public interface ITileSquare310x310ImageAndTextOverlay03 : ISquare310x310TileNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextHeadingWrap { get; }




            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }




            INotificationContentText TextBody3 { get; }
        }





        public interface ITileSquare310x310ImageCollection : ISquare310x310TileNotificationContent
        {



            INotificationContentImage ImageMain { get; }




            INotificationContentImage ImageSmall1 { get; }




            INotificationContentImage ImageSmall2 { get; }




            INotificationContentImage ImageSmall3 { get; }




            INotificationContentImage ImageSmall4 { get; }
        }





        public interface ITileSquare310x310ImageCollectionAndText01 : ISquare310x310TileNotificationContent
        {



            INotificationContentImage ImageMain { get; }




            INotificationContentImage ImageSmall1 { get; }




            INotificationContentImage ImageSmall2 { get; }




            INotificationContentImage ImageSmall3 { get; }




            INotificationContentImage ImageSmall4 { get; }




            INotificationContentText TextCaptionWrap { get; }
        }





        public interface ITileSquare310x310ImageCollectionAndText02 : ISquare310x310TileNotificationContent
        {



            INotificationContentImage ImageMain { get; }




            INotificationContentImage ImageSmall1 { get; }




            INotificationContentImage ImageSmall2 { get; }




            INotificationContentImage ImageSmall3 { get; }




            INotificationContentImage ImageSmall4 { get; }




            INotificationContentText TextCaption1 { get; }




            INotificationContentText TextCaption2 { get; }
        }





        public interface ITileSquare310x310SmallImagesAndTextList01 : ISquare310x310TileNotificationContent
        {



            INotificationContentImage Image1 { get; }




            INotificationContentText TextHeading1 { get; }




            INotificationContentText TextBodyGroup1Field1 { get; }




            INotificationContentText TextBodyGroup1Field2 { get; }




            INotificationContentImage Image2 { get; }




            INotificationContentText TextHeading2 { get; }




            INotificationContentText TextBodyGroup2Field1 { get; }




            INotificationContentText TextBodyGroup2Field2 { get; }




            INotificationContentImage Image3 { get; }




            INotificationContentText TextHeading3 { get; }




            INotificationContentText TextBodyGroup3Field1 { get; }




            INotificationContentText TextBodyGroup3Field2 { get; }
        }





        public interface ITileSquare310x310SmallImagesAndTextList02 : ISquare310x310TileNotificationContent
        {



            INotificationContentImage Image1 { get; }




            INotificationContentText TextWrap1 { get; }




            INotificationContentImage Image2 { get; }




            INotificationContentText TextWrap2 { get; }




            INotificationContentImage Image3 { get; }




            INotificationContentText TextWrap3 { get; }
        }





        public interface ITileSquare310x310SmallImagesAndTextList03 : ISquare310x310TileNotificationContent
        {



            INotificationContentImage Image1 { get; }




            INotificationContentText TextHeading1 { get; }




            INotificationContentText TextWrap1 { get; }




            INotificationContentImage Image2 { get; }




            INotificationContentText TextHeading2 { get; }




            INotificationContentText TextWrap2 { get; }




            INotificationContentImage Image3 { get; }




            INotificationContentText TextHeading3 { get; }




            INotificationContentText TextWrap3 { get; }
        }





        public interface ITileSquare310x310SmallImagesAndTextList04 : ISquare310x310TileNotificationContent
        {



            INotificationContentImage Image1 { get; }




            INotificationContentText TextHeading1 { get; }




            INotificationContentText TextWrap1 { get; }




            INotificationContentImage Image2 { get; }




            INotificationContentText TextHeading2 { get; }




            INotificationContentText TextWrap2 { get; }




            INotificationContentImage Image3 { get; }




            INotificationContentText TextHeading3 { get; }




            INotificationContentText TextWrap3 { get; }
        }




        public interface ITileSquare310x310Text01 : ISquare310x310TileNotificationContent
        {



            INotificationContentText TextHeading { get; }




            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }




            INotificationContentText TextBody3 { get; }




            INotificationContentText TextBody4 { get; }




            INotificationContentText TextBody5 { get; }




            INotificationContentText TextBody6 { get; }




            INotificationContentText TextBody7 { get; }




            INotificationContentText TextBody8 { get; }




            INotificationContentText TextBody9 { get; }
        }





        public interface ITileSquare310x310Text02 : ISquare310x310TileNotificationContent
        {



            INotificationContentText TextHeading { get; }




            INotificationContentText TextColumn1Row1 { get; }




            INotificationContentText TextColumn2Row1 { get; }




            INotificationContentText TextColumn1Row2 { get; }




            INotificationContentText TextColumn2Row2 { get; }




            INotificationContentText TextColumn1Row3 { get; }




            INotificationContentText TextColumn2Row3 { get; }




            INotificationContentText TextColumn1Row4 { get; }




            INotificationContentText TextColumn2Row4 { get; }




            INotificationContentText TextColumn1Row5 { get; }




            INotificationContentText TextColumn2Row5 { get; }




            INotificationContentText TextColumn1Row6 { get; }




            INotificationContentText TextColumn2Row6 { get; }




            INotificationContentText TextColumn1Row7 { get; }




            INotificationContentText TextColumn2Row7 { get; }




            INotificationContentText TextColumn1Row8 { get; }




            INotificationContentText TextColumn2Row8 { get; }




            INotificationContentText TextColumn1Row9 { get; }




            INotificationContentText TextColumn2Row9 { get; }
        }




        public interface ITileSquare310x310Text03 : ISquare310x310TileNotificationContent
        {



            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }




            INotificationContentText TextBody3 { get; }




            INotificationContentText TextBody4 { get; }




            INotificationContentText TextBody5 { get; }




            INotificationContentText TextBody6 { get; }




            INotificationContentText TextBody7 { get; }




            INotificationContentText TextBody8 { get; }




            INotificationContentText TextBody9 { get; }




            INotificationContentText TextBody10 { get; }




            INotificationContentText TextBody11 { get; }
        }





        public interface ITileSquare310x310Text04 : ISquare310x310TileNotificationContent
        {



            INotificationContentText TextColumn1Row1 { get; }




            INotificationContentText TextColumn2Row1 { get; }




            INotificationContentText TextColumn1Row2 { get; }




            INotificationContentText TextColumn2Row2 { get; }




            INotificationContentText TextColumn1Row3 { get; }




            INotificationContentText TextColumn2Row3 { get; }




            INotificationContentText TextColumn1Row4 { get; }




            INotificationContentText TextColumn2Row4 { get; }




            INotificationContentText TextColumn1Row5 { get; }




            INotificationContentText TextColumn2Row5 { get; }




            INotificationContentText TextColumn1Row6 { get; }




            INotificationContentText TextColumn2Row6 { get; }




            INotificationContentText TextColumn1Row7 { get; }




            INotificationContentText TextColumn2Row7 { get; }




            INotificationContentText TextColumn1Row8 { get; }




            INotificationContentText TextColumn2Row8 { get; }




            INotificationContentText TextColumn1Row9 { get; }




            INotificationContentText TextColumn2Row9 { get; }




            INotificationContentText TextColumn1Row10 { get; }




            INotificationContentText TextColumn2Row10 { get; }




            INotificationContentText TextColumn1Row11 { get; }




            INotificationContentText TextColumn2Row11 { get; }
        }





        public interface ITileSquare310x310Text05 : ITileSquare310x310Text02
        {

        }





        public interface ITileSquare310x310Text06 : ITileSquare310x310Text04
        {

        }





        public interface ITileSquare310x310Text07 : ITileSquare310x310Text02
        {

        }





        public interface ITileSquare310x310Text08 : ITileSquare310x310Text04
        {

        }





        public interface ITileSquare310x310TextList01 : ISquare310x310TileNotificationContent
        {



            INotificationContentText TextHeading1 { get; }




            INotificationContentText TextBodyGroup1Field1 { get; }




            INotificationContentText TextBodyGroup1Field2 { get; }




            INotificationContentText TextHeading2 { get; }




            INotificationContentText TextBodyGroup2Field1 { get; }




            INotificationContentText TextBodyGroup2Field2 { get; }




            INotificationContentText TextHeading3 { get; }




            INotificationContentText TextBodyGroup3Field1 { get; }




            INotificationContentText TextBodyGroup3Field2 { get; }
        }




        public interface ITileSquare310x310TextList02 : ISquare310x310TileNotificationContent
        {



            INotificationContentText TextWrap1 { get; }




            INotificationContentText TextWrap2 { get; }




            INotificationContentText TextWrap3 { get; }
        }





        public interface ITileSquare310x310TextList03 : ISquare310x310TileNotificationContent
        {



            INotificationContentText TextHeading1 { get; }




            INotificationContentText TextWrap1 { get; }




            INotificationContentText TextHeading2 { get; }




            INotificationContentText TextWrap2 { get; }




            INotificationContentText TextHeading3 { get; }




            INotificationContentText TextWrap3 { get; }
        }




        public interface ITileSquare99x99IconWithBadge : ISquare99x99TileNotificationContent
        {



            INotificationContentImage ImageIcon { get; }
        }




        public interface ITileSquare210x210IconWithBadge : ISquare210x210TileNotificationContent
        {



            INotificationContentImage ImageIcon { get; }
        }




        public interface ITileWide432x210IconWithBadgeAndText : IWide432x210TileNotificationContent
        {



            INotificationContentImage ImageIcon { get; }




            INotificationContentText TextHeading { get; }




            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }
        }





        public enum TileBranding
        {



            None = 0,



            Logo,



            Name
        }
    }

    namespace ToastContent
    {




        public interface IToastAudio
        {



            ToastAudioContent Content { get; set; }





            bool Loop { get; set; }
        }





        public interface IIncomingCallCommands
        {



            bool ShowVideoCommand { get; set; }




            string VideoArgument { get; set; }




            bool ShowVoiceCommand { get; set; }




            string VoiceArgument { get; set; }




            bool ShowDeclineCommand { get; set; }




            string DeclineArgument { get; set; }
        }





        public interface IAlarmCommands
        {



            bool ShowSnoozeCommand { get; set; }




            string SnoozeArgument { get; set; }




            bool ShowDismissCommand { get; set; }




            string DismissArgument { get; set; }
        }




        public interface IToastNotificationContent : INotificationContent
        {




            bool StrictValidation { get; set; }





            string Lang { get; set; }






            string BaseUri { get; set; }













            bool AddImageQuery { get; set; }




            string Launch { get; set; }




            IToastAudio Audio { get; }




            ToastDuration Duration { get; set; }





            IIncomingCallCommands IncomingCallCommands { get; }






            IAlarmCommands AlarmCommands { get; }

#if !WINRT_NOT_PRESENT




            ToastNotification CreateNotification();
#endif
        }




        public enum ToastAudioContent
        {



            Default = 0,



            Mail,



            SMS,



            IM,



            Reminder,




            LoopingCall,




            LoopingCall2,




            LoopingCall3,




            LoopingCall4,




            LoopingCall5,




            LoopingCall6,




            LoopingCall7,




            LoopingCall8,




            LoopingCall9,




            LoopingCall10,




            LoopingAlarm,




            LoopingAlarm2,




            LoopingAlarm3,




            LoopingAlarm4,




            LoopingAlarm5,




            LoopingAlarm6,




            LoopingAlarm7,




            LoopingAlarm8,




            LoopingAlarm9,




            LoopingAlarm10,



            Silent
        }




        public enum ToastDuration
        {



            Short = 0,



            Long
        }




        public interface IToastImageAndText01 : IToastNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextBodyWrap { get; }
        }




        public interface IToastImageAndText02 : IToastNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextHeading { get; }




            INotificationContentText TextBodyWrap { get; }
        }




        public interface IToastImageAndText03 : IToastNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextHeadingWrap { get; }




            INotificationContentText TextBody { get; }
        }




        public interface IToastImageAndText04 : IToastNotificationContent
        {



            INotificationContentImage Image { get; }




            INotificationContentText TextHeading { get; }




            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }
        }




        public interface IToastText01 : IToastNotificationContent
        {



            INotificationContentText TextBodyWrap { get; }
        }




        public interface IToastText02 : IToastNotificationContent
        {



            INotificationContentText TextHeading { get; }




            INotificationContentText TextBodyWrap { get; }
        }




        public interface IToastText03 : IToastNotificationContent
        {



            INotificationContentText TextHeadingWrap { get; }




            INotificationContentText TextBody { get; }
        }




        public interface IToastText04 : IToastNotificationContent
        {



            INotificationContentText TextHeading { get; }




            INotificationContentText TextBody1 { get; }




            INotificationContentText TextBody2 { get; }
        }
    }

    namespace BadgeContent
    {



        public interface IBadgeNotificationContent : INotificationContent
        {
#if !WINRT_NOT_PRESENT




            BadgeNotification CreateNotification();
#endif
        }




        public enum GlyphValue
        {




            None = 0,



            Activity,



            Alert,



            Available,



            Away,



            Busy,



            NewMessage,



            Paused,



            Playing,



            Unavailable,



            Error,



            Attention
        }
    }
}
