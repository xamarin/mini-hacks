using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

using EvolveApp;
using EvolveApp.UWP;
using Windows.UI.Text;

[assembly: ExportRenderer(typeof(StyledButton),typeof(StyledButtonRenderer))]
namespace EvolveApp.UWP
{
    public class StyledButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if(e.NewElement != null)
            {
                var formsElement = e.NewElement;
                //formsElement.FontFamily = @"\Assets\Fonts\SegoeUIBold.ttf#SegoeUIBold";
                Control.FontWeight = FontWeights.Thin;
                formsElement.FontSize = 16;
                formsElement.TextColor = Color.White;
            }
        }
    }
}