using Diablo2Editor;
using Mono.Cecil;
using System.IO;
using System;
using UnityEngine;
using System.Linq;

namespace Diablo2Editor
{
    public enum TextureLoadMode
    {
        All,
        Albedo,
        None
    }
    public class DependencyTexture : LevelPresetDependency
    {
        private bool NeedLoadTexture(string path)
        {
            TextureLoadMode mode = EditorMain.Settings().common.textureLoadMode;
            switch (mode)
            {
                default:
                case TextureLoadMode.All:
                         return true;
                    case TextureLoadMode.Albedo:
                         return path.EndsWith("_alb.texture");    
                    case TextureLoadMode.None:
                         return false;
            }
        }
        public override void LoadResource()
        {
            if(NeedLoadTexture(path))
            {
                LoadTexture();
            }
        }

        public void LoadTexture()
        {
            PathMapper mapper = EditorMain.Settings().paths;
            string abs_path = mapper.GetAbsolutePath(this.path);
            if (File.Exists(abs_path))
            {
                var bytes = File.ReadAllBytes(abs_path);
                var formatVal = BitConverter.ToInt16(bytes, 4);
                var width = BitConverter.ToInt32(bytes, 8);
                var height = BitConverter.ToInt32(bytes, 0xC);
                var mipLevels = BitConverter.ToInt32(bytes, 0x1C);
                var startFirstSection = BitConverter.ToInt32(bytes, 0x28);
                var format = formatVal == 31 ? TextureFormat.RGBA32 : formatVal <= 58 ? TextureFormat.DXT1 : formatVal <= 62 ? TextureFormat.DXT5 : TextureFormat.BC4;
                Texture2D tex = new Texture2D(width, height, format, mipLevels, mipLevels > 0);
                tex.LoadRawTextureData(bytes.Skip(0x28 + startFirstSection).ToArray());
                tex.Apply();
                resource = tex;
            }
        }
    }
}