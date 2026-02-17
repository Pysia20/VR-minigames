using UnityEngine;

public class LiquidDroplet : MonoBehaviour
{
    public Color color;
    public GameObject splatterPrefab;

    public GameObject originFlask;

    void Start()
    {
        GetComponent<Renderer>().material.color = color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Flask"))
        {
            if(collision.gameObject == originFlask)
            {
                return;
            }

            Flask flask = collision.gameObject.GetComponent<Flask>();

            if (!flask.addFluid(color))
            {
                return;
            }

            Destroy(this.gameObject);
        }
        else
        {
            GameObject splatter = Instantiate(splatterPrefab);
            splatter.GetComponent<Renderer>().material.color = color;

            // splatter.transform.rotation = new Quaternion(Random.Range(0, 1), 0, Random.Range(0,1), 0);

            splatter.transform.position = transform.position;
            splatter.transform.position += new Vector3(0, -0.01f, 0);

            Destroy(this.gameObject);
        }
    }

    public void setOriginFlask(GameObject origin)
    {
        originFlask = origin;
    }

}
