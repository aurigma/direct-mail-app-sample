using System;
using System.Linq;
using System.Linq.Expressions;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Extensions;

public static class FilterExtension
{
    public static Expression<Func<ProductDal, bool>> GetPredicate(this ProductFilter filter)
    {
        Expression<Func<ProductDal, bool>> expr = request => true;

        if (filter.CategoryId.HasValue)
        {
            expr = expr.AndAlso(request => request.CategoryId == filter.CategoryId);
        }

        return expr;
    }

    public static Expression<Func<CampaignDal, bool>> GetPredicate(this CampaignFilter filter)
    {
        Expression<Func<CampaignDal, bool>> expr = request => true;
        if (filter.Type.HasValue)
        {
            expr = expr.AndAlso(request => request.Type == filter.Type.Value);
        }

        if (filter.CompanyId.HasValue)
        {
            expr = expr.AndAlso(request => request.CompanyId == filter.CompanyId);
        }

        return expr;
    }

    public static Expression<Func<LineItemDal, bool>> GetPredicate(this LineItemFilter filter)
    {
        Expression<Func<LineItemDal, bool>> expr = request => true;

        if (filter.ProductId.HasValue)
        {
            expr = expr.AndAlso(request => request.ProductId == filter.ProductId);
        }

        if (filter.CampaignId.HasValue)
        {
            expr = expr.AndAlso(request => request.CampaignId == filter.CampaignId);
        }

        return expr;
    }

    public static Expression<Func<RecipientListDal, bool>> GetPredicate(
        this RecipientListFilter filter
    )
    {
        Expression<Func<RecipientListDal, bool>> expr = request => true;

        if (filter.CampaignId.HasValue)
        {
            expr = expr.AndAlso(request =>
                request
                    .RecipientListCampaigns.Select(rc => rc.CampaignId)
                    .Contains(filter.CampaignId.Value)
            );
        }

        return expr;
    }

    public static Expression<Func<JobDal, bool>> GetPredicate(this JobFilter filter)
    {
        Expression<Func<JobDal, bool>> expr = request => true;

        if (filter.LineItemId.HasValue)
        {
            expr = expr.AndAlso(request => request.LineItemId == filter.LineItemId.Value);
        }

        return expr;
    }
}
