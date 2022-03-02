using System.Collections.Generic;

namespace Battleship.Core.Entities
{
    public class Game
    {
        public Dictionary<int, Board> GameBoards { get; internal set; }
        public int Player1Id = 1;
        public int Player2Id = 2;

        public Game(int? width = null, int? height = null)
        {
            GameBoards = new Dictionary<int, Board>();
            GameBoards.Add(Player1Id, new Board(width, height));
            GameBoards.Add(Player2Id, new Board(width, height));
        }
    }
}
