namespace Library.Authorization
{
    public static class Permissions
    {
        #region Users

        public const string ReadUsers = "applicationusers:read";
        public const string CreateUsers = "applicationusers:create";
        public const string UpdateUsers = "applicationusers:update";
        public const string DeleteUsers = "applicationusers:delete";
        public const string ChangePasswordUsers = "applicationusers:changepassword";

        #endregion Users

        #region Books

        public const string ReadBooks = "books:read";
        public const string CreateBooks = "books:create";
        public const string UpdateBooks = "books:update";
        public const string DeleteBooks = "books:delete";

        #endregion Books

        #region LoanBooks

        public const string ReadLoanBooks = "loanbooks:read";
        public const string CreateLoanBooks = "loanbooks:create";
        public const string UpdateLoanBooks = "loanbooks:update";
        public const string DeleteLoanBooks = "loanbooks:delete";

        #endregion LoanBooks

        #region Members

        public const string ReadMembers = "members:read";
        public const string CreateMembers = "members:create";
        public const string UpdateMembers = "members:update";
        public const string DeleteMembers = "members:delete";

        #endregion Members

        #region Roles

        public const string ReadRoles = "roles:read";
        public const string CreateRoles = "roles:create";
        public const string UpdateRoles = "roles:update";
        public const string DeleteRoles = "roles:delete";

        #endregion Roles
    }
}