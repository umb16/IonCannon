using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;
using System;

public class BaseMenu
{
    public const string pathToUIPrefabs = "UIPrefabs/";

    #region Static part
    static Dictionary<Type, BaseMenu> created_menus = new Dictionary<Type, BaseMenu> ();
    static List<BaseMenu> menus_stack = new List<BaseMenu> ();
    static public BaseMenu current_menu
    {
        get
        {
            BaseMenu _current_menu = null;
            if (menus_stack.Count > 0)
            {
                _current_menu = menus_stack[menus_stack.Count - 1];
            }
            return _current_menu;
        }
    }

    public static T GetMenu<T>() where T : BaseMenu
    {
        for (int i = 0; i < menus_stack.Count; i++)
        {
            if (menus_stack[i].GetType () == typeof (T))
            {
                return (T)menus_stack[i];
            }
        }

        return Create<T>();
    }

	static DontDublicateCanvas _instanceForCoroutine = null;
	public static DontDublicateCanvas instanceForCoroutine{
		get
		{
			// TODO: сие переносимо простым вызовом из BaseLayer
			if (_instanceForCoroutine == null)
			{
				Canvas findedCanvas = GameObject.FindObjectOfType<Canvas>();
				Transform mainCanvas;
				if (findedCanvas != null)
				{
					mainCanvas = findedCanvas.transform;
				}
				else
				{
					GameObject loadedCanvas = MonoBehaviour.Instantiate(Resources.Load(pathToUIPrefabs + "Canvas")) as GameObject;
					mainCanvas = loadedCanvas.transform;
				}
				_instanceForCoroutine = mainCanvas.parent.GetComponent<DontDublicateCanvas>();
			}
			return _instanceForCoroutine;
		}
	}


	

	// Создает экземпляр меню заданного типа
	public static T Create<T>() where T : BaseMenu
	{
        if (!created_menus.ContainsKey (typeof(T)))
        {
            created_menus.Add (typeof (T), (T)System.Activator.CreateInstance (typeof (T)));
        }

        T menuInstance = (T)created_menus[typeof (T)];
        menuInstance.Init ();

        return menuInstance;

    }



	// отображает меню заданного типа в корень (с очисткой стэка)
	public static void ShowGround<T>() where T : BaseMenu
	{
		HideCurrentMenu();
		ClearStack();
		Show<T>();
	}



	/// <summary>
	/// Создаёт и отображает меню заданного типа
	/// </summary>
	/// <typeparam name="T">Тип отображаемого меню, унаследованный от BaseUI</typeparam>
	public static T Show<T>() where T : BaseMenu
	{
		T menu_instance = Create<T>();
		HideCurrentMenu();

		Show(menu_instance);
		return menu_instance;
	}



	// отобразить заданный экземпляр меню
	public static void Show(BaseMenu currentMenu)
	{
		BaseLayer.MakeAllInvisible();
		//BaseLayer.HideInvisible();

		menus_stack.Add(currentMenu);
		currentMenu.OnShow();
	}



	// скрыть текущий - показать предыдущий (из стэка); если нет - показать стартовое меню
	public static void ShowPreviousFromStack()
	{
		int last_idx = menus_stack.Count -1;			// stack pop current menu
		if(last_idx > -1)
		{
			menus_stack[last_idx].Hide();				// аналогично HideCurrentMenu();
			menus_stack.RemoveAt(last_idx);
		}

		last_idx = menus_stack.Count -1;
		if(last_idx > -1)
		{
			BaseMenu last_menu = menus_stack[last_idx];
			menus_stack.RemoveAt(last_idx);
			Show(last_menu);
		}
		else
		{
			//BaseMenu.Show<StartMenu>();
		}
	}



    // загрузка меню, с использованием загрузочного слоя
    public static void ShowWithLoader<T>(System.Action loadingDelegate = null) where T : BaseMenu
    {
        LoadingLayer loadLayer = BaseLayer.Show<LoadingLayer> ();
        loadLayer.StartShowing (() =>
        {
            // TODO: как вся эта хуйня кристаллизуется и утвердится, разобрать нахуй и собрать нормально потом: много невнятного кода вокруг
            HideCurrentMenu ();

            // прячем из отображаемых все слои кроме загрузочного и чистим их;
            BaseLayer.MakeAllInvisible (loadLayer);
            //BaseLayer.HideInvisible();

            loadingDelegate?.Invoke ();
            
            T menu = Create<T> (); // аналог show, но без лишних операций
            menus_stack.Add (menu);
            menu.OnResourcePreload (() =>
            {
                menu.OnShow ();
                new Timer(0.5f).SetEnd(() =>
                {
                    loadLayer.Hide ();
                });
            });
        });
    }

