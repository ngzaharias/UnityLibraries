using UnityEngine;

public class NewSingletonBehaviour : SingletonBehaviour<NewSingletonBehaviour>
{
	protected override void Awake() 
	{
		base.Awake();
	}

	protected override void OnEnable() 
	{
		base.OnEnable();
	}

	protected override void Start() 
	{
		base.Start();
	}
	
	protected override void Update() 
	{
		base.Update();
	}

	protected override void OnDestroy() 
	{
		base.OnDestroy();
	}
}
