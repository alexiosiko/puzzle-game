	using UnityEngine;

	public class CanvasManager : MonoBehaviour
	{

		[SerializeField] GameObject musicCheck;
		[SerializeField] GameObject effectsCheck;
		[SerializeField] GameObject menuGameObject;
		public void NextScene() => SceneLoader.NextScene();
		public void ChangeScene(string sceneName) => SceneLoader.LoadScene(sceneName);
		public void ToggleMenu()
		{
			if (menuGameObject.activeInHierarchy)
			{
				menuGameObject.SetActive(false);
			}
			else
			{
				menuGameObject.SetActive(true);
			}
		}
		public void ToggleMuteMusic()
		{
			if (MusicManager.Singleton == null)
			{
				Debug.Log("MusicManager is null");
				return;
			}
			bool isMuted = MusicManager.Singleton.ToggleMute();
			if (isMuted)
			{
				musicCheck.SetActive(false);
			}
			else
				musicCheck.SetActive(true);
		}
		public void ToggleMuteEffects()
		{
			if (EffectsManager.Singleton == null)
			{
				Debug.Log("EffectsManager is null");
				return;
			}
			bool isMuted = EffectsManager.Singleton.ToggleMute();
			if (isMuted)
			{
				effectsCheck.SetActive(false);
			}
			else
				effectsCheck.SetActive(true);
		}
		public static CanvasManager Singleton;
		void Awake()
		{
			Singleton = this;
		}
	}
