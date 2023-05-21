namespace UI.Extenstions
{
    /// <summary>
    /// Расширение методов
    /// </summary>
    public static class FileExtenstions
    {
        /// <summary>
        /// Получить все PDF в папке
        /// </summary>
        /// <param name="folder">Папка</param>
        public static List<FileInfo> GetAllPdfInFolder(this DirectoryInfo folder)
        {
            var list = new List<FileInfo>();
            foreach (FileInfo file in folder.GetFiles())
            {
                if (file.Extension.Equals(".pdf", StringComparison.CurrentCultureIgnoreCase))
                {
                    list.Add(file);
                }
            }
            return list;
        }

        /// <summary>
        /// Проверки на 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool CheckFilePath(this string filePath)
        {
            if (filePath == null || string.IsNullOrWhiteSpace(filePath) || !new FileInfo(filePath).Exists) return false;
            return true;
        }
    }
}
