using UnityEngine;

public class DS1ObjectLabel
{
    public int rx, ry; // upper/left corner of the label, relative to the sub-cell
                // (in pixels, at zoom of 1:1)
    public int w, h;   // width & height (pixels)
    public int x0, y0; // pixels position on screen
    public int flags;

    // for moving
    public int old_rx;
    public int old_ry;
}
