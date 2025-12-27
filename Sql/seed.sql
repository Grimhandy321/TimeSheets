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