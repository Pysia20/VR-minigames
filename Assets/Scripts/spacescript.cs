using UnityEngine;
using TMPro;

public class space : MonoBehaviour
{
    public bool touched = false;
    bool flagged = false;
    public bool isMine = false;
    public int adjacentMines = 0;
    private GameObject flagObj;

    private GameObject BoomAnim;
    public void Destroy()
    {
        if (BoomAnim != null) BoomAnim.SetActive(false);
        if (flagObj != null) flagObj.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TMP_Text tmp = this.gameObject.GetComponentInChildren<TMP_Text>();
        if (tmp != null) tmp.text = "";

        flagObj = GameObject.Find("Flag");
        if (flagObj != null) flagObj.SetActive(false);

        BoomAnim = GameObject.Find("boomsheet");
        if (BoomAnim != null) BoomAnim.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Flag" && !touched) {
            flagged = !flagged;
            if (flagObj != null)
            {
                flagObj.SetActive(flagged);
                flagObj.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            } 
            return;
        }
        if (collision.gameObject.tag == "Shovel")
        {
            touched = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (flagged) {
            this.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            return;
        }

        if (touched && isMine)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
            if (BoomAnim != null) BoomAnim.SetActive(true);
            if (BoomAnim != null && Camera.main != null)
            {
                Vector3 dir = Camera.main.transform.position - BoomAnim.transform.position;
                dir.y = 0f;
                if (dir.sqrMagnitude > 0.0001f)
                    BoomAnim.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            }
            return;
        } else if (touched && !isMine)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.green;
            TMP_Text tmp = this.gameObject.GetComponentInChildren<TMP_Text>();
            if (tmp != null) tmp.text = adjacentMines.ToString();
        } else
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
