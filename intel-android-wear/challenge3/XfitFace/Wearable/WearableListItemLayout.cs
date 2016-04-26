//
//  Copyright 2016 Google Inc. All Rights Reserved.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using Android.Animation;
using Android.Content;
using Android.Support.Wearable.Views;
using Android.Util;
using Android.Widget;

namespace XFitWatchface
{

	public class WearableListItemLayout : LinearLayout, WearableListView.IOnCenterProximityListener
	{
		CircledImageView circle;
		TextView label;

		// The duration of the expand/shrink animation.
		const int AnimationDurationMs = 150;

		// The ratio for the size of a circle in shrink state.
		const float ShrinkCricleRatio = .75f;

		const float ShrinkLabelAlpha = .5f;
		const float ExpandLabelAlpha = 1f;

		float expandCircleRadius;
		float shrinkCircleRadius;

		ObjectAnimator expandCircleAnimator;
		ObjectAnimator expandLabelAnimator;
		AnimatorSet expandAnimator;

		ObjectAnimator shrinkCircleAnimator;
		ObjectAnimator shrinkLabelAnimator;
		AnimatorSet shrinkAnimator;

		public WearableListItemLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
		}

		public WearableListItemLayout(Context context, IAttributeSet attrs) : base(context, attrs)
		{
		}

		public WearableListItemLayout(Context context) : base(context)
		{
		}

		protected override void OnFinishInflate()
		{
			base.OnFinishInflate();
			label = FindViewById<TextView>(Resource.Id.Label);
			circle = FindViewById<CircledImageView>(Resource.Id.Circle);

			expandCircleRadius = circle.CircleRadius;
			shrinkCircleRadius = expandCircleRadius * ShrinkCricleRatio;

			shrinkCircleAnimator = ObjectAnimator.OfFloat(circle, "circleRadius", expandCircleRadius, shrinkCircleRadius);
			shrinkLabelAnimator = ObjectAnimator.OfFloat(label, "alpha", ExpandLabelAlpha, ShrinkLabelAlpha);

			// FIXME Xamarin: new AnimatorSet().SetDuration(long) should return an AnimatorSet
			shrinkAnimator = new AnimatorSet();
			shrinkAnimator.SetDuration(AnimationDurationMs);
			shrinkAnimator.PlayTogether(shrinkCircleAnimator, shrinkLabelAnimator);

			expandCircleAnimator = ObjectAnimator.OfFloat(circle, "circleRadius", shrinkCircleRadius, expandCircleRadius);
			expandLabelAnimator = ObjectAnimator.OfFloat(label, "alpha", ShrinkLabelAlpha, ExpandLabelAlpha);
			expandAnimator = new AnimatorSet();
			expandAnimator.SetDuration(AnimationDurationMs);
			expandAnimator.PlayTogether(expandCircleAnimator, expandLabelAnimator);
		}


		public void OnCenterPosition(bool animate)
		{
			if (animate)
			{
				shrinkAnimator.Cancel();
				if (!expandAnimator.IsRunning)
				{
					expandCircleAnimator.SetFloatValues(circle.CircleRadius, expandCircleRadius);
					expandLabelAnimator.SetFloatValues(label.Alpha, ExpandLabelAlpha);
					expandAnimator.Start();
				}
			}
			else {
				expandAnimator.Cancel();
				circle.CircleRadius = expandCircleRadius;
				label.Alpha = ExpandLabelAlpha;
			}
		}

		public void OnNonCenterPosition(bool animate)
		{
			if (animate)
			{
				expandAnimator.Cancel();
				if (!shrinkAnimator.IsRunning)
				{
					shrinkCircleAnimator.SetFloatValues(circle.CircleRadius, shrinkCircleRadius);
					shrinkLabelAnimator.SetFloatValues(label.Alpha, ShrinkLabelAlpha);
					shrinkAnimator.Start();
				}
			}
			else {
				shrinkAnimator.Cancel();
				circle.CircleRadius = shrinkCircleRadius;
				label.Alpha = ShrinkLabelAlpha;
			}
		}
	}

}