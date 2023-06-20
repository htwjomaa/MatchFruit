using System;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class SideDotInit : MonoBehaviour
{
    public static void UpdateSpriteAlpha(SpriteRenderer spriteRenderer) => spriteRenderer.color = new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Rl.settings.GetSideDotAlpha);

    public static void SideDotTypeSolver(int columnSideDot, int rowSideDot, ref SideDotType sideDotType  )  //ref
    {
        //for now randomize it only
        int randomEnumSelection = Random.Range(0, 3);
        sideDotType = SideDotType.moveLine;
        foreach (SideDotType sidedotSearch in Enum.GetValues(typeof(SideDotType)))
        {
            if (randomEnumSelection == (int)sidedotSearch) sideDotType = sidedotSearch;
        }

        if (CheckCorners(columnSideDot, rowSideDot, LoadingHelper.THIS.width, LoadingHelper.THIS.height, ref sideDotType))
            return;
        sideDotType = SideDotType.moveLine;
    }

    
    public static bool CheckCorners(int column, int row, int width, int height, ref SideDotType sideDotType)
    {
        switch (sideDotType)
        {
            case SideDotType.turnA:
                if (
                    (column == -1 && row == 0)
                    || (column == width && row == 0)
                    || (column == 0 && row == -1)
                    || (column == 0 && row == height)
                )
                    return false;
                break;
            case SideDotType.turnB:
                if (
                    (column == width && row == height - 1)
                    || (column == -1 && row == height - 1)
                    || (column == width - 1 && row == -1)
                    || (column == width - 1 && row == height)
                )
                    return false;
                break;
        }

        return true;
    }

    public static void SetIconIdentifier(int columnSideDot, int rowSideDot, SideDotType sideDotType, ref GameObject icon, Transform transform)
    {
        Vector3 newPosition = Vector3.zero;
        Quaternion newRotation = Quaternion.identity;
        switch (SideDotUtil.CheckDirection(columnSideDot, rowSideDot, LoadingHelper.THIS.width, LoadingHelper.THIS.height))
        {
            case Directions.left:
                newPosition= new Vector3(transform.position.x - Rl.settings.GetRightPaddingX,
                    transform.position.y + Rl.settings.GetRightPaddingY, transform.position.z);
                switch (sideDotType)
                {
                    case SideDotType.turnA:
                        icon = Instantiate(LoadingHelper.THIS.sideDotIcons[(int)SideDotType.turnA], newPosition, Quaternion.identity);
                        break;
                    case SideDotType.turnB:
                        icon = Instantiate(LoadingHelper.THIS.sideDotIcons[(int)SideDotType.turnB], newPosition, Quaternion.identity);
                        break;
                    
                    case SideDotType.moveLine:
                        icon =  Instantiate(LoadingHelper.THIS.sideDotIcons[(int)SideDotType.moveLine], newPosition, Quaternion.identity);
                        break;
                }
                break;
            
            
            case Directions.right:
                newPosition = new Vector3(transform.position.x + Rl.settings.GetLeftPaddingX,
                    transform.position.y + Rl.settings.GetLeftPaddingY, transform.position.z);
                switch (sideDotType)
                {
                    case SideDotType.turnA:
                        icon =  Instantiate(LoadingHelper.THIS.sideDotIcons[(int)SideDotType.turnA], newPosition, Quaternion.identity);
                        break;
                    case SideDotType.turnB:
                        icon =  Instantiate(LoadingHelper.THIS.sideDotIcons[(int)SideDotType.turnB], newPosition, Quaternion.identity);
                        break;
                    case SideDotType.moveLine:
                        icon = Instantiate(LoadingHelper.THIS.sideDotIcons[(int)SideDotType.moveLine], newPosition, Quaternion.identity);
                        break;
                }
            
                icon.transform.localScale = new Vector3( -1, 1,
                    1);
                break;
            
            
            case Directions.top:
                newPosition = new Vector3(transform.position.x - Rl.settings.GetBottomPaddingX,
                    transform.position.y + Rl.settings.GetBottomPaddingY, transform.position.z);
                newRotation.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y,
                    -90);
                switch (sideDotType)
                {
                    case SideDotType.turnA:
                        icon =  Instantiate(LoadingHelper.THIS.sideDotIcons[(int)SideDotType.turnA], newPosition, newRotation );
                        break;
                    
                    case SideDotType.turnB:
                        icon = Instantiate(LoadingHelper.THIS.sideDotIcons[(int)SideDotType.turnB], newPosition, newRotation );
                        break;
                    
                    case SideDotType.moveLine:
                        icon = Instantiate(LoadingHelper.THIS.sideDotIcons[(int)SideDotType.moveLine], newPosition, newRotation );
                        break;
                }
                break;
            
            
            case Directions.bottom:
                newPosition = new Vector3(transform.position.x + Rl.settings.GetBottomPaddingX,
                    transform.position.y - Rl.settings.GetBottomPaddingY, transform.position.z);
                newRotation.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y,
                    -90);
                switch (sideDotType)
                {
                    case SideDotType.turnA:
                        icon =  Instantiate(LoadingHelper.THIS.sideDotIcons[(int)SideDotType.turnA], newPosition, newRotation );
                        break;
                    case SideDotType.turnB:
                        icon = Instantiate(LoadingHelper.THIS.sideDotIcons[(int)SideDotType.turnB], newPosition, newRotation);
                        break;
                    
                    case SideDotType.moveLine:
                        icon = Instantiate(LoadingHelper.THIS.sideDotIcons[(int)SideDotType.moveLine], newPosition, newRotation );
                        break;
                }
                icon.transform.localScale = new Vector3(-1, 1,
                    1);
                break;
        }

       // if (icon != null) icon.GetComponent<IconScript>().sideDotRef = transform.GetComponent<SideDot>();
    }
}