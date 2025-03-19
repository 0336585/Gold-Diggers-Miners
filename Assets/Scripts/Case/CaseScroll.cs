using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VolFx.OldMoviePass;

public class CaseScroll : MonoBehaviour
{
    [SerializeField] private int cost;

    [Header("References")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private CaseResult caseResult;
    [SerializeField] private GameObject gambleMenu;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip splinClip;

    private float speed;
    private bool isScrolling;
    private Vector3 startPos;

    private List<CaseCell> cells = new List<CaseCell>();

    private void OnEnable()
    {
        MenuManager.Instance.OnMenuOpen += CloseMenu;

    }

    private void OnDisable()
    {
        MenuManager.Instance.OnMenuOpen -= CloseMenu;
    }

    private void Awake()
    {
        startPos = transform.position;
    }

    public void Scroll()
    {
        if (isScrolling || MoneyManager.Instance.Money < cost)
            return;

        audioSource.PlayOneShot(splinClip);

        QuotaManager.Instance.AddSpin();
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
        float elapsedTime = 0f;
        float scrollDuration = speed; // The initial speed determines the duration

        while (elapsedTime < scrollDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to avoid FPS dependence

            float step = Mathf.Lerp(100, 0, elapsedTime / scrollDuration); // Smooth stop
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.left * step, Time.unscaledDeltaTime * 3400);

            yield return null;
        }

        caseResult.ShowResult();
        yield return new WaitForSecondsRealtime(3); // Ensure real-time waiting

        CloseMenu();

        for (int i = 0; i < cells.Count; i++)
        {
            Destroy(cells[i].transform.parent.transform.parent.gameObject);
        }

        cells.Clear();
        isScrolling = false;
        transform.position = startPos;
    }

    public void OpenMenu()
    {
        MenuManager.Instance.MenuEvent();
        gambleMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        gambleMenu.SetActive(false);
        MenuManager.Instance.MenuEventClosed();

        for (int i = 0; i < cells.Count; i++)
        {
            Destroy(cells[i].transform.parent.transform.parent.gameObject);
        }

        cells.Clear();
        isScrolling = false;

        transform.position = startPos;
    }
}