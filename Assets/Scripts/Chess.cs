using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour
{
    public static Object Instance;

    public ChessSO chessSO;

    public DrawWithMouse drawControl;

    public float speed = 5f;

    private Vector3 startPos;

    bool startMovement = false;

    Vector3[] positions;

    int moveIndex = 0;

    private float min_X = -2.15f;

    private float max_X = 2.15f;

    private float min_Y = -4.85f;

    private float max_Y = 4.85f;

    public GameObject vfxOnDeath;

    public GameObject vfxSuccess;

    public GameObject vfxKill;

    private new SpriteRenderer renderer;

    private bool moveAbility;

    private void Awake()
    {
        Instance = this;

        startPos = transform.position;

        renderer = GetComponentInChildren<SpriteRenderer>();

        renderer.sprite = chessSO.sprite;

        moveAbility = chessSO.canRunThroughOstacle;
    }

    private void OnMouseDown()
    {
        if (!chessSO.isMainChess) return;
        drawControl.StartLine(transform.position);
    }

    private void OnMouseDrag()
    {
        if (!chessSO.isMainChess) return;
        drawControl.Updateline();
    }

    private void OnMouseUp()
    {
        if (!chessSO.isMainChess) return;
        positions = new Vector3[drawControl.line.positionCount];
        drawControl.line.GetPositions(positions);
        drawControl.ResetLine();
        startMovement = true;
        moveIndex = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!chessSO.isMainChess) return;
        if (collision.gameObject.tag == "Chess")
        {
            GameObject vfx = Instantiate(vfxKill, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);

            renderer.sprite = collision.gameObject.GetComponent<Chess>().chessSO.sprite;
            moveAbility = collision.gameObject.GetComponent<Chess>().chessSO.canRunThroughOstacle;
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Obstacle")
        {
            if (!moveAbility)
            {
                GameObject vfx = Instantiate(vfxOnDeath, transform.position, Quaternion.identity);
                Destroy(vfx, 1f);

                ReSetPos();
            }
        }
        else if (collision.gameObject.tag == "Goal")
        {
            GameObject vfx = Instantiate(vfxSuccess, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);

            StartCoroutine(ReachedGoal(collision.gameObject));
        }
    }

    private IEnumerator ReachedGoal(GameObject obj)
    {
        yield return new WaitForSeconds(1f);

        GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].goals.Remove(obj);
    }

    public void ReSetPos()
    {
        transform.position = startPos;
        startMovement = false;
        drawControl.ResetLine();
    }

    private void Update()
    {
        if (startMovement)
        {
            CheckPos();

            Vector2 currentPos = positions[moveIndex];
            transform.position = Vector2.MoveTowards(transform.position, currentPos, speed * Time.deltaTime);

            float distance = Vector2.Distance(currentPos, transform.position);
            if (distance <= 0.05f)
            {
                moveIndex++;
            }

            if (moveIndex > positions.Length - 1)
            {
                startMovement = false;
            }
        }
    }

    private void CheckPos()
    {
        if (transform.position.x < min_X)
        {
            Vector3 moveDirX = new Vector3(min_X, transform.position.y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x > max_X)
        {
            Vector3 moveDirX = new Vector3(max_X, transform.position.y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.y < min_Y)
        {
            Vector3 moveDirX = new Vector3(transform.position.x, min_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.y > max_Y)
        {
            Vector3 moveDirX = new Vector3(transform.position.x, max_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x < min_X && transform.position.y < min_Y)
        {
            Vector3 moveDirX = new Vector3(min_X, min_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x < min_X && transform.position.y > max_Y)
        {
            Vector3 moveDirX = new Vector3(min_X, max_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x > max_X && transform.position.y > max_Y)
        {
            Vector3 moveDirX = new Vector3(max_X, max_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x > max_X && transform.position.y < min_Y)
        {
            Vector3 moveDirX = new Vector3(max_X, min_Y, 0f);
            transform.position = moveDirX;
        }
    }
}