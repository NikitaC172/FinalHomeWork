using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class Mover : MonoBehaviour
{
    [SerializeField] private Transform[] _paths = null;
    [SerializeField] private int[] _timeForPatchs = null;
    [SerializeField] private Player _player;

    private Vector3[] _points;
    private Vector3 _pointAttack;
    private Vector3 _pointCover;
    private Transform _path;
    private float _travelTimeToPoint = 0;
    private float _secondsAction = 1f;
    private int _currentPath = 0;
    private bool _isCover = false;
    private bool _isMove = false;

    public bool IsMove => _isMove;

    public event UnityAction ShowedButtonNewStage;
    public event UnityAction LevelComplited;
    public event UnityAction<bool> MovedForward;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        SetPath();
        _isMove = true;
        MovedForward?.Invoke(_isMove);
    }

    public void TakeCover()
    {
        if (_isMove == false)
        {
            if (_isCover == false)
            {
                _isCover = true;
                transform.DOMove(_pointCover, _secondsAction, false);
            }
            else
            {
                _isCover = false;
                transform.DOMove(_pointAttack, _secondsAction, false);
            }
        }
    }

    public void MoveForward()
    {
        if (_currentPath + 1 < _paths.Length)
        {
            MovedForward?.Invoke(true);
            transform.DOMove(_pointAttack, _secondsAction, false);
            _isCover = false;
            _currentPath++;
            Invoke(nameof(SetPath), _secondsAction);
        }
    }

    public void ShowMoveButton()
    {
        if (_currentPath + 1 < _paths.Length)
        {
            ShowedButtonNewStage?.Invoke();
        }
        else
        {
            _player.GetReward();
            LevelComplited?.Invoke();
        }
    }

    private void SetPath()
    {
        _path = _paths[_currentPath];
        _points = new Vector3[_path.childCount];
        _travelTimeToPoint = _timeForPatchs[_currentPath];

        for (int i = 0; i < _path.childCount; i++)
        {
            _points[i] = _path.GetChild(i).transform.position;

            if (i == _path.childCount - 1)
            {
                _pointAttack = _points[i];
                _pointCover = _path.GetChild(i).GetChild(0).transform.position;
            }
        }

        MovePlayer();
    }

    private void MovePlayer()
    {
        float delay = 0.2f;
        Tween tween = transform.DOPath(_points, _travelTimeToPoint, PathType.CatmullRom).SetLookAt(0.1f);
        Invoke(nameof(RotatePlayer), _travelTimeToPoint + delay);
    }

    private void RotatePlayer()
    {
        transform.DORotateQuaternion(_path.GetChild(_path.childCount - 1).transform.rotation, 1);
        MovedForward?.Invoke(false);
        _isMove = false;
        MovedForward?.Invoke(_isMove);
    }
}
