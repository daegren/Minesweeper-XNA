using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper_XNA
{
    public class Mine
    {
        private bool isMine;
        private bool isUncovered;

        public bool IsUncovered { get { return isUncovered; } }
        public bool IsMine { get { return isMine; } }

        public Mine(bool isMine)
        {
            isUncovered = false;
            this.isMine = isMine;
        }
    }
}
