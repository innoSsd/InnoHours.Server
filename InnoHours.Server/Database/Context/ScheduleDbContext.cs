using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InnoHours.Server.Database.Data.Schedule;
using InnoHours.Server.Database.Data.Schedule.Common;
using InnoHours.Server.Database.Data.Schedule.Single;
using InnoHours.Server.Database.Models.Schedule;
using MongoDB.Driver;

namespace InnoHours.Server.Database.Context
{
    public class ScheduleDbContext
    {
        private readonly IMongoCollection<Schedule> _scheduleCollection;
        private readonly IMongoCollection<GroupType> _groupCollection;

        public ScheduleDbContext(MainDbContext context)
        {
            _scheduleCollection = context.MainDatabase.GetCollection<Schedule>("schedule");
            _groupCollection = context.MainDatabase.GetCollection<GroupType>("group_types");
        }

        public async Task<IEnumerable<ProfessorForGrade>> GetProfessorsForGradeAsync(string grade, string group = null)
        {
            var scheduleForProfessorCursor = await _scheduleCollection.FindAsync(schedule =>
                schedule.Grade == grade && group == null ||
                group != null && schedule.Group == group && schedule.Grade == grade);
            var scheduleForProfessor = await scheduleForProfessorCursor.ToListAsync();
            return scheduleForProfessor.Select(schedule => new ProfessorForGrade
            {
                ProfessorId = schedule.Professor.ProfessorId,
                ProfessorName = schedule.Professor.ProfessorName,
                Grade = schedule.Grade,
                Group = schedule.Group,
                CourseName = schedule.Course.CourseName,
                ProfessorType = schedule.Course.LessonType switch
                {
                    "lecture" => "lector",
                    "tutorial" => "tutor",
                    "lab" => "ta",
                    _ => throw new ArgumentException($"Unknown lesson type {schedule.Course.LessonType}"),
                }
            });
        }

        public async Task<ScheduleSingleGroup> GetScheduleForGroupAsync(string grade, string group)
        {
            var scheduleCursor =
                await _scheduleCollection.FindAsync(schedule => schedule.Grade == group && schedule.Group == group);
            var schedule = await scheduleCursor.ToListAsync();
            var scheduleGroupedByDay = schedule.GroupBy(schedule1 => schedule1.Day);
            return new ScheduleSingleGroup
            {
                Grade = grade,
                Group = group,
                Schedule = scheduleGroupedByDay.Select(
                    grouping => new ScheduleDaySingleGroup
                    {
                        DayName = grouping.Key,
                        Lessons = grouping.Select(
                            schedule1 => new ScheduleLesson
                            {
                                TimeStart = schedule1.TimeStart,
                                TimeEnd = schedule1.TimeEnd,
                                Location = schedule1.Location,
                                ProfessorId = schedule1.Professor.ProfessorId,
                                ProfessorName = schedule1.Professor.ProfessorName,
                                LessonType = schedule1.Course.LessonType,
                                Course = schedule1.Course.CourseName
                            }
                        )
                    }
                )
            };
        }

        public async Task<IEnumerable<ScheduleDay>> GetCommonScheduleAsync()
        {
            var scheduleCursor = _scheduleCollection.FindSync(FilterDefinition<Schedule>.Empty);
            var schedule = await scheduleCursor.ToListAsync();
            var scheduleGrouping = schedule.GroupBy(schedule1 => schedule1.Day);
            return scheduleGrouping.Select(grouping =>
                {
                    var groupingByGrade = grouping.GroupBy(schedule1 => schedule1.Grade);
                    return new ScheduleDay
                    {
                        DayName = grouping.Key,
                        Grades = groupingByGrade.Select(grouping2 =>
                            {
                                var groupingByGroup = grouping2.GroupBy(schedule1 => schedule1.Group);
                                return new ScheduleGrade
                                {
                                    GradeName = grouping2.Key,
                                    Groups = groupingByGroup.Select(grouping3 => new ScheduleGroup
                                    {
                                        GroupName = grouping3.Key,
                                        Lessons = grouping3.Select(schedule1 => new ScheduleLesson
                                        {
                                            Course = schedule1.Course.CourseName,
                                            LessonType = schedule1.Course.LessonType,
                                            ProfessorName = schedule1.Professor.ProfessorName,
                                            ProfessorId = schedule1.Professor.ProfessorId,
                                            Location = schedule1.Location,
                                            TimeStart = schedule1.TimeStart,
                                            TimeEnd = schedule1.TimeEnd
                                        })
                                    })
                                };
                            }
                        )
                    };
                }
            );
        }

        public async Task<AvailableGroupsForProfessor> GetAvailableGroupsForProfessorAsync(string professorId)
        {
            var scheduleCursor =
                await _scheduleCollection.FindAsync(schedule => schedule.Professor.ProfessorId == professorId);
            var schedule = await scheduleCursor.ToListAsync();

            var scheduleGroupingByCourse = schedule.GroupBy(schedule1 => schedule1.Course.CourseName);
            return new AvailableGroupsForProfessor
            {
                Groups = new Dictionary<string, Dictionary<string, IEnumerable<string>>>(
                    scheduleGroupingByCourse.Select(grouping =>
                        new KeyValuePair<string, Dictionary<string, IEnumerable<string>>>(grouping.Key,
                            new Dictionary<string, IEnumerable<string>>(grouping.GroupBy(schedule1 => schedule1.Grade)
                                .Select(grouping1 => new KeyValuePair<string, IEnumerable<string>>(grouping1.Key,
                                    grouping1.Select(schedule1 => schedule1.Group))))))
                )
            };
        }

        public async Task<AvailableGroups> GetAvailableGroups()
        {
            var groupsCursor = await _groupCollection.FindAsync(FilterDefinition<GroupType>.Empty);
            var groups = await groupsCursor.ToListAsync();

            var groupsByGrade = groups.GroupBy(group => group.Grade);
            return new AvailableGroups
            {
                Groups = new Dictionary<string, IEnumerable<string>>(groupsByGrade.Select(grouping =>
                    new KeyValuePair<string, IEnumerable<string>>(grouping.Key,
                        grouping.Select(group1 => group1.Group))))
            };
        }

    }
}