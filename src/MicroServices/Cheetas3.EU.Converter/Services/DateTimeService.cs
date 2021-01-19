using Cheetas3.EU.Converter.Interfaces;
using System;

namespace Cheetas3.EU.Converter.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}