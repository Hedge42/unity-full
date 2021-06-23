using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Neet.Collections;
using Neet.Scroller;

// Scroller manager currently blank
public class PresetScroller : MonoBehaviour
{
    // Object references
    public RectTransform prefab;
    public RectTransform container;
    public Scrollbar sb;

    // Parameters
    [Range(-50f, 100f)]
    public float spaceBetweenItems;
    [Range(5, 25)]
    public int maxItems;
    [Range(10, 200)]
    public float xIndentation;

    // TODO: systemize this
    public string musicRootPath;

    // Data
    private DirectoryTree tree;
    private GameObject[] browserItemGOs;

    private DirectoryItem[] availableDirectoryItems;
    private Vector2[] availablePositions;

    private void Start()
    {
        tree = new DirectoryTree(musicRootPath);


        SpawnItems();
        sb.onValueChanged.AddListener(delegate { FixItems(); }); // better than using update()

        sb.value = 1; // sets slider to top
    }
    private void SpawnItems()
    {
        DestroyAll();

        UpdateAvailableDirectoryItems();
        UpdateAvailablePositions();

        browserItemGOs = new GameObject[maxItems];
        for (int i = 0; i < browserItemGOs.Length; i++)
            browserItemGOs[i] = Instantiate(prefab.gameObject, container);
        ResizeContainer();
    }
    public void ExpandOrCollapseFolder(DirectoryItem clicked)
    {
        clicked.isOpen = !clicked.isOpen;

        float clickedY = GetItemGameObject(clicked).transform.position.y;
        print($"clickedY: {clickedY}");
        float oldContainerHeight = container.sizeDelta.y;

        UpdateAvailableDirectoryItems();
        UpdateAvailablePositions();
        ResizeContainer();
        FixItems();

        // instead, use availableDirectoryItems and availablePositions
        // to determine where it WOULD be
        int newIndex = Array.IndexOf(availableDirectoryItems, clicked);
        Vector2 localPos = availablePositions[newIndex];

        float newContainerHeight = container.sizeDelta.y;
        float deltaHeight = (oldContainerHeight - newContainerHeight) / 2;

        // container.position += deltaHeight / 2... but using slider
        float moveRatio = deltaHeight / 2 / container.sizeDelta.y;
        print($"moveRatio {moveRatio}");

        // sb.value += moveRatio;


        //SpawnItems();
        // ResetFull();
    }
    private GameObject GetItemGameObject(DirectoryItem d)
    {
        foreach (GameObject g in browserItemGOs)
        {
            var b = g.GetComponent<BrowserItem>();
            if (b.tmpFullPath.text == d.path)
                return g;
        }

        Debug.LogError("Couldn't find the associated item");
        return null;
    }
    private void ResizeContainer()
    {
        // height == (prefab.height + space) * count
        container.sizeDelta = new Vector2(prefab.sizeDelta.x, (prefab.sizeDelta.y + spaceBetweenItems) * availableDirectoryItems.Length);
    }
    private void DestroyAll()
    {
        if (browserItemGOs != null)
            for (int i = 0; i < browserItemGOs.Length; i++)
                Destroy(browserItemGOs[i]);

        browserItemGOs = null;
    }
    private Vector2[] UpdateAvailablePositions()
    {
        Vector2[] arr = new Vector2[availableDirectoryItems.Length];

        // top-most position
        float currentY = -1 * (spaceBetweenItems / 2);

        for (int i = 0; i < arr.Length; i++)
        {
            float x = xIndentation * (availableDirectoryItems[i].level - 1);

            arr[i] = new Vector2(x, currentY);
            currentY -= (spaceBetweenItems + prefab.sizeDelta.y);
        }

        availablePositions = arr;
        return arr;
    }
    private DirectoryItem[] UpdateAvailableDirectoryItems()
    {
        return availableDirectoryItems = tree.GetVisibleItems();
    }
    private void FixItems()
    {
        Vector2[] positions = GetTargetPositions();
        DirectoryItem[] items = GetTargetDirectoryItems();

        for (int i = 0; i < browserItemGOs.Length; i++)
        {
            browserItemGOs[i].GetComponent<RectTransform>().anchoredPosition = positions[i];
            browserItemGOs[i].GetComponent<PresetScrollerItem>().SetData(items[i], this);
        }
    }
    private Vector2[] GetTargetPositions()
    {
        int firstPositionIndex = GetFirstIndex();

        Vector2[] limitedPositions = new Vector2[maxItems];
        // limitedPositions[i] == positions[firstPositionIndex + i]
        for (int i = 0; i < limitedPositions.Length; i++)
        {
            int positionIndex = firstPositionIndex + i;

            // make sure indexes doesn't break something
            if (positionIndex >= availablePositions.Length)
                continue;

            limitedPositions[i] = availablePositions[positionIndex];
        }

        return limitedPositions;
    }
    private DirectoryItem[] GetTargetDirectoryItems()
    {
        DirectoryItem[] targetItems = new DirectoryItem[maxItems];
        int firstItemIndex = GetFirstIndex();
        for (int i = 0; i < targetItems.Length; i++)
        {
            if (firstItemIndex + i >= availableDirectoryItems.Length)
                continue;

            targetItems[i] = availableDirectoryItems[firstItemIndex + i];
        }
        return targetItems;
    }
    private int GetFirstIndex()
    {
        // get the index of the first vector position to be used
        float centerY = (1 - sb.value) * (availablePositions.Length - 1);
        int centerPositionIndex = Mathf.RoundToInt(centerY);
        int firstPositionIndex = centerPositionIndex - (maxItems / 2);
        while (firstPositionIndex >= availablePositions.Length - maxItems)
            firstPositionIndex--;
        while (firstPositionIndex < 0)
            firstPositionIndex++;


        return firstPositionIndex;
    }
}
