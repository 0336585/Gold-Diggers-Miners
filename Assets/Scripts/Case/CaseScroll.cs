using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseScroll : MonoBehaviour
{
    [SerializeField] private int cost;

    [Header("References")]
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
        if (isScrolling || MoneyManager.Instance.Money < cost)
            return;

        MoneyManager.Instance.RemoveMoney(cost);
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

            float step = Mathf.Lerp(100, 0, elapsedTime / scrollDuration); // Smooth stop
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.left * step, Time.unscaledDeltaTime * 3400);

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