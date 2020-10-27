using System.Collections;
using UnityEngine;

public class ReplayPlayerData : MonoBehaviour, IUnit
{
    [SerializeField] private CharacterController controller = null;
    [SerializeField] private float capturesPerSecond = 5.0f;
    [SerializeField] private ReplayDataScriptable replayData = null;

    private DataHolder playerData = null;

    private void Awake()
    {
        if (controller == null)
        {
            controller = gameObject.GetComponent<CharacterController>();
        }
    }

    private void Start()
    {
        if (replayData.runReplay == true)
        {
            playerData = replayData.data;
            StartCoroutine(MovePlayer());
        }
    }

    private void OnDisable()
    {
        replayData.Clear();
    }

    IEnumerator MovePlayer()
    {
        WaitForSeconds wait = new WaitForSeconds(1.0f/capturesPerSecond);
        for (int i = 0; i < playerData.positions.Count; i++)
        {
            if (i + 1 < playerData.positions.Count)
            {
                StartCoroutine(LerpMovement(playerData.positions[i], playerData.positions[i + 1], playerData.lookDirections[i], playerData.lookDirections[i + 1]));
            }
            yield return wait;
        }
    }

    IEnumerator LerpMovement(Vector3 pos1, Vector3 pos2, Vector3 dir1, Vector3 dir2)
    {
        WaitForSeconds wait = new WaitForSeconds(Time.deltaTime);
        float delta = 0.0f;
        while (delta < 1.0f + 1/capturesPerSecond)
        {
            SetPosition(Vector3.Lerp(pos1, pos2, delta));
            SetLookDirection(Vector3.Lerp(dir1, dir2, delta));
            delta += Time.deltaTime * capturesPerSecond;
            yield return wait;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 position)
    {
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
