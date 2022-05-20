using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;



// механизм отображения компонент
public class BaseComponent : MonoBehaviour
{

	public static IEnumerator LoadComponentAsync<T>(Transform parent = null, string prefab_name = null, System.Action<T> load_delegate = null) where T : BaseComponent
	{
		string path = BaseMenu.pathToUIPrefabs + "Components/" + prefab_name;
		ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(path);
		while (!resourceRequest.isDone)
		{
			yield return 0;
		}
		T component = InstantiateComponent<T>(resourceRequest.asset as GameObject, parent);

		if(load_delegate != null)
		{
			load_delegate(component);
		}
	}

	public static T LoadComponent<T>(Transform parent = null, string prefab_name = null) where T : BaseComponent
	{
        if (prefab_name == null) prefab_name = typeof(T).Name;
        string path = BaseMenu.pathToUIPrefabs + "Components/" + prefab_name;
		T component = InstantiateComponent<T>(Resources.Load(path) as GameObject, parent);

		return component;
	}

	static T InstantiateComponent<T>(GameObject prefab_object, Transform parent) where T : BaseComponent
	{
		if(prefab_object == null)
		{
			Debug.LogError("Prefab is empty! Nothing to instantiate for " + typeof(T) + " !");
		}
		GameObject go_component = Instantiate(prefab_object) as GameObject;
		if(go_component == null)
		{
			Debug.LogError("Missing component prefab: " + typeof(T) + " !");
		}
		T component = go_component.GetComponent<T>();
		if(component == null)
		{
			Debug.LogError("Missing component script: " + typeof(T) + " !");
		}
		if(parent != null)
		{
			component.transform.SetParent(parent, false);
		}
		return component;
	}

	public virtual void Resize()
	{

	}

}
