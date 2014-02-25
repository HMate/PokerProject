using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.PlayerClasses
{
    public class HumanPlayer : BasicPlayer
    {

        public HumanPlayer(Player player)
            : base(player)
        {

        }

        public HumanPlayer(string name)
            : base(name)
        {

        }

        public HumanPlayer()
            : this("Anonymous")
        {

        }

        public override Player Clone()
        {
            return new HumanPlayer(this);
        }

    }
}
