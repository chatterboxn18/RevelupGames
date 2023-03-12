using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    [ShowIf("_fitType", FitType.FixedRows)]public int Rows;
    [ShowIf("_fitType", FitType.FixedColumns)] public int Columns;
    public Vector2 CellSize = Vector2.zero;
    public Vector2 Spacing;
    [EnumToggleButtons]
    [SerializeField] private FitType _fitType;
    [SerializeField] private bool _fixX;
    [SerializeField] private bool _fixY;
    public enum FitType
    {
        Width, 
        Height,
        Uniform,
        FixedColumns, 
        FixedRows
    }
    
    public override void CalculateLayoutInputVertical()
    {
        
        
    }

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if (rectChildren.Count <= 0) return;
        
        if (_fitType == FitType.Width || _fitType == FitType.Height || _fitType == FitType.Uniform)
        {
            _fixX = true;
            _fixY = true;
            
            float squareRoot = Mathf.Sqrt(transform.childCount);
            Rows = Mathf.CeilToInt(squareRoot);
            Columns = Mathf.CeilToInt(squareRoot);
        }

        if (_fitType == FitType.Width || _fitType == FitType.FixedColumns)
        {
            Rows = Mathf.CeilToInt(transform.childCount / (float)Columns);
        }

        if (_fitType == FitType.Height || _fitType == FitType.FixedRows)
        {
            Columns = Mathf.CeilToInt(transform.childCount / (float)Rows);
        }
        
        float parentWidth = rectTransform.rect.width;
        float parentHeight= rectTransform.rect.height;

        var cellWidth = (parentWidth / (float)Columns) - ((Spacing.x/(float)Columns) * (Columns - 1)) - (padding.left / (float)Columns) - (padding.right / (float)Columns);
        var cellHeight =(parentHeight / (float)Rows) - ((Spacing.y/(float)Rows) * (Rows - 1)) - (padding.top / (float)Rows) - ( padding.bottom/(float) Rows);

        CellSize.x = cellWidth;
        // CellSize.x = _fixX ? cellWidth : CellSize.x;
        CellSize.y = cellHeight;
        //CellSize.y = _fixY ? cellHeight : CellSize.y;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / Columns;
            columnCount = i % Columns;

            var item = rectChildren[i];

            var xPos = (CellSize.x * columnCount) + (Spacing.x * columnCount) + padding.left;
            var yPos = (CellSize.y * rowCount) + (Spacing.y * rowCount) + padding.top;
            
            SetChildAlongAxis(item, 0, xPos, CellSize.x);
            SetChildAlongAxis(item, 1, yPos, CellSize.y);
        }
    }

    public override void SetLayoutHorizontal()
    {
        
        
    }

    public override void SetLayoutVertical()
    {
    }
}
