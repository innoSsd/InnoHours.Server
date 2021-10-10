using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InnoHours.Server.Authentication;
using InnoHours.Server.Authentication.Annotations;
using InnoHours.Server.Database.Context;
using InnoHours.Server.Database.Data.OfficeHours;
using InnoHours.Server.Models;
using InnoHours.Server.Models.OfficeHours;

namespace InnoHours.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OfficeHoursController : ControllerBase
    {
        private readonly OfficeHoursDbContext _officeHoursContext;

        public OfficeHoursController(OfficeHoursDbContext context)
        {
            _officeHoursContext = context;
        }

        [HttpPost]
        [JwtAuthorized(UserRole = UserRole.Professor)]
        public async Task<ActionResult<OfficeHoursCreateResponse>> Create([FromBody] OfficeHoursCreateRequest requestData)
        {
            var professorId = GetUserId();
            var createOfficeHoursResult = await _officeHoursContext.CreateOfficeHoursAsync(professorId,
                new OfficeHoursCreating
                {
                    Date = requestData.Date,
                    Grade = requestData.Grade,
                    Group = requestData.Group,
                    Location = requestData.Location,
                    CourseName = requestData.CourseName,
                    RepeatEvery = requestData.RepeatEvery,
                    TimeEnd = requestData.EndTime,
                    Description = requestData.Description,
                    StudentsLimit = requestData.StudentsLimit,
                    TimeStart = requestData.StartTime,
                    Title = requestData.Title
                });
            if (createOfficeHoursResult == null)
            {
                return BadRequest( new ErrorResponse
                {
                    Message = "Error while create office hours"
                });
            }

            return new OfficeHoursCreateResponse
            {
                Status = "success",
                OfficeHoursId = createOfficeHoursResult
            };
        }

        [HttpGet]
        [JwtAuthorized(UserRole = UserRole.Professor)]
        public async Task<ActionResult<OfficeHoursGetForProfessorResponse>> GetForProfessor()
        {
            var professorId = GetUserId();
            try
            {
                var getOfficeHoursResult = await _officeHoursContext.GetOfficeHoursForProfessorAsync(professorId);
                return new OfficeHoursGetForProfessorResponse
                {
                    OfficeHours = getOfficeHoursResult
                };
            }
            catch(ArgumentException exception)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = exception.Message
                });
            }
        }

        [HttpGet]
        [JwtAuthorized(UserRole = UserRole.Student)]
        public async Task<ActionResult<OfficeHoursGetForStudentResponse>> GetForStudent()
        {
            var studentId = GetUserId();
            try
            {
                var getOfficeHoursResult = await _officeHoursContext.GetOfficeHoursForStudentAsync(studentId);
                return new OfficeHoursGetForStudentResponse
                {
                    OfficeHours = getOfficeHoursResult
                };
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = exception.Message
                });
            }
        }

        [HttpPost]
        [JwtAuthorized(UserRole = UserRole.Student)]
        public async Task<ActionResult<SimpleResponse>> Apply(string professorId, string officeHoursId)
        {
            var studentId = GetUserId();
            var applySuccess = await _officeHoursContext.StudentApplyForOfficeHours(professorId, officeHoursId, studentId);
            if (applySuccess)
            {
                return new SimpleResponse
                {
                    Success = true
                };
            }

            return BadRequest(new ErrorResponse
            {
                Message = "Error while applying the office hours"
            });
        }

        [HttpPost]
        [JwtAuthorized(UserRole = UserRole.Student)]
        public async Task<ActionResult<SimpleResponse>> CancelApply(string professorId, string officeHoursId)
        {
            var studentId = GetUserId();
            var cancelApplySuccess =
                await _officeHoursContext.CancelStudentApplyForOfficeHoursAsync(professorId, officeHoursId, studentId);
            if (cancelApplySuccess)
            {
                return new SimpleResponse
                {
                    Success = true
                };
            }

            return BadRequest(new ErrorResponse
            {
                Message = "Error while cancel apllying the office hours"
            });
        }

        [HttpPost]
        [JwtAuthorized(UserRole = UserRole.Student)]
        public async Task<ActionResult<OfficeHoursRequestResponse>> CreateRequest([FromBody] OfficeHoursRequestRequest requestData)
        {
            var studentId = GetUserId();
            var studentName = HttpContext.User.Claims.First(claim => claim.Type == "Name").Value;
            try
            {
                var createRequestSuccess = await _officeHoursContext.CreateOfficeHoursRequest(requestData.ProfessorId,
                    new OfficeHoursRequestCreating
                    {
                        StudentId = studentId,
                        StudentName = studentName
                    });
                if (createRequestSuccess == null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Message = "Error while create office hours request"
                    });
                }

                return new OfficeHoursRequestResponse
                {
                    Status = "Success",
                    RequestId = createRequestSuccess
                };
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = exception.Message
                });
            }
        }

        [HttpPost]
        [JwtAuthorized(UserRole = UserRole.Professor)]
        public async Task<ActionResult<OfficeHoursCreateResponse>> AcceptRequest(string officeHoursRequestId,
            [FromBody] OfficeHoursCreateRequest requestData)
        {
            var professorId = GetUserId();
            var createResult = await _officeHoursContext.AcceptOfficeHoursRequestAsync(professorId,
                officeHoursRequestId, new OfficeHoursCreating
                {
                    Date = requestData.Date,
                    Grade = requestData.Grade,
                    Group = requestData.Group,
                    Location = requestData.Location,
                    CourseName = requestData.CourseName,
                    TimeEnd = requestData.EndTime,
                    Description = requestData.Description,
                    RepeatEvery = requestData.RepeatEvery,
                    StudentsLimit = requestData.StudentsLimit,
                    TimeStart = requestData.StartTime,
                    Title = requestData.Title
                });
            if (createResult == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Error while accepting office hours request"
                });
            }

            return new OfficeHoursCreateResponse
            {
                Status = "Success",
                OfficeHoursId = createResult
            };

        }

        [HttpPost]
        [JwtAuthorized(UserRole = UserRole.Professor)]
        public async Task<ActionResult<SimpleResponse>> DeclineRequest(string officeHoursRequestId)
        {
            var professorId = GetUserId();
            var declineSuccess =
                await _officeHoursContext.DeclineOfficeHoursRequestAsync(professorId, officeHoursRequestId);
            if (declineSuccess)
            {
                return new SimpleResponse
                {
                    Success = true
                };
            }

            return BadRequest(new ErrorResponse
            {
                Message = "Error while declining office hours request"
            });
        }

        [HttpGet]
        [JwtAuthorized(UserRole = UserRole.Student)]
        public async Task<ActionResult<OfficeHoursGetRequestsForStudentResponse>> GetRequestsForStudent()
        {
            var studentId = GetUserId();
            var requestsResult = await _officeHoursContext.GetStudentOfficeHoursRequests(studentId);
            return new OfficeHoursGetRequestsForStudentResponse
            {
                Requests = requestsResult
            };
        }

        [HttpGet]
        [JwtAuthorized(UserRole = UserRole.Professor)]
        public async Task<ActionResult<OfficeHoursGetRequestsForProfessorResponse>> GetRequestsForProfessor()
        {
            var professorId = GetUserId();
            var requestsResult = await _officeHoursContext.GetProfessorOfficeHoursRequests(professorId);
            return new OfficeHoursGetRequestsForProfessorResponse
            {
                Requests = requestsResult
            };
        }

        private string GetUserId()
        {
            return HttpContext.User.Claims.First(claim => claim.Type == "Id").Value;
        }

    }
}
