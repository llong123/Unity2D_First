﻿using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState {

	private Enemy enemy;
	private float patrolTimer;
	private float patrolDuration = 6.0f;

	public void Execute(){
		Patrol ();
	}

	public void Enter (Enemy enemy){
		this.enemy = enemy;
	}

	public void Exit(){

	}

	public void OnTriggerEnter (Collider2D other){

	}

	private void Patrol(){

		enemy.Move ();

		patrolTimer += Time.deltaTime;

		if (patrolTimer >= patrolDuration) {
			enemy.ChangeState(new IdleState());
		}

	}
}
