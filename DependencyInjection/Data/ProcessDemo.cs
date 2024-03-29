﻿namespace DependencyInjection.Data
{
    public class ProcessDemo
    {
        private readonly IDemo _demo;

        public ProcessDemo(IDemo demo)
        {
            _demo = demo;
        }

        public int GetDaysInMonth()
        {
            return _demo.StartupTime.Month switch
            {
                1 => 31,
                2 => 28, //Yes this is a bug
                3 => 31,
                4 => 30,
                5 => 31,
                6 => 30,
                7 => 31,
                8 => 31,
                9 => 30,
                10 => 31,
                11 => 30,
                12 => 31,
                _=> throw new IndexOutOfRangeException()
            };
        }
    }
}
