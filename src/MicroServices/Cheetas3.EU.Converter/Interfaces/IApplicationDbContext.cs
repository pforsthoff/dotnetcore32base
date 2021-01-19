﻿using Cheetas3.EU.Converter.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Converter.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Job> Jobs { get; set; }
        DbSet<Slice> Slices { get; set; }
        DbSet<File> Files { get;set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
