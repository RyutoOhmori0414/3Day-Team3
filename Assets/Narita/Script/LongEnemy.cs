﻿using UnityEngine;

public class LongEnemy : Enemy_B
{
    [SerializeField, Header("弾のPrefab")] GameObject _bullet;
    [SerializeField, Header("弾の発射位置")] Transform _muzzle;
    [SerializeField, Header("弾の発射間隔")] float _interval = 5;
    float _timer = 0;
    Camera _mainCamera;
    protected override void Start_S()
    {
        _timer = Time.time;
        _mainCamera = Camera.main;
    }
    protected override void Update_S()
    {
        if (_player == null) return;
        if (Time.time >= _timer + _interval)
        {
            _timer = Time.time;
            if (IsVisible(_mainCamera))
            {
                var bullet = Instantiate(_bullet, _muzzle.position, Quaternion.identity);
                bullet.GetComponent<EnemyBullet>().AddTarget(_player);
            }
        }
    }
    bool IsVisible(Camera camera)
    {
        Vector3 viewportPos = camera.WorldToViewportPoint(transform.position);
        return viewportPos.x >= 0 && viewportPos.x <= 1 &&
               viewportPos.y >= 0 && viewportPos.y <= 1 &&
               viewportPos.z > 0;
    }
}
