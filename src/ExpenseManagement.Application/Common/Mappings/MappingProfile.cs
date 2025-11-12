using AutoMapper;
using ExpenseManagement.Application.Budgets.DTOs;
using ExpenseManagement.Application.Expenses.DTOs;
using ExpenseManagement.Application.Subscriptions.DTOs;
using ExpenseManagement.Domain.Entities;

namespace ExpenseManagement.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Expense mappings
        CreateMap<Expense, ExpenseDto>()
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name))
            .ForMember(d => d.CategoryColor, opt => opt.MapFrom(s => s.Category.Color))
            .ForMember(d => d.CategoryIcon, opt => opt.MapFrom(s => s.Category.Icon))
            .ForMember(d => d.CurrencyCode, opt => opt.MapFrom(s => s.Currency != null ? s.Currency.Code : null))
            .ForMember(d => d.CurrencySymbol, opt => opt.MapFrom(s => s.Currency != null ? s.Currency.Symbol : null))
            .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.Tags));

        CreateMap<Tag, TagDto>();

        // Budget mappings
        CreateMap<Budget, BudgetDto>()
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name))
            .ForMember(d => d.CategoryColor, opt => opt.MapFrom(s => s.Category.Color))
            .ForMember(d => d.RemainingAmount, opt => opt.MapFrom(s => s.Amount - s.SpentAmount))
            .ForMember(d => d.Percentage, opt => opt.MapFrom(s => s.Amount > 0 ? (s.SpentAmount / s.Amount) * 100 : 0))
            .ForMember(d => d.IsExceeded, opt => opt.MapFrom(s => s.SpentAmount > s.Amount))
            .ForMember(d => d.IsWarning, opt => opt.MapFrom(s => s.Amount > 0 && (s.SpentAmount / s.Amount) * 100 >= s.WarningThreshold));

        // Subscription mappings
        CreateMap<Subscription, SubscriptionDto>()
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name))
            .ForMember(d => d.CategoryColor, opt => opt.MapFrom(s => s.Category.Color))
            .ForMember(d => d.CurrencyCode, opt => opt.MapFrom(s => s.Currency != null ? s.Currency.Code : null))
            .ForMember(d => d.CurrencySymbol, opt => opt.MapFrom(s => s.Currency != null ? s.Currency.Symbol : null));
    }
}
