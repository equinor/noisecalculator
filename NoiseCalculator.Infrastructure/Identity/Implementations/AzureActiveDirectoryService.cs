using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using NoiseCalculator.Infrastructure.Identity.Interfaces;

namespace NoiseCalculator.Infrastructure.Identity.Implementations
{
    public class AzureActiveDirectoryService : IDirectoryService
    {
        private readonly GraphConnection _graphConnection;

        public AzureActiveDirectoryService(GraphConnection graphConnection)
        {
            _graphConnection = graphConnection;
        }


        /// <summary>
        /// Retrieves user details from directory
        /// </summary>
        /// <param name="userPrincipalName">The name as returned from Princial object. Example: NINJA@statoil.com</param>
        /// <returns></returns>
        public Domain.Entities.User GetUser(string userPrincipalName)
        {
            var filter = new FilterGenerator
            {
                QueryFilter = ExpressionHelper.CreateEqualsExpression(typeof (User),
                    GraphProperty.UserPrincipalName, userPrincipalName)
            };
            
            var user = _graphConnection.List<User>(null, filter);
            return CreateUser(user.Results.FirstOrDefault());
        }


        public IList<Domain.Entities.User> FindUsers(string query, int limit)
        {
            var displayNameExpression = ExpressionHelper.CreateStartsWithExpression(typeof(User), GraphProperty.DisplayName, query);
            var mailNickExpression = ExpressionHelper.CreateStartsWithExpression(typeof(User), GraphProperty.MailNickname, query);
            
            var filter = new FilterGenerator
            {
                QueryFilter = ExpressionHelper.JoinExpressions(displayNameExpression, 
                                mailNickExpression, ExpressionType.Or)
            };

            if (limit > 0)
            {
                filter.Top = limit;
            }

            var pagedReslts = _graphConnection.List<User>(null, filter);

            return pagedReslts.Results.Select(CreateUser).ToList();
        }


        private static Domain.Entities.User CreateUser(User graphClientUser)
        {
            return new Domain.Entities.User
            {
                Fullname = graphClientUser.DisplayName,
                Department = graphClientUser.Department,
                Email = graphClientUser.Mail,
                Shortname = graphClientUser.MailNickname,
                Role = graphClientUser.JobTitle
            };
        }
    }
}
