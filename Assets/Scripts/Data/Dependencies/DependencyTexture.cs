using Diablo2Editor;
using Mono.Cecil;
using System.IO;
using System;
using UnityEngine;
using System.Linq;

namespace Diablo2Editor
{
    public class DependencyTexture : LevelPresetDependency
    {
        public override void LoadResource()
        {
            string abs_path = PathMapper.GetAbsolutePath(this.path);
            if (File.Exists(abs_path))
            {
                var bytes = File.ReadAllBytes(path);
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
            else
            {
                Debug.LogError("Texture is not found " + path);
            }
        }
    }
}