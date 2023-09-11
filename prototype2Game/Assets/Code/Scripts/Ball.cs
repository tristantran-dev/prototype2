using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private GameObject holeSounds;
    // [SerializeField] private Text strokeui;
    // [SerializeField] private Text levelDoneUi;
    //[SerializeField] private GameObject gameOverUI;

    [Header("Attributes")]
    [SerializeField] private float maxPower = 6f;
    [SerializeField] private float power = 2f;
    [SerializeField] private int maxStrokes;
    [SerializeField] private float maxGoalSpeed = 4f;

    private bool isDragging;
    private bool inHole;
    private bool outStrokes;
    private bool levelComplete;
    private int strokes = 0;
    public AudioSource audiosource;
    public AudioClip hit;
    public AudioClip win;
    public AudioClip bounce;
    public Renderer ren;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        ren = GetComponent<Renderer>();
        text.text = "Strokes: " + strokes;
    }

    private bool IsReady() {
        return rb.velocity.magnitude <= 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("level1");
        }
        if ( Input.GetKey("escape"))
        {
            Application.Quit();
        }
        PlayerInput();
    }

    private void PlayerInput(){
        /*if (!IsReady()){
            return;
        }*/
        Vector2 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, inputPos);

        if ( Input.GetMouseButtonDown(0) && distance <= 0.5f){
            DragStart();
        }
        if ( Input.GetMouseButtonDown(0) && isDragging){
            DragChange(inputPos);
        }
        if ( Input.GetMouseButtonUp(0) && isDragging){
            DragRelease(inputPos);
        }
    }

    private void DragStart(){
      isDragging = true;
      lr.positionCount = 2;
    }

    private void DragChange(Vector2 pos){
        Vector2 dir = (Vector2)transform.position - pos;
        lr.SetPosition(0,transform.position);
        lr.SetPosition(1, (Vector2)transform.position + Vector2.ClampMagnitude((dir * power)/2, maxPower/2));
    }

    private void DragRelease(Vector2 pos){
        float distance = Vector2.Distance((Vector2)transform.position, pos);
        isDragging = false;
        lr.positionCount = 0;

        if(distance < 1f){
            return;
        }
        audiosource.PlayOneShot(hit);
        strokes++;
        text.text = "Strokes: " + strokes;
        Vector2 dir = (Vector2)transform.position - pos;
        rb.velocity = Vector2.ClampMagnitude(dir * power, maxPower);
    }

    private void CheckWinState(){
        if (inHole) {
            return;
        }
        //if (rb.velocity.magnitude <= maxGoalSpeed){
        if (true) {
          inHole = true;
          audiosource.PlayOneShot(win);
          rb.velocity = Vector2.zero;
          //gameObject.SetActive(false);

          GameObject fx = Instantiate(holeSounds, transform.position, Quaternion.identity);
          Destroy(fx, 1.5f);
          ren.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        audiosource.PlayOneShot(bounce);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "hole") {
            //audiosource.PlayOneShot(win);
            CheckWinState();
        }
    }

    private void OnTriggerStay2D(Collider2D other){
        if(other.tag == "hole") {
            //audiosource.PlayOneShot(win);
            CheckWinState();
        }
    }
}
