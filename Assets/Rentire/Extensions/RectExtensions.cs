using System.Collections.Generic;
using UnityEngine;

namespace Rentire.Utils
{
    
    public static class RectExtensions
    {
        public static Rect RectTransformToScreenSpace(this RectTransform transform, Camera cam, bool cutDecimals = false)
        {
            var worldCorners = new Vector3[4];
            var screenCorners = new Vector3[4];

            transform.GetWorldCorners(worldCorners);

            for (int i = 0; i < 4; i++)
            {
                screenCorners[i] = cam.WorldToScreenPoint(worldCorners[i]);
                if (cutDecimals)
                {
                    screenCorners[i].x = (int) screenCorners[i].x;
                    screenCorners[i].y = (int) screenCorners[i].y;
                }
            }

            return new Rect(screenCorners[0].x,
                screenCorners[0].y,
                screenCorners[2].x - screenCorners[0].x,
                screenCorners[2].y - screenCorners[0].y);
        }
        
        public static Vector2 WorldPointToCanvasLocalRectTransformPoint(Vector3 worldPoint,
            Camera camera, Canvas canvas, RectTransform parentRect)
        {
            var screenPoint = camera.WorldToScreenPoint(worldPoint);
            // Translate screen point to local point of a parent rect transform
            // If canvas render mode is ScreenSpace-Overlay, camera param should be null
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect, screenPoint,
                canvas.renderMode == RenderMode.ScreenSpaceCamera ? camera : null,
                out var result);
            return result;
        }
        
        
        public static void DestroyChildren(this RectTransform go)
        {

            var children = new List<GameObject>();
            foreach (Transform tran in go.transform)
            {
                children.Add(tran.gameObject);
            }
            children.ForEach(Object.Destroy);
        }

        public static void SetDefaultScale(this RectTransform trans)
        {
            trans.localScale = new Vector3(1, 1, 1);
        }

        public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
        {
            trans.pivot = aVec;
            trans.anchorMin = aVec;
            trans.anchorMax = aVec;
        }

        public static Vector2 GetSize(this RectTransform trans)
        {
            return trans.rect.size;
        }

        public static float GetWidth(this RectTransform trans)
        {
            return trans.rect.width;
        }

        public static float GetHeight(this RectTransform trans)
        {
            return trans.rect.height;
        }

        public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
        }

        public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
        }

        public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
        }

        public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
        }

        public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
        }

        public static void SetSize(this RectTransform trans, Vector2 newSize)
        {
            Vector2 oldSize = trans.rect.size;
            Vector2 deltaSize = newSize - oldSize;
            trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
            trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
        }

        public static void SetWidth(this RectTransform trans, float newSize)
        {
            SetSize(trans, new Vector2(newSize, trans.rect.size.y));
        }

        public static void SetHeight(this RectTransform trans, float newSize)
        {
            SetSize(trans, new Vector2(trans.rect.size.x, newSize));
        }
    }
    
}

