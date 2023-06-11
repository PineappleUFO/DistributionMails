namespace UI.Extenstions
{
    public static class ByteExtensions
    {
        public static ImageSource ImageFromByte(this byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                ImageSource image = ImageSource.FromStream(() => stream);
                return image;
            }
        }
    }
}
