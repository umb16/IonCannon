using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FakeCursor : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _wait;
    [SerializeField] private GameObject _normal;

    [Inject]
    private void Construct(GameData gameData)
    {
        gameData.GameStateChanged += GameStateChanged;
    }

    public void SetWait(bool value)
    {
        _wait.SetActive(value);
        _normal.SetActive(!value);
    }

    private void GameStateChanged(GameState obj)
    {
        if (obj == GameState.Gameplay)
        {
            gameObject.SetActive(true);
            Cursor.visible = false;
        }
        else
        {
            gameObject.SetActive(false);
            Cursor.visible = true;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            Input.mousePosition, null,
            out movePos);

        Vector3 mousePos = _canvas.transform.TransformPoint(movePos);

        transform.position = mousePos;
    }
}
