using UnityEngine;
using Cinemachine;
 
/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's axis
/// Cannot be in used concurrently with Confider
/// </summary>
[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
public class LockCameraAxis : CinemachineConfiner2D
{
    public float m_Position = 10;
 
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        base.PostPipelineStageCallback(vcam, stage, ref state, deltaTime);
        
        if (enabled && stage == CinemachineCore.Stage.Finalize)
        {
            var pos = state.RawPosition;
            pos.y = m_Position;
            state.RawPosition = pos;
        }
    }
}
 