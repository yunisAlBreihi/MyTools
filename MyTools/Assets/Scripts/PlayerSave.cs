﻿using UnityEngine;

public class PlayerSave : MonoBehaviour, IUnit
{
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 position)
    {
        //controller.Move(position);
        controller.enabled = false;
        transform.position = position;
        controller.enabled = true;
    }
}
