﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab; // 생성할 총알의 원본 프리팹
    public float spawnRateMin = 0.5f; // 최소 생성 주기
    public float spawnRateMax = 3f; // 최대 생성 주기

    private Transform target; // 발사할 대상
    private float spawnRate; // 생성 주기
    private FloatReactiveProperty timeAfterSpawn = new FloatReactiveProperty(); // 최근 생성 시점에서 지난 시간

    void Start()
    {
        // 최근 생성 이후의 누적 시간을 0으로 초기화
        timeAfterSpawn.Value = 0f;

        this.UpdateAsObservable()
        .Subscribe(_ => timeAfterSpawn.Value += Time.deltaTime);

        // 총알 생성 간격을 spawnRateMin과 spawnRateMax 사이에서 랜덤 지정 
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        // PlayerController 컴포넌트를 가진 게임 오브젝트를 찾아 조준 대상으로 설정
        target = FindObjectOfType<PlayerController>().transform;

        timeAfterSpawn
        .Where(_ => timeAfterSpawn.Value >= spawnRate)
        .Take(1)
        .RepeatUntilDestroy(this)
        .Subscribe(_ =>
        {
            timeAfterSpawn.Value = 0f;

            GameObject bullet = Instantiate(bulletPrefab,
               transform.position, transform.rotation);

            bullet.transform.LookAt(target);

            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        });
    }
}