using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Neat.Tools
{
    public static partial class EditorFunctions
    {
        public const string TEMPLATE_PATH = @"E:\Projects\Unity\NeetDemos\Assets\Neat\Core\Editor\Templates\Template.cs.txt";
        public const string DEFAULT_DIRECTORY = @"E:\Projects\Unity\NeetDemos\Assets\Neat\Core\Editor\Templates\";
        public const string DEFAULT_NAME = "NewScript.cs";

        // https://forum.unity.com/threads/how-to-create-your-own-c-script-template.459977/#post-3533591
        [MenuItem("Neato/Create script using ProjectWindowUtil")]
        public static void CreateScriptAsset()
        {
            var dest = GetDestination();

            // ProjectWindowUtil.CreateScriptAssetFromTemplateFile(PATH, DEST);
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(TEMPLATE_PATH, DEFAULT_NAME);
        }

        [MenuItem("Neato/Create script using AssetDatabase")]
        public static void CreateCustomScript(MenuCommand cmd)
        {
            if (File.Exists(TEMPLATE_PATH))
            {
                var dest = GetDestination();

                // just creates a file...
                File.Copy(TEMPLATE_PATH, Path.Combine(dest, DEFAULT_NAME));
                // AssetDatabase.Refresh();
            }

        }

        static string GetDestination()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var dest = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dest))
                dest = "Assets/";
            return dest;
        }
    }
}
