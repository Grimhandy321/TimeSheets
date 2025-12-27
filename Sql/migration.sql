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

/* ---------- Teacher workload overview ---------- */
GO;
CREATE VIEW View_TeacherLoad AS
SELECT
    t.FullName,
    COUNT(te.Id) AS LessonCount,
    MIN(te.StartTime) AS FirstLesson,
    MAX(te.EndTime) AS LastLesson
FROM Teachers t
JOIN TimetableEntries te ON te.TeacherId = t.Id
GROUP BY t.FullName;

/* ---------- Classroom usage statistics ---------- */
GO;
CREATE VIEW View_ClassroomUsage AS
SELECT
    c.Name AS Classroom,
    COUNT(te.Id) AS UsageCount
FROM Classrooms c
LEFT JOIN TimetableEntries te ON te.ClassroomId = c.Id
GROUP BY c.Name;
