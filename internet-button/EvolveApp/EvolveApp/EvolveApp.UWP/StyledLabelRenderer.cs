using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

using EvolveApp;
using EvolveApp.UWP;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Text;

[assembly: ExportRenderer(typeof(StyledLabel),typeof(StyledLabelRenderer))]
namespace EvolveApp.UWP
{
    public class StyledLabelRenderer : LabelRenderer
    {
        StyledLabel formsElement;
        TextBlock uwpElement;
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                formsElement = e.NewElement as StyledLabel;
                formsElement.HorizontalTextAlignment = TextAlignment.Center;
                uwpElement = Control;

                switch (formsElement.CssStyle)
                {
                    case "h1":
                        SetHeading1Font();
                        break;
                    case "h2":
                        SetHeading2Font();
                        break;
                    case "body":
                        SetBodyFont();
                        break;
                    case "widgetCount":
                        SetWidgetCountFont();
                        break;
                }
            }
        }

        public void SetHeading1Font()
        {
            uwpElement.FontFamily = new FontFamily("Segoe UI");
            uwpElement.FontWeight = FontWeights.SemiLight;
            formsElement.FontSize = 38;
            formsElement.TextColor = Color.FromHex("#1C2B39");
        }

        public void SetHeading2Font()
        {
            uwpElement.FontFamily = new FontFamily("Segoe UI");
            uwpElement.FontWeight = FontWeights.ExtraLight;
            formsElement.FontSize = 20;
            formsElement.TextColor = Color.FromHex("#07add0");
        }

        public void SetBodyFont()
        {
            uwpElement.FontWeight = FontWeights.Light;
            formsElement.FontSize = 16;
            formsElement.TextColor = Color.FromHex("#778687");
        }

        public void SetWidgetCountFont()
        {
            uwpElement.FontWeight = FontWeights.Light;
            formsElement.FontSize = 70;
            formsElement.TextColor = Color.FromHex("#778687");
        }
    }
}
//h1{
//	font-family    		: "SegoeUI-Light";
//	font-size        	: 38px;
//	text-overflow		: "ellipsis";
//	text-align			: "center";
//	lines				: 0;
//	color				: #1C2B39;
//	letter-spacing		: 0;
//	line-height			: 34;
//}

//h2{
//	font-family    		: "SegoeUI";
//	font-size        	: 20px;
//	text-overflow		: "ellipsis";
//	text-align			: "center";
//	lines				: 0;
//	color				: #07add0;
//	text-transform		: "uppercase";
//}

//body{
//	font-family      	: "SegoeUI";
//	color            	: #778687;
//	font-size        	: 16;
//	text-align			: "center";
//	line-height			: 21;
//}
