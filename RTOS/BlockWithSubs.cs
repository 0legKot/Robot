using System.Collections.Generic;

namespace RTOS
{

    public class BlockWithSubs
    {
        public string Simple { get; set; } = "";

        public bool IsControl { get; set; }

        public List<BlockWithSubs> Blocks { get; set; } = new List<BlockWithSubs>();

        public BlockWithSubs(string text, bool isControl, List<BlockWithSubs> subBlocks)
        {
            Simple = text;
            IsControl = isControl;
            Blocks = subBlocks;
        }

        public BlockWithSubs()
        {
        }
    }

}