/* =========================================================
   DATABASE SCHEMA – TABLE DEFINITIONS
   ========================================================= */

/* ---------- Classrooms table ---------- */
CREATE TABLE Classrooms (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(MAX) NOT NULL,
    Capacity INT NOT NULL,
    HasProjector BIT NOT NULL
);

/* ---------- StudentGroups table ---------- */
CREATE TABLE StudentGroups (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(MAX) NOT NULL,
    StudentCount INT NOT NULL
);

/* ---------- Subjects table ---------- */
CREATE TABLE Subjects (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(MAX) NOT NULL,
    Type INT NOT NULL
);

/* ---------- Teachers table ---------- */
CREATE TABLE Teachers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(MAX) NOT NULL,
    Salary REAL NOT NULL
);

/* =========================================================
   MANY-TO-MANY RELATIONSHIP
   Teachers ↔ Subjects
   ========================================================= */

/* ---------- TeacherSubjects junction table ---------- */
CREATE TABLE TeacherSubjects (
    TeacherId INT NOT NULL,
    SubjectId INT NOT NULL,
    CONSTRAINT PK_TeacherSubjects PRIMARY KEY (TeacherId, SubjectId),
    CONSTRAINT FK_TeacherSubjects_Teachers
        FOREIGN KEY (TeacherId) REFERENCES Teachers(Id) ON DELETE CASCADE,
    CONSTRAINT FK_TeacherSubjects_Subjects
        FOREIGN KEY (SubjectId) REFERENCES Subjects(Id) ON DELETE CASCADE
);

/* Index for faster subject lookups */
CREATE INDEX IX_TeacherSubjects_SubjectId
    ON TeacherSubjects(SubjectId);

/* =========================================================
   TIMETABLE ENTRIES
   ========================================================= */

/* ---------- TimetableEntries table ---------- */
CREATE TABLE TimetableEntries (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TeacherId INT NOT NULL,
    SubjectId INT NOT NULL,
    ClassroomId INT NOT NULL,
    StudentGroupId INT NOT NULL,
    StartTime DATETIME2 NOT NULL,
    EndTime DATETIME2 NOT NULL,

    CONSTRAINT FK_TimetableEntries_Teachers
        FOREIGN KEY (TeacherId) REFERENCES Teachers(Id) ON DELETE CASCADE,
    CONSTRAINT FK_TimetableEntries_Subjects
        FOREIGN KEY (SubjectId) REFERENCES Subjects(Id) ON DELETE CASCADE,
    CONSTRAINT FK_TimetableEntries_Classrooms
        FOREIGN KEY (ClassroomId) REFERENCES Classrooms(Id) ON DELETE CASCADE,
    CONSTRAINT FK_TimetableEntries_StudentGroups
        FOREIGN KEY (StudentGroupId) REFERENCES StudentGroups(Id) ON DELETE CASCADE
);

/* ---------- Indexes for TimetableEntries ---------- */
CREATE INDEX IX_TimetableEntries_TeacherId
    ON TimetableEntries(TeacherId);

CREATE INDEX IX_TimetableEntries_SubjectId
    ON TimetableEntries(SubjectId);

CREATE INDEX IX_TimetableEntries_ClassroomId
    ON TimetableEntries(ClassroomId);

CREATE INDEX IX_TimetableEntries_StudentGroupId
    ON TimetableEntries(StudentGroupId);

/* =========================================================
   DATABASE VIEWS
   ========================================================= */

GO;
-- ===============================================
-- View: View_TeacherLoad
-- Purpose: Shows teacher workload statistics
-- Tables used: Teachers, TimetableEntries, Classrooms, StudentGroups
-- ===============================================
CREATE OR ALTER VIEW View_TeacherLoad AS
SELECT
    t.FullName AS TeacherName,                  -- Teacher's full name
    COUNT(te.Id) AS LessonCount,               -- Total number of lessons assigned
    SUM(DATEDIFF(MINUTE, te.StartTime, te.EndTime)) / 60.0 AS TotalHours,  -- Total hours taught
    AVG(DATEDIFF(MINUTE, te.StartTime, te.EndTime)) AS AvgLessonMinutes,    -- Average lesson duration in minutes
    MIN(te.StartTime) AS FirstLesson,          -- Earliest lesson start time
    MAX(te.EndTime) AS LastLesson,             -- Latest lesson end time
    COUNT(DISTINCT sg.Id) AS DistinctGroupsTaught,   -- Number of distinct student groups taught
    COUNT(DISTINCT c.Id) AS DistinctClassroomsUsed  -- Number of distinct classrooms used
FROM Teachers t
LEFT JOIN TimetableEntries te ON te.TeacherId = t.Id
LEFT JOIN Classrooms c ON te.ClassroomId = c.Id
LEFT JOIN StudentGroups sg ON te.StudentGroupId = sg.Id
GROUP BY t.FullName;
GO;

GO;
-- ===============================================
-- View: View_ClassroomUsage
-- Purpose: Shows classroom utilization statistics
-- Tables used: Classrooms, TimetableEntries, Teachers, StudentGroups
-- ===============================================
CREATE OR ALTER VIEW View_ClassroomUsage AS
SELECT
    c.Name AS ClassroomName,                   -- Classroom name
    COUNT(te.Id) AS UsageCount,                -- Total number of lessons scheduled in the classroom
    SUM(DATEDIFF(MINUTE, te.StartTime, te.EndTime)) / 60.0 AS TotalHoursUsed, -- Total hours classroom is used
    COUNT(DISTINCT sg.Id) AS GroupsCount,      -- Number of distinct student groups using the classroom
    COUNT(DISTINCT t.Id) AS DistinctTeachers   -- Number of distinct teachers using the classroom
FROM Classrooms c
LEFT JOIN TimetableEntries te ON te.ClassroomId = c.Id
LEFT JOIN StudentGroups sg ON te.StudentGroupId = sg.Id
LEFT JOIN Teachers t ON te.TeacherId = t.Id
GROUP BY c.Name;
GO;
