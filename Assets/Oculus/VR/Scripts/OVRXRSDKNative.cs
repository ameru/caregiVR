/************************************************************************************
Copyright : Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.

Licensed under the Oculus Utilities SDK License Version 1.31 (the "License"); you may not use
the Utilities SDK except in compliance with the License, which is provided at the time of installation
or download, or which otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at
https://developer.oculus.com/licenses/utilities-1.31

Unless required by applicable law or agreed to in writing, the Utilities SDK distributed
under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
ANY KIND, either express or implied. See the License for the specific language governing
permissions and limitations under the License.
************************************************************************************/

#if USING_XR_MANAGEMENT && USING_XR_SDK_OCULUS
#define USING_XR_SDK
#endif

using System.Runtime.InteropServices;

// C# wrapper for Unity XR SDK Native APIs.

#if USING_XR_SDK
public static class OculusXRPlugin
{
	[DllImport("OculusXRPlugin")]
	public static extern void SetColorScale(float x, float y, float z, float w);

	[DllImport("OculusXRPlugin")]
	public static extern void SetColorOffset(float x, float y, float z, float w);
}
#endif
