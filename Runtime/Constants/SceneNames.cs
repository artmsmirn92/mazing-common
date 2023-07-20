namespace mazing.common.Runtime.Constants
{
    public static class SceneNames
    {
        public const string Preload     = "_preload";
        public const string Level       = "Level";
        public const string Prototyping = "Prot";
        public const string Menu        = "Menu";
        
        public static string GetScenesPath()
        {
            return "Assets\\Scenes\\";
        }
    }
}