namespace UnityEditor
{
    public class UniWindowMenus
    {
        [MenuItem("Window/UniExtension/Uni Console")]
        private static void Open()
        {
            UniConsoleWindow.ShowConsoleWindow(true);
        }
    }
}
