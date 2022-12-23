using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyCustomAttribute;

public class AttachVolleysOfBullets : AbsAttach
{
    [SerializeField] private int numberOfBullet;

    public float _delayAttack;
    [ReadOnly, SerializeField] private float _timerAttack;
    
    private Transform shootPosition;
    public bool fire;
    private float counterBullet;

    private void Awake() {
        counterBullet = numberOfBullet;
    }
    

    public override void Init()
    {
        
    }
    
    private void Update() {
        if(fire){
            //Dem nguoc thoi gian gia cac lan bat
            _timerAttack += Time.deltaTime;
            if(_timerAttack >= _delayAttack){
                Fired();
                _timerAttack = 0;
            }
        }
    }


    public override void Attack(Transform shootPosition)
    {
        this.shootPosition = shootPosition; 
        counterBullet = numberOfBullet;
        fire =  true;
    }
    
    private void Fired() {
        if(counterBullet > 0){
            GameObject newBullet = Instantiate(bullet, shootPosition.transform.position, shootPosition.transform.rotation);
            newBullet.GetComponent<EnemyBulletsBasic>().Fire(shootPosition.forward.normalized);
            counterBullet -= 1;
        } else {
            if(fire) {
                OnAttackeComplete();
            }
            fire = false;
        }
    }
}
