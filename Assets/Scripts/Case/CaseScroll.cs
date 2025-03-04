using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseScroll : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private CaseResult caseResult;
    [SerializeField] private GameObject gambleMenu;

    private float speed;
    private bool isScrolling;
    private Vector3 startPos;

    private List<CaseCell> cells = new List<CaseCell>();

    private void Awake()
    {
        startPos = transform.position;
    }

    public void Scroll()
    {
        if (isScrolling)
            return;

        speed = Random.Range(4, 5);
        isScrolling = true;

        if (cells.Count == 0)
            for (int i = 0; i < 50; i++)
                cells.Add(Instantiate(prefab, transform).transform.GetComponentInChildren<CaseCell>());

        foreach (CaseCell cell in cells)
            cell.Setup();

        StartCoroutine("StartScrolling");
    }

    private IEnumerator StartScrolling()
    {
        while (speed > 0)
        {
            speed -= Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.left * 100, speed * Time.deltaTime * 1500);

            yield return null;
        }

        caseResult.ShowResult();

        yield return new WaitForSeconds(3);

        gambleMenu.SetActive(false);

        for (int i = 0; i < cells.Count; i++)
        {
            Destroy(cells[i].transform.parent.transform.parent.gameObject);
        }

        cells.Clear();
        isScrolling = false;

        transform.position = startPos;
    }
}
