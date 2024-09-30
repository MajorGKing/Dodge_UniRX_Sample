using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerController : MonoBehaviour {
    private Rigidbody playerRigidbody; // 이동에 사용할 리지드바디 컴포넌트
    public float speed = 8f; // 이동 속력

    void Start() {
        // 게임 오브젝트에서 Rigidbody 컴포넌트를 찾아 playerRigidbody에 할당
        playerRigidbody = GetComponent<Rigidbody>();

        this.UpdateAsObservable()
        .Where(_ => Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        .Select(_ => new Vector3(Input.GetAxis("Horizontal") * speed,
                                 0f,
                                 Input.GetAxis("Vertical") * speed))
        //.RepeatUntilDestroy(this)
        .Subscribe(move => 
        {
            playerRigidbody.velocity = move;
        });
    }

    public void Die() {
        // 자신의 게임 오브젝트를 비활성화
        gameObject.SetActive(false);

        // 씬에 존재하는 GameManager 타입의 오브젝트를 찾아서 가져오기
        GameManager gameManager = FindObjectOfType<GameManager>();
        // 가져온 GameManager 오브젝트의 EndGame() 메서드 실행
        gameManager.EndGame();
    }
}