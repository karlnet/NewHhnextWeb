using hhnextWeb.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace hhnextWeb.Data
{
    public static class EFHelper
    {

        public static IQueryable<T> GetAll<T>(this AppDbContext db) where T : class
        {
            DbSet<T> dbSet = db.Set<T>();
            IQueryable<T> queryable = dbSet.AsQueryable<T>();
            return queryable;
        }
        public static T GetByKey<T>(this DbContext db, params object[] keyValues) where T : class
        {
            DbSet<T> dbSet = db.Set<T>();
            T entity = dbSet.Find(keyValues);
            return entity;
        }
        public static bool IsExists<T>(this AppDbContext db, Expression<Func<T, bool>> predicate) where T : class
        {
            DbSet<T> dbSet = db.Set<T>();
            bool isExists = dbSet.Where(predicate).Any();
            //bool isExists = dbSet.Any(predicate); //TODO: dbSet 中的元素必须全部满足 predicate 才为 true?
            return isExists;
        }


        public static IQueryable<T> Where<T, TKey>(this DbContext db, Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, bool isAsc = true, params string[] joinProperties) where T : class
        {
            DbSet<T> dbSet = db.Set<T>();
            DbQuery<T> dbQuery = dbSet;
            //添加要连接查询的导航属性
            if (joinProperties.Length > 0)
            {
                foreach (string joinName in joinProperties)
                {
                    dbQuery = dbQuery.Include(joinName);
                }
            }
            //IQueryable<T> query = null;
            var query = dbQuery.Where(predicate);
            return isAsc ?
                query.OrderBy(keySelector) :
                query.OrderByDescending(keySelector);
        }
        public static IQueryable<T> Where<T>(this AppDbContext db, Expression<Func<T, bool>> predicate) where T : class
        {
            DbSet<T> dbSet = db.Set<T>();
            IQueryable<T> query = dbSet.Where(predicate);
            return query;
        }

        public static bool Insert<T>(this AppDbContext db, T entity, bool saveChanges = true) where T : class
        {

            DbSet<T> dbSet = db.Set<T>();
            dbSet.Attach(entity);
            db.Entry(entity).State = EntityState.Added;
            int effect = 0;
            try
            {
                effect = db.SaveChanges();
            }
            catch (Exception e)
            {
            }
            return effect == 1;

        }
        public static void InsertOrUpdate<T>(this AppDbContext db, T entity, Expression<Func<T, bool>> predicate, bool saveChanges = true) where T : class
        {
            bool isExist = db.IsExists<T>(predicate);
            if (isExist)
            {

                db.Update(entity, saveChanges);
            }
            else
            {
                db.Insert(entity, saveChanges);
            }
        }
        public static int? DeleteByKey<T>(this DbContext db, bool saveChanges = true, params object[] keyValues) where T : class
        {
            DbSet<T> dbSet = db.Set<T>();
            T entity = dbSet.Find(keyValues);
            if (entity != null)
            {
                //2015-04-22 Tony 测试记录：
                //执行 ``TEntity = System.Data.Entity.DbSet<TEntity>.Remove(TEntity entity)`` 之后 ``TEntity == entity`` 为 true。
                //因此下一行代码中的 entityReturn 是不必要的。
                T entityReturn = dbSet.Remove(entity);
                if (saveChanges)
                {
                    int effect = db.SaveChanges();
                    return effect;
                }
            }
            return null;
        }
        public static void Delete<T>(this DbContext db, Expression<Func<T, bool>> predicate, bool saveChanges = true) where T : class
        {
            DbSet<T> dbSet = db.Set<T>();
            T entity = dbSet.First(predicate);
            if (entity != null)
            {
                dbSet.Remove(entity);
                if (saveChanges)
                {
                    db.SaveChanges();
                }
            }
        }

        public static bool? Update<T>(this AppDbContext db, T entity, bool saveChanges = true) where T : class
        {
            DbSet<T> dbSet = db.Set<T>();
            dbSet.Attach(entity);
            db.Entry<T>(entity).State = EntityState.Modified;
            if (saveChanges)
            {
                int effect = db.SaveChanges();

                return effect == 1;
            }
            return null;
        }


    }
}