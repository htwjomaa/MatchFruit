using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CollectionStyleSet
{
    public CollectionStyle CollectionStyle;
    public Sprite CollenctionStyleImage;

    public CollectionStyleSet(CollectionStyle collectionStyle, Sprite collenctionStyleImage)
    {
        CollectionStyle = collectionStyle;
        CollenctionStyleImage = collenctionStyleImage;
    }
}
