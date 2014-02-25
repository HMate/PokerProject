using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.CardClasses
{
    [Serializable]
    public class CardDeckEmptyException : InvalidOperationException
    {
        public CardDeckEmptyException() { }
        public CardDeckEmptyException(string message) : base(message) { }
        public CardDeckEmptyException(string message, Exception inner) : base(message, inner) { }
        protected CardDeckEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
