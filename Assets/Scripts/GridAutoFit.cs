using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup))]
public class GridAutoFitTwoRows : MonoBehaviour
{
    public Vector2 childPreferredSize = new Vector2(100, 100); // default fallback size
    private RectTransform rectTransform;
    private GridLayoutGroup grid;

    void Update()
    {
        if (grid == null)
        {
            grid = GetComponent<GridLayoutGroup>();
            rectTransform = GetComponent<RectTransform>();
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float spacingX = grid.spacing.x;
        float spacingY = grid.spacing.y;

        int totalChildren =  transform.Cast<Transform>().Where(child => child.gameObject.activeSelf).Count();
        int rows = 2;
        int columns = Mathf.CeilToInt((float)totalChildren / rows);

        float totalSpacingX = spacingX * (columns - 1);
        float totalSpacingY = spacingY * (rows - 1);

        float cellWidth = Mathf.Min((parentWidth - totalSpacingX) / columns, 450);
        float cellHeight = (parentHeight - totalSpacingY) / rows;

        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = rows;
        grid.cellSize = new Vector2(cellWidth, cellHeight);
    }
}