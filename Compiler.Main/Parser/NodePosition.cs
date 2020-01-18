namespace Compiler.Main.Parser
{
    internal struct NodePosition
    {
        public int Index { get; private set; }

        public NodePosition(int index)
        {
            Index = index;
        }

        public void Next()
        {
            Index++;
        }

        public override string ToString()
        {
            return $"{{Index:{Index + 1}}}";
        }

    }
}