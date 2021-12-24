namespace Application.Common.Utils
{
    using System;
    public static class Round
    {
        public enum Type
        {
            Nearest,
            Up,
            Down
        }

        /// <summary>
        /// Rounds a number to the nearest X
        /// </summary>
        /// <param name="value">The value to round</param> 
        /// <param name="stepAmount">The amount to round the value by</param>
        /// <param name="type">The type of rounding to perform</param>
        /// <returns>The value rounded by the step amount and type</returns>
        public static decimal Amount(decimal value, decimal stepAmount, Type type = Type.Nearest)
        {
            var inverse = 1 / stepAmount;
            var dividend = value * inverse;
            switch (type)
            {
                case Type.Nearest:
                    dividend = Math.Round(dividend);
                    break;
                case Type.Up:
                    dividend = Math.Ceiling(dividend);
                    break;
                case Type.Down:
                    dividend = Math.Floor(dividend);
                    break;
                default:
                    throw new ArgumentException($"Unknown type: {type}", nameof(type));
            }
            var result = dividend / inverse;
            return result;
        }
    }
}
