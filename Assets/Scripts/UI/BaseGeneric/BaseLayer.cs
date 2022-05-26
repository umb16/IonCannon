using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class BaseLayer : MonoBehaviour
{
    public enum CanvasType
    {
        Main,
        World
    };

    static readonly string[] sub_paths = new string[] { "", "Dialogs/", "Guides/", "Disconnect/" };
    public delegate void BackHandlersChangingDelegate();

    public const int DEPTH_OF_DIALOGS = 30;
    public const int DEPTH_OF_GUIDES = 50;
    public const int DEPTH_OF_DISCONNECT = 55;
    public const int DEPTH_OF_EXIT = 60;

    public const float MIN_CAPTION_FONT = 55f;
    public const float MAX_CAPTION_FONT = 65f;
    public const float MIN_DESCRIPTION_FONT = 35f;
    public const float MAX_DESCRIPTION_FONT = 45f;

    #region Static part
    public static Dictionary<System.Type, BaseLayer> registered_layers = new Dictionary<System.Type, BaseLayer>();
    public static List<BaseLayer> displayed_layers = new List<BaseLayer>();
    public static List<BaseLayer> back_handle_layers = new List<BaseLayer>();
    public static BackHandlersChangingDelegate back_handlers_changing_delegate = null;


    public static DontDublicateCanvas instanceForCoroutine;
    public static Canvas mainCanvasComponent;
    public static CanvasScaler mainCanvasScalerComponent;
    static RectTransform _mainCanvas;
    public static RectTransform mainCanvas
    {
        get
        {
            if (_mainCanvas == null)
            {
                Canvas findedCanvas = null;
                Canvas[] allCanvases = GameObject.FindObjectsOfType<Canvas>();
                for (int i = 0; i < allCanvases.Length; i++)
                {
                    if (allCanvases[i].CompareTag("MainCanvas"))
                    {
                        findedCanvas = allCanvases[i];
                        break;
                    }
                }

                if (findedCanvas != null)
                {
                    _mainCanvas = (RectTransform)findedCanvas.transform;
                }
                else
                {
                    GameObject loadedCanvas = Instantiate(Resources.Load(BaseMenu.pathToUIPrefabs + "Canvas")) as GameObject;
                    _mainCanvas = (RectTransform)loadedCanvas.transform;
                }
                mainCanvasComponent = findedCanvas;
                mainCanvasScalerComponent = findedCanvas.GetComponent<CanvasScaler>();
                instanceForCoroutine = _mainCanvas.GetComponent<DontDublicateCanvas>();
            }
            return _mainCanvas;
        }

        set
        {
            _mainCanvas = value;
            instanceForCoroutine = _mainCanvas.GetComponent<DontDublicateCanvas>();
        }
    }

    static Transform _worldCanvas;
    public static Transform worldCanvas
    {
        get
        {
            if (_worldCanvas == null)
            {
                Canvas findedCanvas = null;
                Canvas[] allCanvases = GameObject.FindObjectsOfType<Canvas>();
                for (int i = 0; i < allCanvases.Length; i++)
                {
                    if (allCanvases[i].renderMode == RenderMode.WorldSpace)
                    {
                        findedCanvas = allCanvases[i];
                        break;
                    }
                }

                if (findedCanvas != null)
                {
                    _worldCanvas = findedCanvas.transform;
                }
                else
                {
                    //TODO Это не корректно, нужно завести префаь для мирового канваса
                    GameObject loadedCanvas = Instantiate(Resources.Load(BaseMenu.pathToUIPrefabs + "Canvas")) as GameObject;
                    _worldCanvas = loadedCanvas.transform;
                }
            }
            return _worldCanvas;
        }
    }

    /// <summary>
    /// Отображает заданный слой
    /// </summary>
    /// <param name="showingDelegate">Делегат, который может быть вызван когда меню закончит процедуру отрисовки.
    /// Eсли меню имеет сложную структуру появления и необходимо продолжить дальнейшее выполнение программы после появления,
    /// то можно использовать этот делегат.</param>
    /// <typeparam name="T">Тип отображаемого слоя, унаследованный от BaseUI</typeparam>
    public static T Show<T>() where T : BaseLayer
    {
        if (!registered_layers.ContainsKey(typeof(T)))
        {
            Create<T>();
        }
        BaseLayer layer = registered_layers[typeof(T)];
        // интерфейс добавляется после последнего элемента с тем же depth_level
        int old_index = displayed_layers.IndexOf(layer);
        if (old_index != -1)
        {
            displayed_layers.RemoveAt(old_index);
        }
        int disp_index = displayed_layers.Count;
        for (int i = 0; i < displayed_layers.Count; i++)
        {
            if (displayed_layers[i].depth_level > layer.depth_level)
            {
                disp_index = i;
                break;
            }
        }
        displayed_layers.Insert(disp_index, layer);					// Mathf.Min(disp_index, displayed_layers.Count)
        for (int i = 0; i < displayed_layers.Count; i++)
        {
            displayed_layers[i].transform.SetSiblingIndex(i);
        }
        layer.gameObject.SetActive(true);
        layer.is_loaded = true;
        layer.Show();                               // TODO: иметь разницу для вызова отображения, если элемент уже был отображён, или только что создан, наверно
        return (T)layer;
    }

    public static void MakeAllInvisible(BaseLayer exclusion = null)
    {
        //List<BaseLayer> remains_layers = new List<BaseLayer>();
        //foreach(var displayed_layer in displayed_layers)
        //{
        //	if(!displayed_layer.is_hidable)							// нескрываемые слои
        //	{
        //		remains_layers.Add(displayed_layer);
        //	}
        //}
        //displayed_layers.Clear();
        //displayed_layers = remains_layers;
        //if (exclusions != null)
        //{
        //	foreach (var exclusion in exclusions)
        //	{
        //		if (exclusion != null)
        //		{
        //			displayed_layers.Add(exclusion);
        //		}
        //	}
        //}
        foreach (var layer in registered_layers.Values)
        {
            if (displayed_layers.Contains(layer) && layer.is_hidable && layer != exclusion && layer.isActiveAndEnabled)
            {
                layer.Hide();
            }
        }
    }

    //public static void MakeAllInvisible(BaseLayer exclusion)
    //{
    //	MakeAllInvisible(new BaseLayer[] {exclusion});
    //}

    public static void HideInvisible()
    {
        foreach (var layer in registered_layers.Values)
        {
            if (!displayed_layers.Contains(layer) && layer.isActiveAndEnabled)
            {
                layer.Hide();
            }
        }
    }

    public static void DropTargetLayer(BaseLayer layerToDrop)
    {
        registered_layers.Remove(layerToDrop.GetType());
        displayed_layers.Remove(layerToDrop);
        back_handle_layers.Remove(layerToDrop);
        DestroyImmediate(layerToDrop.gameObject);
    }

    public static void DropInvisible()
    {
        BaseLayer[] layers_list = new BaseLayer[registered_layers.Count];
        registered_layers.Values.CopyTo(layers_list, 0);
        foreach (var layer in layers_list)
        {
            if (!displayed_layers.Contains(layer))
            {
                registered_layers.Remove(layer.GetType());
                displayed_layers.Remove(layer);
                back_handle_layers.Remove(layer);
                DestroyImmediate(layer.gameObject);
            }
        }
    }

    // проверка, что слой есть и отображён
    public static bool IsDisplayed<T>() where T : BaseLayer
    {
        return (registered_layers.ContainsKey(typeof(T)) && displayed_layers.Contains(registered_layers[typeof(T)]));
    }
    public static bool IsDisplayed(BaseLayer layer)
    {
        return (layer != null && displayed_layers.Contains(layer));
    }

    public static T TryGet<T>() where T : BaseLayer
    {
        if (registered_layers.ContainsKey(typeof(T)))
        {
            return (T)registered_layers[typeof(T)];
        }
        return null;
    }
    public static T ForceGet<T>() where T : BaseLayer
    {
        if (registered_layers.ContainsKey(typeof(T)))
        {
            return (T)registered_layers[typeof(T)];
        }
        return Create<T>();
    }

    /// <summary>
    /// Создает заданное меню на заданном уровне интерфейса. При этом меню автоматически не отображается.
    /// </summary>
    /// <typeparam name="T">Тип отображаемого меню, унаследованный от BaseUI</typeparam>
    public static T Create<T>() where T : BaseLayer
    {
        if (registered_layers.ContainsKey(typeof(T)))
        {
            return (T)registered_layers[typeof(T)];
        }
        GameObject prefab_go = null;
        for (int i = 0; i < sub_paths.Length && prefab_go == null; i++)
        {
            prefab_go = Resources.Load<GameObject>(BaseMenu.pathToUIPrefabs + "Layers/" + sub_paths[i] + typeof(T).Name);
        }
        return (T)InitializeUIGameObject<T>(prefab_go);
    }

    static T InitializeUIGameObject<T>(GameObject UIprefab) where T : BaseLayer
    {
        GameObject interfaceObject;
        T interfaceComponent;
        if (UIprefab == null)
        {
            interfaceObject = new GameObject();
            RectTransform rtransform = interfaceObject.AddComponent<RectTransform>() as RectTransform;
            rtransform.anchorMin = Vector2.zero;
            rtransform.anchorMax = Vector2.one;
            rtransform.pivot = Vector2.one * .5f;
            rtransform.sizeDelta = Vector2.zero;
            interfaceComponent = interfaceObject.AddComponent<T>();
        }
        else
        {
           // Debug.Log(UIprefab.name);
            interfaceObject = Instantiate(UIprefab) as GameObject;
            interfaceComponent = interfaceObject.GetComponent<T>();
        }

        interfaceObject.name = typeof(T).Name;
        if (interfaceComponent.canvasType == CanvasType.World)
        {
            interfaceObject.transform.SetParent(worldCanvas, false);
        }
        else
        {
            interfaceObject.transform.SetParent(mainCanvas, false);
        }

        registered_layers.Add(typeof(T), interfaceComponent);
        interfaceComponent.depth_level = 0;                         // - default depth level

        interfaceComponent.Init();
        interfaceObject.SetActive(false);

        return (T)interfaceComponent;
    }

    public static BaseLayer BackHandleLayer()
    {
        BaseLayer back_handler = null;
        for (int i = back_handle_layers.Count - 1; i >= 0; i--)
        {
            if (IsDisplayed(back_handle_layers[i]))
            {
                back_handler = back_handle_layers[i];
                break;
            }
        }
        return back_handler;
    }

    public static void OnWindowResize()
    {
        for (int l = 0; l < displayed_layers.Count; l++)
        {
            displayed_layers[l].OnResize();
        }
    }

    #endregion

    #region Instance part
    /// <summary>
    /// Глубина слоя.
    /// 
    /// -20 - слой фона;
    /// -15 - игровая карта;
    /// 
    /// 0 - базовая глубина для обычных слоёв интерфейса;
    /// 
    /// 15 - панель ресурсов сверху;
    /// 17 - всплывающие подсказки по панели навигации;
    /// 20 - панель навигации слева; слой поиска оппонента;
    /// 
    /// 30 - DEPTH_OF_DIALOGS, диалоги; всплывающее оповещение о вызове на бой;
    /// 50 - DEPTH_OF_GUIDES, слои обучения;
    /// 55 - DEPTH_OF_DISCONNECT
    /// 60 - DEPTH_OF_EXIT; всплывающие сверху нотификации;
    /// 
    /// 100 - по идее верхний слой (используется перекрывающими слоями во время загрузок)
    /// 
    /// </summary>
    public int depth_level = 0;
    public bool is_hidable = true;                                          // новое полезное слово
    public List<BaseComponent> components = new List<BaseComponent>();      // это список для слепой обработки всех компонент; для сознательного контроля, ссылки на компоненты есть внутри слоёв
    public CanvasType canvasType = CanvasType.Main;

    [HideInInspector]
    public bool is_loaded;                                                  // по умолчанию все слои считаются загруженными,
    public event System.Action<BaseLayer> on_loading_complete;                  //  в случае индивидуальных асинхронных операций слой в OnShow ставит is_loaded = false



    void Show()
    {
        //gameObject.SetActive(true);				// TODO: <- почему без этого активен?
        if (back_handle_layers.Contains(this) && back_handlers_changing_delegate != null)
        {
            back_handlers_changing_delegate();
        }
        OnShow();
    }


    protected virtual void Init()
    {
    }

    // содержимое этого метода может быть переопределено - вызывается при завершении показа
    protected virtual void OnShow()
    {
    }


    public void Hide()
    {
        displayed_layers.Remove(this);
        OnStartHiding(() =>
        {

            if (back_handle_layers.Contains(this) && back_handlers_changing_delegate != null)
            {
                back_handlers_changing_delegate();
            }
            gameObject.SetActive(false);
            OnFinishHiding(); // по идее, в этом месте нужно сделать все интерактивные элементы неактивными
        });
    }

    // метод можно переопределить для асинхронной загрузки или загрузки с анимацией OnHide вызывается только после того, как этот метод вызвал onFinishHiding
    protected virtual void OnStartHiding(System.Action onFinishHiding)
    {
        if (onFinishHiding != null)
        {
            onFinishHiding();
        }
    }

    // содержимое этого метода может быть переопределено
    protected virtual void OnFinishHiding()
    {
    }

    protected virtual void OnResize()
    {
    }

    public void HandleBackAction()
    {
        if (back_handle_layers.Contains(this))
        {
            back_handle_layers.Remove(this);
        }
        back_handle_layers.Add(this);
    }

    public void MakeLayerLoaded()
    {
        is_loaded = true;
        if (on_loading_complete != null)
        {
            on_loading_complete(this);
            on_loading_complete = null;
        }
    }

    public void OnLayerLoadComplete(System.Action action)
    {
        if (is_loaded)
        {
            action();
        }
        else
        {
            on_loading_complete += (_layer) =>
            {
                action();
            };
        }
    }

    public virtual bool OnBackActionApply()
    {
        return false;
    }

    // cинхронная загрузка компонента
    public T LoadComponent<T>(Transform parent = null, string prefab_name = null) where T : BaseComponent
    {
        if (parent == null) parent = transform;
        if (prefab_name == null) prefab_name = typeof(T).Name;

        T loaded_component = BaseComponent.LoadComponent<T>(parent, prefab_name);
        components.Add(loaded_component);
        return loaded_component;
    }

    // асинхронная загрузка компонента, возможно, с отображением болванки
    public void LoadComponentAsync<T>(Transform parent = null, string prefab_name = null, System.Action<T> load_delegate = null) where T : BaseComponent
    {
        if (parent == null) parent = transform;
        if (prefab_name == null) prefab_name = typeof(T).Name;

        instanceForCoroutine.StartCoroutine(BaseComponent.LoadComponentAsync<T>(parent, prefab_name, (loaded_component) =>
        {
            components.Add(loaded_component);
            if (load_delegate != null)
            {
                load_delegate(loaded_component);
            }
        }));
    }

    public void RemoveComponent(BaseComponent component)
    {
        if (components.Contains(component))
        {
            GameObject.Destroy(component.gameObject);
            components.Remove(component);
        }
    }

    public static float GetValByAspect(float _min, float _max)
    {
        float aspect = (float)Screen.width / (float)Screen.height;
        float max_aspect = 1366f / 768f;
        float min_aspect = 4f / 3f;
        float delta_aspect = max_aspect - min_aspect;
        float delta_font = _max - _min;
        float val = _max - ((max_aspect - aspect) * delta_font / delta_aspect);
        return val;
    }

    protected virtual void OnDestroy()
    {
        registered_layers.Remove(this.GetType());
        back_handle_layers.Remove(this);
    }
    #endregion
}
