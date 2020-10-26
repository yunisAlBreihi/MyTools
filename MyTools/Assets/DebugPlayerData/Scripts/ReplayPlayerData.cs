using UnityEngine;

public class ReplayPlayerData : MonoBehaviour, IUnit
{
    [SerializeField] private CharacterController controller;

    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
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

    public void SetLookDirection(Vector3 direction)
    {
        controller.enabled = false;
        transform.rotation = Quaternion.LookRotation(direction);
        controller.enabled = true;
    }
}
