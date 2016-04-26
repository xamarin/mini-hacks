using System;
using Xamarin.Forms;

namespace EvolveApp.Views.Controls
{
	public class DashboardWidget : RelativeLayout
	{
		public StyledLabel WidgetTitle { get; internal set; }
		public StyledLabel WidgetCount { get; internal set; }

		public DashboardWidget ()
		{
			WidgetTitle = new StyledLabel { CssStyle = "widgetTitle" };
			WidgetCount = new StyledLabel { CssStyle = "widgetCount" };

			Func<RelativeLayout, double> getWidgetCountWidth = (p) => WidgetCount.GetSizeRequest (this.Width, this.Height).Request.Width;

			Children.Add (WidgetTitle,
				xConstraint: Constraint.Constant (AppSettings.Margin),
				yConstraint: Constraint.Constant (10),
				widthConstraint: Constraint.RelativeToParent (p => p.Width - AppSettings.Margin * 2)
			);
			Children.Add (WidgetCount,
				xConstraint: Constraint.RelativeToParent (p => (p.Width - getWidgetCountWidth (p)) / 2),
			    yConstraint: Device.OnPlatform (
				Constraint.RelativeToView (WidgetTitle, (p, v) => v.Y + 20),
				Constraint.RelativeToView (WidgetTitle, (p, v) => v.Y + 10),
				Constraint.RelativeToView (WidgetTitle, (p, v) => v.Y + 20)
			   )
			);
			BackgroundColor = AppColors.LightGray;
		}

		bool isInitialized;
		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			if (!isInitialized) {
				//WidgetCount.FontSize = Math.Round (height / 2, 0);
				isInitialized = true;
			}
			base.LayoutChildren (x, y, width, height);
		}
	}
}

