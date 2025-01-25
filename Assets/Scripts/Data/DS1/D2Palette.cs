using Mono.Cecil;
using System.IO;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Pallete storage for D2 graphics;
 */
public class D2Palette
{
    const int ACT_MAX = 5;
    const int PALLETE_SIZE = 256;
    const string PAL_FILE_NAME = "pal.pl2";
    const string PAL_PATH_PREFIX = "act";
    NativeArray<Color>[] palettes;
    NativeArray<Color> default_palette;
    public D2Palette()
    {
        LoadDefaultPalette();
    }
    ~D2Palette()
    {
        CleanUp();
        default_palette.Dispose();
    }

    public void LoadPalleteFiles()
    {
        CleanUp();
        palettes = new NativeArray<Color>[ACT_MAX];
        for (int i = 0; i < ACT_MAX; ++i)
        {
            palettes[i] = LoadPalleteForAct(i);
        }
    }

    public NativeArray<Color> GetPaletteForAct(string act)
    {
        // expecting string "actN" as input
        int actIndex = int.Parse(act.Replace("act", ""));
        if (actIndex > 0)
        {
            // turning to sero-based index
            actIndex--;
            if (actIndex >= 0 && actIndex < ACT_MAX)
            {
                return palettes[actIndex];
            }
        }
        Debug.LogError("[GetPaletteForAct] Palette for " + act + " is not loaded");
        return default_palette;
    }

    private void CleanUp()
    {
        if (palettes != null && palettes.Length > 0)
        {
            for (int i = 0; i < palettes.Length; ++i)
            {
                palettes[i].Dispose();
            }
        }
    }

    private void LoadDefaultPalette()
    {
        default_palette = new NativeArray<Color>(PALLETE_SIZE, Allocator.Persistent);
        for (int i = 0; i < PALLETE_SIZE; i++)
        {
            default_palette[i] = Color.black;
        }
    }

    private NativeArray<Color> LoadPalleteForAct(int act)
    {
        NativeArray<Color> result = new NativeArray<Color>(PALLETE_SIZE, Allocator.Persistent);
        string paletteFolder = EditorMain.Settings().paths.GetPalettesPath();
        string pathToPalette = PAL_PATH_PREFIX + (act + 1);
        string fullPath = Path.Combine(paletteFolder, pathToPalette, PAL_FILE_NAME);

        if (File.Exists(fullPath))
        {

            var content = File.ReadAllBytes(fullPath);
            for (int i = 0; i < PALLETE_SIZE; i++)
            {
                int ridx = 4 * i;
                byte r = content[ridx];
                byte g = content[ridx + 1];
                byte b = content[ridx + 2];
                /*
                 * Gamma correction. TODO : implement gamma
                    r = glb_ds1edit.d2_pal[pal_idx][ridx];
                    g = glb_ds1edit.d2_pal[pal_idx][ridx + 1];
                    b = glb_ds1edit.d2_pal[pal_idx][ridx + 2];
                    r = glb_ds1edit.gamma_table[glb_ds1edit.cur_gamma][r];
                    g = glb_ds1edit.gamma_table[glb_ds1edit.cur_gamma][g];
                    b = glb_ds1edit.gamma_table[glb_ds1edit.cur_gamma][b];
                */
                r = (byte)(r >> 2);
                g = (byte)(g >> 2);
                b = (byte)(b >> 2);

                Color color = new Color();
                color.r = (float)r / 255;
                color.g = (float)g / 255;
                color.b = (float)b / 255;

                result[i] = color;
            }
        }


        return result;
    }

    
}
