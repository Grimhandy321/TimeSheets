
/* ---------- View_ClassroomUsage ---------- */
/* =========================================================
   CLEANUP – DROP VIEWS
   ========================================================= */
DROP VIEW IF EXISTS View_TeacherLoad;
DROP VIEW IF EXISTS View_ClassroomUsage;

/* =========================================================
   CLEANUP – DROP TABLES (dependency order!)
   ========================================================= */
DROP TABLE IF EXISTS TeacherSubjects;
DROP TABLE IF EXISTS TimetableEntries;
DROP TABLE IF EXISTS Teachers;
DROP TABLE IF EXISTS Subjects;
DROP TABLE IF EXISTS Classrooms;
DROP TABLE IF EXISTS StudentGroups;

/* =========================================================
   TABLE DEFINITIONS
   ========================================================= */

/* ---------- Classrooms ---------- */
CREATE TABLE Classrooms (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) UNIQUE NOT NULL,
    Capacity INT NOT NULL,
    HasProjector BIT NOT NULL
);

/* ---------- StudentGroups ---------- */
CREATE TABLE StudentGroups (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) UNIQUE NOT NULL,
    StudentCount INT NOT NULL
);

/* ---------- Subjects ---------- */
CREATE TABLE Subjects (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) UNIQUE NOT NULL,
    Type INT NOT NULL
);

/* ---------- Teachers ---------- */
CREATE TABLE Teachers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) UNIQUE NOT NULL,
    Salary REAL NOT NULL
);

/* =========================================================
   MANY-TO-MANY: Teachers ↔ Subjects
   ========================================================= */
CREATE TABLE TeacherSubjects (
    TeacherId INT NOT NULL,
    SubjectId INT NOT NULL,
    CONSTRAINT PK_TeacherSubjects PRIMARY KEY (TeacherId, SubjectId),
    CONSTRAINT FK_TeacherSubjects_Teachers
        FOREIGN KEY (TeacherId) REFERENCES Teachers(Id) ON DELETE CASCADE,
    CONSTRAINT FK_TeacherSubjects_Subjects
        FOREIGN KEY (SubjectId) REFERENCES Subjects(Id) ON DELETE CASCADE
);

CREATE INDEX IX_TeacherSubjects_SubjectId
    ON TeacherSubjects(SubjectId);

/* =========================================================
   TIMETABLE ENTRIES
   ========================================================= */
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

CREATE INDEX IX_TimetableEntries_TeacherId ON TimetableEntries(TeacherId);
CREATE INDEX IX_TimetableEntries_SubjectId ON TimetableEntries(SubjectId);
CREATE INDEX IX_TimetableEntries_ClassroomId ON TimetableEntries(ClassroomId);
CREATE INDEX IX_TimetableEntries_StudentGroupId ON TimetableEntries(StudentGroupId);

/* =========================================================
   VIEWS
   ========================================================= */

/* ---------- View_TeacherLoad ---------- */
GO
CREATE VIEW View_TeacherLoad AS
SELECT
    t.FullName AS TeacherName,
    COUNT(te.Id) AS LessonCount,
    SUM(DATEDIFF(MINUTE, te.StartTime, te.EndTime)) / 60.0 AS TotalHours,
    AVG(DATEDIFF(MINUTE, te.StartTime, te.EndTime)) AS AvgLessonMinutes,
    MIN(te.StartTime) AS FirstLesson,
    MAX(te.EndTime) AS LastLesson,
    COUNT(DISTINCT sg.Id) AS DistinctGroupsTaught,
    COUNT(DISTINCT c.Id) AS DistinctClassroomsUsed
FROM Teachers t
LEFT JOIN TimetableEntries te ON te.TeacherId = t.Id
LEFT JOIN Classrooms c ON te.ClassroomId = c.Id
LEFT JOIN StudentGroups sg ON te.StudentGroupId = sg.Id
GROUP BY t.FullName;

/* ---------- View_ClassroomUsage ---------- */
GO
CREATE VIEW View_ClassroomUsage AS
SELECT
    c.Name AS ClassroomName,
    COUNT(te.Id) AS UsageCount,
    SUM(DATEDIFF(MINUTE, te.StartTime, te.EndTime)) / 60.0 AS TotalHoursUsed,
    COUNT(DISTINCT sg.Id) AS GroupsCount,
    COUNT(DISTINCT t.Id) AS DistinctTeachers
FROM Classrooms c
LEFT JOIN TimetableEntries te ON te.ClassroomId = c.Id
LEFT JOIN StudentGroups sg ON te.StudentGroupId = sg.Id
LEFT JOIN Teachers t ON te.TeacherId = t.Id
GROUP BY c.Name;


