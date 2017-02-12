using UnityEngine;

abstract public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
	static private T instance;
	static public T Instance
	{
		get
		{
			if (Application.isPlaying)
				return instance;
			return (instance != null) ? instance : GameObject.FindObjectOfType<T>() as T;
		}
	}

	static public bool IsInitalised
	{
		get { return instance != null; }
	}

	static public void Initalise()
	{
		string name = typeof(T).ToString();
		GameObject singleton = new GameObject(name);
		singleton.AddComponent<T>();
	}

	static public void Destroy()
	{
		DestroyImmediate(instance.gameObject);
	}

	protected virtual void Awake() 
	{
		Debug.Assert(instance == null, "Initalising two singletons of the same type!");
		instance = this as T;
	}

	protected virtual void OnEnable() 
	{
	}

	protected virtual void Start() 
	{
	}

	protected virtual void Update() 
	{
	}

	protected virtual void OnDestroy()
	{
		Debug.Assert(this == instance, "Destroying a singleton that is not the main instance!");
		instance = null;
	}
}
