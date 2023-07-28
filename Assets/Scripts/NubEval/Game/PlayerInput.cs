using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Button bUp;
    [SerializeField] private Button bDown;
    [SerializeField] private Button bLeft;
    [SerializeField] private Button bRight;
    [SerializeField] private Button bPunch;


    public event Action<InputMoveDir> InputDirUpdate;
    public event Action InputPunch;

    private void Awake()
    {
        bUp.onClick.AddListener(() => { InputDirUpdate?.Invoke(InputMoveDir.up); });
        bDown.onClick.AddListener(() => { InputDirUpdate?.Invoke(InputMoveDir.down); });
        bLeft.onClick.AddListener(() => { InputDirUpdate?.Invoke(InputMoveDir.left); });
        bRight.onClick.AddListener(() => { InputDirUpdate?.Invoke(InputMoveDir.right); });

        bPunch.onClick.AddListener(() => { InputPunch?.Invoke(); });
    }

    public enum InputMoveDir
    {
        up = 1,
        down = 2,
        left = 3,
        right = 4,
        stay = 0
    }
}
