using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flask : MonoBehaviour
{
    //TODO fix throw liquid out, timeot od wyrzucenie (bo sie dodaje z powrotem, / blokowanie origin flask)
    // GENEROWANIE LOSOWYCH KOLOROW DODAJE W ZLEJ KOLEJNOSCI DO FLASKOW

    List<FlaskLiquid> liquids = new List<FlaskLiquid>();

    public GameObject dropletSpawner;
    public GameObject dropletPrefab;

    int dropletCounter = 0;
    readonly int dropletTimeout = 100;


    Flask()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        getLiquids();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // x, z
        // 135 - 225, -135 - (-225)

        float pourAlignment = Vector3.Dot(transform.up, Vector3.down);
        //  1 = mouth straight down
        //  0 = sideways
        // -1 = mouth straight up

        bool shouldSpill = pourAlignment > 0f; // ~45°

        if (shouldSpill)
        {
            if (dropletCounter == 0)
            {
                dropletCounter = dropletTimeout;
                throwLiquidOut();
            }

            dropletCounter--;
        }
        else
        {
            dropletCounter = dropletTimeout / 2;
        }
    }

    Color getNextColor()
    {
        for (int i = liquids.Count - 1; i >= 0; i--)
        {
            Color c = liquids[i].getColor();

            if (c != new Color(0, 0, 0, 0))
            {
                return c;
            }
        }

        return new Color(0, 0, 0, 0);
    }

    Color removeNextColor()
    {
        for (int i = liquids.Count - 1; i >= 0; i--)
        {
            Color c = liquids[i].getColor();

            if (c != new Color(0, 0, 0, 0))
            {
                liquids[i].setColor(new Color(0, 0, 0, 0));
                return c;
            }
        }

        return new Color(0, 0, 0, 0);
    }

    public bool isFull()
    {
        bool isFull = true;

        for (int i = 0; i < 4; i++)
        {
            if (liquids[i].getColor() == new Color(0, 0, 0, 0))
            {
                isFull = false;
            }
        }

        return isFull;
    }

    public bool isCorrect()
    {
        Color firstColor = liquids[0].getColor();

        foreach (FlaskLiquid l in liquids)
        {
            if (firstColor != l.getColor())
            {
                return false;
            }
        }

        return true;
    }

    public bool addFluid(Color c)
    {
        for (int i = 0; i < 4; i++)
        {
            if (liquids[i].getColor() == new Color(0, 0, 0, 0))
            {
                liquids[i].setColor(c);
                return true;
            }
        }

        return false;
    }

    private void throwLiquidOut()
    {
        Color c = removeNextColor();

        if (c == new Color(0, 0, 0, 0))
        {
            return;
        }

        GameObject droplet = Instantiate(dropletPrefab);
        LiquidDroplet dropletInstance = droplet.GetComponent<LiquidDroplet>();

        dropletInstance.color = c;
        dropletInstance.setOriginFlask(this.gameObject);

        droplet.transform.position = dropletSpawner.transform.position;
        // droplet.transform.position += new Vector3(0, -.3f, 0);


    }

    void getLiquids()
    {
        FlaskLiquid[] f = GetComponentsInChildren<FlaskLiquid>();
        Array.Reverse(f);

        foreach (FlaskLiquid liq in f)
        {
            liquids.Add(liq);
            liq.setColor(new Color(0, 0, 0, 0));
        }
    }

    public void generateRandomColors(List<Color> colors, Dictionary<Color, int> colorCounters)
    {
        for (int i = 0; i < 4; i++)
        {
            if (colors.Count == 0) break;

            int index = UnityEngine.Random.Range(0, colors.Count());
            Color pickedColor = colors[index];
            colorCounters[pickedColor]--;

            liquids[i].setColor(pickedColor);

            if (colorCounters[pickedColor] <= 0)
            {
                colors.RemoveAll(c => c == pickedColor);
            }
        }

    }
}
