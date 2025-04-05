public static class StringHelper
{
    public static string Capitalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < words.Length; i++)
        {
            var word = words[i];
            if (word.Length > 0)
            {
                words[i] = char.ToUpper(word[0]) + word.Substring(1).ToLower();
            }
        }

        return string.Join(' ', words);
    }
}