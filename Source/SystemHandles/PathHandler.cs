namespace OpenVikings.SystemHandles
{
    internal class PathHandler
    {
        // sub_4ec0ea(
        internal static string GetGamePath()
        {
            return Environment.ProcessPath!;
        }

        // sub_4065e2(; logs, Saves, mapshots, screenshots, UserMaps, 
        internal static string GetFolderPath(string folderName)
        {
            return Path.Combine(Environment.ProcessPath!, folderName);
        }
    }
}