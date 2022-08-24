using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [SerializeField] private float _speed = 2f;
    private Vector3 moveDir;
    #endregion

    [SerializeField] private Joystick _joystick;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector3(_joystick.input.x, 0, _joystick.input.y);
        transform.Translate((moveDir * _speed) * Time.deltaTime);
    }
}
