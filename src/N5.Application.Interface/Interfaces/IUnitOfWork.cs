﻿using N5.Domain;

namespace N5.Application.Servicios.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Permiso> PermisoRepository { get; }
        Task<int> SaveChangesAsync();
    }
}