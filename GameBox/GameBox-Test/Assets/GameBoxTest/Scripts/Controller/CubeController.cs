using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _rotateSpeed;

    [SerializeField] private GameInputManager gameInputManager;
    private InputAction _playerMove;

    private AnimationState _currentState;

    private Animator _anim;
    private Rigidbody _rb;

    private Vector2 _moveDirection2D;
    private Vector3 _moveDirection3D;

    private void Awake()
    {
        gameInputManager = new GameInputManager();
    }

    private void OnEnable()
    {
        _playerMove = gameInputManager.Player.Move;
        _playerMove.Enable();
    }

    private void OnDisable()
    {
        _playerMove.Disable();
    }

    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        _anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        _moveDirection2D = _playerMove.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (_moveDirection2D != Vector2.zero)
        {
            Move();
            SocketProxy.GetInstance().SyncPosition(this.transform.position, this.transform.rotation);
        }
    }


    private void Move()
    {
        _moveDirection3D = new Vector3(_moveDirection2D.x, 0, _moveDirection2D.y);
        this.transform.position += _moveDirection3D * _walkSpeed * Time.deltaTime;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_moveDirection3D), Time.deltaTime * _rotateSpeed);
    }
}
