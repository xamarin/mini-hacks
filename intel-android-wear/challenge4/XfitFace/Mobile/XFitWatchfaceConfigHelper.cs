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

using System.Threading.Tasks;
using Android.Gms.Common.Apis;
using Android.Gms.Wearable;

namespace XFitWatchface
{
	public class XFitWatchfaceConfigHelper
	{
		const string Tag = "ConfigHelper";

		XFitWatchfaceConfigHelper() { }

		const string PathWithFeature = "/xfit_watchface";

		public static void SaveStepCountStatus(GoogleApiClient googleApiClient, bool stepCountOn)
		{
			var putDataMapRequest = PutDataMapRequest.Create(PathWithFeature);
			putDataMapRequest.DataMap.PutBoolean("stepcount", stepCountOn);
			var putDataRequest = putDataMapRequest.AsPutDataRequest();
			putDataRequest.SetUrgent();
			WearableClass.DataApi.PutDataItem(googleApiClient, putDataRequest);
		}

		public async static Task<bool> ReadStepCountStatus(GoogleApiClient googleApiClient)
		{
			var nodeApiLocalNode = await WearableClass.NodeApi.GetLocalNodeAsync(googleApiClient);
			if (nodeApiLocalNode == null || !nodeApiLocalNode.Status.IsSuccess || nodeApiLocalNode.Node == null)
			{
				// error
			}
			else {
				var localNode = nodeApiLocalNode.Node.Id;
				var uri = new Android.Net.Uri.Builder()
							.Scheme("wear")
							.Path(PathWithFeature)
							.Authority(localNode)
							.Build();
				var dataItemResult = await WearableClass.DataApi.GetDataItemAsync(googleApiClient, uri) as IDataApiDataItemResult;
				if (dataItemResult == null || !dataItemResult.Status.IsSuccess || dataItemResult.DataItem == null)
				{
					// not found
				}
				else {
					var dataMapItem = DataMapItem.FromDataItem(dataItemResult.DataItem);
					var stepCountOn = dataMapItem.DataMap.GetBoolean("stepcount");
					return stepCountOn;
				}
			}
			return true; // default is "ON"
		}

	}
}

