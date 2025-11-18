using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Transform transform;
    private bool attacking;
    private float move;
    private int life;
    [SerializeField] private int joyId;
    [SerializeField] private float speed;

    [SerializeField] private GameObject punching, kick;
    [SerializeField] private Slider slider;
    [SerializeField] private Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        life = 100;
        attacking = false;

        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        punching.SetActive(false);
        kick.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Joy" + joyId + "Box"))
        {
            attacking = true;
            punching.SetActive(true);
            animator.SetBool("Punching", true);
            animator.SetBool("Kick", false);
        }
        if (Input.GetButtonDown("Joy" + joyId + "Circle"))
        {
            attacking = true;
            kick.SetActive(true);
            animator.SetBool("Kick", true);
            animator.SetBool("Punching", false);
        }

        // Horizontal movement

        // TODO - Diferenciar joysticks
        move = 0;
        if (!attacking)
            move = Input.GetAxis("Horizontal" + joyId);

        if (move > 0) {
            animator.SetBool("Forward", true);
            animator.SetBool("Backward", false);
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        } else if (move < 0) {
            animator.SetBool("Backward", true);
            animator.SetBool("Forward", false);
            transform.Translate(Vector3.left * -speed * Time.deltaTime);
        } else {
            animator.SetBool("Forward", false);
            animator.SetBool("Backward", false);
        }
    }

    public void EndFightMovement(string movement)
    {
        attacking = false;
        animator.SetBool(movement, false);
        switch (movement)
        {
            case "Punching":
                punching.SetActive(false);
                break;
            case "Kick":
                kick.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void IncreasetLife(int life)
    {
        this.life += life;
        if (this.life < 0)
            this.life = 0;

        slider.value = this.life;

        if (this.life < 30)
        {
            image.color = Color.red;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Kick" && other.gameObject != kick)
        {
            IncreasetLife(-7);
        }
        if (other.gameObject.tag == "Punching" && other.gameObject != punching)
        {
            IncreasetLife(-3);
        }
    }
}
