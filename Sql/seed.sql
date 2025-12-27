/* =========================================================
   SEED DATA
   ========================================================= */

/* ---------- Insert Classrooms ---------- */
INSERT INTO Classrooms (Id, Capacity, HasProjector, Name) VALUES
(1, 30, 1, 'A101'),
(2, 25, 0, 'B202'),
(3, 40, 1, 'C303');

/* ---------- Insert Student Groups ---------- */
INSERT INTO StudentGroups (Id, Name, StudentCount) VALUES
(1, '1A', 28),
(2, '2B', 24);

/* ---------- Insert Subjects ---------- */
INSERT INTO Subjects (Id, Name, Type) VALUES
(1, 'Mathematics', 0),
(2, 'Physics', 0),
(3, 'Programming', 1);

/* ---------- Insert Teachers ---------- */
INSERT INTO Teachers (Id, FullName, Salary) VALUES
(1, 'John Smith', 45000),
(2, 'Anna Novak', 48000),
(3, 'Peter Johnson', 43000);

/* ---------- Assign Subjects to Teachers ---------- */
INSERT INTO TeacherSubjects (SubjectId, TeacherId) VALUES
(1, 1),
(2, 1),
(1, 2),
(3, 2),
(3, 3);

/* ---------- Insert Timetable Entries ---------- */
INSERT INTO TimetableEntries
(Id, ClassroomId, StartTime, EndTime, StudentGroupId, SubjectId, TeacherId)
VALUES
(1, 1, '2025-03-10 08:00:00', '2025-03-10 08:45:00', 1, 1, 1),
(2, 3, '2025-03-10 09:00:00', '2025-03-10 09:45:00', 2, 3, 2),
(3, 2, '2025-03-11 10:00:00', '2025-03-11 10:45:00', 1, 2, 1);