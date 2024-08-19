﻿using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);
    Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T,TResult> spec);

    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T,TResult> spec);
    
    Task<int> CountAsync(ISpecification<T> spec);


    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    bool Exists(int id);
}
