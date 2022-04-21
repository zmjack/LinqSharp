namespace LinqSharp.EFCore.Identifiers
{
    public struct IdentifierPair
    {
        public char LeftChar;
        public char RightChar;

        public IdentifierPair(char leftChar, char rightChar)
        {
            LeftChar = leftChar;
            RightChar = rightChar;
        }

        public string Wrap(string content) => $"{LeftChar}{content}{RightChar}";

    }
}
