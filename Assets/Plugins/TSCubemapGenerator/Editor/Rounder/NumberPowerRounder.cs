using System;
using UnityEngine;

namespace TSCubemapGenerator
{
    public class NumberPowerRounder : IRounder
    {
        public NumberPowerRounder(int number)
        {
            if (number <= 0)
            {
                throw new CubemapGeneratorException("Invalid a number. You should use 1 or greater");
            }

            Number = number;
        }

        public int Number { get; }

        public int Round(int value)
        {
            if (value < Number)
            {
                return Number;
            }

            double p = Math.Round(Math.Log(value) / Math.Log(Number));
            int v = (int)Math.Pow(Number, p);

            return v;
        }
    }
}