using System.IO;
using Unity.Collections;
using UnityEngine;
namespace Diablo2Editor
{
    /*
     * Pallete storage for D2 graphics;
     */
    public class D2Palette
    {
        const int GAMMA_LEVELS = 41;
        const int DEFAULT_GAMMA = 22;
        const int PALLETE_SIZE = 256;
        const string PAL_FILE_NAME = "pal.pl2";
        const string PAL_PATH_PREFIX = "act";
        const string GAMMA_FILE = "Assets\\Resources\\gamma.dat";
        byte[][] gamma_table;
        public int cur_gamma = DEFAULT_GAMMA;
        NativeArray<Color>[] palettes;
        NativeArray<Color> default_palette;
        public D2Palette()
        {
            LoadDefaultPalette();
            LoadGamma();
        }
        ~D2Palette()
        {
            CleanUp();
            default_palette.Dispose();
        }

        public void LoadPalleteFiles()
        {
            CleanUp();
            palettes = new NativeArray<Color>[DS1Consts.ACT_MAX];
            for (int i = 0; i < DS1Consts.ACT_MAX; ++i)
            {
                palettes[i] = LoadPalleteForAct(i);
            }
        }

        private void LoadGamma()
        {
            string pathToGammaFile = Path.GetFullPath(GAMMA_FILE);
            if (File.Exists(pathToGammaFile))
            {
                byte[] content = File.ReadAllBytes(pathToGammaFile);
                gamma_table = new byte[GAMMA_LEVELS][];
                for (int i = 0; i < GAMMA_LEVELS; ++i)
                {
                    gamma_table[i] = new byte[PALLETE_SIZE];
                    for (int j = 0; j < PALLETE_SIZE; j++)
                    {
                        gamma_table[i][j] = content[i * PALLETE_SIZE + j];
                    }
                }
            }
        }

        public NativeArray<Color> GetPaletteForAct(int act)
        {
            if (act > 0)
            {
                // turning to sero-based index
                int actIndex = act - 1;
                if (actIndex >= 0 && actIndex < DS1Consts.ACT_MAX)
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

                    if (gamma_table != null)
                    {
                        r = gamma_table[cur_gamma][r];
                        g = gamma_table[cur_gamma][g];
                        b = gamma_table[cur_gamma][b];
                    }

                    Color color = new Color();
                    color.r = (float)r / (PALLETE_SIZE - 1);
                    color.g = (float)g / (PALLETE_SIZE - 1);
                    color.b = (float)b / (PALLETE_SIZE - 1);
                    color.a = 1.0f;

                    result[i] = color;
                }
            }
            return result;
        }
    }
}
