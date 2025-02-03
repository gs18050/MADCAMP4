using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // VideoPlayer 컴포넌트
    public string nextSceneName;     // 다음으로 전환할 씬 이름

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component is not assigned!");
            return;
        }

        // 화면을 꽉 채우도록 설정
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane; // 카메라에 렌더링
        videoPlayer.targetCameraAlpha = 1.0f; // 비디오가 화면 전체를 덮도록 설정

        // 재생 종료 이벤트 연결
        videoPlayer.loopPointReached += OnVideoEnd;

        // 비디오 재생
        videoPlayer.Play();
    }

    // 비디오가 끝났을 때 호출
    void OnVideoEnd(VideoPlayer vp)
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not assigned!");
        }
    }
}
