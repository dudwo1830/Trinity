using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    public float moveSpeed = 10f;
    private Vector3 direction;

    private Camera worldCam;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();

        worldCam = Camera.main;
    }

    private void Update()
    {
        var forward = worldCam.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        var right = worldCam.transform.right;
        right.y = 0f;
        right.Normalize();

        direction = forward * playerInput.vertical;
        direction += worldCam.transform.right * playerInput.horizontal;

        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Move", direction.magnitude);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        var position = playerRigidbody.position;
        position += direction * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(position);

        if (playerInput.vertical > 0f || playerInput.vertical < 0f || playerInput.horizontal > 0f || playerInput.horizontal < 0f)
        {
            BattleSystem.Instance.encountChance += Random.Range(0, Time.deltaTime * 100f);
            Debug.Log($"Encount Chance: {BattleSystem.Instance.encountChance}");
        }

        if (BattleSystem.Instance.encountChance >= 100f)
        {
            BattleSystem.Instance.SetupBattle();
        }
    }
}
