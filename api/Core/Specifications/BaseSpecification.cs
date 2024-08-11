using Core.Interfaces;
using System.Linq.Expressions;

namespace Core.Specifications;

public class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; }

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public bool IsDistinct { get; private set; }

    public int Take { get; private set; }

    public int Skip {  get; private set; }

    public bool IsPagningEnabled {  get; private set; }

    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public BaseSpecification(){}

    protected void AddOrderBy(Expression<Func<T, object>>? orderByExpression) => OrderBy = orderByExpression;
    protected void AddOrderByDescending(Expression<Func<T, object>>? orderByDescendingExpression) => OrderByDescending = orderByDescendingExpression;
    protected void AddDistinct() => IsDistinct = true;

    protected void AddPaging(int skip, int take) 
    {
        Skip = skip;
        Take = take;
        IsPagningEnabled = true;
    }

    public IQueryable<T> ApplyCriteria(IQueryable<T> query)
    {
        if(Criteria is not null) 
        {
            query = query.Where(Criteria);
        }
        return query;
    }
}

public class BaseSpecification<T, TResult> : BaseSpecification<T>, ISpecification<T, TResult>
{
    public Expression<Func<T, TResult>>? Select { get; private set; }
    public BaseSpecification(Expression<Func<T, bool>> criteria) : base(criteria)
    {
    }

    public BaseSpecification()
    {
        
    }

    protected void AddSelect(Expression<Func<T, TResult>> selectExpression) => Select = selectExpression;

}
