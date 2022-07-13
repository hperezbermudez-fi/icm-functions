namespace ICM.Functions.Infrastructure
{
    public static class Utils
    {
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new();
            StreamWriter writer = new(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
