using UnityEngine;

public class boardscript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    int bomblimit = 10;
    int width = 6;
    int height = 6;
    float spacing = 0.25f;
    public GameObject spacePrefab;
    space[,] spaces = new space[6,6];
    bool shouldReset = true;
    public void SpacesDiscovered() {
        int discovered = 0;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (spaces[x, y].touched && !spaces[x, y].isMine) discovered++;
                else if (spaces[x, y].touched && spaces[x, y].isMine) {
                    Debug.Log("You lose!");
                    if(shouldReset) {
                        shouldReset = false;
                        Invoke("ResetBoard", 2f);
                    } 
                    return;
                }
            }
        }
        if (discovered >= width * height - bomblimit) {
            Debug.Log("You win!");
            if(shouldReset) {
                shouldReset = false;
                Invoke("ResetBoard", 2f);
            }
        }
    }
    void ResetBoard() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (spaces[x, y] != null) {
                    spaces[x, y].Destroy();
                    Destroy(spaces[x, y].gameObject);
                    spaces[x, y] = null;
                }
            }
        }
        shouldReset = true;
        Start();
    }
    void Start()
    {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GameObject spaceobj;
                if (spacePrefab != null)
                {
                    spaceobj = Instantiate(spacePrefab);
                }
                else
                {
                    Debug.LogWarning("Resources/space prefab not found — creating empty GameObject instead.");
                    spaceobj = new GameObject("space");
                }

                spaceobj.transform.position = new Vector3((x + this.transform.position.x) * spacing, 2, (y + this.transform.position.z) * spacing);
                if (spaceobj.GetComponent<space>() == null) spaceobj.AddComponent<space>();
                spaceobj.transform.parent = this.gameObject.transform;
                spaces[x, y] = spaceobj.GetComponent<space>();
            }
        }

        int bombcounter = 0;
        while (bombcounter < bomblimit) {
            int rx = Random.Range(0, width);
            int ry = Random.Range(0, height);
            if (!spaces[rx, ry].isMine) {
                spaces[rx, ry].isMine = true;
                bombcounter++;
            }
        }

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (spaces[x, y].isMine) continue;
                int minecount = 0;
                for (int upx = -1; upx <= 1; upx++) {
                    for (int upy = -1; upy <= 1; upy++) {
                        if (upx == 0 && upy == 0) continue;
                        int checkx = x + upx;
                        int checky = y + upy;
                        if (checkx < 0 || checkx >= width || checky < 0 || checky >= height) continue;
                        if (spaces[checkx, checky].isMine) minecount++;
                    }
                }
                spaces[x, y].adjacentMines = minecount;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpacesDiscovered();
    }
}
