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

	IEnumerator GameLoop()
	{
		while (isGameLooping)
			yield return ProcessTurn();
	}
	bool waiting = false;
	void UnWait() => waiting = false;
	float minimumWaitingTimeBetweenTurns;
	bool debugMode = false;
	IEnumerator ProcessTurn()
	{
		yield return new WaitUntil(() => waiting == false);
		waiting = true;
		Invoke(nameof(UnWait), minimumWaitingTimeBetweenTurns);

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
		bool debugYes = debugMode && runningCoroutines > 0;
		foreach (var list in coroutineLists)
		{
			var listCopy = list.ToList(); // make a copy
			foreach (var routine in listCopy)
			{
				if (routine != null)
					StartCoroutine(Wrap(routine, () => runningCoroutines--));
			}
		}

		yield return new WaitUntil(() => runningCoroutines == 0);
		if (debugYes)
			yield return new WaitForSeconds(0.5f);
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
		Goal.OnGameWin -= HandleOnGameWin;
		CancelInvoke();
		isGameLooping = false;
		FreeLists();
		Enemy.reservedPositions.Clear();
		StopAllCoroutines(); // This is to stop game loop
	}
	void FreeList(ref List<IEnumerator> list)
	{
		foreach (var l in list)
			StopCoroutine(l);
		list.Clear();
	}
	public static TurnManager Singleton;
	void Awake()
	{
		minimumWaitingTimeBetweenTurns = GameSettings.tweenDuration + GameSettings.tweenDuration / 1.7f;
		Singleton = this;
		FreeLists();
		isGameLooping = true;
		StartCoroutine(GameLoop());
	}
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
		Debug.LogError("Could not remove attack: " + hashedCode);
	}
	public void AddDie(IEnumerator action) => dieCoroutines.Add(action);
	public void AddExplosion(IEnumerator action) => explosionCoroutines.Add(action);
	public void AddProjectile(IEnumerator action) => projectileCoroutines.Add(action);
	void OnEnable() => Goal.OnGameWin += HandleOnGameWin;
	void HandleOnGameWin()
	{
		enabled = false;
	}

}
