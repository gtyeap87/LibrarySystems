using FluentValidation;
using Library.Dto.Requests;
using Library.Features.Book.Commands;
using Library.Features.LoanBook.Commands;
using Library.Features.Member.Commands;
using Library.Models;

namespace Library.Validators
{
    public static class Extensions
    {
        public static void AddCustomValidators(this IServiceCollection services)
        {
            #region Book

            // Generic command validators for Book
            services.AddScoped<IValidator<CreateBookCommand>>(sp =>
             new GenericCommandValidator<CreateBookCommand, BookRequest, Guid>(
                 sp.GetRequiredService<IValidator<BookRequest>>(),
                 x => x.Request));

            services.AddScoped<IValidator<CreateBooksCommand>>(sp =>
                new GenericCommandValidator<CreateBooksCommand, BooksRequest>(
                    sp.GetRequiredService<IValidator<BooksRequest>>(),
                    x => x.Request));

            services.AddScoped<IValidator<CreateBulkBooksCommand>>(sp =>
                new GenericCommandValidator<CreateBulkBooksCommand, BooksRequest>(
                    sp.GetRequiredService<IValidator<BooksRequest>>(),
                    x => x.Request));

            services.AddScoped<IValidator<UpdateBookCommand>>(sp =>
                new GenericCommandValidator<UpdateBookCommand, BookRequest, Book>(
                    sp.GetRequiredService<IValidator<BookRequest>>(),
                    x => x.Request));

            services.AddScoped<IValidator<UpdateBookStockCommand>>(sp =>
             new GenericCommandValidator<UpdateBookStockCommand, BookStock, BookStock>(
                 sp.GetRequiredService<IValidator<BookStock>>(),
                 x => x.BookStock));

            services.AddScoped<IValidator<UpdateBulkBooksCommand>>(sp =>
             new GenericCommandValidator<UpdateBulkBooksCommand, BooksRequest>(
                 sp.GetRequiredService<IValidator<BooksRequest>>(),
                 x => x.Request));

            #endregion Book

            #region Loaned Book

            // Generic command validators for LoanBook
            services.AddScoped<IValidator<CreateLoanedBookCommand>>(sp =>
                new GenericCommandValidator<CreateLoanedBookCommand, LoanBook, Guid>(
                    sp.GetRequiredService<IValidator<LoanBook>>(),
                    x => x.LoanBook));

            services.AddScoped<IValidator<CreateBulkLoanBooksCommand>>(sp =>
                new GenericCommandValidator<CreateBulkLoanBooksCommand, IEnumerable<LoanBook>>(
                    sp.GetRequiredService<IValidator<IEnumerable<LoanBook>>>(),
                    x => x.LoanBooks));

            services.AddScoped<IValidator<UpdateLoanedBookCommand>>(sp =>
               new GenericCommandValidator<UpdateLoanedBookCommand, LoanBook, LoanBook>(
                   sp.GetRequiredService<IValidator<LoanBook>>(),
                   x => x.LoanBook));

            services.AddScoped<IValidator<UpdateBulkLoanBooksCommand>>(sp =>
                new GenericCommandValidator<UpdateBulkLoanBooksCommand, IEnumerable<LoanBook>>(
                    sp.GetRequiredService<IValidator<IEnumerable<LoanBook>>>(),
                    x => x.LoanBooks));

            #endregion Loaned Book

            #region Member

            // Generic command validators for Member
            services.AddScoped<IValidator<CreateMemberCommand>>(sp =>
                new GenericCommandValidator<CreateMemberCommand, Member, Guid>(
                    sp.GetRequiredService<IValidator<Member>>(),
                    x => x.Member));

            services.AddScoped<IValidator<CreateBulkMembersCommand>>(sp =>
               new GenericCommandValidator<CreateBulkMembersCommand, IEnumerable<Member>>(
                   sp.GetRequiredService<IValidator<IEnumerable<Member>>>(),
                   x => x.Members));

            services.AddScoped<IValidator<CreateMembersCommand>>(sp =>
            new GenericCommandValidator<CreateMembersCommand, IEnumerable<Member>>(
                sp.GetRequiredService<IValidator<IEnumerable<Member>>>(),
                x => x.Members));

            services.AddScoped<IValidator<UpdateMemberCommand>>(sp =>
               new GenericCommandValidator<UpdateMemberCommand, Member, Member>(
                   sp.GetRequiredService<IValidator<Member>>(),
                   x => x.Member));

            services.AddScoped<IValidator<UpdateBulkMembersCommand>>(sp =>
              new GenericCommandValidator<UpdateBulkMembersCommand, IEnumerable<Member>>(
                  sp.GetRequiredService<IValidator<IEnumerable<Member>>>(),
                  x => x.Members));

            #endregion Member
        }
    }
}