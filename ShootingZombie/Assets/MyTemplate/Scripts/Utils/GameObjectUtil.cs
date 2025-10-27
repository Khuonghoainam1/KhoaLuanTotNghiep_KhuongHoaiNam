using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class GameObjectUtil
{
    public static void SetCanvasPos(this Transform transform, Vector3 offset, Camera gameCamera, Canvas parentCanvas, RectTransform targetRect, out bool parentOnScreen)
    {
        parentOnScreen = false;

        //Reset Scale to 1
        targetRect.localScale = new Vector3(1f, 1f, 1f);

        Vector2 parentSize = targetRect.sizeDelta;

        Vector3 wpParentPos = gameCamera.WorldToScreenPoint(transform.position + offset);

        //This keeps wpParentPos.z value from going above 1 causing dissapearing if above 1000 or so
        if (wpParentPos.z > 1)
        {
            wpParentPos.z = 1f;
        }

        float canvasScaleFactor = parentCanvas.scaleFactor;

        //ONSCREEN SNAP TO EDGE OFFSET
        //This ensures the on screen icon boundary edge will trigger offscreen exactly when it touches screen edge
        //Otherwise, the on screen icon will snap to edge before reaching them
        //As long as Canvas > Reference Resolution is the same aspect ratio as the play mode screen, this will always align
        float onScreenSnapOffset_X = 22 - (Screen.width * .024f);
        float onScreenSnapOffset_Y = 0f;

        //Onscreen
        if (wpParentPos.z > 0f &&
            (wpParentPos.x / canvasScaleFactor) > (targetRect.sizeDelta.x / 2) && wpParentPos.x < (Screen.width - (targetRect.sizeDelta.x / 2)) + onScreenSnapOffset_X &&
            wpParentPos.y / canvasScaleFactor > (targetRect.sizeDelta.y / 2) && wpParentPos.y < (Screen.height - (targetRect.sizeDelta.y / 2)) + onScreenSnapOffset_Y)
        {
            parentOnScreen = true;

            targetRect.transform.position = wpParentPos;
        }
        else //(Offscreen)
        {
            parentOnScreen = false;

            if (wpParentPos.z < 0)
            {
                //Flip coordinates when things are behind
                wpParentPos *= -1;
            }

            //Find center of screen
            Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0f) / 2;

            //Set 0,0 DEAD CENTER from lower left
            wpParentPos -= screenCenter;

            //Find angle from center of screen to mouse pos
            float angle = Mathf.Atan2(wpParentPos.y, wpParentPos.x);
            angle -= 90f * Mathf.Deg2Rad;

            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            //y = mx+b format
            float m = cos / sin;

            Vector3 screenBounds = screenCenter;

            //Get Offscreen Padding Offset: (1 / Reference Resolution)
            //This positions the boundary right along the edge once its gone offscreen
            //This stops the bouncing back and passing beyond edges when icon reaches screen edge
            //As long as Canvas > Reference Resolution is the same aspect ratio as the play mode screen, this will always align
            Vector2 canvasRefRez = parentCanvas.GetComponent<CanvasScaler>().referenceResolution;
            float parentPaddingX = 1 / canvasRefRez.x;
            float parentPaddingY = 1 / canvasRefRez.y;

            screenBounds.x = screenCenter.x * (.999f - (parentSize.x * parentPaddingX));
            screenBounds.y = screenCenter.y * (.999f - (parentSize.y * parentPaddingY));

            //Check up and down first
            if (cos > 0f)
            {
                //up
                wpParentPos = new Vector3(-screenBounds.y / m, screenBounds.y, 0f);
            }
            else
            {
                //down
                wpParentPos = new Vector3(screenBounds.y / m, -screenBounds.y, 0f);
            }

            //If out of bounds, get point on appropriate side
            if (wpParentPos.x > screenBounds.x) //Out of bounds! Must be on the right
            {
                wpParentPos = new Vector3(screenBounds.x, -screenBounds.x * m, 0f);
            }
            else if (wpParentPos.x < -screenBounds.x) //Out of bounds! Must be on the left
            {
                wpParentPos = new Vector3(-screenBounds.x, screenBounds.x * m, 0);
            }

            //Remove coordinate translation
            wpParentPos += screenCenter;

            targetRect.transform.position = wpParentPos;
        }

        //Size
        targetRect.sizeDelta = parentSize;
    }
}
