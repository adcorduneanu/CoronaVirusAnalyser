namespace Corona.Domain.ValueObjects
{
    using System;

    public struct DateValuePair
    {
        public DateValuePair(DateTime dateTime, int value, int previousValue)
        {
            DateTime = dateTime;
            Value = value;
            PreviousValue = previousValue;
        }

        public DateTime DateTime { get; }
        public int Value { get; }
        public int PreviousValue { get; }
        public int Difference => Value - PreviousValue;
    }
}