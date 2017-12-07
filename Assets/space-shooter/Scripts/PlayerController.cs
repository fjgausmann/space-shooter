using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public Transform ZusatzwaffeLinks;
    public Transform ZusatzwaffeRechts;
    private bool ZusatzwaffeAktiv;
    public float fireRate;
    public GameController gameController;
    private float scoreNow;
    private float NaechstesUpgradeBei;
    public float ZusatzwaffeBeiScore;

    private float nextFire;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire && ZusatzwaffeAktiv == false)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }

        if (ZusatzwaffeAktiv && Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, ZusatzwaffeLinks.position, ZusatzwaffeLinks.rotation);
            GetComponent<AudioSource>().Play();
            Instantiate(shot, ZusatzwaffeRechts.position, ZusatzwaffeRechts.rotation);
            GetComponent<AudioSource>().Play();

        }
        scoreNow = gameController.score;
        StartCoroutine(Zusatzwaffe());
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;

        GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find gameController Script");
        }
        scoreNow = gameController.score;
        NaechstesUpgradeBei = ZusatzwaffeBeiScore;
        ZusatzwaffeAktiv = false;
    }

    IEnumerator Zusatzwaffe()
    {
        if (scoreNow == NaechstesUpgradeBei)
        {
            ZusatzwaffeAktiv = true;

            NaechstesUpgradeBei = scoreNow + ZusatzwaffeBeiScore;
            yield return new WaitForSeconds(5);
            ZusatzwaffeAktiv = false;
        }
    }
}