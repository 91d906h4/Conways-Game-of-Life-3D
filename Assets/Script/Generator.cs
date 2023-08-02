using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] GameObject cell;
    [SerializeField] float update_rate = 0.2f;

    GameObject[,] platform = new GameObject[100, 100];

    int[,] cells = new int[100, 100];
    float nextTime = 0f;
    bool starting = false;

    // Start is called before the first frame update
    void Start()
    {
        // Generate cells.
        for (float i = -49.5f; i <= 49.5f; i++)
        {
            for (float j = -49.5f; j <= 49.5f; j++)
            {
                GameObject c = Instantiate(cell, new Vector3(i, 1f, j), Quaternion.identity);
                platform[(int)(i + 49.5), (int)(j + 49.5)] = c;
                
                if (Random.Range(0, 3) == 0) {
                    cells[(int)(i + 49.5), (int)(j + 49.5)] = 1;
                }
                else
                {
                    c.SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            starting = !starting;
            nextTime = Time.time;
        }

        if (starting && Time.time >= nextTime)
        {
            nextGeneration();
            updatePlatform();
            
            nextTime += update_rate;
        }
    }

    void updatePlatform()
    {
        for (int i = 0; i < 100; i++) 
        {
            for (int j = 0; j < 100; j++)
            {
                if (cells[i, j] == 0)
                {
                    platform[i, j].SetActive(false);
                }
                else
                {
                    platform[i, j].SetActive(true);
                }
            }
        }
    }

    void nextGeneration()
    {
        int[,] update_cells = new int[100, 100];

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                int count = getCellsAround(i, j);

                if (count < 2 || count > 3)
                {
                    update_cells[i, j] = 0;
                }
                else if (count == 2)
                {
                    update_cells[i, j] = cells[i, j];
                }
                else
                {
                    update_cells[i, j] = 1;
                }
            }
        }

        cells = update_cells;
    }

    int getCellsAround(int x, int y)
    {
        int count = 0;

        bool a = x - 1 >= 0, b = x + 1 < 100;
        bool c = y - 1 >= 0, d = y + 1 < 100;

        if (a && cells[x - 1, y] == 1) count++;
        if (b && cells[x + 1, y] == 1) count++;
        if (c && cells[x, y - 1] == 1) count++;
        if (d && cells[x, y + 1] == 1) count++;
        if (a && c && cells[x - 1, y - 1] == 1) count++;
        if (a && d && cells[x - 1, y + 1] == 1) count++;
        if (b && c && cells[x + 1, y - 1] == 1) count++;
        if (b && d && cells[x + 1, y + 1] == 1) count++;

        return count;
    }
}
