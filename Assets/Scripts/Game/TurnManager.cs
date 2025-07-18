using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	List<IEnumerator> playerCoroutines = new();
	List<IEnumerator> interactableCoroutines = new();
	List<IEnumerator> bombCoroutines = new();
	List<IEnumerator> projectileCoroutines = new();
	List<IEnumerator> enemyCoroutines = new();
	List<IEnumerator> attackCoroutines = new();
	List<IEnumerator> dieCoroutines = new();
	List<IEnumerator> explosionCoroutines = new();
	public bool isGameLooping = true;
	public static event Action OnPlayerPhase;
	public static event Action OnBombPhase;
	public static event Action OnEnemyPhase;
	public static event Action OnProjectilePhase;
	public void Start()
	{
		FreeLists();
		isGameLooping = true;
		StartCoroutine(GameLoop());
	}
	IEnumerator GameLoop()
	{
		while (isGameLooping)
			yield return ProcessTurn();
	}
	IEnumerator ProcessTurn()
	{
		OnPlayerPhase?.Invoke();
		yield return ProcessPhase(playerCoroutines);

		OnEnemyPhase?.Invoke();
		OnProjectilePhase?.Invoke();
		Enemy.reservedPositions.Clear();
		yield return ProcessPhase(enemyCoroutines, projectileCoroutines);

		OnBombPhase?.Invoke();
		yield return ProcessPhase(bombCoroutines, interactableCoroutines);

		yield return ProcessPhase(explosionCoroutines);

		yield return ProcessPhase(attackCoroutines, dieCoroutines);
	}
	IEnumerator ProcessPhase(params List<IEnumerator>[] coroutineLists)
	{
		int runningCoroutines = coroutineLists.Sum(list => list.Count);
		foreach (var list in coroutineLists)
			foreach (var routine in list)
			{
				if (routine != null)
					StartCoroutine(Wrap(routine, () => runningCoroutines--));
			}
		yield return new WaitUntil(() => runningCoroutines == 0);
	}  
	IEnumerator Wrap(IEnumerator routine, Action onDone)
	{
		if (routine != null)
			yield return routine;
		onDone();
	}
	
	public void FreeLists()
	{
		FreeList(ref playerCoroutines);
		FreeList(ref interactableCoroutines);
		FreeList(ref bombCoroutines);
		FreeList(ref enemyCoroutines);
		FreeList(ref attackCoroutines);
		FreeList(ref dieCoroutines);
		FreeList(ref explosionCoroutines);
		FreeList(ref projectileCoroutines);
	}
	void OnDisable()
	{
		isGameLooping = false;
		FreeLists();
		StopAllCoroutines(); // This is to stop game loop
	}
	void FreeList(ref List<IEnumerator> list)
	{
		foreach (var l in list)
			StopCoroutine(l);
		list.Clear();
	}



	public static TurnManager Singleton;
	void Awake() => Singleton = this;
	// Adders for each phase
	public void AddPlayer(IEnumerator action) => playerCoroutines.Add(action);
	public void AddInteractable(IEnumerator action) => interactableCoroutines.Add(action);
	public void AddBomb(IEnumerator action) => bombCoroutines.Add(action);
	public void AddEnemy(IEnumerator action) => enemyCoroutines.Add(action);
	public void AddAttack(IEnumerator action) => attackCoroutines.Add(action);
	public void RemoveAttack(int hashedCode)
	{
		foreach (var a in attackCoroutines)
			if (a.GetHashCode() == hashedCode)
			{
				attackCoroutines.Remove(a);
				return;
			}
	} 
	public void AddDie(IEnumerator action) => dieCoroutines.Add(action);
	public void AddExplosion(IEnumerator action) => explosionCoroutines.Add(action);
	public void AddProjectile(IEnumerator action) => projectileCoroutines.Add(action);
	
}
