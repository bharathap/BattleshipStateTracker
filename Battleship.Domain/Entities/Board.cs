using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Battleship.Core.Exceptions;

namespace Battleship.Core.Entities
{
    public class Board
    {
        public Board(int? width = null, int? height = null)
        {
            State = new Guid?[width ?? Common.DefaultBoardWidth, width ?? Common.DefaultBoardHeight];
            Ships = new List<Ship>();
        }

        public List<Ship> Ships { get; set; }
        public Guid?[,] State { get; internal set; }

        public bool AddShip(Point startPoint, int length, Orientation orientation)
        {
            var ship = new Ship(startPoint, length, orientation);
            var shipCoordinates = new List<Point>();

            for (int i = 0; i < length; i++)
            {
                var coordinate = new Point(startPoint.X + (orientation == Orientation.Horizontal ? i : 0),
                                            startPoint.Y + (orientation == Orientation.Horizontal ? 0 : i));
                shipCoordinates.Add(coordinate);

                if (State[coordinate.X, coordinate.Y] != null)
                {
                    throw new BattleshipValidationException($"Cell ({coordinate.X},{coordinate.Y}) is already occupied.");
                }
            }

            foreach (var coordinate in shipCoordinates)
            {
                State[coordinate.X, coordinate.Y] = ship.Id;
            }
            Ships.Add(ship);
            return true;
        }

        public Guid? Attack(Point coordinate)
        {
            var shipId = State[coordinate.X, coordinate.Y];
            if (shipId == null)
            {
                return shipId;
            }

            State[coordinate.X, coordinate.Y] = null;
            var ship = Ships.FirstOrDefault(s => s.Id == shipId);
            ship.Hit();
            return shipId;
        }
    }
}
