using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalController
{
    public static int CurrentLODLevel;

    #region 全局静态变量
    public static int LODLevelCount = 8;
    public static float CameraMinHeight = 4.0f;
    public static float CameraHeightLodRangeLow = 8.0f;
    public static float CameraHeightLodRangeHigh = 80.0f;
    public static float CameraMaxHeight = 80.0f;
    public static float LODCameraHeightDelta = (CameraHeightLodRangeHigh - CameraHeightLodRangeLow) / LODLevelCount;

    public static float OrthoCameraSizeRangeLow = 7.0f;
    public static float OrthoCameraSizeRangeHigh = 100.0f;

    public static float CameraFovMin = 4.0f;
    public static float CameraFovLodRangeLow = 20.0f;
    public static float CameraFovLodRangeHigh = 50.0f;
    public static float CameraFovLodDelta = (CameraFovLodRangeHigh - CameraFovLodRangeLow) / LODLevelCount;
    #endregion
}
