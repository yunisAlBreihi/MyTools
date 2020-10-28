using UnityEngine;

public class ReplayPlayerData : MonoBehaviour
{
    #region Variables
    private ReplayDataScriptable replayData = null;
    private PlayerCharacterController controller = null;
    private DataHolder playerData = null;

    private float lerpDelta = 0.0f;
    private int positionIndex = 0;
    #endregion Variables

    #region GameLoop
    private void Awake()
    {
        controller = gameObject.GetComponent<PlayerCharacterController>();
        replayData = (ReplayDataScriptable)Resources.Load("ReplayData", typeof(ReplayDataScriptable));
    }

    private void Start()
    {
        if (replayData.RunReplay == true)
        {
            playerData = replayData.Data;
        }
    }

    private void OnDisable()
    {
        replayData.Clear();
    }

    private void Update()
    {
        if (replayData.RunReplay == true)
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
                    //When replay is complete, reset the X and Z rotations, otherwise will create bugs
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
    #endregion GameLoop

    #region Movement
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

    private void LerpMovement(Vector3 pos1, Vector3 pos2, Vector3 dir1, Vector3 dir2, float delta)
    {
        transform.position = Vector3.Lerp(pos1, pos2, delta);
        transform.eulerAngles = Quaternion.LookRotation(Vector3.Lerp(dir1, dir2, delta)).eulerAngles;
    }
    #endregion Movement
}
