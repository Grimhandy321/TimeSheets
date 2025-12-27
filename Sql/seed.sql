/* =========================================================
   SEED DATA
   ========================================================= */

-- Classrooms
INSERT INTO Classrooms (Capacity, HasProjector, Name) VALUES
(30, 1, 'A101'),
(25, 0, 'B202'),
(40, 1, 'C303');

-- StudentGroups
INSERT INTO StudentGroups (Name, StudentCount) VALUES
('1A', 28),
('2B', 24);

-- Subjects
INSERT INTO Subjects (Name, Type) VALUES
('Mathematics', 0),
('Physics', 0),
('Programming', 1);

-- Teachers
INSERT INTO Teachers (FullName, Salary) VALUES
('John Smith', 45000),
('Anna Novak', 48000),
('Peter Johnson', 43000);

-- TimetableEntries
INSERT INTO TimetableEntries (ClassroomId, StartTime, EndTime, StudentGroupId, SubjectId, TeacherId) VALUES
-- Pondìlí
(1, '2025-03-10 08:00:00', '2025-03-10 08:45:00', 1, 1, 1), -- John Smith uèí Mathematics pro 1A v A101
(3, '2025-03-10 09:00:00', '2025-03-10 09:45:00', 2, 3, 2), -- Anna Novak uèí Programming pro 2B v C303
(2, '2025-03-10 10:00:00', '2025-03-10 10:45:00', 1, 2, 1), -- John Smith uèí Physics pro 1A v B202

-- Úterý
(1, '2025-03-11 08:00:00', '2025-03-11 08:45:00', 2, 1, 2), -- Anna Novak uèí Mathematics pro 2B v A101
(2, '2025-03-11 09:00:00', '2025-03-11 09:45:00', 1, 3, 3), -- Peter Johnson uèí Programming pro 1A v B202
(3, '2025-03-11 10:00:00', '2025-03-11 10:45:00', 2, 2, 2), -- Anna Novak uèí Physics pro 2B v C303

-- Støeda
(1, '2025-03-12 08:00:00', '2025-03-12 08:45:00', 1, 1, 1), -- John Smith uèí Mathematics pro 1A v A101
(2, '2025-03-12 09:00:00', '2025-03-12 09:45:00', 2, 3, 3), -- Peter Johnson uèí Programming pro 2B v B202
(3, '2025-03-12 10:00:00', '2025-03-12 10:45:00', 1, 2, 1); -- John Smith uèí Physics pro 1A v C303