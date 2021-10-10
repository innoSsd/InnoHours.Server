using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace InnoHours.Server.Authentication.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorizedAttribute : Attribute, IAuthorizationFilter
    {
        public UserRole UserRole { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userService = context.HttpContext.RequestServices.GetService<IUserAuthenticator>() 
                              ?? throw new Exception("User service not found in the DI");

            switch (UserRole)
            {
                case UserRole.Unknown:
                    return;
                case UserRole.Admin:
                    context.Result = new UnauthorizedResult();
                    return;
                case UserRole.Student:
                    var student = userService.GetStudentByIdAsync(context.HttpContext.Items["user_id"] as string).Result;
                    if (student == null)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                    context.HttpContext.User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                            {
                                new Claim("Email", student.Email),
                                new Claim("Name", student.FullName),
                                new Claim("Grade", student.Grade),
                                new Claim("Group", student.Group),
                                new Claim("Id", student.Id)
                            }
                        )
                    );
                    break;
                case UserRole.Professor:
                    var professor = userService.GetProfessorByIdAsync(context.HttpContext.Items["user_id"] as string).Result;
                    if (professor == null)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                    
                    context.HttpContext.User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                            {
                                new Claim("Email", professor.Email),
                                new Claim("Name", professor.FullName),
                                new Claim("Id", professor.Id)
                            }
                        )
                    );
                    break;

            }
        }
    }
}