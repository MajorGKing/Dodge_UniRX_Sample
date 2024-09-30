using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 관련 라이브러리
using UnityEngine.SceneManagement; // 씬 관리 관련 라이브러리
using UniRx;
using UniRx.Triggers;

public class GameManager : MonoBehaviour {
    public GameObject gameoverText; // 게임오버시 활성화 할 텍스트 게임 오브젝트
    public Text timeText; // 생존 시간을 표시할 텍스트 컴포넌트
    public Text recordText; // 최고 기록을 표시할 텍스트 컴포넌트

    private FloatReactiveProperty surviveTime = new FloatReactiveProperty(); // 생존 시간
    private bool isGameover; // 게임 오버 상태

    void Start() 
    {
        isGameover = false;

        // 생존 시간과 게임 오버 상태를 초기화
        surviveTime.Value = 0;

        this.UpdateAsObservable().Where(_ => isGameover == false)
        //.RepeatUntilDestroy(this)
        .DoOnCompleted(() => EndGame())
        .Subscribe(_ => surviveTime.Value += Time.deltaTime);

        this.UpdateAsObservable()
        .Where(_ => isGameover == true && Input.GetKeyDown(KeyCode.R))
        .Subscribe(_ => SceneManager.LoadScene("SampleScene"));

        surviveTime.Subscribe(_ =>
            {
                timeText.text = "Time: " + (int) surviveTime.Value;
            });
        
    }

    // 현재 게임을 게임 오버 상태로 변경하는 메서드
    public void EndGame() 
    {
        // 현재 상태를 게임 오버 상태로 전환
        isGameover = true;
        // 게임 오버 텍스트 게임 오브젝트를 활성화
        gameoverText.SetActive(true);

        // BestTime 키로 저장된, 이전까지의 최고 기록 가져오기
        float bestTime = PlayerPrefs.GetFloat("BestTime");

        // 이전까지의 최고 기록보다 현재 생존 시간이 더 크다면
        if (surviveTime.Value > bestTime)
        {
            // 최고 기록의 값을 현재 생존 시간의 값으로 변경 
            bestTime = surviveTime.Value;
            // 변경된 최고 기록을 BestTime 키로 저장
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        // 최고 기록을 recordText 텍스트 컴포넌트를 통해 표시
        recordText.text = "Best Time: " + (int) bestTime;
    }
}