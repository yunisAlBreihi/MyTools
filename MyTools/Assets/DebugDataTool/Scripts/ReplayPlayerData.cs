using System.Collections;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ReplayPlayerData : MonoBehaviour, IUnit
{
    private ReplayDataScriptable replayData = null;
    private PlayerCharacterController controller = null;
    private DataHolder playerData = null;

    private float lerpDelta = 0.0f;
    private int positionIndex = 0;

    private void Awake()
    {
        controller = gameObject.GetComponent<PlayerCharacterController>();
        replayData = (ReplayDataScriptable)Resources.Load("ReplayData", typeof(ReplayDataScriptable));
    }

    private void Start()
    {
        if (replayData.runReplay == true)
        {
            playerData = replayData.data;
        }
    }

    private void OnDisable()
    {
        replayData.Clear();
    }

    private void Update()
    {
        if (replayData.runReplay == true)
        {
            controller.enabled = false;

            if (lerpDelta < 1.0f)
            {
                if (positionIndex + 1 < playerData.positions.Count)
                {
                    LerpMovement(playerData.positions[positionIndex], playerData.positions[positionIndex + 1],
                    playerData.lookDirections[positionIndex], playerData.lookDirections[positionIndex + 1], lerpDelta);
                }
                else
                {
                    replayData.Clear();
                    transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);
                }
                lerpDelta += Time.deltaTime * playerData.capturesPerSecond;
            }
            else
            {
                positionIndex++;
                lerpDelta = 0.0f;
            }
            controller.enabled = true;

        }
    }

    void LerpMovement(Vector3 pos1, Vector3 pos2, Vector3 dir1, Vector3 dir2, float delta)
    {
        SetPosition(Vector3.Lerp(pos1, pos2, delta));
        SetLookDirection(Vector3.Lerp(dir1, dir2, delta));
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetLookDirection(Vector3 direction)
    {
        transform.eulerAngles = Quaternion.LookRotation(direction).eulerAngles;
    }
}
