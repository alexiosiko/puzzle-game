using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static List<IEnumerator> playerCoroutines = new();
    static List<IEnumerator> interactableCoroutines = new();
    static List<IEnumerator> bombCoroutines = new();
    static List<IEnumerator> enemyCoroutines = new();
    static List<IEnumerator> killAndBreakCoroutines = new();
    public bool isGameLooping = true;
    public static event Action OnPlayerPhase;
    public static event Action OnBombPhase;
    public static event Action OnEnemyPhase;
    void Start() => StartCoroutine(GameLoop());

    IEnumerator GameLoop()
    {
		playerCoroutines.Clear();
		interactableCoroutines.Clear();
		bombCoroutines.Clear();
		enemyCoroutines.Clear();
		killAndBreakCoroutines.Clear();

        while (isGameLooping)
			yield return ProcessTurn();
    }
	IEnumerator ProcessTurn()
	{
		OnPlayerPhase?.Invoke();
		yield return ProcessPhase(playerCoroutines);

		yield return ProcessPhase(interactableCoroutines);

		OnBombPhase?.Invoke();
		yield return ProcessPhase(bombCoroutines);

		yield return ProcessPhase(killAndBreakCoroutines);

		OnEnemyPhase?.Invoke();
		Enemy.reservedPositions.Clear();
		yield return ProcessPhase(enemyCoroutines);
    }
    IEnumerator ProcessPhase(List<IEnumerator> coroutines)
    {
        int runningCoroutines = coroutines.Count;

        foreach (var coroutine in coroutines)
            StartCoroutine(ProcessSingle(coroutine, () => runningCoroutines--));

        coroutines.Clear();

        yield return new WaitUntil(() => runningCoroutines == 0);
    }

    IEnumerator ProcessSingle(IEnumerator coroutine, Action onComplete)
    {
        yield return coroutine;
        onComplete();
    }
	public static TurnManager Singleton;
	void Awake() => Singleton = this;
	// Adders for each phase
	public static void AddPlayer(IEnumerator action) => playerCoroutines.Add(action);
    public static void AddInteractable(IEnumerator action) => interactableCoroutines.Add(action);
    public static void AddBomb(IEnumerator action) => bombCoroutines.Add(action);
    public static void AddEnemy(IEnumerator action) => enemyCoroutines.Add(action);
    public static void AddKillOrBreak(IEnumerator action) => killAndBreakCoroutines.Add(action);
}
