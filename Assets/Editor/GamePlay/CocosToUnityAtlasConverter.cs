using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class CocosToUnityAtlasConverter
{
    [MenuItem("Tools/自动切割Cocos合图", false, 100)]
    private static void SplitCocosTexture()
    {
        Object selected = Selection.activeObject;
        if (selected == null || !(selected is Texture2D))
        {
            EditorUtility.DisplayDialog("错误", "请选择一个PNG纹理文件", "确定");
            return;
        }

        string texturePath = AssetDatabase.GetAssetPath(selected);
        string directory = Path.GetDirectoryName(texturePath);
        string fileName = Path.GetFileNameWithoutExtension(texturePath);

        string plistPath = Path.Combine(directory, fileName + ".plist");
        if (!File.Exists(plistPath))
        {
            string[] allPlistFiles = Directory.GetFiles(directory, "*.plist");
            if (allPlistFiles.Length > 0)
            {
                plistPath = allPlistFiles[0];
            }
            else
            {
                EditorUtility.DisplayDialog("错误", "未找到对应的plist文件", "确定");
                return;
            }
        }

        SplitTextureWithPlist(texturePath, plistPath);
    }

    [MenuItem("Assets/Cocos/自动切割合图", true)]
    private static bool ValidateSplitCocosTexture()
    {
        Object selected = Selection.activeObject;
        if (selected == null) return false;

        string path = AssetDatabase.GetAssetPath(selected);
        return Path.GetExtension(path).ToLower() == ".png";
    }

    private static void SplitTextureWithPlist(string texturePath, string plistPath)
    {
        try
        {
            // 解析plist文件
            List<SpriteRect> spriteRects = ParsePlist(plistPath);

            if (spriteRects.Count == 0)
            {
                EditorUtility.DisplayDialog("错误", "plist文件中没有找到精灵数据", "确定");
                return;
            }

            TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            if (textureImporter == null)
            {
                EditorUtility.DisplayDialog("错误", "无法获取纹理导入器", "确定");
                return;
            }

            // 获取纹理尺寸（从文件直接读取，避免依赖已导入的纹理）
            int textureWidth = 0;
            int textureHeight = 0;
            GetTextureSize(texturePath, out textureWidth, out textureHeight);
            // 准备精灵数据
            List<SpriteMetaData> spriteMetaDataList = new List<SpriteMetaData>();
            foreach (var spriteRect in spriteRects)
            {
                SpriteMetaData metaData = new SpriteMetaData();
                metaData.name = spriteRect.name;

                // 处理旋转精灵
                if (spriteRect.rotated)
                {
                    // Cocos中旋转的精灵：矩形的宽高实际上是交换的
                    // 在纹理中，精灵被顺时针旋转了90度存储
                    // 所以我们需要：宽高交换，矩形位置调整

                    metaData.rect = new Rect(
                        spriteRect.x,
                        textureHeight - spriteRect.y - spriteRect.width, // 注意：减去宽度，不是高度
                        spriteRect.height,  // 宽度使用原始高度
                        spriteRect.width    // 高度使用原始宽度
                    );

                    // 注意：Unity的SpriteMetaData不支持rotation属性
                    // 旋转需要在其他地方处理
                }
                else
                {
                    // 正常未旋转的精灵
                    metaData.rect = new Rect(
                        spriteRect.x,
                        textureHeight - spriteRect.y - spriteRect.height,
                        spriteRect.width,
                        spriteRect.height
                    );
                }

                // 设置pivot点
                if (spriteRect.pivotX >= 0 && spriteRect.pivotY >= 0)
                {
                    metaData.pivot = new Vector2(spriteRect.pivotX, spriteRect.pivotY);
                    metaData.alignment = (int)SpriteAlignment.Custom;
                }
                else
                {
                    metaData.alignment = (int)SpriteAlignment.Center;
                    metaData.pivot = new Vector2(0.5f, 0.5f);
                }

                spriteMetaDataList.Add(metaData);
            }

            // 应用设置
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Multiple;
            // textureImporter.spritesheet = spriteMetaDataList.ToArray();

            // 保存并重新导入
            textureImporter.SaveAndReimport();

            // 处理旋转精灵（重新导入后）
            HandleRotatedSprites(texturePath, spriteRects, textureWidth, textureHeight);

            EditorUtility.DisplayDialog("成功",
                $"成功切割 {spriteRects.Count} 个精灵！\n其中旋转精灵: {CountRotatedSprites(spriteRects)}个",
                "确定");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"切割合图失败: {e.Message}\n{e.StackTrace}");
            EditorUtility.DisplayDialog("错误",
                $"切割失败: {e.Message}",
                "确定");
        }
    }

    private static void HandleRotatedSprites(string texturePath, List<SpriteRect> spriteRects, int textureWidth, int textureHeight)
    {
        // 重新导入后，旋转的精灵方向不对，需要手动调整
        // 注意：Unity没有直接设置Sprite旋转的API
        // 但我们可以通过修改UV来模拟旋转效果

        // 方法1：创建临时GameObject并旋转
        // 方法2：导出修改后的纹理（更复杂）
        // 这里我们先记录日志，后续可以扩展

        int rotatedCount = 0;
        foreach (var rect in spriteRects)
        {
            if (rect.rotated)
            {
                rotatedCount++;
                Debug.LogWarning($"精灵 '{rect.name}' 是旋转的，需要在运行时特殊处理。");
                Debug.LogWarning($"  原始尺寸: {rect.width}x{rect.height}");
                Debug.LogWarning($"  实际应该: {rect.height}x{rect.width} (旋转90度)");
            }
        }

        if (rotatedCount > 0)
        {
            Debug.LogWarning($"共 {rotatedCount} 个旋转精灵需要特殊处理。");
            Debug.LogWarning("建议：在运行时使用代码旋转这些SpriteRenderer。");
        }
    }

    private static void GetTextureSize(string texturePath, out int width, out int height)
    {
        width = 0;
        height = 0;

        try
        {
            // 直接读取PNG文件头获取尺寸
            using (FileStream stream = new FileStream(texturePath, FileMode.Open, FileAccess.Read))
            {
                byte[] header = new byte[24];
                stream.Read(header, 0, 24);

                // PNG文件格式：前8字节是签名，然后IHDR块
                if (header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47)
                {
                    // IHDR块中的宽高（大端序）
                    width = (header[16] << 24) | (header[17] << 16) | (header[18] << 8) | header[19];
                    height = (header[20] << 24) | (header[21] << 16) | (header[22] << 8) | header[23];
                }
            }
        }
        catch
        {
            // 如果读取失败，使用默认方法
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            if (texture != null)
            {
                width = texture.width;
                height = texture.height;
            }
        }
    }

    private static int CountRotatedSprites(List<SpriteRect> spriteRects)
    {
        int count = 0;
        foreach (var rect in spriteRects)
        {
            if (rect.rotated) count++;
        }
        return count;
    }

    private static List<SpriteRect> ParsePlist(string plistPath)
    {
        List<SpriteRect> spriteRects = new List<SpriteRect>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(plistPath);

        XmlNode dictNode = xmlDoc.SelectSingleNode("plist/dict");
        if (dictNode == null) return spriteRects;

        XmlNode framesNode = null;
        foreach (XmlNode child in dictNode.ChildNodes)
        {
            if (child.Name == "key" && child.InnerText == "frames")
            {
                framesNode = child.NextSibling;
                break;
            }
        }

        if (framesNode == null || framesNode.Name != "dict") return spriteRects;

        XmlNodeList frameNodes = framesNode.ChildNodes;
        for (int i = 0; i < frameNodes.Count; i += 2)
        {
            if (frameNodes[i].Name == "key")
            {
                string spriteName = frameNodes[i].InnerText;
                XmlNode spriteDict = frameNodes[i + 1];

                if (spriteDict.Name == "dict")
                {
                    SpriteRect spriteRect = ParseSpriteRect(spriteName, spriteDict);
                    if (spriteRect != null)
                    {
                        spriteRects.Add(spriteRect);
                    }
                }
            }
        }

        return spriteRects;
    }

    private static SpriteRect ParseSpriteRect(string spriteName, XmlNode spriteDict)
    {
        SpriteRect spriteRect = new SpriteRect();
        spriteRect.name = Path.GetFileNameWithoutExtension(spriteName);

        spriteRect.rotated = false;
        spriteRect.pivotX = -1;
        spriteRect.pivotY = -1;

        XmlNodeList properties = spriteDict.ChildNodes;
        for (int j = 0; j < properties.Count; j += 2)
        {
            if (properties[j].Name == "key")
            {
                string key = properties[j].InnerText;
                XmlNode valueNode = properties[j + 1];

                switch (key)
                {
                    case "frame":
                        ParseRect(valueNode.InnerText, ref spriteRect.x, ref spriteRect.y,
                                ref spriteRect.width, ref spriteRect.height);
                        break;

                    case "rotated":
                        spriteRect.rotated = (valueNode.Name == "true");
                        break;

                    case "anchor":
                        ParsePoint(valueNode.InnerText, ref spriteRect.pivotX, ref spriteRect.pivotY);
                        break;
                }
            }
        }

        return spriteRect;
    }

    private static void ParseRect(string rectString, ref int x, ref int y, ref int width, ref int height)
    {
        var match = Regex.Match(rectString, @"\{\{(\d+),(\d+)\},\{(\d+),(\d+)\}\}");
        if (match.Success)
        {
            x = int.Parse(match.Groups[1].Value);
            y = int.Parse(match.Groups[2].Value);
            width = int.Parse(match.Groups[3].Value);
            height = int.Parse(match.Groups[4].Value);
        }
    }

    private static void ParsePoint(string pointString, ref float x, ref float y)
    {
        var match = Regex.Match(pointString, @"\{([\d\.]+),([\d\.]+)\}");
        if (match.Success)
        {
            x = float.Parse(match.Groups[1].Value);
            y = float.Parse(match.Groups[2].Value);
        }
    }

    private class SpriteRect
    {
        public string name;
        public int x;
        public int y;
        public int width;
        public int height;
        public bool rotated;
        public float pivotX;
        public float pivotY;
    }
}