    // асинхронная загрузка сцены с меню, с использованием загрузочного слоя
    public static void AsyncShowWithLoader<T>(string sceneName = "", System.Action loadingDelegate = null) where T : BaseMenu
    {
        LoadingLayer loadLayer = BaseLayer.Show<LoadingLayer> ();
        loadLayer.StartShowing (() =>
        {
            HideCurrentMenu ();
            // удаляем из отображаемых все слои кроме загрузочного и чистим их
            BaseLayer.MakeAllInvisible (loadLayer);
            ClearStack ();
            
            instanceForCoroutine.StartCoroutine (LoadingSceneAsync (sceneName, () =>
            {
                loadingDelegate?.Invoke ();
                
                T menu = Create<T> (); // аналог show, но без лишних операций
                menus_stack.Add (menu);
                menu.OnResourcePreload (() =>
                {
                    menu.OnShow ();
                    loadLayer.Hide ();
                });
            }));
        });
    }



    public static IEnumerator LoadingSceneAsync(string sceneName = "", System.Action loadingDelegate = null)
	{
		if (sceneName != "" && SceneManager.GetActiveScene().name != sceneName)
		{
			AsyncOperation async_operation = SceneManager.LoadSceneAsync (sceneName);
			yield return async_operation;
		}
		loadingDelegate();
	}



	public static bool IsBackactionAvailable()
	{
		bool is_active = (BaseLayer.BackHandleLayer() != null);
		int menu_back_activity = -9;
		if(!is_active && menus_stack.Count > 0)
		{
			menu_back_activity = current_menu.OnBackActionCheck();
			
			if(menu_back_activity == 1)
			{
				is_active = true;
			}
			else if(menu_back_activity == 0)
			{
				is_active = (menus_stack.Count > 0);
			}
		}
		//Debug.Log("current_menu: "+ current_menu +"; menu_back_activity: "+ menu_back_activity +"; is_active: "+ is_active);

		return is_active;
	}



	public static void BackActionApply()
	{
		bool is_used = false;
		BaseLayer layer_back_handler = BaseLayer.BackHandleLayer();
		if(layer_back_handler != null)
		{
			is_used = layer_back_handler.OnBackActionApply();
		}
		if(!is_used && current_menu != null)
		{
			is_used = current_menu.OnBackActionApply();
		}
		if(!is_used)
		{
			ShowPreviousFromStack();
		}
	}



	// скрыть текущее отображаемое меню (фактически - сообщить меню, что оно будет скрыто)
	public static void HideCurrentMenu()
	{
		int last_idx = menus_stack.Count -1;
		if(last_idx > -1)		// && !menus_stack[last_idx].isNeedFreezeUIInDisplayStack)
		{
			menus_stack[last_idx].Hide();
            menus_stack.RemoveAt (last_idx);

        }
	}

	
	// очистить стэк
	public static void ClearStack()
	{
		menus_stack.Clear();
	}
    #endregion

    #region Instance part
    public bool isNeedDestroyUIBetweenScenes = true;			// TODO: пока что не применяется, если не будут нужны, можно убрать
    //public bool isNeedFreezeUIInDisplayStack = false;

	//public LoadingPool loading_pool;


	void Hide()
	{
//		if(loading_pool != null)
//		{
//			loading_pool.BreakPool();
//		}
		OnHide();
	}

	protected virtual void OnResourcePreload(System.Action onFinishLoadResources) // метод нужно переопределить, если нужно грузить тяжелые ресурсы до отображения
	{
		onFinishLoadResources();
	}

    protected virtual void Init()
    {
    }

	protected virtual void OnShow()
    {
    }

	protected virtual void OnHide()
    {
    }

	// проверка, использует ли это меню сейчас кнопку "назад" ( 0 - не использует, 1 - использует, доступна, 2 - использует, доступна и серая, -1 - использует, отключена )
	protected virtual int OnBackActionCheck()
	{
		return 0;
	}

	protected virtual bool OnBackActionApply()					// произвести действие по кнопке "назад", если вернулось false - выполняется стандартный переход по стэку
	{
		return false;
	}
    #endregion
}
