﻿using BlogSystem.DataAccess.Data;
using BlogSystem.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        // The following Variable is going to hold the DbSet Entity
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;

            // This will set our DbSet to the particular instance of the class that is calling our repository.
            dbSet = _context.Set<T>();
        }


        public async Task<T> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<T> Delete(T entity)
        {
            dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }
    }
}
