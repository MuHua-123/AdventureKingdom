#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;

namespace Kamgam.UIToolkitGlow
{
    public static class VersionHelper
    {
        public static string VersionFileName = "." + typeof(VersionHelper).FullName + ".txt";
        public static Version DefaultVersion = new Version(0, 0, 0, 1);

        public delegate bool UpgradeVersionDelegate(Version oldVersion, Version newVersion);

        public static Version Parse(string version)
        {
            if (string.IsNullOrEmpty(version))
                return DefaultVersion;

            if (Version.TryParse(version, out var versionObj))
                return versionObj;
            else
                return VersionHelper.DefaultVersion;
        }






        public static Version GetInstalledVersion()
        {
            string filePath = getVersionFilePath();

            if (!File.Exists(filePath))
            {
                return DefaultVersion;
            }

            string version = File.ReadAllText(filePath);
            return Parse(version);
        }

        static string getVersionFilePath()
        {
            string dir = getVersionFileDir();

            // fix empty dir path
            if (string.IsNullOrEmpty(dir))
            {
                dir = "Assets/";
            }

            // Fix missing ending slash
            if (!dir.EndsWith("/") && !dir.EndsWith("\\"))
            {
                dir = dir + "/";
            }

            return getBasePath() + dir + VersionFileName;
        }

        static string getVersionFileDir()
        {
            return Installer.AssetRootPath.Trim();
        }

        static string getBasePath()
        {
            // Unity Editor: <path to project folder>/Assets
            // See: https://docs.unity3d.com/ScriptReference/Application-dataPath.html
            string basePath = Application.dataPath.Replace("/Assets", "/");
            basePath = basePath.Replace("\\Assets", "\\");
            return basePath;
        }
    }
}
#endif