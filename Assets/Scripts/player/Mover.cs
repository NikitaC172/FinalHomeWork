using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform[] _paths = null;
    [SerializeField] private int[] _timeForPatchs = null;
    [SerializeField] private PlayerStats _playerStats;

    private Vector3[] _points;
    private Vector3 _lastPoint;
    private Transform _path;
    private float _travelTimeToPoint = 0;
    private int _currentPath = 0;

    public event UnityAction DestroyedEnemies;
    public event UnityAction DestroyedAllEnemies;
    public event UnityAction<bool> Moved;

    private void Start()
    {
        SetPatch();
    }

    public void MoveForward()
    {
        if (_currentPath + 1 < _paths.Length)
        {
            Moved?.Invoke(true);
            transform.DOMove(_lastPoint, 1, false);
            _currentPath++;
            Invoke(nameof(SetPatch),1);
        }
    }

    public void ShowMoveButton()
    {
        if (_currentPath + 1 < _paths.Length)
        {
            DestroyedEnemies?.Invoke();
        }
        else
        {
            _playerStats.GetReward();
            DestroyedAllEnemies?.Invoke();
        }
    }

    private void SetPatch()
    {
        _path = _paths[_currentPath];
        _points = new Vector3[_path.childCount];
        _travelTimeToPoint = _timeForPatchs[_currentPath];

        for (int i = 0; i < _path.childCount; i++)
        {
            _points[i] = _path.GetChild(i).transform.position;

            if (i == _path.childCount - 1)
            {
                _lastPoint = _points[i];
            }
        }

        MovePlayer();
    }

    private void MovePlayer()
    {
        Tween tween = transform.DOPath(_points, _travelTimeToPoint, PathType.CatmullRom).SetLookAt(0.1f);
        Invoke(nameof(RotatePlayer), _travelTimeToPoint + 0.2f);
    }

    private void RotatePlayer()
    {
        transform.DORotateQuaternion(_path.GetChild(_path.childCount - 1).transform.rotation, 1);
        Moved?.Invoke(false);
    }
}
