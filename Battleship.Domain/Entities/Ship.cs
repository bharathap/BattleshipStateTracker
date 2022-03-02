using System;
using System.Drawing;

namespace Battleship.Core.Entities
{
    public class Ship
    {
        private int _health;
        private Point _startPoint;

        public Ship(Point startPoint, int length, Orientation orientation)
        {
            Id = Guid.NewGuid();
            Length = length;
            Orientation = orientation;
            _health = length;
            _startPoint = startPoint;
        }

        public Guid Id { get; private set; }
        public int Length { get; set; }
        public Orientation Orientation { get; set; }

        public bool IsSunk => _health <= 0;
        public int Hit()
        {
            return _health--;
        }
    }
}
