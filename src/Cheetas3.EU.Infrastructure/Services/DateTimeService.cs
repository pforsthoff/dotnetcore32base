using Cheetas3.EU.Application.Interfaces;
using System;

namespace Cheetas3.EU.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}