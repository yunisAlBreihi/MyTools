using System.Collections;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ReplayPlayerData : MonoBehaviour, IUnit
{
    [SerializeField] private float capturesPerSecond = 5.0f;
    [SerializeField] private ReplayDataScriptable replayData = null;

    private PlayerInputHandler inputHandler = null;
    private PlayerCharacterController controller = null;
    private DataHolder playerData = null;

    private float moveTimer = 0.0f;
    private float lerpDelta = 0.0f;
    private int positionIndex = 0;

    private void Awake()
    {
        controller = gameObject.GetComponent<PlayerCharacterController>();
    }

    private void Start()
    {
        if (replayData.runReplay == true)
        {
            playerData = replayData.data;
            //StartCoroutine(MovePlayer());
        }
    }

    private void OnDisable()
    {
        replayData.Clear();
    }

    private void FixedUpdate()
    {
        if (replayData.runReplay == true)
        {
            controller.enabled = false;
            if (moveTimer >= 1.0f)
            {
                if (lerpDelta <= 1.0f)
                {
                    if (positionIndex + 1 < playerData.positions.Count)
                    {
                        LerpMovement(playerData.positions[positionIndex], playerData.positions[positionIndex + 1],
                        playerData.lookDirections[positionIndex], playerData.lookDirections[positionIndex + 1], lerpDelta);
                    }
                    else
                    {
                        replayData.Clear();
                    }
                    lerpDelta += Time.deltaTime * capturesPerSecond;
                }
                else
                {
                    positionIndex++;
                    lerpDelta = 0.0f;
                }
                moveTimer = 0.0f;
            }

            moveTimer += Time.deltaTime * capturesPerSecond;
        }
    }

    IEnumerator MovePlayer()
    {
        WaitForSeconds wait = new WaitForSeconds(1.0f / capturesPerSecond);

        controller.enabled = false;
        for (int i = 0; i < playerData.positions.Count; i++)
        {
            if (i + 1 < playerData.positions.Count)
            {
                //StartCoroutine(LerpMovement(playerData.positions[i], playerData.positions[i + 1], playerData.lookDirections[i], playerData.lookDirections[i + 1]));
            }
            yield return wait;
        }
        controller.enabled = true;

        Debug.Log("Done Moving!");
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
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
