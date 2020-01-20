using System;

namespace AdventOfCode2019
{
    public class ResultAttribute : Attribute
    {
        public object Result { get; }

        public ResultAttribute(object result)
        {
            Result = result;
        }
    }
}